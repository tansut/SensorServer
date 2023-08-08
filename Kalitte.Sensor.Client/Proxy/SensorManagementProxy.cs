using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Client.Proxy
{
    class SensorManagementProxy: ProxyBase
    {
        internal SensorManagementProxy(string host, int port)
            : base(host, port)
        {

        }

        protected override Type GetWcfProxyType()
        {
            return typeof(ManagementServiceClient);
        }

        protected override string GetEndPoint()
        {
            return "Management";
        }
    }
}
