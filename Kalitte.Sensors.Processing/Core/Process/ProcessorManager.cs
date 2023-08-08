using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;
using System.Threading;
using Kalitte.Sensors.Processing.ServerAnalyse;
using Kalitte.Sensors.Processing.ServerAnalyse.Context;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;


namespace Kalitte.Sensors.Processing.Core.Process
{
    class ProcessorManager: QueBasedEntityManager<SingleProcessor, ProcessorEntity, VirtualProcess>
    {
        internal ProcessorManager(ServerManager serverManager)
            : base(serverManager)
        {
        }

        public override ProcessingItem ItemType
        {
            get { return ProcessingItem.Processor; }
        }

        protected override SingleProcessor NewSingleManagerInstanceFromEntity(ProcessorEntity entity)
        {
            return new SingleProcessor(this, entity);
        }

        protected override void CreateUsingProvider(SingleProcessor singleManager)
        {
            MetadataManager.CreateEventProcessor(singleManager.Entity);
        }

        public override IEnumerable<ProcessorEntity> RetreiveEntitiesFromProvider()
        {
            return MetadataManager.GetProcessors();
        }

        public override void DeleteEntityFromProvider(SingleProcessor singleManager)
        {
            MetadataManager.UpdateProcessor2ModuleBindings(singleManager.Entity.Name, singleManager.Entity.ModuleBindings.ToArray());
            MetadataManager.DeleteProcessor(singleManager.Entity);
        }

        internal void Notify(string source, Events.SensorEventBase evt, Logical2ProcessorBindingEntity[] bindings)
        {
            try
            {
                foreach (var binding in bindings)
                {
                    SingleProcessor singleManager = TryGetItem(binding.ProcessorName, false);
                    if (singleManager != null)
                    {
                        var durationContext = ServerAnalyseManager.CreateContext<DurationAnalyseContext>();
                        singleManager.SendEventToQue(source, evt);
                        WatchManager.ProcessorMessageQueSend(singleManager.Entity.Name, new QueSendEventArgs(source, evt, durationContext));
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Error("Error in ProcessorManager.Notify. {0}", exc);
            }

        }

        internal void Update(string processorName, string description, ProcessorProperty properties)
        {
            var info = ValidateAndGetItem(processorName);
            info.Update(description, properties);
            MetadataManager.UpdateProcessor(info.Entity);
        }

        internal ProcessorEntity CreateProcessor(string processorName, string description, ItemStartupType startup)
        {
            ProcessorProperty properties = new ProcessorProperty(startup);
            ProcessorEntity entity = new ProcessorEntity(processorName, properties, ProcessorRuntime.Empty);
            entity.Description = description;
            var singleManager = CreateSingleManager(entity, true);
            return singleManager.CheckAndSendItem();
        }

        internal Sensors.Configuration.ProcessorMetadata GetMetadata(string processorName)
        {
            ProcessorMetadata result = new ProcessorMetadata(new Dictionary<PropertyKey, ProcessorPropertyMetadata>());
            return result;
        }

        internal IEnumerable<Processor2ModuleBindingEntity> GetModuleBindings(string processorName)
        {
            var singleManager = ValidateAndGetItem(processorName);
            return singleManager.GetModuleBindings();
        }

        internal void UpdateModuleBindings(string processorName, Processor2ModuleBindingEntity[] bindings)
        {
            var item = ValidateAndGetItem(processorName);
            var copy = new List<Processor2ModuleBindingEntity>(bindings);
            item.UpdateModuleBindings(copy);
        }

        internal IEnumerable<Logical2ProcessorBindingEntity> GetLogicalSensorBindings(string processorName)
        {
            var singleManager = ValidateAndGetItem(processorName);
            return singleManager.GetLogicalSensorBindings();
        }

        internal void UpdateLogicalSensorBindings(string processorName, Logical2ProcessorBindingEntity[] bindings)
        {
            var item = ValidateAndGetItem(processorName);
            var copy = new List<Logical2ProcessorBindingEntity>(bindings);
            item.UpdateLogicalSensorBindings(copy);
        }

        internal void UpdateWithBindings(string processorName, string description, ProcessorProperty properties, Processor2ModuleBindingEntity[] moduleBindings, Logical2ProcessorBindingEntity[] logicalSensorBindings)
        {
            this.Update(processorName, description, properties);
            this.UpdateModuleBindings(processorName, moduleBindings);
            this.UpdateLogicalSensorBindings(processorName, logicalSensorBindings);
        }

        internal void ChangeProcessorModuleState(string processorName, string moduleName, ItemState newState)
        {
            var item = ValidateAndGetItem(processorName);
            item.ChangeModuleState(moduleName, newState);
        }

        internal void ChangeLogicalSensorBindingState(string processorName, string logicalSensorName, ItemState newState)
        {
            var item = ValidateAndGetItem(processorName);
            item.ChangeLogicalSensorState(logicalSensorName, newState);
        }

        internal void ValidateModuleReference(string moduleName)
        {
            var list = GetCurrentEntityList();
            foreach (var item in list)
            {
                var relation = item.ModuleBindings.SingleOrDefault(p => p.Module == moduleName);
                if (relation != null)
                    throw new InvalidOperationException(string.Format("Cannot update/delete event module. Reference exists in {0}", item.Name));
            }
        }

        internal void NotifyDispatcher(ProcessorEntity entity, string source, Events.SensorEventBase sensorEvent)
        {
            DispatcherManager.Notify(source, sensorEvent, entity);
        }

        internal void SetModuleProfile(string processor, string module, PropertyList properties)
        {
            var singleManager = ValidateAndGetItem(processor);
            singleManager.SetModuleProperties(module, properties);
        }

        internal void HandlePropertyChangeFromModule(object sender, SetPropertyEventArgs e)
        {
            var singleManager = ValidateAndGetItem(e.ProcessorName);
            singleManager.HandlePropertyChangeFromModule(sender, e);
        }


    }
}
