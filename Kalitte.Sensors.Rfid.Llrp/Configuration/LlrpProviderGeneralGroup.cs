using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Rfid.Llrp.Properties;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpProviderGeneralGroup
    {
        // Fields
        public const string LlrpVersion = "LLRP Version";
        internal static readonly PropertyKey LlrpVersionKey = new PropertyKey("General", "LLRP Version");
        internal static readonly ProviderPropertyMetadata LlrpVersionMetadata = new ProviderPropertyMetadata(typeof(string), LlrpResources.LlrpVersionDescription, "1.0.1", false, false, false);
        private const string Version = "1.0.1";
    }

 

 

}
