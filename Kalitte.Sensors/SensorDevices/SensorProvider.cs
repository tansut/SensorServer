using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Processing;



namespace Kalitte.Sensors.SensorDevices
{
    public abstract class SensorProvider
    {

        public abstract event EventHandler<DiscoveryEventArgs> DiscoveryEvent;
        public abstract event EventHandler<NotificationEventArgs> ProviderNotificationEvent;


        protected SensorProvider()
        {
        }

        public abstract SensorProxy GetPhysicalSensor(ConnectionInformation connectionInformation);
        public abstract void Startup(SensorProviderContext providerContext, string providerName, PropertyList properties);
        public abstract void SetProperty(EntityProperty property);
        public abstract void Shutdown();

        //public abstract void StartDiscovery();
        //public abstract void StopDiscovery();
        //public abstract void TriggerDiscovery();
    }





}
