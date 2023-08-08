using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Events;
using System.Reflection;
using System.Threading;

using Kalitte.Sensors.Exceptions;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.ServerAnalyse;
using Kalitte.Sensors.Processing.ServerAnalyse.Context;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;

namespace Kalitte.Sensors.Processing.Core.Process
{
    class SingleProcessor : QueBasedSingleManager<ProcessorEntity, VirtualProcess>
    {
        public ProcessorManager manager { get; private set; }

       

        protected override void ProcessMessageFromQue(string messageId, System.Messaging.Message msgFromQue, AnalyseContext context)
        {
            KeyValuePair<string, SensorEventBase> eventToProcess = (KeyValuePair<string, SensorEventBase>)msgFromQue.Body;
            QueReadEventArgs args = new QueReadEventArgs(eventToProcess.Key, eventToProcess.Value, context);
            AddEventToLastEvents(eventToProcess.Key, eventToProcess.Value);
            Manager.WatchManager.ProcessorMessageQueRead(Entity.Name, args);
            Notify(eventToProcess.Key, eventToProcess.Value);
        }

        protected override void CleanupBeforeDeleting()
        {
            base.CleanupBeforeDeleting();
            ServerManager.LogicalSensorManager.DeleteProcessorBindings(Entity.Name);
            ServerManager.DispatcherManager.DeleteBindingsOfProcessor(Entity.Name);
            Entity.ModuleBindings.Clear();
        }

        protected override string QuePrefix
        {
            get { return "Process"; }
        }

        public SingleProcessor(ProcessorManager manager, ProcessorEntity entity)
            : base(entity)
        {
            this.manager = manager;
        }

        protected override VirtualProcess CreateVirtualRunnable()
        {
            itemlock.EnterReadLock();
            try
            {
                var activeRelations = Entity.ModuleBindings.Where(p => p.Startup == ItemStartupType.Automatic).ToList();
                return new VirtualProcess(this.manager, Entity, this, activeRelations);
            }
            finally
            {
                itemlock.ExitReadLock();
            }
            
        }


        public override OperationManagerBase Manager
        {
            get { return manager; }
        }


        internal void Update(string description, ProcessorProperty properties)
        {
            StopMonitoring();
            itemlock.EnterWriteLock();
            try
            {
                Entity.Description = description;
                Entity.Properties.Startup = properties.Startup;
                Entity.Properties.Profile = properties.Profile;
                Entity.Properties.ExtendedProfile = properties.ExtendedProfile;
                Entity.Properties.MonitoringData = properties.MonitoringData;
                Entity.Properties.ModuleNonExistEventHandlerBehavior = properties.ModuleNonExistEventHandlerBehavior;
                Entity.Properties.LogLevel = properties.LogLevel;
            }
            finally
            {
                itemlock.ExitWriteLock();
                StartMonitoring();
            }
        }

        internal IEnumerable<Processor2ModuleBindingEntity> GetModuleBindings()
        {
            itemlock.EnterReadLock();
            try
            {
                foreach (var relation in Entity.ModuleBindings)
                {
                    var metaDataOfItem = this.ServerManager.EventModuleManager.GetMetadata(relation.Module);
                    if (metaDataOfItem != null)
                    {
                        Dictionary<PropertyKey, EntityMetadata> metaData = GetEntityMetadata<EventModulePropertyMetadata>(metaDataOfItem.ModulePropertyMetadata);
                        SyncProfile(relation.Properties.Profile, metaData);
                    }
                }
            }
            finally
            {
                itemlock.ExitReadLock();
            }


            return Entity.ModuleBindings.ToArray();
        }

        internal void UpdateModuleBindings(IEnumerable<Processor2ModuleBindingEntity> bindings)
        {
            ValidateState(ItemState.Stopped);
            itemlock.EnterWriteLock();
            var oldBindings = new List<Processor2ModuleBindingEntity>(Entity.ModuleBindings);
            short order = 0;
            try
            {
                this.Entity.ModuleBindings.Clear();
                foreach (var item in bindings)
                {
                    if (!oldBindings.Any(p => p.Module == item.Module))
                    {
                        item.Properties.Profile = ServerManager.EventModuleManager.GetEntity(item.Module).Properties.Profile.Clone();
                        item.Properties.ExtendedProfile = ServerManager.EventModuleManager.GetEntity(item.Module).Properties.ExtendedProfile.Clone();
                    }
                    item.ExecOrder = order++;
                    Entity.ModuleBindings.Add(item);
                }
                MetadataManager.UpdateProcessor2ModuleBindings(this.Entity.Name, Entity.ModuleBindings.ToArray());
            }
            finally
            {
                itemlock.ExitWriteLock();
            }
        }

        internal IEnumerable<Logical2ProcessorBindingEntity> GetLogicalSensorBindings()
        {
            return this.ServerManager.LogicalSensorManager.GetBindingsOfProcessor(Entity.Name);
        }

        internal void UpdateLogicalSensorBindings(IEnumerable<Logical2ProcessorBindingEntity> bindings)
        {
            ValidateState(ItemState.Stopped);
            ServerManager.LogicalSensorManager.UpdateProcessorBindings(Entity.Name, bindings);
        }

        internal void ChangeModuleState(string moduleName, ItemState newState)
        {

        }

        internal void ChangeLogicalSensorState(string logicalSensorName, ItemState newState)
        {

        }



        protected override Collection<IEntityPropertyProvider> RelatedModules
        {
            get
            {
                return new Collection<IEntityPropertyProvider>(Entity.ModuleBindings.Select(p => (IEntityPropertyProvider)p).ToArray());
            }
        }

        protected override void SetRelationStates(VirtualProcess virtualRunnable, ItemState itemState)
        {
            if (virtualRunnable != null)
            {
                itemlock.EnterWriteLock();
                try
                {
                    foreach (var relation in virtualRunnable.Relations)
                    {
                        Entity.ModuleBindings.Single(p => p.Name == relation.Name).Properties.StateInfo = new ItemStateInfo(itemState);
                    }
                }
                finally
                {
                    itemlock.ExitWriteLock();
                }

            }
            else base.SetRelationStates(virtualRunnable, itemState);

        }


        protected override void ResetRelationStates()
        {
            base.ResetRelationStates();

            foreach (var item in GetLogicalSensorBindings())
            {
                item.Properties.ResetState();
            }
        }

        public override void Shutdown()
        {
            MetadataManager.UpdateProcessor(Entity);
            MetadataManager.UpdateProcessor2ModuleBindings(Entity.Name, Entity.ModuleBindings.ToArray());            
            base.Shutdown();
        }

        protected override void ModulePropertiesUpdated(IEntityPropertyProvider module, PropertyList profile)
        {
            Processor2ModuleBindingEntity entity = Entity.ModuleBindings.SingleOrDefault(p => p.Name == module.Name);
            foreach (var item in profile)
            {
                if (entity.Properties.Profile.ContainsKey(item.Key))
                    entity.Properties.Profile[item.Key] = item.Value;
            }
        }


        internal void HandlePropertyChangeFromModule(object sender, SetPropertyEventArgs e)
        {
            //itemlock.EnterWriteLock();
            try
            {
                Processor2ModuleBindingEntity entity = Entity.ModuleBindings.SingleOrDefault(p => p.Name == e.Module);
                if (entity.Properties.Profile.ContainsKey(e.Property.Key))
                        entity.Properties.Profile[e.Property.Key] = e.Property.PropertyValue;
            }
            finally
            {
                //itemlock.ExitWriteLock();
            }
        }
    }
}
