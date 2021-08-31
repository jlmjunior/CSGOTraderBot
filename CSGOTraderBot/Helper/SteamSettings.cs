using Newtonsoft.Json;
using SteamAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTraderBot.Helper
{
    public class SteamSettings
    {
        public static SteamGuardAccount GetRemoteAccount()
        {
            var jsonAccount = Config.Get("remoteAccount", "SteamSettings");

            if (!string.IsNullOrWhiteSpace(jsonAccount))
            {
                return JsonConvert.DeserializeObject<SteamGuardAccount>(jsonAccount);
            }

            return null;
        }
    }
}
