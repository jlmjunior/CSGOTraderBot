using CSGOTraderBot.Services;
using SteamTrade.Models;
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
    public partial class Main : Form
    {
        private Timer _timerBot;

        public Main()
        {
            InitializeComponent();
            InitializeTimer();
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
                RunBot();
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
                _timerBot.Stop();

                btnStart.Text = "Start";
                lblInfo.Text = "Ready";

                SendMessageScreen("Serviço pausado.");
            }
            else
            {
                Application.DoEvents();
                Steam steam = new Steam();

                SendMessageScreen("Verificando autenticação com a steam");

                var result = Task.Run(() => steam.CheckLogin()).Result;
                result.Message.ForEach(m => SendMessageScreen($"{m}"));

                if (result.Success)
                {
                    _timerBot.Start();

                    btnStart.Text = "Stop";
                    lblInfo.Text = "Running...";

                    SendMessageScreen("Serviço inicializado.");
                }
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
        private void SendMessageScreen(string message)
        {
            txtBoxStatus.Text += $"[{DateTime.Now}] {message}" + Environment.NewLine;
        }
        #endregion

    }
}
