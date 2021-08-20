using SteamTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SteamTrade
{
    public class Status
    {
        private const string _urlGetNotificationCounts = "https://steamcommunity.com/actions/GetNotificationCounts";
        private readonly Request request;

        private readonly string _sessionId;
        private readonly string _steamLoginSecure;

        public Status(string sessionId, string steamLoginSecure)
        {
            request = new Request();

            _sessionId = sessionId;
            _steamLoginSecure = steamLoginSecure;
        }

        public async Task<ResultModel> LoginSuccess()
        {
            if (string.IsNullOrWhiteSpace(_steamLoginSecure))
                return new ResultModel()
                {
                    Success = false,
                    Messages = new List<string>() { "Falha na autenticação." }
                };

            HttpResponseMessage result = null;
            object additional = null;

            if (string.IsNullOrWhiteSpace(_sessionId))
            {
                var cookies = new List<Cookie>()
                {
                    new Cookie("steamLoginSecure", _steamLoginSecure)
                };

                var cookiesResult = Task.Run(() => request.GetAsyncReturnCookies(_urlGetNotificationCounts, cookies)).Result;

                if (cookiesResult.StatusCode == HttpStatusCode.OK && cookiesResult.Cookies != null)
                {
                    var sessionIdCookie = cookiesResult.Cookies["sessionId"].Value;

                    if (!string.IsNullOrWhiteSpace(sessionIdCookie))
                        additional = new
                        {
                            sessionId = sessionIdCookie
                        };
                }

                result = new HttpResponseMessage
                {
                    StatusCode = (additional == null && cookiesResult.StatusCode == HttpStatusCode.OK) ? HttpStatusCode.NotFound : cookiesResult.StatusCode
                };
            }
            else
            {
                var cookies = new List<Cookie>()
                {
                    new Cookie("sessionid", _sessionId),
                    new Cookie("steamLoginSecure", _steamLoginSecure)
                };

                result = await request.GetAsync(_urlGetNotificationCounts, cookies);
            }

            HttpStatusCode statusCode = result.IsSuccessStatusCode ? HttpStatusCode.OK : result.StatusCode;
            result.Dispose();

            #region RESULTS
            if (statusCode == HttpStatusCode.OK)
            {
                return new ResultModel()
                {
                    Success = true,
                    Messages = null,
                    Additional = additional
                };
            }
            else if (statusCode == HttpStatusCode.Unauthorized)
            {
                return new ResultModel()
                {
                    Success = false,
                    Messages = new List<string>() { "Falha na autenticação." }
                };
            }

            return new ResultModel()
            {
                Success = false,
                Messages = new List<string>() { "Falha ao tentar se comunicar com o servidor da steam." }
            };
            #endregion
        }
    }
}
