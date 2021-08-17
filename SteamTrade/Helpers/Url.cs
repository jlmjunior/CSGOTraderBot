using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SteamTrade.Helpers
{
    public static class Url
    {
        public static string GetUrlParameterFromString(string url, string parameter)
        {
            Uri uri = new Uri(url);

            return HttpUtility.ParseQueryString(uri.Query)
                .Get(parameter);
        }
    }
}
