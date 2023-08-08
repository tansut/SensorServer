using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing
{
    [Serializable]
    public sealed class EventModuleInformation
    {
        public Processor2ModuleBindingEntity InstanceEntity { get; private set; }
        public EventModuleEntity ModuleEntity { get; private set; }
        public ProcessorEntity ProcessorEntity { get; private set; }

        internal EventModuleInformation(Processor2ModuleBindingEntity entity, EventModuleEntity moduleEntity, ProcessorEntity processorEntity)
        {
            this.InstanceEntity = entity;
            this.ModuleEntity = moduleEntity;
            this.ProcessorEntity = processorEntity;
        }

        public PropertyList PropertyProfile
        {
            get
            {
                return InstanceEntity.Properties.Profile;
            }
        }

        public PropertyList ExtendedProfile
        {
            get
            {
                return InstanceEntity.Properties.Profile;
            }
        }

        public NonExistEventHandlerBehavior NonExistEventHandlerBehavior
        {
            get
            {
                return InstanceEntity.Properties.InheritNonExistEventHandlerBehavior ?
                    ProcessorEntity.Properties.ModuleNonExistEventHandlerBehavior :
                    InstanceEntity.Properties.ModuleNonExistEventHandlerBehavior;
            }
        }

        public PipeNullEventBehavior NullSensorEventHandlerBehavior
        {
            get
            {
                return InstanceEntity.Properties.InheritNullEventBehaviorBehavior ?
                    ProcessorEntity.Properties.PipeNullEventBehavior :
                    InstanceEntity.Properties.ModuleNullEventBehavior;
            }
        }
    }
}
