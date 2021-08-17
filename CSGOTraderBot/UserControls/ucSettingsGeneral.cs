using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSGOTraderBot.UserControls
{
    public partial class ucSettingsGeneral : UserControl
    {
        public ucSettingsGeneral()
        {
            InitializeComponent();
        }

        public void CustomLoad()
        {
            try
            {
                LoadTimer();
                LoadSteamId();
                LoadCookies();
            }
            catch
            {
                MessageBox.Show($"Ocorreu um erro ao carregar as configurações!", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTimer()
        {
            txtBoxTimer.Text = Helper.Config
                .Get("Timer", "BotSettings");
        }

        private void LoadSteamId()
        {
            txtBoxSteamId.Text = Helper.Config
                .Get("idSteam64", "SteamSettings");
        }
        
        private void LoadCookies()
        {
            txtBoxCookieSessionId.Text = Helper.Config
                .Get("sessionid", "SteamSettings");

            txtBoxCookieSteamLoginSecure.Text = Helper.Config
                .Get("steamLoginSecure", "SteamSettings");

            txtBoxCookieSteamRememberLogin.Text = Helper.Config
                .Get("steamRememberLogin", "SteamSettings");

            txtBoxCookieSteamMachineAuth.Text = Helper.Config
                .Get("steamMachineAuth", "SteamSettings");
        }

        public void SaveSettings()
        {
            try
            {
                SaveTimer();
                SaveSteamId();
                SaveCookies();
            }
            catch
            {
                MessageBox.Show($"Ocorreu um erro ao salvar as configurações!", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveTimer()
        {
            uint time = Convert.ToUInt32(txtBoxTimer.Text);

            Helper.Config.Set("Timer", time.ToString(), "BotSettings");
        }

        private void SaveSteamId()
        {
            Helper.Config.Set("idSteam64", txtBoxSteamId.Text, "SteamSettings");
        }

        private void SaveCookies()
        {
            Helper.Config.Set("sessionid", txtBoxCookieSessionId.Text, "SteamSettings");
            Helper.Config.Set("steamLoginSecure", txtBoxCookieSteamLoginSecure.Text, "SteamSettings");
            Helper.Config.Set("steamRememberLogin", txtBoxCookieSteamRememberLogin.Text, "SteamSettings");
            Helper.Config.Set("steamMachineAuth", txtBoxCookieSteamMachineAuth.Text, "SteamSettings");
        }
    }
}
