using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using System.Configuration;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Metadata
{
    public abstract class MetadadataProvider : ProviderBase
    {
        private string connectionString;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
            if (config["connectionString"] != null)
            {
                if (ConfigurationManager.ConnectionStrings[config["connectionString"]] != null)
                    connectionString = ConfigurationManager.ConnectionStrings[config["connectionString"]].ConnectionString;
                else connectionString = "";
            }

        }

        protected string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }

        public abstract void CreateSensorProvider(SensorProviderEntity entity);
        public abstract IEnumerable<SensorProviderEntity> GetSensorProviders();
        public abstract SensorProviderEntity GetSensorProvider(string name);


        public abstract void CreateSensorDevice(SensorDeviceEntity entity);
        public abstract IEnumerable<SensorDeviceEntity> GetSensorDevices();


        public abstract IEnumerable<LogicalSensorEntity> GetLogicalSensors();

        public abstract void CreateLogicalSensor(LogicalSensorEntity entity);
        //public abstract void CreateLogicalSensor2SensorBinding(IEnumerable<Logical2SensorBindingEntity> bindings);
        //public abstract IEnumerable<Logical2SensorBindingEntity> GetLogicalSensor2SensorBindings(LogicalSensorEntity logicalSensor);

        public abstract SensorDeviceEntity GetSensorDevice(string name);

        public abstract LogicalSensorEntity GetLogicalSensor(string name);

        public abstract IEnumerable<LogicalSensorEntity> GetLogicalSensors(string name);

        public abstract void CreateProcessor(ProcessorEntity entity);
        public abstract ProcessorEntity GetProcessor(string name);
        public abstract IEnumerable<ProcessorEntity> GetProcessors();

        public abstract IEnumerable<EventModuleEntity> GetEventModules();

        public abstract EventModuleEntity GetEventModule(string name);

        public abstract void CreateEventModule(EventModuleEntity entity);

        public abstract IEnumerable<EventModuleEntity> GetEventModules(string processorName);

        public abstract DispatcherEntity GetDispatcher(string name);
        public abstract void CreateDispatcher(DispatcherEntity entity);

        public abstract IEnumerable<DispatcherEntity> GetDispatchers();

        public abstract IEnumerable<DispatcherEntity> GetDispatchers(string processorName);

        public abstract void DeleteSensorDevice(SensorDeviceEntity entity);

        public abstract void UpdateSensorDevice(SensorDeviceEntity entity);

        public abstract void UpdateLogical2SensorBindings(string sensorName, Logical2SensorBindingEntity[] bindings);

        public abstract void DeleteLogicalSensor(LogicalSensorEntity logicalSensorEntity);

        public abstract void DeleteProcessor(ProcessorEntity entity);

        public abstract void UpdateLogicalSensor(LogicalSensorEntity logicalSensorEntity);

        public abstract void UpdateProcessor(ProcessorEntity processorEntity);

        public abstract void UpdateEventModule(EventModuleEntity eventModuleEntity);

        public abstract void UpdateProcessor2ModuleBindings(string processorName, Processor2ModuleBindingEntity[] bindings);

        public abstract void DeleteDispatcher(DispatcherEntity entity);

        public abstract void UpdateLogical2ProcessorBindings(string processorName, Logical2ProcessorBindingEntity[] bindings);

        public abstract void UpdateDispatcher(DispatcherEntity entity);

        public abstract void UpdateDispatcher2ProcessorBindings(string dispatcherName, Dispatcher2ProcessorBindingEntity[] bindings);

        public abstract void UpdateSensorProvider(SensorProviderEntity sensorProviderEntity);

        public abstract void DeleteSensorProvider(SensorProviderEntity sensorProviderEntity);

        public abstract void DeleteEventModule(EventModuleEntity eventModuleEntity);
    }
}
