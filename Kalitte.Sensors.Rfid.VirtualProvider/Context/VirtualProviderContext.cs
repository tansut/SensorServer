using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Rfid.VirtualProvider.Context
{
    internal static class VirtualProviderContext
    {
        private static ILogger m_logger;

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
    }
}
