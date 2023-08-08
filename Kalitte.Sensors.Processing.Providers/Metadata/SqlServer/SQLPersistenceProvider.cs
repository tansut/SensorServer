using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Metadata;
using System.Data.EntityClient;
using System.Collections.ObjectModel;

namespace Kalitte.Sensors.Processing.Providers.Metadata.SqlServer
{
    public class SQLPersistenceProvider : Kalitte.Sensors.Processing.Metadata.MetadadataProvider
    {
        internal const string AllSource = "ALL";

        private string buildConnectionString()
        {
            string providerName = "System.Data.SqlClient";
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();

            entityBuilder.Provider = providerName;

            entityBuilder.ProviderConnectionString = ConnectionString;

            entityBuilder.Metadata = @"res://*/Metadata.SqlServer.KalitteSensorServerEntities.csdl|
                            res://*/Metadata.SqlServer.KalitteSensorServerEntities.ssdl|
                            res://*/Metadata.SqlServer.KalitteSensorServerEntities.msl";
            return entityBuilder.ToString();
        }

        private KalitteSensorServerEntities GetDataContext()
        {
            KalitteSensorServerEntities dataContext = new KalitteSensorServerEntities(buildConnectionString());
            return dataContext;
        }

        public override void CreateSensorProvider(SensorProviderEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                SensorProvider provider = new SensorProvider();
                provider.LoadFromFrameworkEntity(entity);
                context.AddToSensorProvider(provider);
                context.SaveChanges();
            }
        }

