using Newtonsoft.Json;
using SteamAuth;
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
    public partial class RemoteAccountValidation : Form
    {
        private readonly string _jsonRemoteAccount;
        private string _customError;

        public RemoteAccountValidation(string jsonRemoteAccount)
        {
            this.DialogResult = DialogResult.None;
            _jsonRemoteAccount = jsonRemoteAccount;
            _customError = null;

            InitializeComponent();
            InitializeAuthentication();
        }

        private void InitializeAuthentication()
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
                FinishWithError();
            } 
            else
                backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var steamGuardAccount = JsonConvert.DeserializeObject<SteamGuardAccount>(_jsonRemoteAccount);

                steamGuardAccount.RefreshSession();
                var confirmations = steamGuardAccount.FetchConfirmations();

                //steamGuardAccount.AcceptConfirmation()

                if (confirmations != null)
                {
                    var jsonResult = JsonConvert.SerializeObject(steamGuardAccount);
                    Helper.Config.Set("remoteAccount", jsonResult, "SteamSettings");
                    Helper.Config.Set("steamLoginSecure", steamGuardAccount.Session.SteamLoginSecure, "SteamSettings");
                    Helper.Config.Set("sessionid", steamGuardAccount.Session.SessionID, "SteamSettings");

                    e.Result = true;
                }
                else
                    e.Result = false;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Helper.Log.SaveError(ex.ToString());
                e.Result = false;

                _customError = "Falha ao tentar se comunicar com o servidor.";
            }
            catch (Exception ex)
            {
                Helper.Log.SaveError(ex.ToString());
                e.Result = false;
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundWorker.IsBusy)
                backgroundWorker.CancelAsync();

            try
            {
                if ((bool)e.Result)
                    FinishWithSuccess();
                else
                    FinishWithError();
            }
            catch (Exception ex)
            {
                Helper.Log.SaveError(ex.ToString());
                FinishWithError();
            }
        }

        private void FinishWithError()
        {
            string message = string.IsNullOrEmpty(_customError) ? "Falha na autenticação." : _customError;

            MessageBox.Show(message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.DialogResult = DialogResult.No;
            this.Dispose();
        }

        private void FinishWithSuccess()
        {
            MessageBox.Show("Sucesso na autenticação", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }
    }
}
