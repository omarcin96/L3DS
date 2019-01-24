using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Configuration;
using DirectShowLib;
using System.Threading;
using L3DS.Engine.Processing;


namespace L3DS.Engine.Drivers
{


    // Klasa obsługująca kamerę.
    public sealed class Webcam
    {
        // Stałe.
        private const string CONTRAST_KEY   = "contrast";
        private const string SATURATION_KEY = "saturation";
        private const string EXPOSURE_KEY   = "exposure";
        private const string BRIGHTNESS_KEY = "brightness";
        private const string WIDTH_KEY      = "width";
        private const string HEIGHT_KEY     = "height";
        private const string WH_KEY         = "white_balance";
		private const string CAMERA_IDX_KEY = "cam_idx";

		// Singleton:
		private static Webcam m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        // OpenCV
        private VideoCapture videoCapture = null;
        private Mat frame = null;
        private int _contrast = 0;
        private int _saturation = 0;
        private int _exposure = 0;
        private int _brightness = 0;
        private int _width = 0;
        private int _height = 0;
        private int _whitebalance = 0;
		private int _index = 0;
        private bool changed = false;
        private bool dimmensiomChanged = true;

        // Delegaci:
        public delegate void ProcessFrame(Mat frame);
        public event ProcessFrame OnProcessFrame;

        // Pobieranie instancji klasy.
        public static Webcam Instance
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new Webcam();
                    }