        public override IEnumerable<SensorProviderEntity> GetSensorProviders()
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                List<SensorProvider> providers = context.SensorProvider.Select(p => p).ToList();
                List<SensorProviderEntity> result = providers.Select(p => p.Convert2FrameworkEntity(this)).ToList();
                return result;
            }
        }

        public override SensorProviderEntity GetSensorProvider(string name)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                SensorProvider provider = context.SensorProvider.SingleOrDefault(p => p.Name == name);
                if (provider == null)
                    return null;
                else return provider.Convert2FrameworkEntity(this);
            }
        }

        public override void CreateSensorDevice(SensorDeviceEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                SensorDevice device = new SensorDevice();
                Collection<KeyValuePair<string, object>> coll = new Collection<KeyValuePair<string,object>>();
                coll.Add(new KeyValuePair<string,object>("Name", entity.ProviderName));
                device.SensorProviderReference.EntityKey = new System.Data.EntityKey("KalitteSensorServerEntities.SensorProvider", coll);

                device.LoadFromFrameworkEntity(entity);
                context.AddToSensorDevice(device);
                context.SaveChanges();
            }
        }

        public override IEnumerable<SensorDeviceEntity> GetSensorDevices()
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                List<SensorDevice> providers = context.SensorDevice.Select(p => p).ToList();
                List<SensorDeviceEntity> result = providers.Select(p => p.Convert2FrameworkEntity(this)).ToList();
                return result;
            }
        }

        public override IEnumerable<LogicalSensorEntity> GetLogicalSensors()
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                List<LogicalSensor> providers = context.LogicalSensor.Select(p => p).ToList();
                List<LogicalSensorEntity> result = providers.Select(p => p.Convert2FrameworkEntity(this)).ToList();
                return result;
            }
        }

        public override void CreateLogicalSensor(LogicalSensorEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                LogicalSensor device = new LogicalSensor();
                device.LoadFromFrameworkEntity(entity);
                context.AddToLogicalSensor(device);
                context.SaveChanges();
            }
        }


        public override SensorDeviceEntity GetSensorDevice(string name)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                SensorDevice sensor = context.SensorDevice.SingleOrDefault(p => p.Name == name);
                if (sensor != null)
                {
                    return sensor.Convert2FrameworkEntity(this);
                }
                else return null;
            }
        }

        public override LogicalSensorEntity GetLogicalSensor(string name)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                LogicalSensor sensor = context.LogicalSensor.SingleOrDefault(p => p.Name == name);
                if (sensor != null)
                {
                    return sensor.Convert2FrameworkEntity(this);
                }
                else return null;
            }
        }

        public override IEnumerable<LogicalSensorEntity> GetLogicalSensors(string name)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                List<LogicalSensor> providers = context.LogicalSensor.Select(p => p).ToList();
                List<LogicalSensorEntity> result = providers.Select(p => p.Convert2FrameworkEntity(this)).ToList();
                return result;
            }
        }

        public override void CreateProcessor(ProcessorEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                EventProcessor processor = new EventProcessor();
                processor.LoadFromFrameworkEntity(entity);
                context.AddToEventProcessor(processor);
                context.SaveChanges();
            }
        }

        public override ProcessorEntity GetProcessor(string name)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                EventProcessor processor = context.EventProcessor.SingleOrDefault(p => p.Name == name);
                if (processor != null)
                {
                    return processor.Convert2FrameworkEntity(this);
                }
                else return null;
            }
        }

        public override IEnumerable<ProcessorEntity> GetProcessors()
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                List<EventProcessor> processors = context.EventProcessor.Select(p => p).ToList();
                List<ProcessorEntity> result = processors.Select(p => p.Convert2FrameworkEntity(this)).ToList();
                return result;
            }
        }

        public override IEnumerable<EventModuleEntity> GetEventModules()
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                List<EventModule> processors = context.EventModule.Select(p => p).ToList();
                List<EventModuleEntity> result = processors.Select(p => p.Convert2FrameworkEntity(this)).ToList();
                return result;
            }
        }

        public override EventModuleEntity GetEventModule(string name)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                EventModule processor = context.EventModule.SingleOrDefault(p => p.Name == name);
                if (processor != null)
                {
                    return processor.Convert2FrameworkEntity(this);
                }
                else return null;
            }
        }

        public override void CreateEventModule(EventModuleEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                EventModule processor = new EventModule();
                processor.LoadFromFrameworkEntity(entity);
                context.AddToEventModule(processor);
                context.SaveChanges();
            }
        }

        public override IEnumerable<EventModuleEntity> GetEventModules(string processorName)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                List<EventProcessorModuleBinding> processors = context.EventProcessorModuleBinding.Where(p => p.ProcessorID == processorName).ToList();
                List<EventModuleEntity> result = new List<EventModuleEntity>();

                foreach (var binding in processors)
                {
                    binding.EventModuleReference.Load();
                    result.Add(binding.EventModule.Convert2FrameworkEntity(this));
                }
                return result;
            }
        }

        public override DispatcherEntity GetDispatcher(string name)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                Dispatcher processor = context.Dispatcher.SingleOrDefault(p => p.Name == name);
                if (processor != null)
                {
                    return processor.Convert2FrameworkEntity(this);
                }
                else return null;
            }
        }

        public override void CreateDispatcher(DispatcherEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                Dispatcher processor = new Dispatcher();
                processor.LoadFromFrameworkEntity(entity);
                context.AddToDispatcher(processor);
                context.SaveChanges();
            }
        }

        public override IEnumerable<DispatcherEntity> GetDispatchers()
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                List<Dispatcher> processors = context.Dispatcher.Select(p => p).ToList();
                List<DispatcherEntity> result = processors.Select(p => p.Convert2FrameworkEntity(this)).ToList();
                return result;
            }
        }

        public override IEnumerable<DispatcherEntity> GetDispatchers(string processorName)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                List<EventProcessorDispatcherBinding> processors = context.EventProcessorDispatcherBinding.Where(p => p.EventProcessorID == processorName).ToList();
                List<DispatcherEntity> result = new List<DispatcherEntity>();
                foreach (var binding in processors)
                    result.Add(binding.Dispatcher.Convert2FrameworkEntity(this));
                return result;
            }
        }

        public override void DeleteSensorDevice(Processing.Metadata.SensorDeviceEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                SensorDevice sensor = context.SensorDevice.SingleOrDefault(p => p.Name == entity.Name);
                if (sensor != null)
                {
                    context.DeleteObject(sensor);
                    context.SaveChanges();
                }

            }
        }

        public override void UpdateSensorDevice(SensorDeviceEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                SensorDevice sensor = context.SensorDevice.SingleOrDefault(p => p.Name == entity.Name);

                if (sensor != null)
                {
                    sensor.LoadFromFrameworkEntity(entity, false);
                    context.SaveChanges();
                }
            }
        }

        public override void UpdateLogical2SensorBindings(string sensorName, Processing.Metadata.Logical2SensorBindingEntity[] bindings)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var currentBindings = context.LogicalSensorBinding.Where(p => p.SensorDeviceID == sensorName).ToList();
                foreach (var currentBinding in currentBindings)
                    context.LogicalSensorBinding.DeleteObject(currentBinding);
                foreach (var newBinding in bindings)
                {
                    LogicalSensorBinding newItem = new LogicalSensorBinding();
                    newItem.LoadFromFrameworkEntity(newBinding);
                    context.LogicalSensorBinding.AddObject(newItem);
                    
                }
                context.SaveChanges();
            }
        }

        public override void DeleteLogicalSensor(Processing.Metadata.LogicalSensorEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var item = context.LogicalSensor.SingleOrDefault(p => p.Name == entity.Name);
                if (item != null)
                {
                    context.DeleteObject(item);
                    context.SaveChanges();
                }
            }
        }

        public override void DeleteProcessor(ProcessorEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var item = context.EventProcessor.SingleOrDefault(p => p.Name == entity.Name);
                if (item != null)
                {
                    context.DeleteObject(item);
                    context.SaveChanges();
                }
            }
        }

        public override void UpdateLogicalSensor(Processing.Metadata.LogicalSensorEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var item = context.LogicalSensor.SingleOrDefault(p => p.Name == entity.Name);

                if (item != null)
                {
                    item.LoadFromFrameworkEntity(entity, false);
                    context.SaveChanges();
                }
            }
        }

        public override void UpdateProcessor(Processing.Metadata.ProcessorEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var item = context.EventProcessor.SingleOrDefault(p => p.Name == entity.Name);

                if (item != null)
                {
                    item.LoadFromFrameworkEntity(entity, false);
                    context.SaveChanges();
                }
            }
        }

        public override void UpdateEventModule(Processing.Metadata.EventModuleEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var item = context.EventModule.SingleOrDefault(p => p.Name == entity.Name);

                if (item != null)
                {
                    item.LoadFromFrameworkEntity(entity);
                    context.SaveChanges();
                }
            }
        }

        public override void UpdateProcessor2ModuleBindings(string processorName, Processing.Metadata.Processor2ModuleBindingEntity[] bindings)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var currentBindings = context.EventProcessorModuleBinding.Where(p => p.ProcessorID == processorName).ToList();
                foreach (var currentBinding in currentBindings)
                    context.EventProcessorModuleBinding.DeleteObject(currentBinding);
                foreach (var newBinding in bindings)
                {
                    var newItem = new EventProcessorModuleBinding();
                    newItem.LoadFromFrameworkEntity(newBinding);
                    context.EventProcessorModuleBinding.AddObject(newItem);
                    
                }
                context.SaveChanges();
            }
        }

        public override void UpdateLogical2ProcessorBindings(string processorName, Logical2ProcessorBindingEntity[] bindings)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var currentBindings = context.EventProcessorLogicalSensorBinding.Where(p => p.EventProcessorID == processorName).ToList();
                foreach (var currentBinding in currentBindings)
                    context.EventProcessorLogicalSensorBinding.DeleteObject(currentBinding);
                foreach (var newBinding in bindings)
                {
                    var newItem = new EventProcessorLogicalSensorBinding();
                    newItem.LoadFromFrameworkEntity(newBinding);
                    context.EventProcessorLogicalSensorBinding.AddObject(newItem);
                }
                context.SaveChanges();
            }
        }

        public override void DeleteDispatcher(DispatcherEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var item = context.Dispatcher.SingleOrDefault(p => p.Name == entity.Name);
                if (item != null)
                {
                    context.DeleteObject(item);
                    context.SaveChanges();
                }
            }
        }

        public override void UpdateDispatcher(DispatcherEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var item = context.Dispatcher.SingleOrDefault(p => p.Name == entity.Name);

                if (item != null)
                {
                    item.LoadFromFrameworkEntity(entity, false);
                    context.SaveChanges();
                }
            }
        }

        public override void UpdateDispatcher2ProcessorBindings(string dispatcherName, Dispatcher2ProcessorBindingEntity[] bindings)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var currentBindings = context.EventProcessorDispatcherBinding.Where(p => p.DispatcherID == dispatcherName).ToList();
                foreach (var currentBinding in currentBindings)
                    context.EventProcessorDispatcherBinding.DeleteObject(currentBinding);
                foreach (var newBinding in bindings)
                {
                    var newItem = new EventProcessorDispatcherBinding();
                    newItem.LoadFromFrameworkEntity(newBinding);
                    context.EventProcessorDispatcherBinding.AddObject(newItem);
                   
                }
                context.SaveChanges();
            }
        }

        public override void UpdateSensorProvider(SensorProviderEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var item = context.SensorProvider.SingleOrDefault(p => p.Name == entity.Name);

                if (item != null)
                {
                    item.LoadFromFrameworkEntity(entity);
                    context.SaveChanges();
                }
            }
        }

        public override void DeleteSensorProvider(SensorProviderEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var item = context.SensorProvider.SingleOrDefault(p => p.Name == entity.Name);
                if (item != null)
                {
                    context.DeleteObject(item);
                    context.SaveChanges();
                }
            }
        }

        public override void DeleteEventModule(EventModuleEntity entity)
        {
            using (KalitteSensorServerEntities context = GetDataContext())
            {
                var item = context.EventModule.SingleOrDefault(p => p.Name == entity.Name);
                if (item != null)
                {
                    context.DeleteObject(item);
                    context.SaveChanges();
                }
            }
        }
    }
}
