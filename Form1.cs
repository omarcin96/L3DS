using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Configuration;
using L3DS.Engine.Drivers;
using L3DS.Engine.Processing;
using L3DS.Forms;
using System.Threading;
using System.Collections;
using System.Numerics;
using System.IO;
using Vector3 = OpenTK.Vector3;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;

namespace L3DS
{
    public partial class L3DS : Form
    {
        private static StreamWriter leftWriter = null;
        private static StreamWriter rightWriter = null;

        public class ProcessingImageList
        {
            public Mat lasers = null;
            public Mat background = null;
            public float xstep = 0.5f;

            public ProcessingImageList(Mat _laser, Mat _bg, float xstep)
            {
                this.lasers = _laser.Clone();
            //    this.background = _bg.Clone();
                this.xstep = xstep;
            }
        }


        // configuration special variables.
        private const string LKX_KEY = "lkx";
        private const string LKY_KEY = "lky";
        private const string LKZ_KEY = "lkz";
        private const string RKX_KEY = "rkx";
        private const string RKY_KEY = "rky";
        private const string RKZ_KEY = "rkz";
        private const string XMAX_KEY = "xmax";
        private const string XRES_KEY = "xres";

        // forms.
        private SaveFileDialog leftLaserDialog, rightLaserDialog;

        // flags:


        // factors:
        float lkx, lky, lkz;
        float rkx, rky, rkz;

        // camera calibration data.
        Mat distCoeffs, mtxCamera;


        // forms variable.
        float xmax = 210.0f;
        float xstep = 0.0f;
        float xres = 0.1f;
        bool scanClicked = false;
        bool onProcessing = false;
        bool bgAttach = false;
        Mat frame;
        Mat bg;
        Queue<Vector3> leftPoints = new Queue<Vector3>();
        Queue<Vector3> rightPoints = new Queue<Vector3>();
        Queue<ProcessingImageList> processingList = new Queue<ProcessingImageList>();
        OpenTKWidget tkWidget;
        Vector3 max_pos = new Vector3();
        Vector3 min_pos = new Vector3();
        Vector3 offsetPointCloud = new Vector3(0.0f);

        public L3DS()
        {
            InitializeComponent();
            SetCultureInfo();
            splitContainer1.MinimumSize = new Size(400, splitContainer1.Height);
            frame = new Mat();
            tkWidget = new OpenTKWidget();
            tkWidget.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(tkWidget);

            /*
            Mat laser = CvInvoke.Imread("laser.png");
            Mat bg = CvInvoke.Imread("laser_background.png");

            float[] lu, lv, ru, rv;

            Laser.Instance.GenerateWeightMatrix(1280, 360);

            Stopwatch ws = new Stopwatch();
            ws.Start();

            float[] x, y, z;

            for (int i = 0; i < 2000; i++)
            {

                Laser.Instance.ProcessingImageCL(laser, bg, out lu, out lv, out ru, out rv);
                Laser.Instance.TransformPixelsTo3DPointCL(lu, lv, 0.18f, 0.18f, 0.18f, 0.0f, out x, out y, out z);
                Laser.Instance.TransformPixelsTo3DPointCL(ru, rv, 0.18f, 0.18f, 0.18f, 0.0f, out x, out y, out z);

                lu = lv = ru = rv = x = y = z = null;
            }

            ws.Stop();
            Debug.WriteLine(ws.Elapsed);*/

        }

        private void SetCultureInfo()
        {
            var culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        private void inicjalizacyjneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var setting = new Forms.SettingsForm();
            setting.ShowDialog();
        }

        private void eEPROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var eeprom = new Forms.EEPROM();
            eeprom.ShowDialog();
        }

        private void FloatParse(string value, out float temp)
        {
            bool isInitialized = false;

            //
            isInitialized = float.TryParse(value, out temp);
            if (!isInitialized)
            {
                MessageBox.Show("Błąd wczytywanie pozycji wierzchołków - niewłaściwy format");
            }
        }

