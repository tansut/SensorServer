using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Core.Dispatch
{
    class SingleDispatcher : QueBasedSingleManager<DispatcherEntity, VirtualDispatcher>
    {
        public DispatcherManager manager { get; set; }

        public SingleDispatcher(DispatcherManager manager, DispatcherEntity entity)
            : base(entity)
        {
            this.manager = manager;
        }

        public override OperationManagerBase Manager
        {
            get { return this.manager; }
        }


        protected override ItemStateInfo Run()
        {
            var result = base.Run();
            if (result.State == ItemState.Running)
            {
                foreach (var item in Entity.ProcessorBindings)
                {
                    if (item.Startup == ItemStartupType.Automatic)
                        item.Properties.StateInfo = ItemStateInfo.Running;
                    else item.Properties.StateInfo = ItemStateInfo.Stopped;
                }
            }
            return result;
        }

        protected internal override DispatcherEntity CheckAndSendItem()
        {
            DispatcherMetadata metaDataOfItem = base.GetMetadata<DispatcherMetadata>(Entity.TypeQ);
            if (metaDataOfItem != null)
            {
                Dictionary<PropertyKey, EntityMetadata> metaData = GetEntityMetadata<DispatcherPropertyMetadata>(metaDataOfItem.DispatcherPropertyMetadata);
                SyncProfile(Entity.Properties.Profile, metaData);
            }
            return base.CheckAndSendItem();
        }



        protected override ItemStateInfo Stop()
        {
            var result = base.Stop();

            if (result.State == ItemState.Running)
            {
                foreach (var item in Entity.ProcessorBindings)
                {
                    if (item.Startup == ItemStartupType.Automatic)
                        item.Properties.StateInfo = ItemStateInfo.Running;
                    else item.Properties.StateInfo = ItemStateInfo.Stopped;
                }
            }

            return result;
        }

        internal void Update(string description, string type, DispatcherProperty properties)
        {
            StopMonitoring();
            itemlock.EnterWriteLock();
            try
            {

                Entity.Description = description;
                Entity.Properties.Startup = properties.Startup;
                Entity.TypeQ = type;
                Entity.Properties.Profile = properties.Profile;
                Entity.Properties.ExtendedProfile = properties.ExtendedProfile;
                Entity.Properties.MonitoringData = properties.MonitoringData;
            }
            finally
            {                
                itemlock.ExitWriteLock();
                StartMonitoring();
            }
        }

        internal void UpdateProcessorBindings(List<Dispatcher2ProcessorBindingEntity> bindings)
        {
            ValidateState(ItemState.Stopped);
            itemlock.EnterWriteLock();
            try
            {
                this.Entity.ProcessorBindings.Clear();
                foreach (var item in bindings)
                {
                    Entity.ProcessorBindings.Add(item);
                }
                MetadataManager.UpdateDispatcher2ProcessorBindings(this.Entity.Name, Entity.ProcessorBindings.ToArray());
            }
            finally
            {
                itemlock.ExitWriteLock();
            }
        }

        internal IEnumerable<Dispatcher2ProcessorBindingEntity> GetProcessorBindings()
        {
            return Entity.ProcessorBindings.ToArray();

        }

        internal void ChangeProcessorState(string processorName, ItemState newState)
        {

        }

        public override void Shutdown()
        {
            MetadataManager.UpdateDispatcher(Entity);
            MetadataManager.UpdateDispatcher2ProcessorBindings(Entity.Name, Entity.ProcessorBindings.ToArray());
            base.Shutdown();
        }

        protected override void ProcessMessageFromQue(string messageId, System.Messaging.Message msgFromQue, ServerAnalyse.Context.AnalyseContext context)
        {
            KeyValuePair<string, SensorEventBase> eventToProcess = (KeyValuePair<string, SensorEventBase>)msgFromQue.Body;
            QueReadEventArgs args = new QueReadEventArgs(eventToProcess.Key, eventToProcess.Value, context);
            AddEventToLastEvents(eventToProcess.Key, eventToProcess.Value);
            Manager.WatchManager.DispatcherMessageQueRead(Entity.Name, args);
            Notify(eventToProcess.Key, eventToProcess.Value);
        }

        protected override string QuePrefix
        {
            get { return "Dispatcher"; }
        }

        protected override VirtualDispatcher CreateVirtualRunnable()
        {
            VirtualDispatcher dispatcher = new VirtualDispatcher(manager, Entity, this);
            return dispatcher;
        }

        protected override void ModulePropertiesUpdated(IEntityPropertyProvider module, PropertyList profile)
        {
            foreach (var item in Entity.Properties.Profile)
            {
                if (Entity.Properties.Profile.ContainsKey(item.Key))
                    Entity.Properties.Profile[item.Key] = item.Value;
            }
        }

        protected override void CleanupBeforeDeleting()
        {
            base.CleanupBeforeDeleting();
            Entity.ProcessorBindings.Clear();
        }

        internal bool SendEventToQue(string source, Events.SensorEventBase evt, ProcessorEntity entity)
        {
            var binding = Entity.ProcessorBindings.SingleOrDefault(p => p.Processor == entity.Name);
            if (binding != null && binding.State == ItemState.Running)
                return base.SendEventToQue(source, evt);
            return false;
        }




        internal void HandlePropertyChangeFromModule(object sender, SetPropertyEventArgs e)
        {
            //itemlock.EnterWriteLock();
            try
            {

                if (Entity.Properties.Profile.ContainsKey(e.Property.Key))
                    Entity.Properties.Profile[e.Property.Key] = e.Property.PropertyValue;
            }
            finally
            {
                //itemlock.ExitWriteLock();
            }
        }

        internal void RemoveProcessorBinding(string bindingName)
        {
            itemlock.EnterWriteLock();
            try
            {
                var binding = Entity.ProcessorBindings.SingleOrDefault(p => p.Name == bindingName);
                Entity.ProcessorBindings.Remove(binding);
            }
            finally
            {
                itemlock.ExitWriteLock();
            }
        }
    }
}
