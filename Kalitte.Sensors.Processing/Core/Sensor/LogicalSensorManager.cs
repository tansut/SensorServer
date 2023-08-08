using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kalitte.Sensors.Processing.Metadata;
using System.Threading;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Processing.Core;
using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Interfaces;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.ServerAnalyse;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;


namespace Kalitte.Sensors.Processing.Core.Sensor
{
    internal class LogicalSensorManager : EntityOperationManager<SingleLogicalSensor, LogicalSensorEntity>
    {


        internal LogicalSensorManager(ServerManager serverManager)
            : base(serverManager)
        {

        }

        public override ProcessingItem ItemType
        {
            get { return ProcessingItem.LogicalSensor; }
        }


        internal void Notify(string sensorDeviceName, Events.SensorEventBase evt, Logical2SensorBindingEntity[] bindings)
        {
            try
            {
                foreach (var binding in bindings)
                {
                    var singleManager = ValidateAndGetItem(binding.LogicalSensorName, false);
                    if (singleManager.Entity.State == ItemState.Running)
                    {                     
                        singleManager.ProcessEvent(sensorDeviceName, evt, binding);
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Error("Error in LogicalSensorManager.Notify. {0}", exc);
            }
        }



        internal LogicalSensorEntity CreateLogicalSensor(string sensorName, string description, ItemStartupType startup)
        {
            var entity = new LogicalSensorEntity(sensorName, new LogicalSensorProperty(startup), LogicalSensorRuntime.Empty);
            entity.Description = description;
            CreateSingleManager(entity, true);
            return entity;
        }

        internal void Update(string logicalSensor, string description, LogicalSensorProperty properties)
        {
            var info = ValidateAndGetItem(logicalSensor);
            info.Update(description, properties);
            MetadataManager.UpdateLogicalSensor(info.Entity);
        }


        protected override SingleLogicalSensor NewSingleManagerInstanceFromEntity(LogicalSensorEntity entity)
        {
            var singleManager = new SingleLogicalSensor(this, entity);
            return singleManager;
        }

        protected override void CreateUsingProvider(SingleLogicalSensor singleManager)
        {
            MetadataManager.CreateLogicalSensor(singleManager.Entity);
        }

        public override IEnumerable<LogicalSensorEntity> RetreiveEntitiesFromProvider()
        {
            return MetadataManager.GetLogicalSensors();
        }

        public override void DeleteEntityFromProvider(SingleLogicalSensor singleManager)
        {
            MetadataManager.DeleteLogicalSensor(singleManager.Entity);
        }

        internal IEnumerable<Logical2ProcessorBindingEntity> GetBindingsOfProcessor(string processorName)
        {
            List<Logical2ProcessorBindingEntity> result = new List<Logical2ProcessorBindingEntity>();

            var singleManagers = CurrentItems.GetCopiedItems();

            foreach (var manager in singleManagers)
            {
                manager.Value.GetProcessorBinding(result, processorName);
            }
            return result;
        }



        internal void UpdateProcessorBindings(string processorName, IEnumerable<Logical2ProcessorBindingEntity> bindings)
        {
            var singleManagers = CurrentItems.GetCopiedItems();

            foreach (var manager in singleManagers)
            {
                manager.Value.UpdateProcessorBinding(processorName, bindings);
                MetadataManager.UpdateLogical2ProcessorBindings(processorName, bindings.ToArray());
            }

        }

        internal void DeleteProcessorBindings(string processorName)
        {
            UpdateProcessorBindings(processorName, new List<Logical2ProcessorBindingEntity>());
        }

        internal LogicalSensorMetadata GetMetadata()
        {
            var result = new Dictionary<PropertyKey, LogicalSensorPropertyMetadata>();

            var locationName = new LogicalSensorPropertyMetadata(typeof(string), "Dictionary<PropertyKey, LogicalSensorPropertyMetadata> dict = new Dictionary<PropertyKey, LogicalSensorPropertyMetadata>();", "", false);
            var locationAdress1 = new LogicalSensorPropertyMetadata(typeof(string), "", "", false);
            var locationAdress2 = new LogicalSensorPropertyMetadata(typeof(string), "", "", false);
            var locationcity = new LogicalSensorPropertyMetadata(typeof(string), "", "", false);
            var locationZip = new LogicalSensorPropertyMetadata(typeof(string), "", "", false);
            var locationCountry = new LogicalSensorPropertyMetadata(typeof(string), "", "", false);
            var locationX = new LogicalSensorPropertyMetadata(typeof(double), "", null, false);
            var locationY = new LogicalSensorPropertyMetadata(typeof(double), "", null, false);
            var locationZ = new LogicalSensorPropertyMetadata(typeof(double), "", null, false);
            var locationDescripton = new LogicalSensorPropertyMetadata(typeof(string), "", "", false);


            Dictionary<PropertyKey, LogicalSensorPropertyMetadata> dict = new Dictionary<PropertyKey, LogicalSensorPropertyMetadata>();
            dict.Add(MetadataKeys.LocationNameKey, locationName);
            dict.Add(MetadataKeys.LocationAdressLine1Key, locationAdress1);
            dict.Add(MetadataKeys.LocationAdressLine2Key, locationAdress2);
            dict.Add(MetadataKeys.LocationZipcodeKey, locationcity);
            dict.Add(MetadataKeys.LocationCityKey, locationZip);
            dict.Add(MetadataKeys.LocationCountryKey, locationCountry);
            dict.Add(MetadataKeys.LocationXKey, locationX);
            dict.Add(MetadataKeys.LocationYKey, locationY);
            dict.Add(MetadataKeys.LocationZKey, locationZ);
            dict.Add(MetadataKeys.LocationDescriptionKey, locationDescripton);

            LogicalSensorMetadata m1 = new LogicalSensorMetadata(dict);

            return m1;
        }

        protected internal override Dictionary<PropertyKey, EntityMetadata> GetDefaultMetadata(string entityName)
        {
            return SensorCommon.GetEntityMetadata<LogicalSensorPropertyMetadata>(GetMetadata().LogicalSensorPropertyMetadata);
        }


        internal IEnumerable<Logical2SensorBindingEntity> GetSensor2LogicalBindings(string logicalSensorName)
        {
            var sensors = SensorManager.GetCurrentEntityList();
            var result = new Collection<Logical2SensorBindingEntity>();
            foreach (var sensor in sensors)
            {
                var foundItem = sensor.LogicalSensorBindings.FirstOrDefault(p => p.LogicalSensorName == logicalSensorName);
                if (foundItem != null)
                    result.Add(foundItem);
            }
            return result;
        }
    }

}
