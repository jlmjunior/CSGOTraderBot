using SteamTrade.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SteamTrade
{
    public class Request
    {
        public async Task<HttpResponseMessage> PostAsync(string url, List<Cookie> cookies, List<KeyValuePair<string, string>> listKeyValue, string referer = null)
        {
            HttpResponseMessage response;

            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                client.DefaultRequestHeaders.Accept.Clear();

                if (cookies != null)
                {
                    cookies.ForEach(c => cookieContainer.Add(baseAddress, c));
                }

                var content = new FormUrlEncodedContent(listKeyValue);

                if (!string.IsNullOrEmpty(referer))
                    client.DefaultRequestHeaders.Add("Referer", referer);

                client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("*/*"));

                response = await client.PostAsync(baseAddress, content);

                content.Dispose();
            }

            return response;
        }

        public async Task<HttpResponseMessage> GetAsync(string url, List<Cookie> cookies)
        {
            HttpResponseMessage response;

            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                client.DefaultRequestHeaders.Accept.Clear();

                if (cookies != null)
                {
                    cookies.ForEach(c => cookieContainer.Add(baseAddress, c));
                }

                client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("*/*"));

                client.DefaultRequestHeaders.UserAgent
                    .ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                response = await client.GetAsync(baseAddress);
            }

            return response;
        }

        public async Task<CookieResultModel> GetAsyncReturnCookies(string url, List<Cookie> cookies)
        {
            HttpResponseMessage response = null;

            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                client.DefaultRequestHeaders.Accept.Clear();

                if (cookies != null)
                {
                    cookies.ForEach(c => cookieContainer.Add(baseAddress, c));
                }

                client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("*/*"));

                client.DefaultRequestHeaders.UserAgent
                    .ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                response = await client.GetAsync(baseAddress);

                if (response.IsSuccessStatusCode)
                {
                    var cookiesResult = handler.CookieContainer
                        .GetCookies(new Uri(url));

                    return new CookieResultModel
                    {
                        StatusCode = HttpStatusCode.OK,
                        Cookies = cookiesResult
                    };
                }

                return new CookieResultModel
                {
                    StatusCode = response.StatusCode,
                    Cookies = null
                };
            }
        }
    }
}
