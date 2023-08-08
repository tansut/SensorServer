using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Core.Dispatch
{
    public class VirtualDispatcherModule: VirtualModule<DispatcherEntity, DispatcherEntity, DispatcherModule>
    {
        DispatcherContext context;
        DispatcherModuleInformation moduleInformation;
        DispatcherEntity entity;

        public VirtualDispatcherModule(DispatcherEntity entity)
            : base(entity, entity)
        {
            this.entity = entity;
        }

        public override void SetModuleProperty(EntityProperty property)
        {
            try
            {
                RunHelper.Execute(ActualModuleInstance, "SetProperty", TIMEOUT, property);
            }
            catch (Exception exc)
            {
                throw new DispatcherException("SetProperty Exception.", exc);
            }
        }

        protected internal override void Start()
        {
            try
            {
                RunHelper.Execute(ActualModuleInstance, "Startup", TIMEOUT, context, Entity.Name, moduleInformation);
            }
            catch (System.Exception exc)
            {
                throw new DispatcherException("Start Exception", exc);
            }
        }

        protected internal override void Stop()
        {
            try
            {
                RunHelper.Execute(ActualModuleInstance, "Shutdown", TIMEOUT);
            }
            catch (System.Exception exc)
            {
                throw new DispatcherException("Stop Exception", exc);
            }
        }


        protected internal void Notify(string source, SensorEventBase evt)
        {
            try
            {
                RunHelper.Execute(ActualModuleInstance, "Notify", TIMEOUT, source, evt);
            }
            catch (System.Exception exc)
            {
                throw new DispatcherException("Notify Exception", exc);
            }
        }




        internal void InitContext(DispatcherContext context)
        {
            this.context = context;
            moduleInformation = new DispatcherModuleInformation(this.Relation);// new EventModuleInformation() { InstanceEntity = this.Relation, ModuleEntity = Entity, ProcessorEntity = this.ProcessorEntity };
        }

    }
}