        private void LoadAscFileTo3DWidget(string filename)
        {
            tkWidget.ClearData();

                string line = "";
                using (StreamReader reader = new StreamReader(File.Open(filename, FileMode.Open), Encoding.UTF8, false, 65000, false))
                {
                    while( (line = reader.ReadLine()) != null)
                    {
                        Vector3 pos = new Vector3();
                        string[] values = line.Split(' ');
                        FloatParse(values[0], out pos.X);
                        FloatParse(values[1], out pos.Y);
                        FloatParse(values[2], out pos.Z);

                    // X Axis.
                    if (pos.X > max_pos.X)
                    {
                        max_pos.X = pos.X;
                    }
                    else if (pos.X < min_pos.X)
                    {
                        min_pos.X = pos.X;
                    }

                    // Y Axis
                    if (pos.Y > max_pos.Y)
                    {
                        max_pos.Y = pos.Y;
                    }
                    else if (pos.Y < min_pos.Y)
                    {
                        min_pos.Y = pos.Y;
                    }

                    // Z Axis
                    if (pos.Z > max_pos.Z)
                    {
                        max_pos.Z = pos.Z;
                    }
                    else if (pos.Z < min_pos.Z)
                    {
                        min_pos.Z = pos.Z;
                    }

                    offsetPointCloud.X = -(max_pos.X + min_pos.X) / 2;
                    offsetPointCloud.Y = -(max_pos.Y + min_pos.Y) / 2;
                    offsetPointCloud.Z = -(max_pos.Z + min_pos.Z) / 2;
                    tkWidget.SetOffset(offsetPointCloud);

                    tkWidget.pointClouds.Add(-pos);
                    }

                    reader.Close();
                }

                 
            }

        private void LoadCameraCalibration()
        {
            mtxCamera = Forms.CalibrateCameraForm.LoadFile(Forms.CalibrateCameraForm.CAMERA_MATRIX_DIR);
            distCoeffs = Forms.CalibrateCameraForm.LoadFile(Forms.CalibrateCameraForm.CAMERA_DIST_COEFFS_DIR);
        }

        private void polaczToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eEPROMToolStripMenuItem.Enabled = true;
            polaczToolStripMenuItem.Enabled = false;
            rozlaczToolStripMenuItem.Enabled = true;
            kalibracjaToolStripMenuItem.Enabled = true;
            dystorsjaToolStripMenuItem.Enabled = true;
            laserToolStripMenuItem.Enabled = true;
            inicjalizacyjneToolStripMenuItem.Enabled = false;

