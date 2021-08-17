using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSGOTraderBot
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            LoadUserControls();
        }

        private void LoadUserControls()
        {
            ucSettingsSitesCSGO5001.CustomLoad();
            ucSettingsGeneral1.CustomLoad();
        }

        private void BtnSettingsCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSettingsOk_Click(object sender, EventArgs e)
        {
            try
            {
                ucSettingsSitesCSGO5001.SaveSettings();
                ucSettingsGeneral1.SaveSettings();
            }
            catch
            {
                MessageBox.Show($"Ocorreu um erro ao salvar as configurações", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();
        }

        private void TreeView1_DoubleClick(object sender, EventArgs e)
        {
            var selectedNode = treeView1.SelectedNode;

            ChangeUserControl(selectedNode.Name);
        }

        private void ChangeUserControl(string controlName)
        {
            switch (controlName.ToUpper())
            {
                case "CSGO500":
                    ucSettingsSitesCSGO5001.BringToFront();
                    break;
                case "GENERAL":
                    ucSettingsGeneral1.BringToFront();
                    break;
                default:
                    return;
            }
        }
    }
}
