using CSGOTraderBot.Models;
using Newtonsoft.Json;
using SteamAuth;
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

        public Task<ResultModel> CheckAccount()
        {
            SteamGuardAccount account = Helper.SteamSettings.GetRemoteAccount();
            string defaultError = "Falha na autenticação";

            try
            {
                if (account != null)
                {
                    Confirmation[] confirmations = null;
                    bool success = false;

                    try
                    {
                        confirmations = account.FetchConfirmations();
                        success = confirmations != null;

                        Helper.Config.Set("steamLoginSecure", account.Session.SteamLoginSecure, "SteamSettings");
                        Helper.Config.Set("sessionid", account.Session.SessionID, "SteamSettings");
                    }
                    catch
                    {
                        success = RefreshAccount(account);
                    }
                    
                    if (success)
                        return Task.FromResult(new ResultModel
                        {
                            Success = true,
                            Message = new List<string> { "Sucesso na autenticação" }
                        });
                }
            }
            catch (Exception ex)
            {
                Helper.Log.SaveError(ex.ToString());
            }

            return Task.FromResult(new ResultModel
            {
                Success = false,
                Message = new List<string>() { defaultError }
            });
        }

        public Task<ResultModel> CheckLogin()
        {
            try
            {
                //CheckRemoteAccount();

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

        private bool RefreshAccount(SteamGuardAccount account)
        {
            try
            {
                account.RefreshSession();
                var confirmations = account.FetchConfirmations();

                if (confirmations != null)
                {
                    var jsonResult = JsonConvert.SerializeObject(account);

                    Helper.Config.Set("remoteAccount", jsonResult, "SteamSettings");
                    Helper.Config.Set("steamLoginSecure", account.Session.SteamLoginSecure, "SteamSettings");
                    Helper.Config.Set("sessionid", account.Session.SessionID, "SteamSettings");

                    return true;
                }
            }
            catch (Exception ex)
            {
                Helper.Log.SaveError(ex.ToString());
            }

            return false;
        }

        public Task<bool> SendConfirmRemoteOffer(string tradeOfferId)
        {
            bool success = false;

            try
            {
                var account = Helper.SteamSettings.GetRemoteAccount();
                ulong offerId = Convert.ToUInt64(tradeOfferId);

                try
                {
                    success = ConfirmRemoteOffer(account, offerId);
                }
                catch
                {
                    RefreshAccount(account);
                    success = ConfirmRemoteOffer(account, offerId);
                }
            }
            catch (Exception ex)
            {
                Helper.Log.SaveError(ex.ToString());
            }

            return Task.FromResult(success);
        }

        private bool ConfirmRemoteOffer(SteamGuardAccount account, ulong tradeOfferId)
        {
            var confirmation = account.FetchConfirmations()
                .FirstOrDefault(x => x.Creator == tradeOfferId);

            if (confirmation != null)
                return account.AcceptConfirmation(confirmation);

            return false;
        }
    }
}
