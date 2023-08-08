using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Web.Business
{
    public class EventModuleBusiness: EntityBusiness<EventModuleEntity>
    {
        public override EventModuleEntity GetItem(string id)
        {
            return SensorProxy.GetEventModule(id);
        }

        public override void UpdateItem(EventModuleEntity entity)
        {
            SensorProxy.UpdateEventModule(entity.Name, entity.Description, entity.TypeQ, entity.Properties);
        }

        public override void DeleteItem(string id)
        {
            SensorProxy.DeleteEventModule(id);
        }

        public override void ChangeState(string id, Processing.ItemState newState)
        {
            SensorProxy.ChangeEventModuleState(id, newState);
        }

        public override System.Collections.IList GetItems()
        {
            return SensorProxy.GetEventModules();
        }

        public EventModuleEntity CreateItem(string id, string description, string type, Processing.ItemStartupType startup)
        {
            return SensorProxy.CreateEventModule(id, description, type, startup);
        }

        public EventModuleMetadata GetMetadata(string id)
        {
            return SensorProxy.GetEventModuleMetadata(id);
        }
    }
}
