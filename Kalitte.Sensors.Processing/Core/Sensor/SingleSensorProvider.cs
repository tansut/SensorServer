using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.SensorDevices;

using Kalitte.Sensors.Configuration;
using System.Reflection;

using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Core.Sensor
{
    internal class SingleSensorProvider : SingleManager<SensorProviderEntity>
    {
        public VirtualProvider SensorProvider { get; private set; }
        private SensorProviderManager manager;
        private SensorProviderContext providerContext;


        public override OperationManagerBase Manager
        {
            get { return manager; }
        }

        public override void Shutdown()
        {
            MetadataManager.UpdateSensorProvider(Entity);
            base.Shutdown();
        }

        protected internal override SensorProviderEntity CheckAndSendItem()
        {
            ProviderMetadata metaDataOfItem = base.GetMetadata<ProviderMetadata>(Entity.TypeQ);
            if (metaDataOfItem != null)
            {
                Dictionary<PropertyKey, EntityMetadata> metaData = GetEntityMetadata<ProviderPropertyMetadata>(metaDataOfItem.ProviderPropertyMetadata);
                SyncProfile(Entity.Properties.Profile, metaData);
            }
            return base.CheckAndSendItem();
        }

        internal PropertyList GetProviderProfile(ProviderMetadata metaData)
        {
            PropertyList profile = new PropertyList("current");
            if (metaData != null)
            {
                foreach (var key in metaData.ProviderPropertyMetadata)
                {
                    profile.Add(key.Key, key.Value.DefaultValue);
                }
            }
            return profile;
        }

        private PropertyList GetSensorProfile(ProviderMetadata metaData)
        {
            PropertyList profile = new PropertyList("current");
            if (metaData != null)
            {
                foreach (var key in metaData.DevicePropertyMetadata)
                {
                    if (key.Value.PropertyTargets == SensorPropertyRelation.Device || key.Value.PropertyTargets == SensorPropertyRelation.DeviceAndSource)
                        profile.Add(key.Key, key.Value.DefaultValue);
                }
            }
            return profile;
        }

        private PropertyList GetSensorSourceProfile(ProviderMetadata metaData)
        {
            PropertyList profile = new PropertyList("current");
            if (metaData != null)
            {
                foreach (var key in metaData.DevicePropertyMetadata)
                {
                    if (key.Value.PropertyTargets == SensorPropertyRelation.Source)
                        profile.Add(key.Key, key.Value.DefaultValue);
                }
            }
            return profile;
        }

        internal ProviderMetadata GetProviderMetadata()
        {
            ValidateSensorProviderInstance();
            return SensorProvider.GetProviderMetadata();
        }



        private void CreateSensorProviderInfo()
        {
            SensorProvider = new VirtualProvider(Entity, this.manager);
            SensorProvider.ProviderNotificationEvent += SensorProvider_ProviderNotificationEvent;
            SensorProvider.DiscoveryEvent += new EventHandler<DiscoveryEventArgs>(SensorProvider_DiscoveryEvent);
        }

        void SensorProvider_DiscoveryEvent(object sender, DiscoveryEventArgs e)
        {
            this.ServerManager.SensorProviderManager.HandleDiscoveryEvent(e);
        }

        void SensorProvider_ProviderNotificationEvent(object sender, Events.NotificationEventArgs e)
        {

        }


        public SingleSensorProvider(SensorProviderManager manager, SensorProviderEntity entity)
            : base(entity)
        {
            this.manager = manager;
            this.SensorProvider = null;
            providerContext = new SensorProviderContext(null, manager.Logger);
            //try
            //{
            //    CreateSensorProviderInfo();
            //}
            //catch (System.Exception exc)
            //{
            //    Entity.Properties.StateInfo = new ItemStateInfo(exc);
            //}
            //if (SensorProvider != null)
            //{
            //    if (entity.Properties.Profile == null)
            //    {
            //        var providerMetadata = GetProviderMetadata();
            //        entity.Properties.Profile = GetProviderProfile(providerMetadata);
            //    }
            //}
        }

        protected override ItemStateInfo Run()
        {
            if (SensorProvider == null)
                CreateSensorProviderInfo();
            SensorProvider.Startup(providerContext, Entity.Name, Entity.Properties.Profile);
            return ItemStateInfo.Running;
        }

        protected override void CleanupBeforeDeleting()
        {
            this.ServerManager.SensorManager.DeleteSensors(Entity.Name);
        }

        protected override ItemStateInfo Stop()
        {
            this.ServerManager.SensorManager.StopAllSensors(this.Entity.Name);
            if (SensorProvider != null)
            {
                SensorProvider.ProviderNotificationEvent -= SensorProvider_ProviderNotificationEvent;
                SensorProvider.DiscoveryEvent -= SensorProvider_DiscoveryEvent;
                SensorProvider.Shutdown();
                if (SensorProvider != null && SensorProvider is IDisposable)
                    ((IDisposable)SensorProvider).Dispose();
                SensorProvider = null;
            }
            return ItemStateInfo.Stopped;
        }

        internal PropertyList CreateSensorProfile()
        {
            var providerMetadata = GetMetadata();
            return GetSensorProfile(providerMetadata);
        }

        internal PropertyList CreateSensorSourceProfile()
        {
            var providerMetadata = GetMetadata();
            return GetSensorSourceProfile(providerMetadata);
        }

        internal VirtualSensor CreatePhysicalSensor(Communication.ConnectionInformation connectionInformation)
        {
            ValidateState(ItemState.Running);
            return SensorProvider.GetPhysicalSensor(connectionInformation) as VirtualSensor;
        }

        void ValidateSensorProviderInstance()
        {
            if (SensorProvider == null)
                throw new InvalidOperationException("SensorProvider instance doesnot exists. See last exception.");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (SensorProvider != null && SensorProvider is IDisposable)
                    ((IDisposable)SensorProvider).Dispose();
            }
        }



        internal void Update(string description, string type, SensorProviderProperty properties)
        {
            StopMonitoring();
            itemlock.EnterWriteLock();

            try
            {
                var oldItemState = Entity.State;

                Entity.Properties.Startup = properties.Startup;
                Entity.Description = description;
                Entity.Properties.ExtendedProfile = properties.ExtendedProfile;
                Entity.Properties.Profile = properties.Profile;
                Entity.Properties.LogLevel = properties.LogLevel;
                Entity.Properties.DiscoveryBehavior = properties.DiscoveryBehavior;
                Entity.Properties.MonitoringData = properties.MonitoringData;
                Entity.TypeQ = type;
                setProviderProperties(Entity.Properties.Profile);
            }
            finally
            {
                itemlock.ExitWriteLock();
                StartMonitoring();
            }
        }

        private void setProviderProperties(PropertyList propertyProfile)
        {
            if (SensorProvider != null)
            {
                itemlock.EnterWriteLock();
                try
                {
                    foreach (var item in propertyProfile)
                    {
                        SensorProvider.SetProperty(new EntityProperty(item.Key, item.Value));
                    }
                }
                finally
                {
                    itemlock.ExitWriteLock();
                }
            }
        }

        internal void HandleDiscoveryEvent(DiscoveryEventArgs e)
        {

            if (GetState() == ItemState.Stopped)
                Logger.Warning("Retreived discovery for sensor {0} but {1} is not running. Ignoring.", e.DeviceInfo.DeviceId, this.Entity.Name);
            else
            {
                itemlock.EnterReadLock();
                try
                {
                    var device = SensorProvider.GetPhysicalSensor(e.DeviceInfo.ConnectionInformation);
                    ServerManager.SensorManager.HandleDiscoveryEvent(e, (VirtualSensor)device, Entity.Properties.DiscoveryBehavior);
                }
                finally
                {
                    itemlock.ExitReadLock();
                }
                
            }
        }


        internal ProviderMetadata GetMetadata()
        {
            return base.GetMetadata<ProviderMetadata>(Entity.TypeQ);
        }

        internal void SetProfile(PropertyList properties)
        {
            ValidateState(ItemState.Running);
            setProviderProperties(properties);
        }
    }
}
