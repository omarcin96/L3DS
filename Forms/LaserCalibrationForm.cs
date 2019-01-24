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
using L3DS.Engine.Processing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;
using System.Threading;
using ZedGraph;

namespace L3DS.Forms
{
    public partial class LaserCalibrationForm : Form
    {
        private static readonly string[] WorkModeStringList = new string[] {"GRAY", "RGB", "THRESHOLD", "CENTER_OF_MASS", "LASER MASK", "BINARIZED_MODE", "SUBSTRACT_MODE"};

        // delegates:
        delegate void SetLaserPointGraphDelegate(bool left, float[] u, float [] v);

        public class ProcessingImageList
        {
            public Mat background = null;
            public Mat lasers = null;

            public ProcessingImageList(Mat _background, Mat _laser)
            {
              //  this.background = _background.Clone();
                this.lasers = _laser.Clone();
            }
        }

        enum WorkMode
        {
            GrayMode = 0,
            RgbMode,
            ThresholdMode,
            CenterOfMassMode,
            LaserMaskMode,
            BinarizedMode,
            SubstractMode,
        }

        private Queue<ProcessingImageList> processingList;
        private bool homed;
        private WorkMode mode;
        private Mat frame;
        private bool onProcessingRunning;
        private Mat grayFrame;
        private Mat bgFrame;
        private Mat substractFrame;
        private Mat rgbFrame;
        private float[] lu, lv, ru, rv;
        private bool attachBg = false;
        private Mat bg;
        Thread processingThread;

        private void GRBL_Idle()
        {
            // None.
        }

        private void GRBL_Alarm()
        {
            GRBL.Instance.Reset();
            GRBL.Instance.UnlockPositon();
            GRBL.Instance.HomeOnlyX();
        }

        public LaserCalibrationForm()
        {
            InitializeComponent();
            GRBL.Instance.UnlockPositon();
            GRBL.Instance.OnAlarmEvent += GRBL_Alarm;
            GRBL.Instance.OnIdleEvent += GRBL_Idle;
            Laser.Instance.LoadConfiguration();
			Webcam.Instance.Start();

            // imageBoxes.
            channelLeftImageBox.Visible = false;
            channelRightImageBox.Visible = false;
            videoMainImage.Visible = true;
            processingList = new Queue<ProcessingImageList>();

            // sliders.
            contrastSlider.Value = Webcam.Instance.Contrast;
            exposureSlider.Value = Webcam.Instance.Exposure;
            saturationSlider.Value = Webcam.Instance.Saturation;
            brightnessSlider.Value = Webcam.Instance.Brightness;
            laserAngleNumber.Value = (int)Laser.Instance.LaserAngle;
            thresholdNumber.Value = (int)Laser.Instance.Threshold;
            sigmaNumber.Value = (int)Laser.Instance.Sigma;
            windowNumber.Value = (int)Laser.Instance.Window;
            videoMainImage.MinimumSize = new Size(Webcam.Instance.CameraWidth, Webcam.Instance.CameraHeight);
            delayLaser.Value = (int)Laser.Instance.DelayLaserMS;


            // graph.
            laserLeftPointGraph.IsShowPointValues = true;
            laserLeftPointGraph.Visible = false;

            laserRightPointGraph.IsShowPointValues = true;
            laserRightPointGraph.Visible = false;

            // mode combo.
            foreach(var item in WorkModeStringList)
            {
                workModeBox.Items.Add(item);
            }

            workModeBox.SelectedIndex = (int)WorkMode.RgbMode;
            mode = WorkMode.RgbMode;
            frame = new Mat();
            grayFrame = new Mat();
            bgFrame = new Mat();
            substractFrame = new Mat();
            rgbFrame = new Mat();
      
            onProcessingRunning = true;

            // homing & set positon.
            GRBL.Instance.HomeOnlyX();
            //GRBL.Instance.SetPositionZ(Laser.Instance.OffsetZ, //GRBL.Position.Absolute);
            heightNumber.Value = (int)Laser.Instance.OffsetZ;

            // Start Thread
            processingThread = new Thread(delegate ()
            {
                OnProcessing();
            });
            processingThread.Start();
			Webcam.Instance.OnProcessFrame += OnFrame;
		}

