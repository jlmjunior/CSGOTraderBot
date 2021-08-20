using System.Net;

namespace SteamTrade.Models
{
    public class CookieResultModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public CookieCollection Cookies { get; set; }
    }
}
