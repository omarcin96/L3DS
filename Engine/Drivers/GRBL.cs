using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Timers;

namespace L3DS.Engine.Drivers
{
    public sealed class GRBL
    {
        // Enumerator.
        public enum Position
        {
            Absolute = 0,
            Relative = 1,
        }

        // Stałe:
        const double INTERVAL_PING = 50;
        const string BAUDRATE_KEY = "baudrate";
        const string PORT_KEY = "port";

        // Singleton:
        private static GRBL m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        // Pobieranie instancji klasy.
        public static GRBL Instance
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new GRBL();
                    }

                    return m_oInstance;
                }
            }
        }

        // Settery i Gettery.
        #region predkosc transmisji portu szeregowego

        public int Baudrate
        {
            get
            {
                return port.BaudRate;
            }
            set
            {
                if(port != null)
                {
                    if (isConnect)
                    {
                        Stop();
                        port.BaudRate = value;
                        Start();
                    }
                    else
                    {
                        port.BaudRate = value;
                    }
                }
                else
                {
                    MessageBox.Show("Nie zainicjalizowano klasy portu szeregowego!");
                }
            }
        }

        #endregion
        #region ustawienie portu
        public string PortName
        {
            get
            {
                return port.PortName;
            }
            set
            {
                string[] lists = GRBL.GetListPorts();
                bool has = lists.Contains(value);

                if(has && port != null)
                {
                    if (isConnect)
                    {
                        Stop();
                        port.PortName = value;
                        Start();
                    }
                    else
                    {
                        port.PortName = value;
                    }
                }
                else if(port == null)
                {
                    MessageBox.Show("Nie zainicjalizowano klasy portu szeregowego!");
                }
                else
                {
                    MessageBox.Show("Nie znaleziono wybranego portu. Ustawiono port domyślny");
                    port.PortName = lists[0];
                }

            }

        }
        #endregion

        // Serial:
        private SerialPort port = null;
        private bool isConnect = false;
        private bool isLocked = false;
        private System.Timers.Timer timer = null;

        // Eventy:
        public delegate void _IdleEvent();
        public event _IdleEvent OnIdleEvent;

        public delegate void _AlarmEvent();
        public event _AlarmEvent OnAlarmEvent;

        // Konstruktor klasy.
        private GRBL()
        {
            port = new SerialPort();
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            port.Parity = Parity.None;
            port.DtrEnable = true;
            port.DataBits = 8;
            port.StopBits = StopBits.One;
            isLocked = true;
            LoadConfiguration();
            TimerInit();
        }
        
        private void TimerInit()
        {
            timer = new System.Timers.Timer(INTERVAL_PING);
            timer.Elapsed += PingGRBLEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        // Pobierz wartość z pliku konfiguracyjnego.
        private static string GetConfigValue(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        // query grbl.
        private void PingGRBLEvent(Object source, ElapsedEventArgs e)
        {
            if(port != null && port.IsOpen)
            {
                port.Write("?\n");
            }
        }

        // Połącz z urządzeniem.
        public void Start()
        {
            if (isConnect)
                return;

            try
            {
                port.Open();

            } catch(System.IO.IOException ex)
            {
                isConnect = false;
                MessageBox.Show(ex.Message);
            } finally
            {
                isConnect = true;
            }
        }

        // Rozlacz z urzadzeniem
        public void Stop()
        {
            if (!isConnect)
                return;
            try
            {
                timer.Close();
                port.Close();
            } catch(System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message);
            } finally
            {
                isConnect = false;
            }
        }

        // Pobierz listę dostępnych portów.
        public static string[] GetListPorts()
        {
            return SerialPort.GetPortNames();
        }

        // Zapisz konfiguracje.
        private static void SetConfigValue(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            configFile.AppSettings.Settings[key].Value = value;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public void SaveConfiguration()
        {
            SetConfigValue(BAUDRATE_KEY, String.Format("{0}", Baudrate));
            SetConfigValue(PORT_KEY, PortName);
            
        }

        // Ladowanie konfiguracji z pliku.
        public void LoadConfiguration()
        {
            bool isInitialized = false;
            int temp;

            // predkosc transmisji.
            isInitialized = int.TryParse(GetConfigValue(BAUDRATE_KEY), out temp);
            if (isInitialized)
            {
                Baudrate = temp;
            }

            // nazwa portu.
            PortName = GetConfigValue(PORT_KEY);
        }

        private void IdleEvent()
        {
            isLocked = false;
            OnIdleEvent?.Invoke();
        }

        private void AlarmEvent()
        {
            isLocked = true;
            OnAlarmEvent?.Invoke();
        }

        // Delegat, odbierajacy dane z portu.
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            string line = port.ReadExisting().ToLower();

            if(line != null && (line.Contains("idle") || line.Contains("Check")))
            {
                IdleEvent();
            } else if(line != null && line.Contains("alarm"))
            {
                AlarmEvent();
            }
        }

        private void BeginLock()
        {
            isLocked = true;
        }

        private void EndLock()
        {
            while (isLocked) Thread.Sleep(100);
        }

        public void Reset()
        {
            if (!port.IsOpen)
            {
                MessageBox.Show("Port nie jest otwarty! Połącz się z urządzeniem");
                return;
            }

            port.Write("\r\n\r\n");
            port.Write("?\n");
        }

        public void HomeOnlyX()
        {
            if (!port.IsOpen)
            {
                MessageBox.Show("Port nie jest otwarty! Połącz się z urządzeniem");
                return;
            }

            BeginLock();

            port.Write("$HX\n");

            EndLock();
        }

        // Wyzeruj osie.
        public void HomeAxis()
        {
            if (!port.IsOpen)
            {
                MessageBox.Show("Port nie jest otwarty! Połącz się z urządzeniem");
                return;
            }

            port.Write("G28 X0 Z0\n");
            port.Write("G90 G10 L20 P0 Z0\n");

            BeginLock();

            port.Write("G90 G10 L20 P0 X0\n");

            EndLock();
        }

        public void SetZeroX()
        {
            if (!port.IsOpen)
            {
                MessageBox.Show("Port nie jest otwarty! Połącz się z urządzeniem");
                return;
            }

            port.Write("G90 G10 L20 P0 Z0\n");
        }

        // ustaw pozycje x.
        public void SetPositionX(float x, Position positionType)
        {
            if (!port.IsOpen)
            {
                MessageBox.Show("Port nie jest otwarty! Połącz się z urządzeniem");
                return;
            }

            string command = "";
            if(positionType == Position.Absolute)
            {
                command += "G90";
            }
            else
            {
                command += "G91";
            }

            command += String.Format(" G21 X{0:0.000} F1000\n", x);

            BeginLock();
            port.Write(command);
            EndLock();
        }

        // ustaw pozycje x.
        public void SetPositionZ(float z, Position positionType)
        {
            if (!port.IsOpen)
            {
                MessageBox.Show("Port nie jest otwarty! Połącz się z urządzeniem");
                return;
            }

            string command = "";
            if (positionType == Position.Absolute)
            {
                command += "G90";
            }
            else
            {
                command += "G91";
            }

            command += String.Format(" G21 Z{0:0.000} F1000\n", z);
            BeginLock();
            port.Write(command);
            EndLock();
        }

        // wlacz laser
        public void LaserOn()
        {
            if (!port.IsOpen)
            {
                MessageBox.Show("Port nie jest otwarty! Połącz się z urządzeniem");
                return;
            }

            port.Write("M3\n");
        }

        // wlacz laser
        public void UnlockPositon()
        {
            if (!port.IsOpen)
            {
                MessageBox.Show("Port nie jest otwarty! Połącz się z urządzeniem");
                return;
            }

            port.Write("$X\n");
            BeginLock();
            port.Write("G90 G10 L20 P0 X0\n");
            EndLock();
        }

        // wylacz laser
        public void LaserOff()
        {
            if(!port.IsOpen)
            {
                MessageBox.Show("Port nie jest otwarty! Połącz się z urządzeniem");
                return;
            }

            port.Write("M5\n");
        }

    }
}
