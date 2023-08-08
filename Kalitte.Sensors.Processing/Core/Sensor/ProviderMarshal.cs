using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Security;
using System.Reflection;
using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Processing.Logging;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Core.Sensor
{
    public class ProviderMarshal : MarshalBase
    {
        private SensorProvider provider;
        public event EventHandler<DiscoveryEventArgs> DiscoveryEvent;
        public event EventHandler<NotificationEventArgs> ProviderNotificationEvent;
        private ILogger logger = null;
        private SensorProviderEntity entity;

        public ProviderMarshal(SensorProviderEntity entity, ILogger logger)
        {
            this.entity = entity;
            Type t = Type.GetType(entity.TypeQ);
            this.logger = logger;
            provider = (SensorProvider)Activator.CreateInstance(t);
            provider.ProviderNotificationEvent += new EventHandler<NotificationEventArgs>(provider_ProviderNotificationEvent);
            provider.DiscoveryEvent += new EventHandler<DiscoveryEventArgs>(provider_DiscoveryEvent);
        }

        void provider_DiscoveryEvent(object sender, DiscoveryEventArgs e)
        {
            if (this.DiscoveryEvent != null)
                DiscoveryEvent(this, e);
        }


        void provider_ProviderNotificationEvent(object sender, Events.NotificationEventArgs e)
        {
            if (this.ProviderNotificationEvent != null)
                ProviderNotificationEvent(this, e);
        }

        public ProviderMetadata GetProviderMetadata()
        {
            MethodInfo method = provider.GetType().GetMethod("GetMetadata");
            if (method == null)
                return null;
            else
            {
                ProviderMetadata metaData = method.Invoke(method, new object[] { }) as ProviderMetadata;
                return metaData;
            }
        }


        public SensorMarshal GetDevice(ConnectionInformation connectionInformation)
        {
            SensorProxy device = this.provider.GetPhysicalSensor(connectionInformation);
            return new SensorMarshal(device, this);
        }


        internal void Startup(SensorProviderContext providerContext, string providerName, PropertyList providerInitParameters, ServerConfiguration configuration)
        {
            ServerConfiguration.Init(configuration);
            AppContext.Initialize(ServerConfiguration.GetItemLogPath(ProcessingItem.SensorProvider, providerName), providerName, ServerConfiguration.Current, entity.LogLevel.Inherit ? ServerConfiguration.Current.LogConfiguration.Level:
                entity.LogLevel.Level);
            SensorProviderContext context = new SensorProviderContext(null, AppContext.Logger);
            provider.Startup(context, providerName, providerInitParameters);
        }

        internal void SetProviderProperty(EntityProperty property)
        {
            provider.SetProperty(property);
        }

        internal void Shutdown()
        {
            if (logger is IDisposable)
                ((IDisposable)logger).Dispose();
            provider.Shutdown();
        }

        //internal void StartDiscovery()
        //{
        //    provider.StartDiscovery();
        //}

        //internal void StopDiscovery()
        //{
        //    provider.StopDiscovery();
        //}

        //internal void TriggerDiscovery()
        //{
        //    provider.TriggerDiscovery();
        //}
    }
}
