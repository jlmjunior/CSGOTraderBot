namespace SteamTrade.Models
{
    public class OfferModel
    {
        public string TradeLink { get; set; }
        public string Partner { get; set; }
        public JsonTradeOfferModel JsonOffer { get; set; }
    }
}
