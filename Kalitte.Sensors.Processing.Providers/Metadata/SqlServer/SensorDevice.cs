using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Providers.Metadata.SqlServer
{
    public partial class SensorDevice
    {
        public SensorDeviceEntity Convert2FrameworkEntity(MetadadataProvider provider)
        {
            SensorDeviceProperty properties = SerializationHelper.DeserializeFromXmlDataContract<SensorDeviceProperty>(this.Definition);
            SensorDeviceRuntime runtime = SerializationHelper.DeserializeFromXmlDataContract<SensorDeviceRuntime>(this.Runtime);
            this.SensorProviderReference.Load();
            SensorDeviceEntity entity = new SensorDeviceEntity(this.Name, this.SensorProvider.Name, properties, runtime);
            entity.Description = this.Description;
            entity.SensorId = this.SensorId;
            this.LogicalSensorBinding.Load();
            foreach (var binding in this.LogicalSensorBinding)
            {
                entity.LogicalSensorBindings.Add(binding.Convert2FrameworkEntity(provider, binding));
            }
            return entity;
        }

        public void LoadFromFrameworkEntity(SensorDeviceEntity entity, bool loadReferences = true)
        {
            this.Name = entity.Name;
            this.Definition = SerializationHelper.SerializeToXmlDataContract(entity.Properties, typeof(SensorDeviceProperty), false);
            this.Runtime = SerializationHelper.SerializeToXmlDataContract(entity.Runtime, typeof(SensorDeviceRuntime), false);
            this.Description = entity.Description;
            this.SensorId = entity.SensorId;
            if (loadReferences)
            {
                this.LogicalSensorBinding.Clear();
                foreach (var binding in entity.LogicalSensorBindings)
                {
                    var sensorBinding = new LogicalSensorBinding();
                    sensorBinding.LoadFromFrameworkEntity(binding);
                    this.LogicalSensorBinding.Add(sensorBinding);
                }
            }
        }
    }
}
