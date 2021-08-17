using CSGOTraderBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTraderBot.Api
{
    public class Get
    {
        public async Task<HttpResponseMessage> Async(string url, List<Cookie> cookies)
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
    }
}
