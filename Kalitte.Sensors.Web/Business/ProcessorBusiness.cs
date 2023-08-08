using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing;

namespace Kalitte.Sensors.Web.Business
{
    public class ProcessorBusiness : EntityBusiness<ProcessorEntity>
    {
        public override ProcessorEntity GetItem(string id)
        {
            return SensorProxy.GetProcessor(id);
        }

        public void SetEventModuleProfile(string processor, string module, PropertyList properties)
        {
            SensorProxy.SetEventModuleProfile(processor, module, properties);
        }

        public override void UpdateItem(ProcessorEntity entity)
        {
            SensorProxy.UpdateProcessor(entity.Name, entity.Description, entity.Properties);
        }

        public override void DeleteItem(string id)
        {
            SensorProxy.DeleteProcessor(id);
        }

        public override void ChangeState(string id, Processing.ItemState newState)
        {
            SensorProxy.ChangeProcessorState(id, newState);
        }

        public override System.Collections.IList GetItems()
        {
            return SensorProxy.GetProcessors();
        }

        public ProcessorMetadata GetProcessorMetadata(string processorName)
        {
            return SensorProxy.GetProcessorMetadata(processorName);
        }

        public ProcessorEntity CreateItem(string processorName, string description, Processing.ItemStartupType startup)
        {
            return SensorProxy.CreateProcessor(processorName, description, startup);
        }

        public Processor2ModuleBindingEntity[] GetProcessor2ModuleBindings(string processorName)
        {
            return SensorProxy.GetProcessor2ModuleBindings(processorName);
        }

        public void UpdateProcessor2ModuleBindings(string processorName, Processor2ModuleBindingEntity[] bindings)
        {
            SensorProxy.UpdateProcessor2ModuleBindings(processorName, bindings);
        }

        public Logical2ProcessorBindingEntity[] GetProcessor2LogicalBindings(string processorName)
        {
            return SensorProxy.GetProcessor2LogicalSensorBindings(processorName);
        }

        public void UpdateProcessor2LogicalBindings(string processorName, Logical2ProcessorBindingEntity[] bindings)
        {
            SensorProxy.UpdateProcessor2LogicalSensorBindings(processorName, bindings);
        }

        public void UpdateProcessorWithBindings(ProcessorEntity entity, Processor2ModuleBindingEntity[] moduleBindings, Logical2ProcessorBindingEntity[] logicalSensorBindings)
        {
            SensorProxy.UpdateProcessorWithBindings(entity.Name, entity.Description, entity.Properties, moduleBindings, logicalSensorBindings);
        }

        public void ChangeProcessorModuleState(string processorName, string moduleName, ItemState newState)
        {
            SensorProxy.ChangeProcessorModuleState(processorName, moduleName, newState);
        }

        public void ChangeProcessorLogicalSensorBindingState(string processorName, string logicalSensorName, ItemState newState)
        {
            SensorProxy.ChangeProcessorLogicalSensorBindingState(processorName, logicalSensorName, newState);
        }
    }
}
