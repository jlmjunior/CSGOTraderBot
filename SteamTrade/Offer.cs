using Newtonsoft.Json;
using SteamTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SteamTrade
{
    public class Offer
    {
        private readonly Request request;

        private readonly string _sessionId;
        private string _steamLoginSecure;

        public Offer(string sessionId, string steamLoginSecure)
        {
            request = new Request();

            _sessionId = sessionId;
            _steamLoginSecure = steamLoginSecure;
        }

        public void SetSteamLoginSecure(string value)
        {
            _steamLoginSecure = value;
        }

        public async Task<ResultModel> Send(OfferModel offer)
        {
            var cookies = new List<Cookie>()
            {
                new Cookie("sessionid", _sessionId),
                new Cookie("steamLoginSecure", _steamLoginSecure)
            };

            object createParams = new
            {
                trade_offer_access_token = Helpers.Url.GetUrlParameterFromString(offer.TradeLink, "token")
            };

            var jsonParameters = JsonConvert.SerializeObject(createParams);
            var jsonOffer = JsonConvert.SerializeObject(offer.JsonOffer);

            var formBody = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("sessionid", _sessionId),
                new KeyValuePair<string, string>("serverid", "1"),
                new KeyValuePair<string, string>("partner", offer.Partner),
                new KeyValuePair<string, string>("tradeoffermessage", string.Empty),
                new KeyValuePair<string, string>("json_tradeoffer", jsonOffer),
                new KeyValuePair<string, string>("captcha", string.Empty),
                new KeyValuePair<string, string>("trade_offer_create_params", jsonParameters),
            };

            var result = await request.PostAsync("https://steamcommunity.com/tradeoffer/new/send", cookies, formBody, offer.TradeLink);

            #region RESULTS
            if (result.IsSuccessStatusCode)
            {
                return new ResultModel()
                {
                    Success = true,
                    Messages = null
                };
            }
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new ResultModel()
                {
                    Success = false,
                    Messages = new List<string>() { "Cookie inválido." }
                };
            }

            return new ResultModel()
            {
                Success = false,
                Messages = new List<string>() { "Falha inesperada ao consultar o serviço." }
            };
            #endregion
        }
    }
}
