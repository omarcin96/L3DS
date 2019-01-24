using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L3DS.Forms
{
    public partial class PrescanSettings : Form
    {
        // configuration special variables.
        private const string LKX_KEY = "lkx";
        private const string LKY_KEY = "lky";
        private const string LKZ_KEY = "lkz";
        private const string RKX_KEY = "rkx";
        private const string RKY_KEY = "rky";
        private const string RKZ_KEY = "rkz";
        private const string XMAX_KEY = "xmax";
        private const string XRES_KEY = "xres";

        // globals:
        float lkx, lky, lkz;
        float rkx, rky, rkz;
        float xmax, xres;

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

        private void LoadButtonsValueFromSettings()
        {
            xmaxBox.Value = (decimal)xmax;
            xresBox.Value = (decimal)xres;
            klxBox.Value = (decimal)lkx;
            klyBox.Value = (decimal)lky;
            klzBox.Value = (decimal)lkz;
            krxBox.Value = (decimal)rkx;
            kryBox.Value = (decimal)rky;
            krzBox.Value = (decimal)rkz;
        }

        private void xmax_changed(object sender, EventArgs e)
        {

            xmax = (float)xmaxBox.Value;
        }

        private void xres_changed(object sender, EventArgs e)
        {
            xres = (float)xresBox.Value;
        }

        private void klx_changed(object sender, EventArgs e)
        {
            lkx = (float)klxBox.Value;
        }

        private void kly_changed(object sender, EventArgs e)
        {
            lky = (float)klyBox.Value;
        }

        private void klz_changed(object sender, EventArgs e)
        {
            lkz = (float)klzBox.Value;
        }

        private void krx_changed(object sender, EventArgs e)
        {
            rkx = (float)krxBox.Value;
        }

        private void kry_changed(object sender, EventArgs e)
        {
            rky = (float)kryBox.Value;
        }

        private void krz_changed(object sender, EventArgs e)
        {
            rkz = (float)krzBox.Value;
        }

        private void PrescanSettings_Load(object sender, EventArgs e)
        {

        }

        private void SaveConfiguration()
        {
            SetConfigValue(LKX_KEY, String.Format("{0}", lkx));
            SetConfigValue(LKY_KEY, String.Format("{0}", lky));
            SetConfigValue(LKZ_KEY, String.Format("{0}", lkz));
            SetConfigValue(RKX_KEY, String.Format("{0}", rkx));
            SetConfigValue(RKY_KEY, String.Format("{0}", rky));
            SetConfigValue(RKZ_KEY, String.Format("{0}", rkz));
            SetConfigValue(XMAX_KEY, String.Format("{0}", xmax));
            SetConfigValue(XRES_KEY, String.Format("{0}", xres));
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

        public PrescanSettings()
        {
            InitializeComponent();
            LoadConfiguration();
            LoadButtonsValueFromSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveConfiguration();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
