using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTrade.Models.CsgoOffer
{
    public class ResponseCreateOffer
    {
        [JsonProperty("tradeofferid")]
        public string TradeofferId { get; set; }

        [JsonProperty("needs_mobile_confirmation")]
        public bool NeedsMobileConfirmation { get; set; }

        [JsonProperty("needs_email_confirmation")]
        public bool NeedsEmailConfirmation { get; set; }

        [JsonProperty("email_domain")]
        public string EmailDomain { get; set; }
    }
}