        private void SetGraphLaserData(bool left, float[] u, float [] v)
        {
            ZedGraph.GraphPane myPane;

            if (left)
            {
                if (laserLeftPointGraph == null)
                    return;
                myPane = laserLeftPointGraph.GraphPane;
            }
            else
            {
                if (laserRightPointGraph == null)
                    return;
                myPane = laserRightPointGraph.GraphPane;
            }

            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();

            // Set the Titles
            myPane.Title.Text = "Wykryte punkty lini lasera";
            myPane.XAxis.Title.Text = "Szerokość [px]";
            myPane.YAxis.Title.Text = "Wysokość [px]";

            PointPairList pairList = new PointPairList();
            int k = 0;

            for (int i = 0; i < v.Length; i++)
            {
                if (v[i] > 0)
                {
                    pairList.Add(v[i], u[k++]);
                }
            }


            LineItem teamACurve = myPane.AddCurve("Lewy Laser",
                   pairList, Color.Red, SymbolType.Circle);

            teamACurve.Symbol.Size = 0.1f;
            teamACurve.Line.IsVisible = false;
            laserLeftPointGraph.AxisChange();

            myPane.XAxis.Scale.Max = Webcam.Instance.CameraWidth;
            myPane.YAxis.Scale.Max = Webcam.Instance.CameraHeight;

            myPane.XAxis.Scale.Min = 0;
            myPane.YAxis.Scale.Min = 0;


            if (left)
            {
                laserLeftPointGraph.Refresh();
            }
            else
            {
                laserRightPointGraph.Refresh();
            }
        }


        private void SetLeftPointGraphOnThread(float[] u, float [] v)
        {
            if (laserLeftPointGraph != null && !this.IsDisposed)
            {

                SetLaserPointGraphDelegate d = new SetLaserPointGraphDelegate(SetGraphLaserData);
                this.Invoke(d, new object[] { true, u, v});
            }
        }

        private void SetRightPointGraphOnThread(float[] u, float[] v)
        {
            if (laserRightPointGraph != null && !this.IsDisposed)
            {
                SetLaserPointGraphDelegate d = new SetLaserPointGraphDelegate(SetGraphLaserData);
                this.Invoke(d, new object[] { false, u, v });
            }
        }


        private void SetLeftChannelVideoImageBox(Mat image)
        {
            if (image == null)
                return;

            lock (channelLeftImageBox)
            {
                if (channelLeftImageBox != null)
                {
                    channelLeftImageBox.Image = image;
                    CvInvoke.Resize(channelLeftImageBox.Image, channelLeftImageBox.Image, new Size(640, 240));
                }
            }
        }

        private void SetRightChannelVideoImageBox(Mat image)
        {
            if (image == null)
                return;

            lock (channelRightImageBox)
            {
                if (channelRightImageBox != null)
                {
                    channelRightImageBox.Image = image;
                    CvInvoke.Resize(channelRightImageBox.Image, channelRightImageBox.Image, new Size(640, 240));
                }
            }
        }

        // watek zajmujacy sie obrabianiem danych zgromadzonych przez watek OnFrame.
        private void OnProcessing()
        {
            while(onProcessingRunning)
            {
                        // przetworz jeden zestaw.
                        if (processingList.Count() > 0)
                        {
                            // pobierz jeden element.
                            var item = processingList.Dequeue();
                            bool success = Laser.Instance.ProcessingImageCL(item.lasers, item.background, out lu, out lv, out ru, out rv);

                            // ustw dane.
                            if (success)
                            {

						        if (mode == WorkMode.ThresholdMode)
						        {
                                    SetLeftChannelVideoImageBox(Laser.Instance.Threshold_Image);
                                    SetRightChannelVideoImageBox(Laser.Instance.Blur_Threshold_Image);
                                }
                                else if(mode == WorkMode.CenterOfMassMode && lu.Length > 0)
                                {
                                    SetLeftPointGraphOnThread(lu, lv);
                                    SetRightPointGraphOnThread(ru, rv);
                                }
                                else if(mode == WorkMode.BinarizedMode)
                                {
                                    SetLeftChannelVideoImageBox(Laser.Instance.Left_Binarized_Image);
                                    SetRightChannelVideoImageBox(Laser.Instance.Right_Binarized_Image);
                                }
                                else if (mode == WorkMode.LaserMaskMode)
                                {
                                    SetLeftChannelVideoImageBox(Laser.Instance.Left_Mask_Image);
                                    SetRightChannelVideoImageBox(Laser.Instance.Right_Mask_Image);
                                }
                                else if (mode == WorkMode.SubstractMode)
                                {
                                    SetLeftChannelVideoImageBox(Laser.Instance.Left_Substract_Image);
                                    SetRightChannelVideoImageBox(Laser.Instance.Right_Substract_Image);
                                }

                    }

                        }
            }
        }


