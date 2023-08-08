using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]
    public class ProcessorEntity : PersistEntityBase, IEntityPropertyProvider, ISupportsLog
    {
        private ProcessorProperty properties;

        [DataMember]
        public ProcessorProperty Properties
        {
            get
            {
                return properties;
            }
            private set { properties = value; }
        }

        private ProcessorRuntime runtime;

        [DataMember]
        public ProcessorRuntime Runtime
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


        private Collection<Processor2ModuleBindingEntity> moduleBindings;



        [DataMember]
        public Collection<Processor2ModuleBindingEntity> ModuleBindings
        {
            get
            {
                if (moduleBindings == null)
                    moduleBindings = new Collection<Processor2ModuleBindingEntity>();
                return moduleBindings;
            }
            internal set { moduleBindings = value; }
        }

        public ProcessorEntity(string name, ProcessorProperty properties, ProcessorRuntime runtime)
            : base(name)
        {
            this.Properties = properties;
            this.Runtime = runtime;
        }


        #region ISupportsLog Members

        public LogLevelInformation LogLevel
        {
            get { return Properties.LogLevel; }
        }

        #endregion
    }



}
