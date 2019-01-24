using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using L3DS.Engine.Drivers;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;
using System.Globalization;
using System.Threading;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace L3DS.Forms
{
    public partial class CalibrateCameraForm : Form
    {
        // Constants:
        public static readonly int NUM_FRAMES = 100;
        public static readonly string CAMERA_MATRIX_DIR = "data/matrixCamera.dat";
        public static readonly string CAMERA_DIST_COEFFS_DIR = "data/distCoeffs.dat";

        private enum Modes
        {
            Calibration = 0,
            Saving = 1,
            View = 2,
            CalculateIntristrics = 3,
            Calibrated = 4
        }

        // OpenCV globals:
        private Modes           currentMode;
        private Mat             grayFrame;
        private Mat             frame;
        private bool            hasCorners;
        private Size            patternSize;
        private int             squareSize;
        private VectorOfPointF  corners;
        private Mat[]           frameArrayBuffer;
        int                     frameBufferSavepoint;
        MCvPoint3D32f[][]       cornersObjectList;
        PointF[][]              cornersPointsList;
        VectorOfPointF[]        cornersPointsVec;
        readonly Mat            cameraMatrix = new Mat(3, 3, DepthType.Cv64F, 1);
        readonly Mat            distCoeffs = new Mat(8, 1, DepthType.Cv64F, 1);
        Mat[]                   rvecs, tvecs;
        int                     width, height;
        bool                    calibrationSaved;


        delegate void StringArgReturningVoidDelegate(string text);


        // Constructor:
        public CalibrateCameraForm()
        {
            InitializeComponent();
            currentMode = Modes.View;
            grayFrame = new Mat();
            patternSize = new Size(7, 6);
            corners = new VectorOfPointF();
            frameBufferSavepoint = 0;
            frameArrayBuffer = new Mat[NUM_FRAMES];
            cornersObjectList = new MCvPoint3D32f[frameArrayBuffer.Length][];
            cornersPointsList = new PointF[frameArrayBuffer.Length][];
            cornersPointsVec = new VectorOfPointF[frameArrayBuffer.Length];
            frame = new Mat();
            squareSize = 25;
            width = 7;
            height = 6;
        }

        private void CalibrateCameraForm_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            Webcam.Instance.OnProcessFrame += onImageGrab;
            Webcam.Instance.Start();
            videoImage.Size = new Size(Webcam.Instance.CameraWidth, Webcam.Instance.CameraHeight);
            brightnessSlider.Value = Webcam.Instance.Brightness;
            saturationSlider.Value = Webcam.Instance.Saturation;
            exposureSlider.Value = Webcam.Instance.Exposure;
            contrastSlider.Value = Webcam.Instance.Contrast;
            calibrationSaved = false;
        }

        private void SetLabelText(string text)
        {
            this.countLabel.Text = text;
        }

        private void SetImageBox(Mat newImage)
        {
            if (newImage == null)
                return;

            lock(newImage)
            {
                lock (videoImage)
                {
                    videoImage.Image = newImage;
                }
            }
        }

        private void onImageGrab(Mat image)
        {
            frame = image;

            if (frame == null)
                return;

            // konwersja do koloru graya.
            CvInvoke.CvtColor(frame, grayFrame, ColorConversion.Bgr2Gray);

            // tryb zapisywania punktow.
            if(currentMode == Modes.Saving)
            {
                hasCorners = CvInvoke.FindChessboardCorners(grayFrame, patternSize, corners, CalibCbType.AdaptiveThresh | CalibCbType.FastCheck | CalibCbType.NormalizeImage);
                //we use this loop so we can show a colour image rather than a gray:
                if (hasCorners) //chess board found
                {
                    //make mesurments more accurate by using FindCornerSubPixel
                    CvInvoke.CornerSubPix(grayFrame, corners, new Size(11, 11), new Size(-1, -1),
                        new MCvTermCriteria(30, 0.1));

                    //if go button has been pressed start aquiring frames else we will just display the points
                    frameArrayBuffer[frameBufferSavepoint] = grayFrame; //store the image
                    frameBufferSavepoint++; //increase buffer positon

                    //check the state of buffer
                    if (frameBufferSavepoint == frameArrayBuffer.Length)
                        currentMode = Modes.CalculateIntristrics; //buffer full

                    Thread.Sleep(100);
                    CvInvoke.DrawChessboardCorners(frame, patternSize, corners, hasCorners);

                    string text = String.Format("{0}", frameBufferSavepoint);

                    if (this.countLabel.InvokeRequired)
                    {
                        StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetLabelText);
                        this.Invoke(d, new object[] { text });
                    }
                    else
                    {
                        SetLabelText(text);
                    }
                }

                corners = new VectorOfPointF();
                hasCorners = false;

            }
            
            // Kalibracja.
            if(currentMode == Modes.CalculateIntristrics)
            {
                for (int k = 0; k < frameArrayBuffer.Length; k++)
                {
                    cornersPointsVec[k] = new VectorOfPointF();
                    CvInvoke.FindChessboardCorners(frameArrayBuffer[k], patternSize, cornersPointsVec[k], CalibCbType.AdaptiveThresh
                        | CalibCbType.FastCheck | CalibCbType.NormalizeImage);
                    //for accuracy
                    CvInvoke.CornerSubPix(grayFrame, cornersPointsVec[k], new Size(11, 11), new Size(-1, -1),
                         new MCvTermCriteria(30, 0.1));

                    //Fill our objects list with the real world mesurments for the intrinsic calculations
                    var objectList = new List<MCvPoint3D32f>();
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            objectList.Add(new MCvPoint3D32f(j * squareSize, i * squareSize, 0.0F));
                        }
                    }

                    //corners_object_list[k] = new MCvPoint3D32f[];
                    cornersObjectList[k] = objectList.ToArray();
                    cornersPointsList[k] = cornersPointsVec[k].ToArray();
                }

                //our error should be as close to 0 as possible

                double error = CvInvoke.CalibrateCamera(cornersObjectList, cornersPointsList, grayFrame.Size,
                     cameraMatrix, distCoeffs, CalibType.RationalModel, new MCvTermCriteria(30, 0.1), out rvecs, out tvecs);
                MessageBox.Show(@"Intrinsic Calculation Error: " + error.ToString(CultureInfo.InvariantCulture), @"Results", MessageBoxButtons.OK, MessageBoxIcon.Information); //display the results to the user
                currentMode = Modes.Calibrated;
            }

            if(currentMode == Modes.Calibrated)
            {
                Mat outFrame = grayFrame.Clone();
                CvInvoke.Undistort(image, outFrame, cameraMatrix, distCoeffs);
                grayFrame = outFrame.Clone();

                if(calibrationSaved)
                {
                    SaveCalibration();
                    calibrationSaved = false;
                }
            }

            SetImageBox(frame);
        }

        private void SaveCalibration()
        {
            SaveFile(CAMERA_MATRIX_DIR, cameraMatrix);
            SaveFile(CAMERA_DIST_COEFFS_DIR, distCoeffs);
        }

        // http://www.emgu.com/forum/viewtopic.php?t=16328
        public static Mat LoadFile(string filename)
        {
            try
            {
                string path = Application.ExecutablePath;
                path = path.Replace(Path.GetFileName(path), filename);

                if (!File.Exists(path)) return null;

                using (FileStream fs = File.Open(path, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Mat data = (Mat)formatter.Deserialize(fs);

                    return data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: LoadFile: " + e.Message);
                return null;
            }
        }

        // http://www.emgu.com/forum/viewtopic.php?t=16328
        public static bool SaveFile(string filename, Mat data)
        {
            try
            {
                string path = Application.ExecutablePath;
                path = path.Replace(Path.GetFileName(path), filename);

                string dir = Application.ExecutablePath;
                dir = dir.Replace(Path.GetFileName(dir), "data/");

                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }

                using (FileStream fs = File.Open(path, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, data);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: SaveFile: " + e.Message);
                return false;
            }

            return true;
        }


        private void acceptButton_Click(object sender, EventArgs e)
        {
            
            Webcam.Instance.SaveConfiguration();
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void contrast_changed(object sender, EventArgs e)
        {
            Webcam.Instance.Contrast = contrastSlider.Value;
        }

        private void saturation_changed(object sender, EventArgs e)
        {
            Webcam.Instance.Saturation = saturationSlider.Value;
        }

        private void exposure_changed(object sender, EventArgs e)
        {
            Webcam.Instance.Exposure = exposureSlider.Value;
        }

        private void window_close(object sender, FormClosedEventArgs e)
        {
            videoImage = null;
            Webcam.Instance.OnProcessFrame -= onImageGrab;
            Webcam.Instance.Stop();
        }

        private void brightness_changed(object sender, EventArgs e)
        {
            Webcam.Instance.Brightness = brightnessSlider.Value;
        }

        private void calibrateButton_Click(object sender, EventArgs e)
        {
            currentMode = Modes.Saving;
            calibrateButton.Enabled = false;
        }
    }
}
