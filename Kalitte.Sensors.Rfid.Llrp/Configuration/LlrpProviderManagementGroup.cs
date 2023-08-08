using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{

    public sealed class LlrpProviderManagementGroup
    {
        // Fields
        public const string LlrpMessageTimeout = "LLRP Message timeout";
        internal static readonly PropertyKey LlrpMessageTimeoutKey = new PropertyKey("Management", "LLRP Message timeout");
        //tansu: timeout default değiştirildi.
        internal static readonly ProviderPropertyMetadata LlrpMessageTimeoutMetadata = new ProviderPropertyMetadata(typeof(int), LlrpResources.ProviderLlrpMessageTimeoutDescription, 45000, false, false, false, 10000.0, 2147483647.0);
        public const string TcpKeepAliveTime = "TCP KeepAlive Time";
        internal static readonly PropertyKey TcpKeepAliveTimeKey = new PropertyKey("Management", "TCP KeepAlive Time");
        internal static readonly ProviderPropertyMetadata TcpKeepAliveTimeMetadata = new ProviderPropertyMetadata(typeof(int), LlrpResources.TcpKeepAliveTimeDescription, 0xea60, false, false, false, 30000.0, 2147483647.0);
    }

 

 

}
