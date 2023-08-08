using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Providers.Metadata.SqlServer
{
    public partial class LogicalSensor
    {
        public LogicalSensorEntity Convert2FrameworkEntity(MetadadataProvider provider)
        {
            LogicalSensorProperty properties = SerializationHelper.DeserializeFromXmlDataContract<LogicalSensorProperty>(this.Definition);
            LogicalSensorRuntime runtime = SerializationHelper.DeserializeFromXmlDataContract<LogicalSensorRuntime>(this.Runtime);
            LogicalSensorEntity entity = new LogicalSensorEntity(this.Name, properties, runtime);
            entity.Description = this.Description;
            this.EventProcessorLogicalSensorBinding.Load();
            foreach (var binding in this.EventProcessorLogicalSensorBinding)
            {
                entity.ProcessorBindings.Add(binding.Convert2FrameworkEntity());
            }

            return entity;
        }

        public void LoadFromFrameworkEntity(LogicalSensorEntity entity, bool loadReferences = true)
        {
            this.Name = entity.Name;
            this.Definition = SerializationHelper.SerializeToXmlDataContract(entity.Properties, typeof(LogicalSensorProperty), false);
            this.Runtime = SerializationHelper.SerializeToXmlDataContract(entity.Runtime, typeof(LogicalSensorRuntime), false);
            this.Description = entity.Description;
            if (loadReferences)
            {
                foreach (var binding in entity.ProcessorBindings)
                {
                    var b = new EventProcessorLogicalSensorBinding();
                    b.LoadFromFrameworkEntity(binding);
                    this.EventProcessorLogicalSensorBinding.Add(b);
                }
            }
        }
    }
}
