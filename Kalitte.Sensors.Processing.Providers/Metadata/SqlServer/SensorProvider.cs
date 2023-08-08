using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Providers.Metadata.SqlServer
{
    public partial class SensorProvider
    {
        public SensorProviderEntity Convert2FrameworkEntity(MetadadataProvider provider)
        {
            SensorProviderProperty properties = SerializationHelper.DeserializeFromXmlDataContract<SensorProviderProperty>(this.Definition);
            SensorProviderRuntime runtime = SerializationHelper.DeserializeFromXmlDataContract<SensorProviderRuntime>(this.Runtime);
            SensorProviderEntity entity = new SensorProviderEntity(this.Name, this.TypeQ, properties, runtime);
            entity.Description = this.Description;
            return entity;
        }

        public void LoadFromFrameworkEntity(SensorProviderEntity entity)
        {
            this.Name = entity.Name;
            this.TypeQ = entity.TypeQ;
            this.Definition = SerializationHelper.SerializeToXmlDataContract(entity.Properties, typeof(SensorProviderProperty), false);
            this.Runtime = SerializationHelper.SerializeToXmlDataContract(entity.Runtime, typeof(SensorProviderRuntime), false);
            this.Description = entity.Description;
        }
    }
}
