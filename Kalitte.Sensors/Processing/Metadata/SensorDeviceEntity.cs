using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]
    
    public class SensorDeviceEntity : PersistEntityBase, IEntityPropertyProvider
    {
        private SensorDeviceProperty properties;

        [DataMember]
        public string SensorId { get; set; }

        [DataMember] 
        public SensorDeviceProperty Properties
        {
            get
            {
                return properties;
            }
            set { properties = value; }
        }

        private SensorDeviceRuntime runtime;

        [DataMember] 
        public SensorDeviceRuntime Runtime
        {
            get
            {
                return runtime;
            }
            set { runtime = value; }
        }

        [DataMember]
        public string ProviderName { get; private set; }

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

        private Collection<Logical2SensorBindingEntity> logicalSensorBindings;

        [DataMember]
        public Collection<Logical2SensorBindingEntity> LogicalSensorBindings
        {
            get
            {
                if (logicalSensorBindings == null)
                    logicalSensorBindings = new Collection<Logical2SensorBindingEntity>();
                return logicalSensorBindings;
            }
            set { logicalSensorBindings = value; }
        }


        public SensorDeviceEntity(string name, string providerName, SensorDeviceProperty properties, SensorDeviceRuntime runtime)
            : base(name)
        {
            this.Properties = properties;
            this.Runtime = runtime;
            this.ProviderName = providerName;

        }


        public EntityPropertyBase GetProperties()
        {
            return properties;
        }


    }
}
