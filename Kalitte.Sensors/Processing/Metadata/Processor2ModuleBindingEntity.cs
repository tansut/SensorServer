using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]
    
    public class Processor2ModuleBindingEntity : PersistEntityBase, IEntityPropertyProvider
    {
        private Processor2ModuleBindingProperty properties;

        [DataMember]
        public int ExecOrder { get; set; }

        [DataMember]
        public Processor2ModuleBindingProperty Properties
        {
            get
            {
                return properties;
            }
            private set { properties = value; }
        }

        private Processor2ModuleBindingRuntime runtime;

        [DataMember]
        public Processor2ModuleBindingRuntime Runtime
        {
            get
            {
                return runtime;
            }
            private set { runtime = value; }
        }

        [DataMember]
        public string Processor { get; set; }

        [DataMember]
        public string Module { get; set; }

        public Processor2ModuleBindingEntity(string processor, string module,
        Processor2ModuleBindingProperty properties)
            : this(processor, module, properties, Processor2ModuleBindingRuntime.Empty)
        {

        }

        public override string Name
        {
            get
            {
                return string.Format("{0}-{1}-{2}", Processor, Module, ExecOrder);
            }
        }

        public Processor2ModuleBindingEntity(string processor, string module,
            Processor2ModuleBindingProperty properties, Processor2ModuleBindingRuntime runtime)
            : base("")
        {
            this.Processor = processor;
            this.Module = module;
            this.Runtime = runtime;
            this.Properties = properties;
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
    }
}
