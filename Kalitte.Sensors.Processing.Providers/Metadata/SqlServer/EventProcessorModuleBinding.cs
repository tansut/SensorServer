using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Metadata;


namespace Kalitte.Sensors.Processing.Providers.Metadata.SqlServer
{
    public partial class EventProcessorModuleBinding
    {
        public Processor2ModuleBindingEntity Convert2FrameworkEntity(MetadadataProvider provider, ProcessorEntity processor)
        {
            Processor2ModuleBindingProperty properties = SerializationHelper.DeserializeFromXmlDataContract<Processor2ModuleBindingProperty>(this.Definition);
            Processor2ModuleBindingRuntime runtime = SerializationHelper.DeserializeFromXmlDataContract<Processor2ModuleBindingRuntime>(this.Runtime);
            this.EventModuleReference.Load();
            Processor2ModuleBindingEntity entity = new Processor2ModuleBindingEntity(processor.Name,
                this.EventModule.Name, properties, runtime);
            entity.Description = this.Description;
            entity.ExecOrder = this.ExecOrder;
            return entity;
        }

        public void LoadFromFrameworkEntity(Processor2ModuleBindingEntity entity)
        {
            this.ProcessorID = entity.Processor;
            this.ModuleID = entity.Module;
            this.Definition = SerializationHelper.SerializeToXmlDataContract(entity.Properties, typeof(Processor2ModuleBindingProperty), false);
            this.Runtime = SerializationHelper.SerializeToXmlDataContract(entity.Runtime, typeof(Processor2ModuleBindingRuntime), false);
            this.Description = entity.Description;
            this.ExecOrder = entity.ExecOrder;
        }
    }
}
