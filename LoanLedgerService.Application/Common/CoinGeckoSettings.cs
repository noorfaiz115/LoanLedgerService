using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoHub.Application.Common
{
    public class CoinGeckoSettings
    {

        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public int SyncIntervalMinutes { get; set; }
    }
}
