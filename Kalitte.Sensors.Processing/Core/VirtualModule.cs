using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Events;
using System.Threading;
using System.Reflection;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Core
{
    public abstract class VirtualModule<B, R, M>: VirtualModuleBase where B: PersistEntityBase, ICanInstanceCreate 
        where R: PersistEntityBase, IEntityPropertyProvider where M: class
    {
        protected int TIMEOUT = ServerConfiguration.Current.MethodCallTimeout;


        protected class NotifyContext
        {
            public string Source { get; set; }
            public SensorEventBase Event { get; set; }
        }


        public B Entity { get; set; }
        public R Relation { get; set; }

        protected M ActualModuleInstance
        {
            get
            {
                return (M)base.actualModuleInstance;
            }
        }



        internal new R TryGetModuleNameFromInstance(object sender)
        {
            return (R)base.TryGetModuleNameFromInstance(sender);
        }


        public VirtualModule(B entity, R relation)
            : base(entity.TypeQ, relation)
        {
            this.Entity = entity;
            this.Relation = relation;
        }





    }
}
