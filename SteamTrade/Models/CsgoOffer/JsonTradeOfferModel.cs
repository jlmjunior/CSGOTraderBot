using Newtonsoft.Json;
using System.Collections.Generic;

namespace SteamTrade.Models
{
    public class JsonTradeOfferModel
    {
        [JsonProperty("newversion")]
        public bool NewVersion { get; set; } = true;

        [JsonProperty("version")]
        public int Version { get; set; } = 2;

        [JsonProperty("me")]
        public Me MeOffer { get; set; }

        [JsonProperty("them")]
        public Them ThemOffer { get; set; }
    }

    public class Me
    {
        [JsonProperty("assets")]
        public IEnumerable<Assets> Assets { get; set; }

        [JsonProperty("currency")]
        public IEnumerable<Currency> Currency { get; set; }

        [JsonProperty("ready")]
        public bool Ready { get; set; } = false;
    }

    public class Assets
    {
        [JsonProperty("appid")]
        public int AppId { get; set; } = 730;

        [JsonProperty("contextid")]
        public string ContextId { get; set; } = "2";

        [JsonProperty("amount")]
        public uint Amount { get; set; } = 1;

        [JsonProperty("assetid")]
        public string AssetId { get; set; }
    }

    public class Them
    {
        [JsonProperty("assets")]
        public IEnumerable<Assets> Assets { get; set; }

        [JsonProperty("currency")]
        public IEnumerable<Currency> Currency { get; set; }

        [JsonProperty("ready")]
        public bool Ready { get; set; } = false;
    }

    public class Currency { }
}
