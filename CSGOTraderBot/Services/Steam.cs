using CSGOTraderBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTraderBot.Services
{
    public class Steam
    {
        private readonly SteamTrade.Status steamOffer;

        public Steam()
        {
            steamOffer = new SteamTrade.Status(
                Helper.Config.Get("sessionid", "SteamSettings"),
                Helper.Config.Get("steamLoginSecure", "SteamSettings"));
        }

        public Task<ResultModel> CheckLogin()
        {
            try
            {
                var result = Task.Run(() => steamOffer.LoginSuccess()).Result;

                if (!result.Success)
                {
                    var loginSuccess = TryLogin();

                    if (loginSuccess) 
                        result.Success = true;
                }

                if (result.Success)
                {
                    if (result.Additional != null)
                    {
                        var propertyInfo = result.Additional.GetType().GetProperty("sessionId");
                        var value = propertyInfo.GetValue(result.Additional);

                        Helper.Config.Set("sessionid", value.ToString(), "SteamSettings");
                    }
                        
                    return Task.FromResult(new ResultModel
                    {
                        Success = true,
                        Message = new List<string>() { "Sucesso na autenticação." }
                    });
                }

                return Task.FromResult(new ResultModel
                {
                    Success = result.Success,
                    Message = result.Messages.ToList()
                });
            }
            catch (Exception ex)
            {
                Helper.Log.SaveError(ex.ToString());

                return Task.FromResult(new ResultModel
                {
                    Success = false,
                    Message = new List<string> { "Falha ao tentar se comunicar com o servidor da steam." }
                });
            }
        }

        public bool TryLogin()
        {
            var steamLogin = new SteamTrade.Login(
                Helper.Config.Get("steamRememberLogin", "SteamSettings"),
                Helper.Config.Get("steamMachineAuth", "SteamSettings"),
                Helper.Config.Get("idSteam64", "SteamSettings"));

            var result = Task.Run(() => steamLogin.GetCookies()).Result;

            if (result.Success)
            {
                if (result.Additional != null)
                {
                    var propertyInfoSession = result.Additional.GetType().GetProperty("sessionId");
                    var valueSession = propertyInfoSession.GetValue(result.Additional);

                    Helper.Config.Set("sessionid", valueSession.ToString(), "SteamSettings");

                    var propertyInfoLoginSecure = result.Additional.GetType().GetProperty("steamLoginSecure");
                    var valueLoginSecure = propertyInfoLoginSecure.GetValue(result.Additional);

                    Helper.Config.Set("steamLoginSecure", valueLoginSecure.ToString(), "SteamSettings");

                    return true;
                }
            }

            return false;
        }
    }
}