        // Przetwarza klatki z kamery w czasie rzeczywistym.
        private void OnFrame(Mat image)
        {
            if (!onProcessingRunning && image == null && videoMainImage == null) return;

          //  Mat laser = CvInvoke.Imread("laser.png");
          //  Mat bg = CvInvoke.Imread("laser_background.png");

          //  var processing = new ProcessingImageList(bg, laser);
          //  processingList.Enqueue(processing);



            frame = image.Clone();

            // Tryb GRAYA - pokazuje obraz w odcieniu graya ( wszystkie kanaly na raz )
            if(mode == WorkMode.GrayMode)
            {
                CvInvoke.CvtColor(image, grayFrame, ColorConversion.Bgr2Gray);
                CvInvoke.Resize(grayFrame, grayFrame, new Size(640, 480), 0, 0, Inter.Linear);

                if(videoMainImage != null)
                    videoMainImage.Image = grayFrame;
            }

            // Tryb RGB - pokazuje obraz w trybie koloru ( wszystkie kanaly na raz ).
            else if( mode == WorkMode.RgbMode)
            {
                CvInvoke.Resize(image, rgbFrame, new Size(640, 480));

                if (videoMainImage != null)
                    videoMainImage.Image = rgbFrame;
            }
            else
            {
                    var processing = new ProcessingImageList(bg, frame);
                    processingList.Enqueue(processing);
                    attachBg = false;
            }
            
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void threshold_changed(object sender, EventArgs e)
        {
            Laser.Instance.Threshold = (int)thresholdNumber.Value;
        }

        private void height_changed(object sender, EventArgs e)
        {
            if (homed)
            {
                Laser.Instance.OffsetZ = (float)heightNumber.Value;
                //GRBL.Instance.SetPositionZ(Laser.Instance.OffsetZ, //GRBL.Position.Absolute);
            }
        }

        private void sigma_changed(object sender, EventArgs e)
        {
            Laser.Instance.Sigma = (int)sigmaNumber.Value;
        }

        private void window_changed(object sender, EventArgs e)
        {
            Laser.Instance.Window = (int)windowNumber.Value;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Webcam.Instance.SaveConfiguration();
            Laser.Instance.SaveConfiguration();
        }

        private void closeButton_Click(object sender, EventArgs e)
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

        private void brightness_changed(object sender, EventArgs e)
        {
            Webcam.Instance.Brightness = brightnessSlider.Value;
        }

        private void exposure_changed(object sender, EventArgs e)
        {
            Webcam.Instance.Exposure = exposureSlider.Value;
        }

        private void mode_changed(object sender, EventArgs e)
        {
            //mode = (WorkMode)modeBox.SelectedIndex;
           // onLaser = false;
        }

        private void laserAngle_changed(object sender, EventArgs e)
        {
            Laser.Instance.LaserAngle = (int)laserAngleNumber.Value;
        }
      

        private void window_close(object sender, FormClosedEventArgs e)
        {
            if(!homed)
            {
              // GRBL.Instance.UnlockPositon();
               //GRBL.Instance.SetZeroX();
            }

            if (processingThread != null)
            {
                processingThread.Abort();
            }

            onProcessingRunning = false;

			Webcam.Instance.OnProcessFrame -= OnFrame;
			Webcam.Instance.Stop();
            GRBL.Instance.OnAlarmEvent -= GRBL_Alarm;
            GRBL.Instance.OnIdleEvent -= GRBL_Idle;
        }

        private void workmode_changed(object sender, EventArgs e)
        {

        }

        private void videoImage_Click(object sender, EventArgs e)
        {

        }

        private void SetVisibleWindowByChangeMode()
        {
            // one channel modes.
            if(mode == WorkMode.GrayMode || mode == WorkMode.RgbMode)
            {
                channelLeftImageBox.Visible = false;
                channelRightImageBox.Visible = false;
                laserLeftPointGraph.Visible = false;
                laserRightPointGraph.Visible = false;
                videoMainImage.Visible = true;
                laserSwitch.Enabled = true;
                laserSwitch.Checked = false;
            }
            else if (mode == WorkMode.CenterOfMassMode)
            {
                GRBL.Instance.LaserOn();
                laserSwitch.Enabled = false;
                processingList.Clear();
                channelLeftImageBox.Visible = false;
                channelRightImageBox.Visible = false;
                laserLeftPointGraph.Visible = true;
                laserRightPointGraph.Visible = true;
                videoMainImage.Visible = false;
            }
            // left & right channel
            else
            {
                GRBL.Instance.LaserOn();
                laserSwitch.Enabled = false;
                processingList.Clear();
                channelLeftImageBox.Visible = true;
                channelRightImageBox.Visible = true;
                laserLeftPointGraph.Visible = false;
                laserRightPointGraph.Visible = false;
                videoMainImage.Visible = false;
            }
        }

        private void workModeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mode = (WorkMode)workModeBox.SelectedIndex;
            SetVisibleWindowByChangeMode();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void laser_changed(object sender, EventArgs e)
        {
            if(laserSwitch.Checked)
            {
                GRBL.Instance.LaserOn();
            }
            else
            {
                GRBL.Instance.LaserOff();
            }
        }

        private void whiteBalance_changed(object sender, EventArgs e)
        {
            Webcam.Instance.WhiteBalance = (int)whiteBalance.Value;
        }

        private void homeAxis_Click(object sender, EventArgs e)
        {
            GRBL.Instance.HomeAxis();
            homed = true;
            heightNumber.Enabled = true;
            GRBL.Instance.SetPositionZ((float)heightNumber.Value, GRBL.Position.Relative);
        }

        private void delayLaser_change(object sender, EventArgs e)
        {
            Laser.Instance.DelayLaserMS = (int)delayLaser.Value;
        }
    }
}
