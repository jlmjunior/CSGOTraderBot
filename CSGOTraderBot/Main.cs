using CSGOTraderBot.Services;
using Newtonsoft.Json;
using SteamAuth;
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
            ResetConfig();
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
            string json = "{ \"shared_secret\": \"a1gVn4lJUqFSfsv/EsVpvbuz2u4=\", \"serial_number\": \"8116970612227285651\", \"revocation_code\": \"R64023\", \"uri\": \"otpauth://totp/Steam:viniciusvmt19986?secret=NNMBLH4JJFJKCUT6ZP7RFRLJXW53HWXO&issuer=Steam\", \"server_time\":1616071072,\"account_name\": \"viniciusvmt19986\", \"token_gid\": \"27ede7ae7ec4b093\", \"identity_secret\": \"wl2ngKwLc7Ule7L+pErI9gAHSQU=\", \"secret_1\": \"W0N6zv8A0bI+8C5SJ3q63Ppv6oU=\", \"status\":1, \"device_id\": \"android:9f592f22-0597-4106-8088-2266673d1f90\", \"fully_enrolled\":true, \"Session\":{ \"SessionID\": \"4490be870d9d304fdab415bc\", \"SteamLogin\": \"76561199029413489%7C%7CBBC93DAFCD04B42F8105D160A6E001F07931B0D6\", \"SteamLoginSecure\": \"76561199029413489%7C%7C98C20BF805DC739E67E9C7B9A1DD7AE71E2C789F\", \"WebCookie\": \"534CDE22514DFE439DF2B27DE479CE70E4B9187B\", \"OAuthToken\": \"8d0216f990d6d3ec85706191e7426f53\", \"SteamID\":76561199029413489}}";

            var result = JsonConvert.DeserializeObject<SteamGuardAccount>(json);

            var te = result.FetchConfirmations();
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
