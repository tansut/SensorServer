using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Rfid.Llrp.Properties;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpProviderDiscoveryGroup
    {
        // Fields
        public const string MatchTimeout = "Match Timeout";
        internal static readonly PropertyKey MatchTimeoutKey = new PropertyKey("Discovery", "Match Timeout");
        internal static readonly ProviderPropertyMetadata MatchTimeoutMetadata = new ProviderPropertyMetadata(typeof(int), LlrpResources.MatchTimeoutDescription, 10, false, false, false, 1.0, 2147483647.0);
    }

 

 

}
