using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]

    public class Logical2SensorBindingEntity : PersistEntityBase
    {
        private Logical2SensorBindingProperty properties;

        [DataMember]
        public Logical2SensorBindingProperty Properties
        {
            get
            {
                return properties;
            }
            private set { properties = value; }
        }

        private Logical2SensorBindingRuntime runtime;

        [DataMember]
        public Logical2SensorBindingRuntime Runtime
        {
            get
            {
                return runtime;
            }
            private set { runtime = value; }
        }


        [DataMember]
        public string LogicalSensorName
        {
            get;
            private set;
        }

        [DataMember]
        public string SensorName
        {
            get;
            private set;
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


        public override string Name
        {
            get
            {
                return string.Format("{0}-{1}", SensorName, LogicalSensorName);
            }
        }

        [DataMember]
        public string SensorSource { get; set; }

        public Logical2SensorBindingEntity(string logicalSensor, string sensorDevice, string source, Logical2SensorBindingProperty properties)
            : this(logicalSensor, sensorDevice, source, properties, Logical2SensorBindingRuntime.Empty)
        {
        }


        public Logical2SensorBindingEntity(string logicalSensor, string sensorDevice, string source, Logical2SensorBindingProperty properties,
            Logical2SensorBindingRuntime runtime)
            : base("")
        {
            this.LogicalSensorName = logicalSensor;
            this.SensorName = sensorDevice;
            this.SensorSource = source;
            this.Properties = properties;
            this.Runtime = runtime;
        }
    }
}
