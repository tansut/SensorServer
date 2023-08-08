using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Security;
using System.Security;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Commands;
using System.Threading;
using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;

namespace Kalitte.Sensors.Processing.Core.Sensor
{
    internal class SensorManager : EntityOperationManager<SingleSensor, SensorDeviceEntity>
    {
        internal void HandleDiscoveryEvent(SensorDevices.DiscoveryEventArgs e, VirtualSensor sensor, ProviderDiscoveryBehaviour discoveryBehavior)
        {
            SingleSensor singleSensor = null;
            if (discoveryBehavior.SensorMatch == DiscoverySensorMatchType.SensorName)
                singleSensor = TryGetItem(e.DeviceInfo.DeviceId);
            else if (discoveryBehavior.SensorMatch == DiscoverySensorMatchType.SensorId)
                singleSensor = GetUsingId(e.DeviceInfo.DeviceId);
            if (singleSensor == null && discoveryBehavior.CreateSensorIfNoMatch)
            {
                Logger.Warning("Cannot find a matching sensor for discovered device {0}. Automatically creating", e.DeviceInfo.DeviceId);
                CreateSensor(e.DeviceInfo.DeviceId, e.DeviceInfo.DeviceId, "Automatically created.", e.DeviceInfo.ConnectionInformation, null, ItemStartupType.Manual);
                singleSensor = ValidateAndGetItem(e.DeviceInfo.DeviceId);
            }
            if (singleSensor != null)
            {
                singleSensor.HandleDiscoveredSensor(e, sensor);
            }
        }


        public override ProcessingItem ItemType
        {
            get { return ProcessingItem.SensorDevice; }
        }

        internal SensorManager(ServerManager serverManager)
            : base(serverManager)
        {
        }

        public override IEnumerable<SensorDeviceEntity> RetreiveEntitiesFromProvider()
        {
            return MetadataManager.GetSensorDevices();
        }

        protected override SingleSensor NewSingleManagerInstanceFromEntity(SensorDeviceEntity entity)
        {
            return new SingleSensor(this, entity);
        }

        //public void SetProperties(string name, string source, PropertyProfile updatedProfile)
        //{
        //    var singleSensor = ValidateAndGetItem(name);
        //    singleSensor.SetProperties(source, updatedProfile);
        //}


        internal SensorDeviceEntity CreateSensor(string sensorName, string sensorId, string description,
            ConnectionInformation connInfo, AuthenticationInformation authInfo,
            ItemStartupType initialStatus)
        {
            string providerName = connInfo.Provider;
            var deviceProfile = ProviderManager.GetSensorPropertyProfile(providerName);
            var extendedProfile = new PropertyList();

            var providerEntity = ProviderManager.GetEntity(providerName);
            SensorDeviceProperty properties = new SensorDeviceProperty(connInfo, authInfo, deviceProfile, extendedProfile, null, initialStatus);
            SensorDeviceEntity entity = new SensorDeviceEntity(sensorName, providerEntity.Name, properties, SensorDeviceRuntime.Empty);
            entity.Description = description;
            if (string.IsNullOrWhiteSpace(entity.SensorId))
                entity.SensorId = sensorName;
            else entity.SensorId = sensorId;
            SingleSensor singleManager = CreateSingleManager(entity, true);

            return singleManager.CheckAndSendItem();
        }

        public override void DeleteEntityFromProvider(SingleSensor singleManager)
        {
            MetadataManager.DeleteSensorDevice(singleManager.Entity);
        }

        internal void Notify(SingleSensor singleManager, Events.SensorEventBase evt)
        {
            WatchManager.SensorEvent(singleManager.Entity.Name,
                new SensorEventArgs(singleManager.Entity.ProviderName, evt));
            try
            {
                LogicalManager.Notify(singleManager.Entity.Name, evt, singleManager.Entity.LogicalSensorBindings.ToArray());
            }
            catch (System.Exception exc)
            {
                Logger.LogException("Error in notifying for {0}", exc, singleManager.Entity.Name);
            }
        }

        internal void CmdResponse(SingleSensor sensorRuntimeInformation, ResponseEventArgs e)
        {

        }

        internal ResponseEventArgs ExecuteCommand(string sensorName, string source, SensorCommand command)
        {
            var sensor = ValidateAndGetItem(sensorName);
            ResponseEventArgs args = sensor.ExecuteCommand(source, command);
            return args;
        }

        internal void UpdateSensor(string sensorName, string sensorId, string description, SensorDeviceProperty properties)
        {
            var info = ValidateAndGetItem(sensorName);
            var item = GetUsingId(sensorId);
            if (item != info && item != null)
                throw new ArgumentException("Dublicate SensorId");
            info.Update(sensorId, description, properties);
            MetadataManager.UpdateSensorDevice(info.Entity);
        }

        private SingleSensor GetUsingId(string sensorId)
        {
            var items = CurrentItems.GetCopiedList();
            foreach (var item in items)
            {
                if (item.Entity.SensorId == sensorId)
                    return item;
            }
            return null;
        }

        internal Dictionary<string, PropertyList> GetSources(string sensorName)
        {
            var info = ValidateAndGetItem(sensorName);
            return info.GetSources();
        }

        internal void SetSensorProfile(string sensorName, string source, PropertyList properties)
        {
            var singleSensor = ValidateAndGetItem(sensorName);
            singleSensor.SetProfile(source, properties);
        }

        protected override void CreateUsingProvider(SingleSensor singleManager)
        {
            MetadataManager.CreateSensorDevice(singleManager.Entity);
        }

        internal IEnumerable<SensorDeviceEntity> GetSensorDevicesForProvider(string providerName)
        {
            var list = GetCurrentEntityList().Where(p => p.ProviderName == providerName).AsEnumerable();
            return list;
        }

        public IEnumerable<Logical2SensorBindingEntity> GetLogical2SensorBindings(string sensorDeviceName)
        {
            var item = ValidateAndGetItem(sensorDeviceName);
            return item.GetLogicalSensorBindings();
        }


        internal void UpdateLogical2SensorBindings(string sensorDeviceName, Logical2SensorBindingEntity[] bindings)
        {
            var item = ValidateAndGetItem(sensorDeviceName);
            item.UpdateLogicalSensorBindings(bindings);
            MetadataManager.UpdateLogical2SensorBindings(sensorDeviceName, bindings);
        }

        internal void UpdateWithBindings(string sensorName, string sensorId, string description, SensorDeviceProperty properties, Logical2SensorBindingEntity[] bindings)
        {
            this.UpdateSensor(sensorName, sensorId, description, properties);
            this.UpdateLogical2SensorBindings(sensorName, bindings);

        }

        internal void StopAllSensors(string providerName)
        {
            var list = CurrentItems.GetCopiedItems().Where(p => p.Value.Entity.ProviderName == providerName).Select(p => p.Value).ToList();
            foreach (var item in list)
                item.ChangeState(ItemState.Stopped);

        }

        internal void DeleteSensors(string providerName)
        {
            var list = CurrentItems.GetCopiedItems().Where(p => p.Value.Entity.ProviderName == providerName).Select(p => p.Value).ToList();
            foreach (var item in list)
                DeleteItem(item.Entity.Name);
        }

        internal override List<SensorDeviceEntity> GetCurrentEntityList()
        {
            return base.GetCurrentEntityListWithChecking(true);
        }


    }
}
