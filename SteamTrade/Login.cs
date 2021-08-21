using SteamTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SteamTrade
{
    public class Login
    {
        private const string _urlGetNotificationCounts = "https://steamcommunity.com/actions/GetNotificationCounts";
        private readonly Request request;

        private readonly string _steamRememberLogin;
        private readonly string _steamMachineAuth;
        private readonly string _idSteam64;

        public Login(string steamRememberLogin, string steamMachineAuth, string idSteam64)
        {
            request = new Request();

            _steamRememberLogin = steamRememberLogin.Trim();
            _steamMachineAuth = steamMachineAuth.Trim();
            _idSteam64 = idSteam64.Trim();
        }

        public Task<ResultModel> GetCookies()
        {
            HttpResponseMessage result = null;
            object additional = null;

            var cookies = new List<Cookie>()
                {
                    new Cookie("steamRememberLogin", _steamRememberLogin),
                    new Cookie($"steamMachineAuth{_idSteam64}", _steamMachineAuth)
                };

            var cookiesResult = Task.Run(() => request.GetAsyncReturnCookies(_urlGetNotificationCounts, cookies)).Result;

            if (cookiesResult.StatusCode == HttpStatusCode.OK && cookiesResult.Cookies != null)
            {
                var sessionIdCookie = cookiesResult.Cookies["sessionId"].Value;
                var LoginCookie = cookiesResult.Cookies["steamLoginSecure"].Value;

                if (!string.IsNullOrWhiteSpace(sessionIdCookie) && !string.IsNullOrWhiteSpace(LoginCookie))
                    additional = new
                    {
                        sessionId = sessionIdCookie,
                        steamLoginSecure = LoginCookie
                    };
            }

            result = new HttpResponseMessage
            {
                StatusCode = (additional == null && cookiesResult.StatusCode == HttpStatusCode.OK) ? HttpStatusCode.NotFound : cookiesResult.StatusCode
            };

            HttpStatusCode statusCode = result.IsSuccessStatusCode ? HttpStatusCode.OK : result.StatusCode;
            result.Dispose();

            #region RESULTS
            if (statusCode == HttpStatusCode.OK)
            {
                return Task.FromResult(new ResultModel()
                {
                    Success = true,
                    Messages = null,
                    Additional = additional
                });
            }
            else if (statusCode == HttpStatusCode.Unauthorized)
            {
                return Task.FromResult(new ResultModel()
                {
                    Success = false,
                    Messages = new List<string>() { "Falha na autenticação." }
                });
            }

            return Task.FromResult(new ResultModel()
            {
                Success = false,
                Messages = new List<string>() { "Falha ao tentar se comunicar com o servidor da steam." }
            });
            #endregion
        }
    }
}