                    return m_oInstance;
                }
            }
        }

		public int CameraIndex
		{
			get
			{
				return _index;
			}
			set
			{
				_index = value;
                changed = true;
			}
		}

        // Ustawienie kontrastu.
        public int Contrast
        {
            get
            {
                return _contrast;
            }
            set
            {
                if (value < 0 && value > 10)
                {
                    MessageBox.Show("Kontrast musi być w przedziałach od 0 do 10!");
                    _contrast = 0;
                }
                else
                {
                    _contrast = value;
                }

                changed = true;
            }
        }

        // Ustawienie kontrastu.
        public int WhiteBalance
        {
            get
            {
                return _whitebalance;
            }
            set
            {
                if (value < 2000 && value > 10000)
                {
                    MessageBox.Show("Kontrast musi być w przedziałach od 0 do 10!");
                    _whitebalance = 2000;
                }
                else
                {
                    _whitebalance = value;
                }

                changed = true;
            }
        }

        // Ustawienie jasnosci.
        public int Brightness
        {
            get
            {
                return _brightness;
            }
            set
            {
                    if (value < 0 && value > 255)
                    {
                        MessageBox.Show("Jasność musi być w przedziałach od 0 do 255!");
                        _brightness = 0;
                    }
                    else
                    {
                        _brightness = value;
                    }

                changed = true;
            }
        }

        // Ustawienie saturacji.
        public int Saturation
        {
            get
            {
                return _saturation;
            }
            set
            {
                if (value < 0 && value > 200)
                {
                    MessageBox.Show("Saturacja musi być w przedziałach od 0 do 200!");
                    _saturation = 0;
                }
                else
                {
                    _saturation = value;
                }

                changed = true;

            }
        }

        // Ustawienie szerokosci.
        public int CameraWidth
        {
            get
            {
                return _width;
            }
            set
            {
                if (videoCapture != null && videoCapture.Ptr != IntPtr.Zero)
                {
                    _width = value;

                    //deoCapture.SetCaptureProperty(CapProp.FrameWidth, _width);
                }
                else
                {
                    MessageBox.Show("Nie pobrano uchwytu kamery do ustawiania szerokosci kamery");
                }

                changed = true;
                dimmensiomChanged = true;

            }
        }

        // Ustawienie wysokosci.
        public int CameraHeight
        {
            get
            {
                return _height;
            }
            set
            {
                if (videoCapture != null && videoCapture.Ptr != IntPtr.Zero)
                {
                    _height = value;

                   //ideoCapture.SetCaptureProperty(CapProp.FrameHeight, _height);
                }
                else
                {
                    MessageBox.Show("Nie pobrano uchwytu kamery do ustawiania wysokosci kamery");
                }

                changed = true;
                dimmensiomChanged = true;

            }
        }


        // Ustawienie saturacji.
        public int Exposure
        {
            get
            {
                return _exposure;
            }
            set
            {
                if (value < -11 && value > 1)
                {
                    MessageBox.Show("Saturacja musi być w przedziałach od 0 do 200!");
                    _exposure = 0;
                }
                else
                {
                    _exposure = value;
                }

                changed = true;
            }
        }

		public static List<KeyValuePair<int, string>> GetCameras()
		{

			//-> Create a List to store for ComboCameras
			List<KeyValuePair<int, string>> ListCamerasData = new List<KeyValuePair<int, string>>();

			//-> Find systems cameras with DirectShow.Net dll 
			DsDevice[] _SystemCamereas = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

			int _DeviceIndex = 0;
			foreach (DirectShowLib.DsDevice _Camera in _SystemCamereas)
			{
				ListCamerasData.Add(new KeyValuePair<int, string>(_DeviceIndex, _Camera.Name));
				_DeviceIndex++;
			}

			return ListCamerasData;
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

        // Zapisz  konfiguracje.
        public void SaveConfiguration()
        {
            SetConfigValue(CONTRAST_KEY, String.Format("{0}", Contrast));
            SetConfigValue(BRIGHTNESS_KEY, String.Format("{0}", Brightness));
            SetConfigValue(EXPOSURE_KEY, String.Format("{0}", Exposure));
            SetConfigValue(SATURATION_KEY, String.Format("{0}", Saturation));
            SetConfigValue(WIDTH_KEY, String.Format("{0}", CameraWidth));
            SetConfigValue(HEIGHT_KEY, String.Format("{0}", CameraHeight));
            SetConfigValue(WH_KEY, String.Format("{0}", WhiteBalance));
			SetConfigValue(CAMERA_IDX_KEY, String.Format("{0}", CameraIndex));
		}

        // Zaladuj konfiguracje.
        public void LoadConfiguration()
        {
            bool isInitialized = false;
            int temp;

            // Czulosc.
            isInitialized = int.TryParse(GetConfigValue(EXPOSURE_KEY), out temp);
            if (isInitialized)
            {
                Exposure = temp;
            }

            // Kontrast.
            isInitialized = int.TryParse(GetConfigValue(CONTRAST_KEY), out temp);
            if (isInitialized)
            {
                Contrast = temp;
            }

            // Jasnosc.
            isInitialized = int.TryParse(GetConfigValue(BRIGHTNESS_KEY), out temp);
            if (isInitialized)
            {
                Brightness = temp;
            }

            // Saturacja.
            isInitialized = int.TryParse(GetConfigValue(SATURATION_KEY), out temp);
            if (isInitialized)
            {
                Saturation = temp;
            }

            // wysokosc kamery.
            isInitialized = int.TryParse(GetConfigValue(WIDTH_KEY), out temp);
            if (isInitialized)
            {
                CameraWidth = temp;
            }

            // szerokosc kamery.
            isInitialized = int.TryParse(GetConfigValue(HEIGHT_KEY), out temp);
            if (isInitialized)
            {
                CameraHeight = temp;
            }

            // szerokosc kamery.
            isInitialized = int.TryParse(GetConfigValue(WH_KEY), out temp);
            if (isInitialized)
            {
                WhiteBalance = temp;
            }

            ChangeCamera();
        }

        // Start przechwytywania kamery.
        public void Start()
        {

            if(videoCapture == null)
            {
				ChangeCamera();
				videoCapture = new VideoCapture(CameraIndex, VideoCapture.API.DShow);

                dimmensiomChanged = true;
				LoadConfiguration();
                UpdateParameters();
				videoCapture.ImageGrabbed += OnFrameProcess;
				videoCapture.Start();
			}
            else
            {
				videoCapture.Dispose();
				videoCapture = new VideoCapture();
				ChangeCamera();
				videoCapture.ImageGrabbed += OnFrameProcess;
				videoCapture.Start();
			}
        }

        public void Stop()
        {
            if (videoCapture != null)
            {
                if(OnProcessFrame != null)
                {
                    videoCapture.ImageGrabbed -= OnFrameProcess;
                }
                videoCapture.Stop();
				videoCapture.Dispose();
				videoCapture = null;
            }
            else
            {
                MessageBox.Show("Nie można zatrzymać przechwytywania klatek kamery");
            }
        }

        private void OnFrameProcess(object sender, EventArgs args)
        {
            if(OnProcessFrame != null && videoCapture != null && videoCapture.Ptr != IntPtr.Zero)
            {
                UpdateParameters();
                videoCapture.Retrieve(frame, 0);

                if(frame != null && OnProcessFrame != null)
                    OnProcessFrame(frame);
            }
        }

        public void UpdateParameters()
        {
            if (videoCapture != null && changed)
            {

                if (dimmensiomChanged)
                {
                    Laser.Instance.GenerateWeightMatrix(_width, _height / 2);
                    videoCapture.SetCaptureProperty(CapProp.FrameHeight, _height);
                    videoCapture.SetCaptureProperty(CapProp.FrameWidth, _width);
                    dimmensiomChanged = false;
                }

                disableAutoExposure();
                videoCapture.SetCaptureProperty(CapProp.Contrast, _contrast);
                videoCapture.SetCaptureProperty(CapProp.AndroidWhiteBalance, _whitebalance);
                videoCapture.SetCaptureProperty(CapProp.Brightness, _brightness);
                videoCapture.SetCaptureProperty(CapProp.Saturation, _saturation);
                videoCapture.SetCaptureProperty(CapProp.Exposure, _exposure);

                changed = false;
            }
        }

        private void disableAutoExposure()
        {
            if(videoCapture != null)
            {
                videoCapture.SetCaptureProperty(CapProp.AutoExposure, 0);
            }
        }

		public void ChangeCamera()
		{
			bool isInitialized;
			int temp;
			// szerokosc kamery.
			isInitialized = int.TryParse(GetConfigValue(CAMERA_IDX_KEY), out temp);
			if (isInitialized)
			{
				CameraIndex = temp;
			}

            // Get available cameras.
            var lists = GetCameras();
            var cameraAvailable = lists.Find(x => x.Key == CameraIndex);

            // Check available.
            if(cameraAvailable.Value == null)
            {
                MessageBox.Show("Kamera nie jest dostępna!");
                return;
            }

		}

        // Inicjalizacja zmiennych.
        private Webcam()
        {
            CvInvoke.UseOpenCL = true;
            try
            {
                frame = new Mat();
			} catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

}