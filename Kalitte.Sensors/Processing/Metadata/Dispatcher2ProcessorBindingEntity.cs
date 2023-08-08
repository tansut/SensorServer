using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]
    public class Dispatcher2ProcessorBindingEntity : PersistEntityBase, IEntityPropertyProvider
    {
        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }

        private Dispatcher2ProcessorBindingProperty properties;

        [DataMember]
        public Dispatcher2ProcessorBindingProperty Properties
        {
            get
            {
                return properties;
            }
            private set { properties = value; }
        }

        private Dispatcher2ProcesorBindingRuntime runtime;

        [DataMember]
        public Dispatcher2ProcesorBindingRuntime Runtime
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
        public string Dispatcher { get; set; }

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
                return string.Format("{0}-{1}", Dispatcher, Processor);
            }
        }


        public Dispatcher2ProcessorBindingEntity(string dispatcher, string processor, Dispatcher2ProcessorBindingProperty properties)
            : this(dispatcher, processor, properties, Dispatcher2ProcesorBindingRuntime.Empty)
        {

        }



        public Dispatcher2ProcessorBindingEntity(string dispatcher, string processor, 
            Dispatcher2ProcessorBindingProperty properties, Dispatcher2ProcesorBindingRuntime runtime)
            : base("")
        {
            this.Processor = processor;
            this.Dispatcher = dispatcher;
            this.Runtime = runtime;
            this.Properties = properties;
        }

        #region IEntityPropertyProvider Members

        public EntityPropertyBase GetProperties()
        {
            return properties;
        }

        #endregion
    }
}
