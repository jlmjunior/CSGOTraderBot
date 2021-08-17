using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTraderBot.Models.CSGO500.Inventory
{
    public class RootObject
    {
        [JsonProperty("listings")]
        public List<Listings> Listings { get; set; }
    }
}
