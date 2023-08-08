using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Utilities;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable]
    
    public class LogicalSensorEntity : PersistEntityBase, ICloneable, IEntityPropertyProvider
    {
        private LogicalSensorProperty properties;

        [DataMember]
        public LogicalSensorProperty Properties
        {
            get
            {
                return properties;
            }
            private set { properties = value; }
        }

        private LogicalSensorRuntime runtime;

        [DataMember]
        public LogicalSensorRuntime Runtime
        {
            get
            {
                return runtime;
            }
            private set { runtime = value; }
        }



        public ItemState State
        {
            get
            {
                return Properties.StateInfo.State;
            }
        }

        public ItemStartupType Startup
        {
            get
            {
                return Properties.Startup;
            }
        }

        public string StateText
        {
            get
            {
                return Properties.StateInfo.StateText;
            }
        }

        public EntityPropertyBase GetProperties()
        {
            return properties;
        }
        private Collection<Logical2ProcessorBindingEntity> processorBindings;

        [DataMember]
        public Collection<Logical2ProcessorBindingEntity> ProcessorBindings
        {
            get
            {
                if (processorBindings == null)
                    processorBindings = new Collection<Logical2ProcessorBindingEntity>();
                return processorBindings;
            }
            internal set { processorBindings = value; }
        }

        public LogicalSensorEntity(string name, LogicalSensorProperty properties, LogicalSensorRuntime runtime)
            : base(name)
        {
            this.Properties = properties;
            this.Runtime = runtime;
        }

        #region ICloneable Members

        public object Clone()
        {
            var serialized = SerializationHelper.SerializeToXmlDataContract(this, false);
            return SerializationHelper.DeserializeFromXmlDataContract<LogicalSensorEntity>(serialized);
        }

        #endregion



    }
}
