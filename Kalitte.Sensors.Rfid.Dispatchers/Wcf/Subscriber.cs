using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Xml;
using System.ServiceModel.Channels;
using System.Globalization;
using Kalitte.Sensors.Utilities;
using System.ServiceModel.Description;
using Kalitte.Sensors.Interfaces;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Dispatchers.Wcf
{
    public class Subscriber : DispatcherModule
    {
        ServiceHost host;

        public override void Startup(DispatcherContext providerContext, string providerName, DispatcherModuleInformation dispatcherInformation)
        {

            host = new ServiceHost(typeof(SubscriptionService));

            foreach (var epp in host.Description.Endpoints)

                TypesHelper.AddKnownTypes(epp);

            host.Open();

        }

        public override void SetProperty(Sensors.Configuration.EntityProperty property)
        {

        }

        public override void Shutdown()
        {
            host.Close();
        }

        public override void Notify(string source, SensorEventBase sensorEvent)
        {
            PublishService service = new PublishService();
            service.Notify(source, sensorEvent);
        }
    }
}
