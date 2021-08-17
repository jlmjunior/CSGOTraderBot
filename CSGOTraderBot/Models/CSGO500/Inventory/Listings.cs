using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTraderBot.Models.CSGO500.Inventory
{
    public class Listings
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("openId")]
        public string OpenId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("shortStatus")]
        public string ShortStatus { get; set; }

        [JsonProperty("niceStatus")]
        public string NiceStatus { get; set; }

        [JsonProperty("requestOpenId")]
        public string RequestOpenId { get; set; }

        [JsonProperty("requestTradeURL")]
        public string RequestTradeURL { get; set; }

        [JsonProperty("items")]
        public IEnumerable<Item> Items { get; set; }
    }
}
