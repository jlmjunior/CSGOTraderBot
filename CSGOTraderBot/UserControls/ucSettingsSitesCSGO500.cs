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
    public partial class ucSettingsSitesCSGO500 : UserControl
    {
        public ucSettingsSitesCSGO500()
        {
            InitializeComponent();
        }

        public void CustomLoad()
        {
            try
            {
                LoadCookies();
            }
            catch
            {
                MessageBox.Show($"Ocorreu um erro ao carregar os configurações de CSGO500!", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCookies()
        {
            txtBoxCookieExpressSid.Text = Helper.Config
                .Get("express.sid", "CSGO500");
        }

        public void SaveSettings()
        {
            try
            {
                SaveCookies();
            }
            catch
            {
                MessageBox.Show($"Ocorreu um erro ao salvar as configurações de CSGO500!", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveCookies()
        {
            Helper.Config.Set("express.sid", txtBoxCookieExpressSid.Text, "CSGO500");
        }
    }
}
