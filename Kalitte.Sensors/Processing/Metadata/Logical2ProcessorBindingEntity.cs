using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]

    public class Logical2ProcessorBindingEntity : PersistEntityBase, IEntityPropertyProvider
    {
        private Logical2ProcessorBindingProperty properties;

        [DataMember]
        public Logical2ProcessorBindingProperty Properties
        {
            get
            {
                return properties;
            }
            private set { properties = value; }
        }

        private Logical2ProcessorBindingRuntime runtime;

        [DataMember]
        public Logical2ProcessorBindingRuntime Runtime
        {
            get
            {
                return runtime;
            }
            private set { runtime = value; }
        }

        [DataMember]
        public string LogicalSensorName { get; private set; }

        [DataMember]
        public string ProcessorName { get; private set; }

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

        
        public override string Name
        {
            get
            {
                return string.Format("{0}-{1}", ProcessorName, LogicalSensorName);
            }
        }

        public string StateText
        {
            get
            {
                return Properties.StateInfo.StateText;
            }
        }

        public Logical2ProcessorBindingEntity(string logicalSensorName, string processorName,
    Logical2ProcessorBindingProperty property)
            : this(logicalSensorName, processorName, property, Logical2ProcessorBindingRuntime.Empty)
        {

        }

        public Logical2ProcessorBindingEntity(string logicalSensorName, string processorName,
            Logical2ProcessorBindingProperty property, Logical2ProcessorBindingRuntime runtime)
            : base("")
        {
            this.LogicalSensorName = logicalSensorName;
            this.ProcessorName = processorName;
            this.Properties = property;
            this.Runtime = runtime;
        }

        #region IEntityPropertyProvider Members

        public EntityPropertyBase GetProperties()
        {
            return properties;
        }

        #endregion
    }
}
