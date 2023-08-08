using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]
    public class DispatcherEntity : PersistEntityBase, IEntityPropertyProvider, ICanInstanceCreate, ISupportsLog
    {
        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }


        private DispatcherProperty properties;

        [DataMember]
        public DispatcherProperty Properties
        {
            get
            {
                return properties;
            }
            private set { properties = value; }
        }

        private DispatcherRuntime runtime;

        [DataMember]
        public DispatcherRuntime Runtime
        {
            get
            {
                return runtime;
            }
            private set { runtime = value; }
        }

        private Collection<Dispatcher2ProcessorBindingEntity> processorBindings;

        [DataMember]
        public Collection<Dispatcher2ProcessorBindingEntity> ProcessorBindings
        {
            get
            {
                if (processorBindings == null)
                    processorBindings = new Collection<Dispatcher2ProcessorBindingEntity>();
                return processorBindings;
            }
            internal set { processorBindings = value; }
        }

        [DataMember]
        public string TypeQ { get; set; }

        public DispatcherEntity(string name, string typeQ, DispatcherProperty properties, DispatcherRuntime runtime)
            : base(name)
        {
            this.Properties = properties;
            this.Runtime = runtime;
            this.TypeQ = typeQ;
        }

        public DispatcherEntity(string name, string typeQ, DispatcherProperty properties)
            : this(name, typeQ, properties, DispatcherRuntime.Empty)
        {
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


        #region IEntityPropertyProvider Members

        public EntityPropertyBase GetProperties()
        {
            return properties;
        }

        #endregion

        #region ISupportsLog Members

        public LogLevelInformation LogLevel
        {
            get { return properties.LogLevel; }
        }

        #endregion
    }


}
