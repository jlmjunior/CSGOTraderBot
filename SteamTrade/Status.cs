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
            HttpResponseMessage result = null;
            object additional = null;

            if (string.IsNullOrWhiteSpace(_sessionId))
            {
                var cookies = new List<Cookie>()
                {
                    new Cookie("steamLoginSecure", _steamLoginSecure)
                };

                var baseAddress = new Uri(_urlGetNotificationCounts);
                var cookieContainer = new CookieContainer();

                using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
                using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    cookies.ForEach(c => cookieContainer.Add(baseAddress, c));

                    client.DefaultRequestHeaders.Accept
                        .Add(new MediaTypeWithQualityHeaderValue("*/*"));

                    client.DefaultRequestHeaders.UserAgent
                        .ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                    result = await client.GetAsync(baseAddress);

                    var sessionIdCookie = handler.CookieContainer
                        .GetCookies(new Uri(_urlGetNotificationCounts))["sessionId"].Value;

                    if (!string.IsNullOrWhiteSpace(sessionIdCookie))
                        additional = new
                        {
                            sessionId = sessionIdCookie
                        };
                }
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

            #region RESULTS
            if (result.IsSuccessStatusCode)
            {
                return new ResultModel()
                {
                    Success = true,
                    Messages = null,
                    Additional = additional
                };
            }
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
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
