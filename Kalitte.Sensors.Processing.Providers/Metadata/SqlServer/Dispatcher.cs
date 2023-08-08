using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Providers.Metadata.SqlServer
{
    public partial class Dispatcher
    {
        public DispatcherEntity Convert2FrameworkEntity(MetadadataProvider provider)
        {
            DispatcherProperty properties = SerializationHelper.DeserializeFromXmlDataContract<DispatcherProperty>(this.Definition);
            DispatcherRuntime runtime = SerializationHelper.DeserializeFromXmlDataContract<DispatcherRuntime>(this.Runtime);
            DispatcherEntity entity = new DispatcherEntity(this.Name, this.TypeQ, properties, runtime);
            entity.Description = this.Description;
            this.EventProcessorDispatcherBinding.Load();
            foreach (var binding in this.EventProcessorDispatcherBinding)
            {
                entity.ProcessorBindings.Add(binding.Convert2FrameworkEntity(provider, entity));
            }

            return entity;
        }

        public void LoadFromFrameworkEntity(DispatcherEntity entity, bool loadReferences = true)
        {
            this.Name = entity.Name;
            this.Definition = SerializationHelper.SerializeToXmlDataContract(entity.Properties, typeof(DispatcherProperty), false);
            this.Runtime = SerializationHelper.SerializeToXmlDataContract(entity.Runtime, typeof(DispatcherRuntime), false);
            this.Description = entity.Description;
            this.TypeQ = entity.TypeQ;
            if (loadReferences)
            {
                foreach (var binding in entity.ProcessorBindings)
                {
                    var dispatcherBinding = new EventProcessorDispatcherBinding();
                    dispatcherBinding.LoadFromFrameworkEntity(binding);
                    this.EventProcessorDispatcherBinding.Add(dispatcherBinding);
                }
            }
        }
    }
}
