using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Rfid.Llrp.Configuration;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Rfid.Llrp.PhysicalDevices
{
    internal static class LlrpProviderContext
    {
        // Fields
        private static long m_discoveryHeartbeat = ((long)LlrpProviderConnectionGroup.DeviceInitiatedConnectionDiscoveryHeartbeatMetadata.DefaultValue);
        private static volatile int m_llrpMessageTimeout = ((int) LlrpProviderManagementGroup.LlrpMessageTimeoutMetadata.DefaultValue);
        private static ILogger m_logger;
        private static volatile int m_tcpKeepAlive =  ((int) LlrpProviderManagementGroup.TcpKeepAliveTimeMetadata.DefaultValue);

        // Properties
        internal static long DiscoveryHeartbeat
        {
            get
            {
                return m_discoveryHeartbeat;
            }
            set
            {
                m_discoveryHeartbeat = value;
            }
        }

        internal static int LlrpMessageTimeout
        {
            get
            {
                return m_llrpMessageTimeout;
            }
            set
            {
                m_llrpMessageTimeout = value;
            }
        }

        internal static ILogger Logger
        {
            get
            {
                return m_logger;
            }
            set
            {
                m_logger = value;
            }
        }

        internal static int TcpKeepAliveTime
        {
            get
            {
                return m_tcpKeepAlive;
            }
            set
            {
                m_tcpKeepAlive = value;
            }
        }
    }




}
