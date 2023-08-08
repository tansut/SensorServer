using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpProviderConnectionGroup
    {
        // Fields
        public const string DeviceInitiatedConnectionDiscoveryHeartbeat = "Device initiated connection Discovery Heartbeat";
        internal static readonly PropertyKey DeviceInitiatedConnectionDiscoveryHeartbeatKey = new PropertyKey("Connection", "Device initiated connection Discovery Heartbeat");
        internal static readonly ProviderPropertyMetadata DeviceInitiatedConnectionDiscoveryHeartbeatMetadata = new ProviderPropertyMetadata(typeof(long), LlrpResources.DeviceInitiatedConnectionDiscoveryHeartbeatDescription, 60L, true, false, false, 1.0, 2147483647.0);
        public const string Port = "Port";
        internal static readonly PropertyKey PortKey = new PropertyKey("Connection", "Port");
        internal static readonly ProviderPropertyMetadata PortMetadata = new ProviderPropertyMetadata(typeof(int), LlrpResources.ProviderListeningPortDescription, 0x13dc, true, false, false, 1.0, 65535.0);
        public const string ListenForIncomingConnections = "Listen for incoming connections";
        internal static readonly PropertyKey ListenForIncomingConnectionsKey = new PropertyKey("Connection", ListenForIncomingConnections);
        internal static readonly ProviderPropertyMetadata ListenForIncomingConnectionsMetadata = new ProviderPropertyMetadata(typeof(bool), "Set true to listen incoming connections.", true, true, false, false);
    }
}
