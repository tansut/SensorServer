using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Providers.Metadata.SqlServer
{
    partial class LogicalSensorBinding
    {
        public Logical2SensorBindingEntity Convert2FrameworkEntity(MetadadataProvider provider, LogicalSensorBinding binding)
        {
            Logical2SensorBindingProperty properties = SerializationHelper.DeserializeFromXmlDataContract<Logical2SensorBindingProperty>(this.Definition);
            Logical2SensorBindingRuntime runtime = SerializationHelper.DeserializeFromXmlDataContract<Logical2SensorBindingRuntime>(this.Runtime);
            SensorDeviceReference.Load();
            Logical2SensorBindingEntity entity = new Logical2SensorBindingEntity(binding.LogicalSensorID, 
                this.SensorDevice.Name, this.SensorSource == SQLPersistenceProvider.AllSource ? null: this.SensorSource, properties, runtime);
            entity.Description = this.Description;
            return entity;
        }

        public void LoadFromFrameworkEntity(Logical2SensorBindingEntity entity)
        {
            this.LogicalSensorID = entity.LogicalSensorName;
            this.SensorDeviceID = entity.SensorName;
            this.Definition = SerializationHelper.SerializeToXmlDataContract(entity.Properties, typeof(Logical2SensorBindingProperty), false);
            this.Runtime = SerializationHelper.SerializeToXmlDataContract(entity.Runtime, typeof(Logical2SensorBindingRuntime), false);
            this.Description = entity.Description;
            this.SensorSource = string.IsNullOrEmpty(entity.SensorSource) ? SQLPersistenceProvider.AllSource : entity.SensorSource;
        }
    }
}
