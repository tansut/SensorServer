using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Providers.Metadata.SqlServer
{
    public partial class EventModule
    {
        public EventModuleEntity Convert2FrameworkEntity(MetadadataProvider provider)
        {
            EventModuleProperty properties = SerializationHelper.DeserializeFromXmlDataContract<EventModuleProperty>(this.Definition);
            EventModuleRuntime runtime = SerializationHelper.DeserializeFromXmlDataContract<EventModuleRuntime>(this.Runtime);
            EventModuleEntity entity = new EventModuleEntity(this.Name, this.TypeQ, properties, runtime);
            entity.Description = this.Description;
            return entity;
        }

        public void LoadFromFrameworkEntity(EventModuleEntity entity)
        {
            this.Name = entity.Name;
            this.Definition = SerializationHelper.SerializeToXmlDataContract(entity.Properties, typeof(EventModuleProperty), false);
            this.Runtime = SerializationHelper.SerializeToXmlDataContract(entity.Runtime, typeof(EventModuleRuntime), false);
            this.Description = entity.Description;
            this.TypeQ = entity.TypeQ;
        }
    }

    
}
