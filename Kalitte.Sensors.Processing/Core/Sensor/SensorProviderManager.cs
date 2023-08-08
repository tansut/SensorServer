using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Utilities;



namespace Kalitte.Sensors.Processing.Core.Sensor
{
    internal class SensorProviderManager : EntityOperationManager<SingleSensorProvider, SensorProviderEntity>
    {

        internal SensorProviderManager(ServerManager serverManager)
            : base(serverManager)
        {

        }

        internal void SetSensorProviderProfile(string providerName, PropertyList properties)
        {
            var provider = ValidateAndGetItem(providerName);

            provider.SetProfile(properties);
        }

        public override ProcessingItem ItemType
        {
            get { return ProcessingItem.SensorProvider; }
        }

        internal PropertyList GetSensorPropertyProfile(string providerName)
        {
            var singleProvider = ValidateAndGetItem(providerName);
            return singleProvider.CreateSensorProfile();
        }

        internal PropertyList GetSensorSourcePropertyProfile(string providerName)
        {
            var singleProvider = ValidateAndGetItem(providerName);
            return singleProvider.CreateSensorSourceProfile();
        }

        protected override SingleSensorProvider NewSingleManagerInstanceFromEntity(Metadata.SensorProviderEntity entity)
        {
            return new SingleSensorProvider(this, entity);
        }

        protected override void CreateUsingProvider(SingleSensorProvider singleManager)
        {

            MetadataManager.CreateSensorProvider(singleManager.Entity);
        }

        public override IEnumerable<Metadata.SensorProviderEntity> RetreiveEntitiesFromProvider()
        {
            return MetadataManager.GetSensorProviders();
        }

        internal void HandleDiscoveryEvent(SensorDevices.DiscoveryEventArgs e)
        {
            try
            {
                var provider = ValidateAndGetItem(e.DeviceInfo.ConnectionInformation.Provider);
                provider.HandleDiscoveryEvent(e);
            }
            catch (Exception exc)
            {
                Logger.LogException("Error while managing discovery for device {0} on provider {1}", exc, e.DeviceInfo.DeviceId, e.DeviceInfo.ConnectionInformation.Provider);
            }

        }



        internal VirtualSensor GetPhysicalSensor(string providerName, Communication.ConnectionInformation connectionInformation)
        {
            var singleManager = ValidateAndGetItem(providerName);
            return singleManager.CreatePhysicalSensor(connectionInformation);
        }

        internal ProviderMetadata GetMetadata(string providerName)
        {
            var item = ValidateAndGetItem(providerName);
            return item.GetMetadata();
        }

        internal Dictionary<PropertyKey, DevicePropertyMetadata> GetSensorMetadata(string providerName)
        {
            var metaData = GetMetadata(providerName);
            if (metaData != null)
                return metaData.DevicePropertyMetadata;
            else return null;
        }

        internal SensorProviderEntity Create(string name, string description, string type, ItemStartupType startup)
        {
            TypeParser.Validate(type);
            SensorProviderProperty properties = new SensorProviderProperty(startup);
            SensorProviderEntity entity = new SensorProviderEntity(name, type, properties);
            entity.Description = description;
            var singleManager = CreateSingleManager(entity, true);
            return singleManager.CheckAndSendItem();
        }

        internal void Update(string providerName, string description, string type, SensorProviderProperty properties)
        {
            TypeParser.Validate(type);
            var item = ValidateAndGetItem(providerName);
            item.Update(description, type, properties);
            MetadataManager.UpdateSensorProvider(item.Entity);
        }

        public override void DeleteEntityFromProvider(SingleSensorProvider singleManager)
        {
            MetadataManager.DeleteSensorProvider(singleManager.Entity);
        }

        protected internal override Dictionary<PropertyKey, EntityMetadata> GetDefaultMetadata(string entityName)
        {
            var metaData = GetMetadata(entityName);
            if (metaData != null)
                return SensorCommon.GetEntityMetadata<ProviderPropertyMetadata>(metaData.ProviderPropertyMetadata);
            else return null;
        }
    }
}