            GRBL.Instance.LoadConfiguration();
            GRBL.Instance.Start();
            Thread.Sleep(100);
            scanButton.Enabled = true;
        }

        private void rozlaczToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eEPROMToolStripMenuItem.Enabled = false;
            polaczToolStripMenuItem.Enabled = true;
            rozlaczToolStripMenuItem.Enabled = false;
            kalibracjaToolStripMenuItem.Enabled = false;
            dystorsjaToolStripMenuItem.Enabled = false;
            laserToolStripMenuItem.Enabled = false;
            inicjalizacyjneToolStripMenuItem.Enabled = true;
            scanButton.Enabled = false;

            GRBL.Instance.Stop();
        }

        private void dystorsjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var calibrationCamera = new Forms.CalibrateCameraForm();
            calibrationCamera.Show();
        }

        private void laserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var laserCalibration = new Forms.LaserCalibrationForm();
            laserCalibration.Show();
        }

        private void oProgramieToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void oProgramieToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
        }

        private void zamknijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        delegate void OnFinalSucessDelegate();

        private void SetFinalVisible()
        {
            GRBL.Instance.OnAlarmEvent -= OnAlarmGRBL;
            GRBL.Instance.OnIdleEvent -= OnIdleGRBL;
            Webcam.Instance.OnProcessFrame -= OnFrame;
            Webcam.Instance.Stop();
            MessageBox.Show("Zakończono proces skanowania - przetwarzam dane");
            scanButton.Enabled = true;
            stopButton.Enabled = false;
            pauseButton.Enabled = false;
            while (processingList.Count > 0) Thread.Sleep(1000);
            Thread.Sleep(1000);
            onProcessing = false;
            MessageBox.Show("Dane przetworzone");
        }

        private void OnFinalSucess()
        {
            OnFinalSucessDelegate d = new OnFinalSucessDelegate(SetFinalVisible);
            this.Invoke(d, new object[] {});
        }

        private void OnFrame(Mat image)
        {
            if (image == null || image.Ptr == IntPtr.Zero|| !onProcessing)
                return;

            CvInvoke.Undistort(image, frame, mtxCamera, distCoeffs);

          /*  if(!bgAttach)
            {
                bg = frame.Clone();

                GRBL.Instance.LaserOn();
                Thread.Sleep(400);
                bgAttach = true;
            }
            else
            {*/
                var processing = new ProcessingImageList(frame, bg, xstep);
                processingList.Enqueue(processing);

                GRBL.Instance.SetPositionX(xres, GRBL.Position.Relative);

                xstep += xres;

                if (xstep >= xmax)
                {
                    OnFinalSucess();
                    return;
                }
            /*
                CvInvoke.Imshow("W", bg);
                CvInvoke.Imshow("W2", frame);
                CvInvoke.WaitKey(2000);

                GRBL.Instance.SetPositionX(xres, GRBL.Position.Relative);

                xstep += xres;

                if (xstep >= xmax)
                {
                    OnFinalSucess();
                    return;
                }

                GRBL.Instance.LaserOff();
                Thread.Sleep(00);
                bgAttach = false;*/
        }

        private static string GetConfigValue(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        private static void SetConfigValue(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            configFile.AppSettings.Settings[key].Value = value;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            Webcam.Instance.Stop();
            scanButton.Enabled = true;
            stopButton.Enabled = false;
            pauseButton.Enabled = false;
            repauseButton.Enabled = false;

        }

        private void LoadConfiguration()
        {
            bool isInitialized = false;
            float temp;

            // Czulosc.
            isInitialized = float.TryParse(GetConfigValue(LKX_KEY), out temp);
            if (isInitialized)
            {
                lkx = temp;
            }

            // Czulosc.
            isInitialized = float.TryParse(GetConfigValue(LKY_KEY), out temp);
            if (isInitialized)
            {
                lky = temp;
            }

            // Czulosc.
            isInitialized = float.TryParse(GetConfigValue(LKZ_KEY), out temp);
            if (isInitialized)
            {
                lkz = temp;
            }

            // Czulosc.
            isInitialized = float.TryParse(GetConfigValue(RKX_KEY), out temp);
            if (isInitialized)
            {
                rkx = temp;
            }

            // Czulosc.
            isInitialized = float.TryParse(GetConfigValue(RKY_KEY), out temp);
            if (isInitialized)
            {
                rky = temp;
            }

            // Czulosc.
            isInitialized = float.TryParse(GetConfigValue(RKZ_KEY), out temp);
            if (isInitialized)
            {
                rkz = temp;
            }

            // Czulosc.
            isInitialized = float.TryParse(GetConfigValue(XMAX_KEY), out temp);
            if (isInitialized)
            {
                xmax = temp;
            }

            // Czulosc.
            isInitialized = float.TryParse(GetConfigValue(XRES_KEY), out temp);
            if (isInitialized)
            {
                xres = temp;
            }
        }

        private void RightSavePointsThread()
        {
            while (onProcessing)
            {
                if (rightPoints.Count > 0)
                {
                    if(rightWriter == null)
                    {
                        rightWriter = new StreamWriter(File.Open(rightLaserDialog.FileName, FileMode.OpenOrCreate), Encoding.UTF8, 65000);
                    }


                    lock (rightPoints)
                    {
                        Vector3 pos = rightPoints.Dequeue();
                        rightWriter.WriteLine(String.Format("{0:0.0000000} {1:0.0000000} {2:0.0000000}", pos.X, pos.Y, pos.Z));
                    }
                }
            }

            if(rightWriter != null)
            {
                rightWriter.Close();
            }
        }

        private void LeftSavePointsThread()
        {
            while (onProcessing)
            {
                if (leftPoints.Count > 0)
                {

                    if (leftWriter == null)
                    {
                       leftWriter = new StreamWriter(File.Open(leftLaserDialog.FileName, FileMode.OpenOrCreate), Encoding.UTF8, 65000);
                    }

                    lock (leftPoints)
                    {
                        Vector3 pos = leftPoints.Dequeue();
                       leftWriter.WriteLine(String.Format("{0:0.0000000} {1:0.0000000} {2:0.0000000}", pos.X, pos.Y, pos.Z));


                        // X Axis.
                        if (pos.X > max_pos.X)
                        {
                            max_pos.X = pos.X;
                        }
                        else if (pos.X < min_pos.X)
                        {
                            min_pos.X = pos.X;
                        }

                        // Y Axis
                        if (pos.Y > max_pos.Y)
                        {
                            max_pos.Y = pos.Y;
                        }
                        else if (pos.Y < min_pos.Y)
                        {
                            min_pos.Y = pos.Y;
                        }

                        // Z Axis
                        if (pos.Z > max_pos.Z)
                        {
                            max_pos.Z = pos.Z;
                        }
                        else if (pos.Z < min_pos.Z)
                        {
                            min_pos.Z = pos.Z;
                        }

                        offsetPointCloud.X = -(max_pos.X + min_pos.X) / 2;
                        offsetPointCloud.Y = -(max_pos.Y + min_pos.Y) / 2;
                        offsetPointCloud.Z = -(max_pos.Z + min_pos.Z) / 2;
                        tkWidget.SetOffset(offsetPointCloud);
                        tkWidget.pointClouds.Add(-pos);
                    }

     

                }
            }

            if (leftWriter != null)
            {
                leftWriter.Close();
            }
        }

        private void SavePositionToLeftLaser(Vector3 pos)
        {
            lock(leftPoints)
            {
                leftPoints.Enqueue(pos);
            }
        }

        private void SavePositionToRightLaser(Vector3 pos)
        {
            lock(rightPoints)
            {
                rightPoints.Enqueue(pos);
            }
        }

        // STEP 3.
        private void SaveButtonWizard()
        {
            leftLaserDialog = new SaveFileDialog();
            leftLaserDialog.Filter = "Point Cloud File|*.asc";
            leftLaserDialog.Title = "Zapisz chmurę punktów dla lasera lewego";
            leftLaserDialog.ShowDialog();

            rightLaserDialog = new SaveFileDialog();
            rightLaserDialog.Filter = "Point Cloud File|*.asc";
            rightLaserDialog.Title = "Zapisz chmurę punktów dla lasera prawego";
            rightLaserDialog.ShowDialog();


            if(leftLaserDialog.FileName != "" && rightLaserDialog.FileName != "")
            {
                scanProcessingEnabled();
            }
            else
            {
                scanButton.Enabled = true;
            }

        }

        private void openScan_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileOpen = new OpenFileDialog();
            fileOpen.Filter = "Point Cloud File|*.asc";
            fileOpen.Title = "Wskaż plik z chmurą punktów";
            fileOpen.ShowDialog();

            if (fileOpen.FileName != "")
            {
                LoadAscFileTo3DWidget(fileOpen.FileName);
            }
            else
            {
                MessageBox.Show("Wskazany plik nie istnieje");
            }
        }

        // alarm state.
        private void OnAlarmGRBL()
        {
            GRBL.Instance.UnlockPositon();
            GRBL.Instance.HomeOnlyX();
        }

        // idle state.
        private void OnIdleGRBL()
        {
            // None
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GRBL.Instance.Start();
            Thread.Sleep(1000);
            GRBL.Instance.UnlockPositon();
            GRBL.Instance.HomeOnlyX();
            GRBL.Instance.LaserOff();
            scanProcessingEnabled();

        }

        // STEP 4.
        private void scanProcessingEnabled()
        {
            tkWidget.ClearData();
            processingList.Clear();

            LoadCameraCalibration();
            LoadConfiguration();
            GRBL.Instance.OnAlarmEvent += OnAlarmGRBL;
            GRBL.Instance.OnIdleEvent += OnIdleGRBL;
            GRBL.Instance.UnlockPositon();
            GRBL.Instance.HomeOnlyX();
            GRBL.Instance.LaserOn();

            scanButton.Enabled = false;
            stopButton.Enabled = true;
            pauseButton.Enabled = true;
            repauseButton.Enabled = false;
            scanButton.Enabled = false;


            Webcam.Instance.OnProcessFrame += OnFrame;
            Webcam.Instance.Start();

            Laser.Instance.LoadConfiguration();

            onProcessing = true;
            xstep = 0.0f;


            // Start Thread
            Thread processingThread = new Thread(delegate ()
            {
                OnProcessing();
            });
            processingThread.Start();

            Thread leftThread = new Thread(delegate ()
            {
                LeftSavePointsThread();
            });
            leftThread.Start();

            Thread rightThread = new Thread(delegate ()
            {
                RightSavePointsThread();
            });
            rightThread.Start();
        }

        private void SavePositions(bool left, float[] x, float [] y, float[] z)
        {
            // przeliczaj wspolrzedne.
            for(int i = 0; i < x.Length; i++)
                if (left)
                    SavePositionToLeftLaser(new Vector3(x[i], y[i], z[i]));
                else
                    SavePositionToRightLaser(new Vector3(x[i], y[i], z[i]));
        }

        private void OnProcessing()
        {
            while(onProcessing)
            {
                if(processingList.Count > 0)
                {
                    var item = processingList.Dequeue();

                    // Pobierz wsp. lasera z obrazków.
                    float[] lu, lv, ru, rv;
                    float[] x, y, z;

                    Laser.Instance.ProcessingImageCL(item.lasers, item.background, out lu, out lv, out ru, out rv);

                    // przelicz na wspolrzedne 3d.
                    Laser.Instance.TransformPixelsTo3DPointCL(lu, lv, lkx, lky, lkz, item.xstep, out x, out y, out z);
                    SavePositions(true, x, y, z);

                    Laser.Instance.TransformPixelsTo3DPointCL(ru, rv, rkx, rky, rkz, item.xstep, out x, out y, out z);
                    SavePositions(false, x, y, z);

                    if (item.lasers != null)
                    {
                        item.lasers.Dispose();
                        item.lasers = null;
                    }

                    if (item.background != null)
                    {
                        item.background.Dispose();
                        item.background = null;
                    }
                }
            }
        }

        // STEP 3.
        private void prescan_onClose(object sender, FormClosedEventArgs e)
        {
            LoadConfiguration();
            SaveButtonWizard();
        }

        // STEP 2.
        private void laser_onClose(object sender, FormClosedEventArgs e)
        {
            if(scanClicked)
            {
                var p = new Forms.PrescanSettings();
                p.Show();
                p.FormClosed += prescan_onClose;
                scanClicked = false;
            }
        }

        // STEP 1.
        private void scanButton_Click(object sender, EventArgs e)
        {
			Thread.Sleep(1000);
            onProcessing = false;
            LaserCalibrationForm laser = new LaserCalibrationForm();
            laser.FormClosed += laser_onClose;
            scanClicked = true;
            laser.ShowDialog();
        }
    }
}
