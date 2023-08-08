using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Providers.Metadata.SqlServer
{
    public partial class EventProcessor
    {
        public ProcessorEntity Convert2FrameworkEntity(MetadadataProvider provider)
        {
            ProcessorProperty properties = SerializationHelper.DeserializeFromXmlDataContract<ProcessorProperty>(this.Definition);
            ProcessorRuntime runtime = SerializationHelper.DeserializeFromXmlDataContract<ProcessorRuntime>(this.Runtime);
            ProcessorEntity entity = new ProcessorEntity(this.Name, properties, runtime);
            entity.Description = this.Description;
            this.EventProcessorLogicalSensorBinding.Load();
            //foreach (var binding in this.EventProcessorLogicalSensorBinding)
            //{
            //    entity.LogicalSensorBindings.Add(binding.Convert2FrameworkEntity(provider, entity));
            //}
            this.EventProcessorModuleBinding.Load();
            foreach (var binding in this.EventProcessorModuleBinding.OrderBy(p=>p.ExecOrder))
            {
                entity.ModuleBindings.Add(binding.Convert2FrameworkEntity(provider, entity));
            }
            //foreach (var binding in this.EventProcessorDispatcherBinding)
            //{
            //    entity.DispatcherBindings.Add(binding.Convert2FrameworkEntity(provider, entity));
            //}
            return entity;
        }

        public void LoadFromFrameworkEntity(ProcessorEntity entity, bool loadReferences = true)
        {
            this.Name = entity.Name;
            this.Definition = SerializationHelper.SerializeToXmlDataContract(entity.Properties, typeof(ProcessorProperty), false);
            this.Runtime = SerializationHelper.SerializeToXmlDataContract(entity.Runtime, typeof(ProcessorRuntime), false);
            this.Description = entity.Description;

            if (loadReferences)
            {
                this.EventProcessorDispatcherBinding.Clear();
                this.EventProcessorLogicalSensorBinding.Clear();
                this.EventProcessorModuleBinding.Clear();

                foreach (var binding in entity.ModuleBindings)
                {
                    var moduleBinding = new EventProcessorModuleBinding();
                    moduleBinding.LoadFromFrameworkEntity(binding);
                    this.EventProcessorModuleBinding.Add(moduleBinding);
                }
            }



        }
    }
}
