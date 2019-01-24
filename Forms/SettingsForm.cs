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

namespace L3DS.Forms
{
    public partial class SettingsForm : Form
    {
        private static readonly int[] BAUDRATES = new int[] { 9600, 19200, 38400, 57600, 115200 };
        private static readonly string[] RESOLUTION_CAMERA = new string[] { "160x120", "320x240", "424x240", "640x360", "800x448", "960x554", "1280x720" };
        private string[] portList;
		private List<KeyValuePair<int, string>> lists;

		public SettingsForm()
        {
            InitializeComponent();
        }

		private void LoadCamerasBox()
		{
			lists  = Webcam.GetCameras();
			foreach(var item in lists)
			{
				camerasBox.Items.Add(item.Value);
				if(item.Key == Webcam.Instance.CameraIndex)
				{
					camerasBox.SelectedIndex = Webcam.Instance.CameraIndex;
				}
			}
		}

        private void LoadBaudratesBox()
        {
            foreach (var item in BAUDRATES)
            {
                baudrateBox.Items.Add(item);
            }

            baudrateBox.SelectedIndex = baudrateBox.Items.IndexOf(GRBL.Instance.Baudrate);

        }

        private void LoadPortBox()
        {
            portList = GRBL.GetListPorts();

            foreach (var item in portList)
            {
                portBox.Items.Add(item);
            }

            int index = portBox.Items.IndexOf(GRBL.Instance.PortName);
            if (index >= 0)
            {
                portBox.SelectedIndex = index;
            }
            else if (portBox.Items.Count > 0)
            {
                portBox.SelectedIndex = 0;
            }
        }

        private void LoadResolutionBox()
        {
            foreach (var item in RESOLUTION_CAMERA)
            {
                resolutionBox.Items.Add(item);
            }

            string resolutionCamera = String.Format("{0}x{1}", Webcam.Instance.CameraWidth, Webcam.Instance.CameraHeight);

            int index = resolutionBox.Items.IndexOf(resolutionCamera);
            if (index >= 0)
            {
                resolutionBox.SelectedIndex = index;
            }
            else if (resolutionBox.Items.Count > 0)
            {
                resolutionBox.SelectedIndex = 0;
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
			Webcam.Instance.Start();
            LoadBaudratesBox();
            LoadPortBox();
            LoadResolutionBox();
			LoadCamerasBox();

		}

        private void port_changed(object sender, EventArgs e)
        {

        }

        private void baudrate_changed(object sender, EventArgs e)
        {

        }

        private void accept_Click(object sender, EventArgs e)
        {
            if(baudrateBox.SelectedIndex >= 0)
            {
                GRBL.Instance.Baudrate = BAUDRATES[baudrateBox.SelectedIndex];
            }

            if (portBox.SelectedIndex >= 0)
            {
                GRBL.Instance.PortName = portList[portBox.SelectedIndex];
            }

			if (camerasBox.SelectedIndex >= 0)
			{
				Webcam.Instance.CameraIndex = lists[camerasBox.SelectedIndex].Key;
			}

			if (resolutionBox.SelectedIndex >= 0)
            {
                string curRes = RESOLUTION_CAMERA[resolutionBox.SelectedIndex];
                string[] res = curRes.Split('x');

                bool init = false;
                int temp = 0;

                init = int.TryParse(res[0], out temp);
                if(init)
                {
                    Webcam.Instance.CameraWidth = temp;
                }
                else
                {
                    MessageBox.Show("Width error");
                }

                init = int.TryParse(res[1], out temp);
                if (init)
                {
                    Webcam.Instance.CameraHeight = temp;
                }
                else
                {
                    MessageBox.Show("height error");
                }



            }


            Webcam.Instance.SaveConfiguration();
            GRBL.Instance.SaveConfiguration();
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void resolutionBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

		private void cameras_changed(object sender, EventArgs e)
		{

		}

		private void close_window(object sender, FormClosedEventArgs e)
		{
			Webcam.Instance.Stop();
		}

        private void OpenCL_DeviceBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
