using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Web.Business
{
    public class DispatcherBusiness: EntityBusiness<DispatcherEntity>
    {
        public override DispatcherEntity GetItem(string id)
        {
            return SensorProxy.GetDispatcher(id);
        }

        public override void UpdateItem(DispatcherEntity entity)
        {
            SensorProxy.UpdateDispatcher(entity.Name, entity.Description, entity.TypeQ, entity.Properties);
        }

        public override void DeleteItem(string id)
        {
            SensorProxy.DeleteDispatcher(id);
        }

        public override void ChangeState(string id, Processing.ItemState newState)
        {
            SensorProxy.ChangeDispatcherState(id, newState);
        }

        public override System.Collections.IList GetItems()
        {
            return SensorProxy.GetDispatchers();
        }

        public DispatcherEntity CreateItem(string dispatcherName,
        string description,
        string type,
        ItemStartupType startup)
        {
            return SensorProxy.CreateDispatcher(dispatcherName, description, type, startup);
        }

        public Dispatcher2ProcessorBindingEntity[] GetDispatcher2ProcessorBindings(string dispatcherName)
        {
            return SensorProxy.GetDispatcher2ProcessorBindings(dispatcherName);
        }

        public void SetProfile(string dispatcher, PropertyList properties)
        {
            SensorProxy.SetDispatcherProfile(dispatcher, properties);
        }

        public void UpdateDispatcher2ProcessorBindings(string dispatcherName, Dispatcher2ProcessorBindingEntity[] bindings)
        {
            SensorProxy.UpdateDispatcher2ProcessorBindings(dispatcherName, bindings);
        }

        public DispatcherMetadata GetMetadata(string dispatchername)
        {
            return SensorProxy.GetDispatcherMetadata(dispatchername);
        }

        public void UpdateWithBindings(DispatcherEntity entity, Dispatcher2ProcessorBindingEntity[] bindings)
        {
            SensorProxy.UpdateDispatcherWithBindings(entity.Name, entity.Description, entity.TypeQ, entity.Properties, bindings);
        }

        public void ChangeDispatcherProcessorBindingState(string dispatcherName, string processorName, ItemState newState)
        {
            SensorProxy.ChangeDispatcherProcessorBindingState(dispatcherName, processorName, newState);
        }
    }
}
