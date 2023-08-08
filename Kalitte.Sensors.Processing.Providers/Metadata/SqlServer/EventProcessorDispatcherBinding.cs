using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Providers.Metadata.SqlServer
{
    public partial class EventProcessorDispatcherBinding
    {
        
        public Dispatcher2ProcessorBindingEntity Convert2FrameworkEntity(MetadadataProvider provider, DispatcherEntity dispatcher)
        {
            Dispatcher2ProcessorBindingProperty properties = SerializationHelper.DeserializeFromXmlDataContract<Dispatcher2ProcessorBindingProperty>(this.Definition);
            Dispatcher2ProcesorBindingRuntime runtime = SerializationHelper.DeserializeFromXmlDataContract<Dispatcher2ProcesorBindingRuntime>(this.Runtime);
            EventProcessorReference.Load();
            Dispatcher2ProcessorBindingEntity entity = new Dispatcher2ProcessorBindingEntity(
                dispatcher.Name, this.EventProcessor.Name,
                properties, runtime);
            entity.Description = this.Description;
            return entity;
        }

        public void LoadFromFrameworkEntity(Dispatcher2ProcessorBindingEntity entity)
        {
            this.EventProcessorID = entity.Processor;
            this.DispatcherID = entity.Dispatcher;
            this.Definition = SerializationHelper.SerializeToXmlDataContract(entity.Properties, typeof(Dispatcher2ProcessorBindingProperty), false);
            this.Runtime = SerializationHelper.SerializeToXmlDataContract(entity.Runtime, typeof(Dispatcher2ProcesorBindingRuntime), false);
            this.Description = entity.Description;
        }
    }
}
