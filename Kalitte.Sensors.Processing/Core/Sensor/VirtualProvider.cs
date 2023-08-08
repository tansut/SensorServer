using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Configuration;
using System.Reflection;
using Kalitte.Sensors.Processing.Utilities;

using Kalitte.Sensors.Core;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Core.Sensor
{
    internal class VirtualProvider : SensorProvider
    {
        private ProviderMarshal providerMarshal;
        private ProviderEventMarshal eventMarshal;
        private SensorProviderManager manager;
        public AppDomain ProviderDomain;

        private EventHandler<NotificationEventArgs> providerNotificationEvent;
        private EventHandler<DiscoveryEventArgs> discoveryEvent;

        public ProviderMetadata GetProviderMetadata()
        {
            return providerMarshal.GetProviderMetadata();
        }


        public VirtualProvider(SensorProviderEntity entity, SensorProviderManager manager)
        {
            this.manager = manager;
            this.ProviderDomain = MarshalHelper.CreateAppDomanin("ProviderDomain");
            Type marshallerType = typeof(ProviderMarshal);

            providerMarshal = (ProviderMarshal)ProviderDomain.CreateInstanceAndUnwrap(
                marshallerType.Assembly.FullName,
                marshallerType.FullName, false, System.Reflection.BindingFlags.CreateInstance,
                null, new object[] { entity, manager.Logger }, null, null);


            this.eventMarshal = new ProviderEventMarshal(new EventHandler<DiscoveryEventArgs>(this.proxyDiscoveryEvent), new EventHandler<NotificationEventArgs>(this.proxyProviderNotificationEvent));
            this.providerMarshal.DiscoveryEvent += new EventHandler<DiscoveryEventArgs>(this.eventMarshal.proxyDiscoveryEvent);
            this.providerMarshal.ProviderNotificationEvent += new EventHandler<NotificationEventArgs>(this.eventMarshal.proxyProviderNotificationEvent);
        }

        void ProviderDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }

        void ProviderDomain_DomainUnload(object sender, EventArgs e)
        {
            
        }

        public override SensorProxy GetPhysicalSensor(Communication.ConnectionInformation connectionInformation)
        {
            return new VirtualSensor(connectionInformation, providerMarshal.GetDevice(connectionInformation));
        }

        public override void Startup(SensorProviderContext providerContext, string providerName, Sensors.Configuration.PropertyList providerInitParameters)
        {
            providerMarshal.Startup(null, providerName, providerInitParameters, ServerConfiguration.Current);
        }

        public override void SetProperty(Sensors.Configuration.EntityProperty property)
        {
            providerMarshal.SetProviderProperty(property);
        }

        public override void Shutdown()
        {
            providerMarshal.Shutdown();
            AppDomain.Unload(ProviderDomain);
        }

        //public override void StartDiscovery()
        //{
        //    providerMarshal.StartDiscovery();
        //}

        //public override void StopDiscovery()
        //{
        //    providerMarshal.StopDiscovery();
        //}

        //public override void TriggerDiscovery()
        //{
        //    providerMarshal.TriggerDiscovery();
        //}

        public override event EventHandler<DiscoveryEventArgs> DiscoveryEvent
        {
            add
            {
                this.discoveryEvent = (EventHandler<DiscoveryEventArgs>)Delegate.Combine(this.discoveryEvent, value);
            }
            remove
            {
                this.discoveryEvent = (EventHandler<DiscoveryEventArgs>)Delegate.Remove(this.discoveryEvent, value);
            }
        }

        public override event EventHandler<NotificationEventArgs> ProviderNotificationEvent
        {
            add
            {
                this.providerNotificationEvent = (EventHandler<NotificationEventArgs>)Delegate.Combine(this.providerNotificationEvent, value);
            }
            remove
            {
                this.providerNotificationEvent = (EventHandler<NotificationEventArgs>)Delegate.Remove(this.providerNotificationEvent, value);
            }
        }

        internal void proxyProviderNotificationEvent(object sender, NotificationEventArgs e)
        {
            var handler = this.providerNotificationEvent;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        internal void proxyDiscoveryEvent(object sender, DiscoveryEventArgs e)
        {
            var handler = this.discoveryEvent;
            if (handler != null)
            {
                handler(sender, e);
            }
        }


    }
}
