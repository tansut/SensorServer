using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Providers.Metadata.SqlServer
{
    public partial class EventProcessorLogicalSensorBinding
    {
        
        public Logical2ProcessorBindingEntity Convert2FrameworkEntity()
        {
            Logical2ProcessorBindingProperty properties = SerializationHelper.DeserializeFromXmlDataContract<Logical2ProcessorBindingProperty>(this.Definition);
            Logical2ProcessorBindingRuntime runtime = SerializationHelper.DeserializeFromXmlDataContract<Logical2ProcessorBindingRuntime>(this.Runtime);
            LogicalSensorReference.Load();
            Logical2ProcessorBindingEntity entity = new Logical2ProcessorBindingEntity(LogicalSensor.Name,
                EventProcessorID, properties, runtime);
            entity.Description = this.Description;
            return entity;
        }

        public void LoadFromFrameworkEntity(Logical2ProcessorBindingEntity entity)
        {
            this.LogicalSensorID = entity.LogicalSensorName;
            this.EventProcessorID = entity.ProcessorName;
            this.Definition = SerializationHelper.SerializeToXmlDataContract(entity.Properties, typeof(Logical2ProcessorBindingProperty), false);
            this.Runtime = SerializationHelper.SerializeToXmlDataContract(entity.Runtime, typeof(Logical2ProcessorBindingRuntime), false);
            this.Description = entity.Description;
        }
    }
}
