using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]

    public class SensorProviderEntity : PersistEntityBase, IEntityPropertyProvider, ICanInstanceCreate, ISupportsLog
    {
        private SensorProviderProperty properties;

        [DataMember]
        public SensorProviderProperty Properties
        {
            get
            {
                return properties;
            }
            set { properties = value; }
        }

        private SensorProviderRuntime runtime;

        [DataMember]
        public SensorProviderRuntime Runtime
        {
            get
            {
                return runtime;
            }
            set { runtime = value; }
        }

        [DataMember]
        public string TypeQ { get; set; }


        public SensorProviderEntity(string name, string typeQ, SensorProviderProperty properties, SensorProviderRuntime runtime)
            : base(name)
        {
            this.TypeQ = typeQ;
            this.Properties = properties;
            this.Runtime = runtime;
        }

        public SensorProviderEntity(string name, string typeQ, SensorProviderProperty properties)
            : this(name, typeQ, properties, SensorProviderRuntime.Empty)
        {

        }

        public EntityPropertyBase GetProperties()
        {
            return properties;
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

        #region ISupportsLog Members

        public LogLevelInformation LogLevel
        {
            get { return Properties.LogLevel; }
        }

        #endregion
    }
}
