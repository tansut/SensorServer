using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.ServerAnalyse;
using Kalitte.Sensors.Processing.ServerAnalyse.Context;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;

namespace Kalitte.Sensors.Processing.Core.Dispatch
{
    class DispatcherManager : QueBasedEntityManager<SingleDispatcher, DispatcherEntity, VirtualDispatcher>
    {

        internal DispatcherManager(ServerManager serverManager)
            : base(serverManager)
        {
        }

        public override ProcessingItem ItemType
        {
            get { return ProcessingItem.Dispatcher; }
        }


        protected override SingleDispatcher NewSingleManagerInstanceFromEntity(DispatcherEntity entity)
        {
            return new SingleDispatcher(this, entity);
        }

        protected override void CreateUsingProvider(SingleDispatcher singleManager)
        {
            MetadataManager.CreateDispatcher(singleManager.Entity);
        }

        public override IEnumerable<DispatcherEntity> RetreiveEntitiesFromProvider()
        {
            return MetadataManager.GetDispatchers();
        }

        public override void DeleteEntityFromProvider(SingleDispatcher singleManager)
        {
            MetadataManager.UpdateDispatcher2ProcessorBindings(singleManager.Entity.Name, singleManager.Entity.ProcessorBindings.ToArray());
            MetadataManager.DeleteDispatcher(singleManager.Entity);
        }

        internal DispatcherEntity Create(string dispatcherName, string description, string type, ItemStartupType startup)
        {
            TypeParser.Validate(type);
            var properties = new DispatcherProperty(startup);
            var entity = new DispatcherEntity(dispatcherName, type, properties);
            entity.Description = description;
            var singleManager = CreateSingleManager(entity, true);
            return singleManager.CheckAndSendItem();
        }

        internal void Update(string dispatcherName, string description, string type, DispatcherProperty properties)
        {
            TypeParser.Validate(type);
            var info = ValidateAndGetItem(dispatcherName);
            info.Update(description, type, properties);
            MetadataManager.UpdateDispatcher(info.Entity);
        }

        internal Sensors.Configuration.DispatcherMetadata GetMetadata(string dispatcherName)
        {
            Dictionary<PropertyKey, DispatcherPropertyMetadata> mData = new Dictionary<PropertyKey, DispatcherPropertyMetadata>();
            return new Sensors.Configuration.DispatcherMetadata(mData);
        }

        internal IEnumerable<Dispatcher2ProcessorBindingEntity> GetProcessorBindings(string dispatcherName)
        {
            var singleManager = ValidateAndGetItem(dispatcherName);
            return singleManager.GetProcessorBindings();
        }

        internal void UpdateProcessorBindings(string dispatcherName, Dispatcher2ProcessorBindingEntity[] bindings)
        {
            var item = ValidateAndGetItem(dispatcherName);
            var copy = new List<Dispatcher2ProcessorBindingEntity>(bindings);
            item.UpdateProcessorBindings(copy);
        }

        internal void UpdateWithBindings(string dispatcherName, string description, string type, DispatcherProperty properties, Dispatcher2ProcessorBindingEntity[] processorBindings)
        {
            this.Update(dispatcherName, description, type, properties);
            this.UpdateProcessorBindings(dispatcherName, processorBindings);
        }

        internal void ChangeProcessorBindingState(string dispatcherName, string processorName, ItemState newState)
        {
            var item = ValidateAndGetItem(dispatcherName);
            item.ChangeProcessorState(processorName, newState);

        }

       

        internal void Notify(string source, Events.SensorEventBase evt, ProcessorEntity entity)
        {
            var currentDispatchers = GetCurrentEntityList();
            try
            {
                foreach (var dispatcher in currentDispatchers)
                {
                    var singleManager = TryGetItem(dispatcher.Name, false);
                    
                    if (singleManager != null)
                    {
                        var durationContext = ServerAnalyseManager.CreateContext<DurationAnalyseContext>();
                        bool sent = singleManager.SendEventToQue(source, evt, entity);
                        if (sent)
                            WatchManager.DispatcherMessageQueSend(singleManager.Entity.Name, new QueSendEventArgs(source, evt, durationContext));
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Error("Error in DispatchManager.Notify. {0}", exc);
            }

        }

        internal void SetProfile(string dispatcher, PropertyList properties)
        {
            var singleManager = ValidateAndGetItem(dispatcher);
            singleManager.SetModuleProperties(dispatcher, properties);
        }

        internal void DeleteBindingsOfProcessor(string processorName)
        {
            var dispatchers = GetCurrentEntityList();
            foreach (var item in dispatchers)
            {
                var pBindings = item.ProcessorBindings.Where(p => p.Processor == processorName).ToList();
                foreach (var binding in pBindings)
                {
                    var singleManager = ValidateAndGetItem(item.Name);
                    singleManager.RemoveProcessorBinding(binding.Name);
                    MetadataManager.UpdateDispatcher2ProcessorBindings(item.Name, item.ProcessorBindings.ToArray());
                }
            }
           
        }
    }
}
