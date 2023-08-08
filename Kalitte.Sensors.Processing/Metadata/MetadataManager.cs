using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Metadata
{
    public static class MetadataManager
    {
        private static MetadadataProvider provider;
        private static MetadataProviderCollection s_Providers;
        private static object syncObj = new object();

        #region ProviderManagement

        public static void CreateSensorProvider(SensorProviderEntity entity)
        {
            provider.CreateSensorProvider(entity);
        }

        public static IEnumerable<SensorProviderEntity> GetSensorProviders()
        {
            return provider.GetSensorProviders();
        }

        public static void UpdateSensorProvider(SensorProviderEntity sensorProviderEntity)
        {
            provider.UpdateSensorProvider(sensorProviderEntity);
        }

        public static void DeleteSensorProvider(SensorProviderEntity sensorProviderEntity)
        {
            provider.DeleteSensorProvider(sensorProviderEntity);   
        }

        #endregion

        #region SensorManagement

        public static void UpdateSensorDevice(SensorDeviceEntity entity)
        {
            provider.UpdateSensorDevice(entity);
        }

        public static void CreateSensorDevice(SensorDeviceEntity entity)
        {
            provider.CreateSensorDevice(entity);
        }

        public static void DeleteSensorDevice(SensorDeviceEntity entity)
        {
            provider.DeleteSensorDevice(entity);
        }

        public static SensorDeviceEntity GetSensorDevice(string name)
        {
            return provider.GetSensorDevice(name);
        }

        public static IEnumerable<SensorDeviceEntity> GetSensorDevices()
        {
            return provider.GetSensorDevices();
        }

        #endregion

        #region LogicalManagement

        public static IEnumerable<LogicalSensorEntity> GetLogicalSensors()
        {
            return provider.GetLogicalSensors();
        }

        public static void CreateLogicalSensor(LogicalSensorEntity entity)
        {
            provider.CreateLogicalSensor(entity);
        }

        public static LogicalSensorEntity GetLogicalSensor(string name)
        {
            return provider.GetLogicalSensor(name);
        }

        public static void UpdateLogical2SensorBindings(string sensorName, Logical2SensorBindingEntity[] bindings)
        {
            provider.UpdateLogical2SensorBindings(sensorName, bindings);
        }

        public static void DeleteLogicalSensor(LogicalSensorEntity logicalSensorEntity)
        {
            provider.DeleteLogicalSensor(logicalSensorEntity);
        }

        public static void UpdateLogicalSensor(LogicalSensorEntity logicalSensorEntity)
        {
            provider.UpdateLogicalSensor(logicalSensorEntity);
        }


        #endregion

        #region ProcessorManagement

        public static IEnumerable<ProcessorEntity> GetProcessors()
        {
            return provider.GetProcessors();
        }

        public static ProcessorEntity GetProcessor(string processorName)
        {
            return provider.GetProcessor(processorName);
        }

        public static void CreateEventProcessor(ProcessorEntity entity)
        {
            provider.CreateProcessor(entity);
        }

        public static void DeleteProcessor(ProcessorEntity entity)
        {
            provider.DeleteProcessor(entity);
        }

        public static void UpdateProcessor(ProcessorEntity processorEntity)
        {
            provider.UpdateProcessor(processorEntity);
        }

        public static void UpdateProcessor2ModuleBindings(string processorName, Processor2ModuleBindingEntity[] bindings)
        {
            provider.UpdateProcessor2ModuleBindings(processorName, bindings);
        }

        public static void UpdateLogical2ProcessorBindings(string processorName, Logical2ProcessorBindingEntity[] bindings)
        {
            provider.UpdateLogical2ProcessorBindings(processorName, bindings);
            
        }

        #endregion

        #region EventModuleManagement

        public static IEnumerable<EventModuleEntity> GetEventModules()
        {
            return provider.GetEventModules();
        }

        public static EventModuleEntity GetEventModule(string name)
        {
            return provider.GetEventModule(name);
        }

        public static void CreateEventModule(EventModuleEntity entity)
        {
            provider.CreateEventModule(entity);
        }

        public static IEnumerable<EventModuleEntity> GetEventModules(string processorName)
        {
            return provider.GetEventModules(processorName);
        }

        public static void UpdateEventModule(EventModuleEntity eventModuleEntity)
        {
            provider.UpdateEventModule(eventModuleEntity);
        }



        public static void DeleteEventModule(EventModuleEntity eventModuleEntity)
        {
            provider.DeleteEventModule(eventModuleEntity);
        }

        #endregion

        #region DispatcherManagement

        public static DispatcherEntity GetDispatcher(string name)
        {
            return provider.GetDispatcher(name);
        }

        public static void CreateDispatcher(DispatcherEntity entity)
        {
            provider.CreateDispatcher(entity);
        }

        public static IEnumerable<DispatcherEntity> GetDispatchers()
        {
            return provider.GetDispatchers();
        }

        public static IEnumerable<DispatcherEntity> GetDispatchers(string processorName)
        {
            return provider.GetDispatchers(processorName);
        }

        public static void DeleteDispatcher(DispatcherEntity entity)
        {
            provider.DeleteDispatcher(entity);
        }

        public static void UpdateDispatcher(DispatcherEntity entity)
        {
            provider.UpdateDispatcher(entity);
        }

        public static void UpdateDispatcher2ProcessorBindings(string dispatcherName, Dispatcher2ProcessorBindingEntity[] bindings)
        {
            provider.UpdateDispatcher2ProcessorBindings(dispatcherName, bindings);
        }

        #endregion






        static MetadataManager()
        {
            Initialize();
        }

        private static void Initialize()
        {
            if (provider == null)
            {
                lock (syncObj)
                {
                    if (provider == null)
                    {
                        SensorServerConfigurationSection section =
                        ConfigurationManager.GetSection("KalitteSensorServer") as SensorServerConfigurationSection;
                        s_Providers = new MetadataProviderCollection();
                        foreach (ProviderSettings settings in section.MetadataProviders)
                        {
                            Type c = Type.GetType(settings.Type, true, true);
                            if (!typeof(MetadadataProvider).IsAssignableFrom(c))
                            {
                                throw new ArgumentException("Must be ServerPersistanceProvider");
                            }
                            MetadadataProvider p = (MetadadataProvider)Activator.CreateInstance(c);
                            NameValueCollection parameters = settings.Parameters;
                            NameValueCollection config = new NameValueCollection(parameters.Count, StringComparer.Ordinal);
                            foreach (string str2 in parameters)
                            {
                                config[str2] = parameters[str2];
                            }
                            p.Initialize(settings.Name, config);
                            s_Providers.Add(p);
                        }
                        provider = s_Providers[section.DefaultMetadataProvider];
                    }
                }
            }
        }

    }
}

