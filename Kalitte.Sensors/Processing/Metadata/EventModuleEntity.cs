using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]
    public class EventModuleEntity : PersistEntityBase, IEntityPropertyProvider, ICanInstanceCreate
    {
        private EventModuleProperty properties;

        public string NameAndType
        {
            get
            {
                return string.Format("{0}, {1}", Name, TypeQ);
            }
        }

        [DataMember]
        public EventModuleProperty Properties
        {
            get
            {
                return properties;
            }
            private set { properties = value; }
        }

        private EventModuleRuntime runtime;

        [DataMember]
        public EventModuleRuntime Runtime
        {
            get
            {
                return runtime;
            }
            private set { runtime = value; }
        }


        [DataMember]
        public string TypeQ { get; set; }

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

        public EventModuleEntity(string name, string typeQ, EventModuleProperty properties): this(name, typeQ, properties, EventModuleRuntime.Empty)
        {

        }

        public EventModuleEntity(string name, string typeQ, EventModuleProperty properties, EventModuleRuntime runtime)
            : base(name)
        {
            this.Properties = properties;
            this.Runtime = runtime;
            this.TypeQ = typeQ;
        }

    }

}
