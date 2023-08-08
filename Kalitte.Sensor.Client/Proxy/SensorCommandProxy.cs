using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Client.Proxy
{
    public class SensorCommandProxy : ProxyBase
    {
        internal SensorCommandProxy(string host, int port, ServiceConfiguration configuration)
            : base(host, port)
        {

        }

        protected override Type GetWcfProxyType()
        {
            return typeof(SensorCommandServiceClient);
        }


        protected override string GetEndPoint()
        {
            return "CommandProcessor";
        }
    }
}
