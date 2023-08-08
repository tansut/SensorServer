using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Interfaces;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Core.Process
{
    public class VirtualProcessorModule : VirtualModule<EventModuleEntity, Processor2ModuleBindingEntity, SensorEventModule>
    {
        ProcessorContext context;
        EventModuleInformation moduleInformation;
        Dictionary<Type, bool> eventHandlerInfoList;
        NotificationPipe pipe;
        public PipeNullEventBehavior NullSensorEventBehavior { get; private set;}

        public VirtualProcessorModule(ProcessorEntity processorEntity, EventModuleEntity entity, Processor2ModuleBindingEntity relation)
            : base(entity, relation)
        {
            eventHandlerInfoList = new Dictionary<Type, bool>();
            this.ProcessorEntity = processorEntity;
            this.NullSensorEventBehavior = relation.Properties.InheritNullEventBehaviorBehavior ?
                processorEntity.Properties.PipeNullEventBehavior :
                relation.Properties.ModuleNullEventBehavior;
        }

        public override void SetModuleProperty(EntityProperty property)
        {
            try
            {
                RunHelper.Execute(ActualModuleInstance, "SetProperty", TIMEOUT, property);
            }
            catch (Exception exc)
            {
                throw new ProcessorException("SetModuleProperty Exception. {0}", exc, "SetModuleProperty", Relation.Name, property.Key);
            }
        }


        public override void CreateModuleInstance()
        {
            base.CreateModuleInstance();
            NonExistEventHandlerBehavior behavior = Relation.Properties.ModuleNonExistEventHandlerBehavior;
            PipeNullEventBehavior nullEventBehavior = Relation.Properties.ModuleNullEventBehavior;
            if (Relation.Properties.InheritNonExistEventHandlerBehavior)
                behavior = ProcessorEntity.Properties.ModuleNonExistEventHandlerBehavior;
            if (Relation.Properties.InheritNullEventBehaviorBehavior)
                nullEventBehavior = ProcessorEntity.Properties.PipeNullEventBehavior;
            pipe = new NotificationPipe(ActualModuleInstance, behavior, nullEventBehavior);
        }

        internal SensorEventBase Notify(string source, SensorEventBase sensorEvent, out PipeInfo usedPipe)
        {
            try
            {
                return pipe.Notify(source, sensorEvent, out usedPipe);
            }
            catch (System.Exception exc)
            {
                throw new ProcessorException("Notify Exception", exc, "Notify", Relation.Name);
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
                throw new ProcessorException("Start Exception", exc, "Start", Relation.Name);
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
                throw new ProcessorException("Stop Exception", exc, "Stop", Relation.Name);
            }
        }


        internal void InitContext(ProcessorContext context)
        {
            this.context = context;
            moduleInformation = new EventModuleInformation(this.Relation, this.Entity, this.ProcessorEntity);
        }

        public ProcessorEntity ProcessorEntity { get; private set; }

        internal bool InstanceIs(object sourceModule)
        {
            return sourceModule == ActualModuleInstance;
        }


    }
}
