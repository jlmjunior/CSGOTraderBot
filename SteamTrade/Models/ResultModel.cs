using System.Collections.Generic;

namespace SteamTrade.Models
{
    public class ResultModel
    {
        public bool Success { get; set; }
        public IEnumerable<string> Messages { get; set; }
        public object Additional { get; set; } = null;
    }
}
