using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using System.Collections;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Security;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Web.Business
{
    public class SensorBusiness : EntityBusiness<SensorDeviceEntity>
    {
        public class SensorSourceInfo
        {
            public string Name { get; private set; }
            public string Value { get; private set; }

            public SensorSourceInfo(string name, string value)
            {
                this.Name = name;
                this.Value = value;
            }
        }

        public override SensorDeviceEntity GetItem(string id)
        {
            return SensorProxy.GetSensorDevice(id);
        }

        public Kalitte.Sensors.Processing.Metadata.SensorDeviceEntity[] GetSensorDevicesForProvider(string providerName)
        {
            return SensorProxy.GetSensorDevicesForProvider(providerName);
        }

        public SensorDeviceEntity CreateItem(string sensorName, string sensorId, string description, ConnectionInformation conn, AuthenticationInformation auth, ItemStartupType startup)
        {
            return SensorProxy.CreateSensor(sensorName, sensorId, description, conn, auth, startup);
        }

        public override void UpdateItem(SensorDeviceEntity entity)
        {
            SensorProxy.UpdateSensor(entity.Name, entity.SensorId, entity.Description, entity.Properties);
        }

        public override void DeleteItem(string id)
        {
            SensorProxy.DeleteSensor(id);

        }

        public Dictionary<PropertyKey, DevicePropertyMetadata> GetMetadata(string providerName)
        {
            return SensorProxy.GetSensorMetadata(providerName);
        }



        public override IList GetItems()
        {
            return SensorProxy.GetSensorDevices();
        }

        public IEnumerable<SensorSourceInfo> GetSensorSourcesAsList(string sensorName)
        {
            var sources = GetSensorSources(sensorName);
            List<SensorSourceInfo> result = new List<SensorSourceInfo>(sources.Count + 1);

            result.Add(new SensorSourceInfo("<All Sources>", string.Empty));
            foreach (var item in sources.Keys)
            {
                result.Add(new SensorSourceInfo(item, item));
            }

            return result.AsEnumerable();
        }

        public Dictionary<string, PropertyList> GetSensorSources(string sensorName)
        {

            try
            {
                return SensorProxy.GetSensorSources(sensorName);
            }
            catch
            {
                return new Dictionary<string, PropertyList>();
            }
        }

        public Logical2SensorBindingEntity[] GetLogical2SensorBindings(string sensorName)
        {
            return SensorProxy.GetLogical2SensorBindings(sensorName);
        }

        public void UpdateLogical2SensorBindings(string sensorName, Logical2SensorBindingEntity[] bindings)
        {
            SensorProxy.UpdateLogical2SensorBindings(sensorName, bindings);
        }


        public override void ChangeState(string id, ItemState newState)
        {
            SensorProxy.ChangeSensorState(id, newState);
        }

        public void UpdateSensorWithBindings(SensorDeviceEntity entity, Logical2SensorBindingEntity[] bindings)
        {
            SensorProxy.UpdateSensorWithBindings(entity.Name, entity.SensorId, entity.Description, entity.Properties, bindings);
        }

        public void SetSensorProfile(string sensorName, Dictionary<string, PropertyList> list)
        {
            foreach (var item in list)
            {
                if (sensorName == item.Key)
                    SensorProxy.SetSensorProfile(sensorName, string.Empty, item.Value);
                else SensorProxy.SetSensorProfile(sensorName, item.Key, item.Value);
            }

        }
    }
}
