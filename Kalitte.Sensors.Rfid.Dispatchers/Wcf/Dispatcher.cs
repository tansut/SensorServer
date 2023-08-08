using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Xml;
using System.Globalization;
using Kalitte.Sensors.Utilities;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Kalitte.Sensors.Interfaces;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Dispatchers.Wcf
{
    public class Dispatcher: DispatcherModule
    {
        DispatcherProxy proxy;

        public override void Startup(DispatcherContext providerContext, string providerName, DispatcherModuleInformation dispatcherInformation)
        {

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.ReliableSession.Enabled = false;
            binding.TransactionFlow = false;
            binding.MaxReceivedMessageSize = 0x7fffffffL;
            binding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
            binding.MaxBufferSize = 0x7fffffff;
            binding.MaxBufferPoolSize = 0x7fffffffL;
            //binding.ReliableSession = new OptionalReliableSession(new ReliableSessionBindingElement(true));

            string str2 = "Events";
            string hostName = "localhost";
            int port = 8008;
            EndpointAddress address = new EndpointAddress(new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/{3}", new object[] { "net.tcp", new IdnMapping().GetAscii(hostName), port, str2 })), new SpnEndpointIdentity(""), new AddressHeader[0]);
            object obj2 = Activator.CreateInstance(typeof(DispatcherProxy), new object[] { binding, address });
            ChannelFactory factory = (ChannelFactory)obj2.GetType().GetProperty("ChannelFactory").GetValue(obj2, null);
            TypesHelper.AddKnownTypes(factory.Endpoint);

            proxy = (DispatcherProxy)obj2;

        }

        public override void SetProperty(Sensors.Configuration.EntityProperty property)
        {

        }

        public override void Shutdown()
        {
            if (proxy != null)
                proxy.Close();
        }

        public override void Notify(string source, SensorEventBase sensorEvent)
        {
            proxy.Notify(source, sensorEvent);
            //EventPublishService service = new EventPublishService();
            //service.Notify(source, sensorEvent);
        }
    }
}
