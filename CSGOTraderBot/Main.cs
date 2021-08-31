using CSGOTraderBot.Services;
using Newtonsoft.Json;
using SteamAuth;
using SteamTrade.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSGOTraderBot
{
    public partial class Main : Form
    {
        private Timer _timerBot;
        private static readonly object _lockBot = new object();

        public Main()
        {
            InitializeComponent();

            try
            {
                InitializeTimer();
                InitializeAccount();
                ResetConfig();
            }
            catch (Exception ex)
            {
                Helper.Log.SaveError(ex.ToString());
                throw ex;
            }
        }

        private void InitializeAccount()
        {
            var steamGuardAccount = Helper.SteamSettings.GetRemoteAccount();

            if (steamGuardAccount != null && !string.IsNullOrWhiteSpace(steamGuardAccount.AccountName))
            {
                SetRemoteAccount(steamGuardAccount.AccountName);
            }
        }

        private void InitializeTimer()
        {
            if (_timerBot == null)
            {
                int interval = Convert.ToInt32(Helper.Config.Get("Timer", "BotSettings"));

                _timerBot = new Timer()
                {
                    Interval = interval
                };

                _timerBot.Tick += new EventHandler(Timer_Tick);
            }
        }

        private void AddAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var remoteAccountResult = GetJsonRemoteAccount();

            if (remoteAccountResult.Success)
            {
                var propertyremoteAccount = remoteAccountResult.Additional
                    .GetType()
                    .GetProperty("json");

                var valueRemoteAccount = propertyremoteAccount.GetValue(remoteAccountResult.Additional);
                string json = valueRemoteAccount.ToString();
                
                var resultValidationAccount = new RemoteAccountValidation(json).ShowDialog();

                if (resultValidationAccount == DialogResult.OK)
                {
                    var steamGuardAccount = JsonConvert.DeserializeObject<SteamGuardAccount>(json);
                    SetRemoteAccount(steamGuardAccount.AccountName);
                }
            }
            else
            {
                if (remoteAccountResult.Messages?.Any() == true)
                {
                    MessageBox.Show(remoteAccountResult.Messages.FirstOrDefault(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ResetConfig()
        {
            Helper.Config.Set("sessionid", string.Empty, "SteamSettings");
            //Helper.Config.Set("steamLoginSecure", string.Empty, "SteamSettings");
        }

        #region Events
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Settings().ShowDialog();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                Task.Run(() => 
                {
                    this.Invoke(new Action(() =>
                    {
                        btnStart.Enabled = false;
                    }));

                    lock (_lockBot)
                    {
                        RunBot();
                    }

                    this.Invoke(new Action(() =>
                    {
                        btnStart.Enabled = true;
                    }));
                });
            }
            catch (Exception ex)
            {
                Helper.Log.SaveError(ex.ToString());
                throw ex;
            }
        }
        #endregion

        private void RunBot()
        {
            if (_timerBot.Enabled)
            {
                this.Invoke(new Action(() =>
                {
                    _timerBot.Stop();
                    btnStart.Text = "Start";
                    lblInfo.Text = "Ready";
                    SendMessageScreen("Serviço pausado.");
                }));
            }
            else
            {
                Steam steam = new Steam();

                #region VALIDAÇÃO
                this.Invoke(new Action(() =>
                {
                    SendMessageScreen("Verificando autenticação da conta remota");
                }));

                var remoteAccount = Helper.SteamSettings.GetRemoteAccount();
                var resultRemote = Task.Run(() => steam.CheckRemoteAccount(remoteAccount)).Result;
                resultRemote.Message.ForEach(m => this.Invoke(new Action(() =>
                {
                    SendMessageScreen($"{m}");
                })));

                if (!resultRemote.Success) return;

                this.Invoke(new Action(() =>
                {
                    SendMessageScreen("Verificando autenticação dos cookies com a steam");
                }));
                
                var resultSteamCookies = Task.Run(() => steam.CheckLogin()).Result;
                resultSteamCookies.Message.ForEach(m => this.Invoke(new Action(() =>
                {
                    SendMessageScreen($"{m}");
                })));

                if (!resultSteamCookies.Success) return;
                #endregion

                this.Invoke(new Action(() =>
                {
                    _timerBot.Start();
                    btnStart.Text = "Stop";
                    lblInfo.Text = "Running...";
                    SendMessageScreen("Serviço inicializado.");
                }));
            }   
        }
        
        void Timer_Tick(object sender, EventArgs e)
        {
            if (backgroundWorkerBot.IsBusy)
                return;

            backgroundWorkerBot.RunWorkerAsync();
        }

        private void BackgroundWorkerBot_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                SendMessageScreen("Rodou");
            }));
            #region CSGO500
            if (chkBoxCSGO500.Checked)
            {
                try
                {
                    CSGO500 csgo500 = new CSGO500();

                    #region CONFIRMAÇÃO DE ITENS NO CSGO500
                    var resultConfirmItemsCsgo500 = Task.Run(() => csgo500.ConfirmItems()).Result;

                    this.Invoke(new Action(() =>
                    {
                        resultConfirmItemsCsgo500.Message
                        .ForEach(m => SendMessageScreen($"[CSGO500] {m}"));
                    }));
                    #endregion
                }
                catch (Exception ex)
                {
                    Helper.Log.SaveError(ex.ToString());

                    this.Invoke(new Action(() =>
                    {
                        SendMessageScreen("[CSGO500] Ocorreu um erro inesperado ao procurar itens para confirmação.");
                    }));
                }
            }
            #endregion
        }

        private void BackgroundWorkerBot_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundWorkerBot.IsBusy)
                backgroundWorkerBot.CancelAsync();
        }

        #region AUX
        private void SetRemoteAccount(string accountName)
        {
            string label = string.IsNullOrWhiteSpace(accountName) ? "Not linked" : accountName.Trim();

            lblRemoteAccount.Text = $"Remote account: {label}";
        }

        private void SendMessageScreen(string message)
        {
            txtBoxStatus.Text += $"[{DateTime.Now}] {message}" + Environment.NewLine;
        }

        private ResultModel GetJsonRemoteAccount()
        {
            using (var file = new OpenFileDialog())
            {
                if (file.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string extension = Path.GetExtension(file.FileName);

                        if (!ValidExtensionsFile(extension))
                            return new ResultModel
                            {
                                Success = false,
                                Messages = new string[] { "Extensão de arquivo não compatível." }
                            };

                        var fileText = File.ReadAllText(file.FileName);

                        return new ResultModel
                        {
                            Success = true,
                            Messages = null,
                            Additional = new
                            {
                                json = fileText
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        Helper.Log.SaveError(ex.ToString());

                        return new ResultModel
                        {
                            Success = false,
                            Messages = new string[] { "Falha inesperada ao tentar ler o arquivo." }
                        };
                    }
                }
            }

            return new ResultModel
            {
                Success = false,
                Messages = null
            };
        }

        private bool ValidExtensionsFile(string extension)
        {
            switch (extension.ToLower())
            {
                case ".mafile":
                case ".mafiles":
                case ".txt":
                    return true;

                default: 
                    return false;
            }
        }
        #endregion
    }
}
