using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Core.Process
{
    class EventModuleManager: EntityOperationManager<SingleEventModule, EventModuleEntity>
    {

        public override ProcessingItem ItemType
        {
            get { return ProcessingItem.EventModule; }
        }
        protected override SingleEventModule NewSingleManagerInstanceFromEntity(EventModuleEntity entity)
        {
            return new SingleEventModule(this, entity);
        }

        protected override void CreateUsingProvider(SingleEventModule singleManager)
        {
            MetadataManager.CreateEventModule(singleManager.Entity);
        }

        public override IEnumerable<EventModuleEntity> RetreiveEntitiesFromProvider()
        {
            return MetadataManager.GetEventModules();
        }

        public override void DeleteEntityFromProvider(SingleEventModule singleManager)
        {
            MetadataManager.DeleteEventModule(singleManager.Entity);
        }

        public EventModuleManager(ServerManager serverManager): base(serverManager)
        {

        }

        public override void DeleteItem(string itemName)
        {
            ValidateAndGetItem(itemName);
            this.ProcessManager.ValidateModuleReference(itemName);
            base.DeleteItem(itemName);
        }

        internal EventModuleEntity CreateEventModule(string eventModuleName, string description, string type, ItemStartupType startup)
        {
            TypeParser.Validate(type);
            var properties = new EventModuleProperty(startup);
            var entity = new EventModuleEntity(eventModuleName, type, properties, EventModuleRuntime.Empty);
            entity.Description = description;
            var singleManager = CreateSingleManager(entity, true);
            return singleManager.CheckAndSendItem();
        }

        internal void Update(string eventModuleName, string description, string type, EventModuleProperty properties)
        {
            TypeParser.Validate(type);
            var info = ValidateAndGetItem(eventModuleName);
            info.Update(description, type, properties);
            MetadataManager.UpdateEventModule(info.Entity);
        }

        internal EventModuleMetadata GetMetadata(string eventModuleName)
        {
            var item = ValidateAndGetItem(eventModuleName);
            return item.GetMetadata();
        }

        protected internal override Dictionary<PropertyKey, EntityMetadata> GetDefaultMetadata(string entityName)
        {
            var metaData = GetMetadata(entityName);
            if (metaData != null)
                return SensorCommon.GetEntityMetadata<EventModulePropertyMetadata>(metaData.ModulePropertyMetadata);
            else return null;
        }

        internal List<EventModuleEntity> GetModulesOfRelation(List<Processor2ModuleBindingEntity> source)
        {
            List<EventModuleEntity> result = new List<EventModuleEntity>();
            var items = GetCurrentEntityList();
            foreach (var item in items)
            {
                bool inList = source.Any(p => p.Module == item.Name);
                if (inList)
                    result.Add(item);
            }
            return result;
        }
    }
}
