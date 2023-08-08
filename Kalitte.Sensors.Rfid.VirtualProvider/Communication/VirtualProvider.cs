using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.PhysicalDevices;
using System.Collections.Concurrent;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Rfid.VirtualProvider.Context;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Rfid.VirtualProvider.Communication
{
    public class VirtualProvider : RfidDeviceProvider
    {
        private ConcurrentDictionary<ConnectionInformation, VirtualDeviceProxy> currentDevices;
        private string providerName;
        private ILogger logger;

        public override event EventHandler<SensorDevices.DiscoveryEventArgs> DiscoveryEvent;

        public override event EventHandler<Sensors.Events.NotificationEventArgs> ProviderNotificationEvent;


        public override SensorDevices.SensorProxy GetPhysicalSensor(Sensors.Communication.ConnectionInformation connectionInformation)
        {
            var device = currentDevices.GetOrAdd(connectionInformation, (key) =>  new VirtualDeviceProxy(key, VirtualProviderContext.Logger) );
            return device;
        }

        public override void Startup(Processing.SensorProviderContext providerContext, string providerName, Sensors.Configuration.PropertyList properties)
        {
            currentDevices = new ConcurrentDictionary<ConnectionInformation, VirtualDeviceProxy>();
            VirtualProviderContext.Logger = this.logger = providerContext.Logger;
            this.providerName = providerName;

        }

        public override void SetProperty(Sensors.Configuration.EntityProperty property)
        {

        }

        public override void Shutdown()
        {
            foreach (var device in currentDevices)
            {
                device.Value.Close();
            }
            currentDevices = null;
        }
    }
}
