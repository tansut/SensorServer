using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Client.Proxy;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Security;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Core;
using System.Net;
using Kalitte.Sensors.Events;
using System.ServiceModel.Description;
using System.ServiceModel.Security;


namespace Kalitte.Sensors.Client
{
    public sealed class SensorClient
    {
        ProxyBase commandProxy;
        ProxyBase managementProxy;

        private object CallMethodOnProxy(ProxyBase proxy, string method, params object[] parameters)
        {
            return proxy.CallMethod(method, parameters);
        }

        #region watch

        public NameDescriptionList GetServerWatcherCategoryNames(string watch, ServerAnalyseItem related)
        {
            return (NameDescriptionList)CallMethodOnProxy(managementProxy, "GetServerWatcherCategoryNames", watch, related);
        }

        public NameDescriptionList GetServerWatcherNames()
        {
            return (NameDescriptionList)CallMethodOnProxy(managementProxy, "GetServerWatcherNames");
        }

        public NameDescriptionList GetServerWatcherCategories(string watch)
        {
            return (NameDescriptionList)CallMethodOnProxy(managementProxy, "GetServerWatcherCategories", watch);
        }

        public NameDescriptionList GetServerWatcherInstanceNames(string watch, string category)
        {
            return (NameDescriptionList)CallMethodOnProxy(managementProxy, "GetServerWatcherInstanceNames", watch, category);
        }

        public NameDescriptionList GetServerWatcherMeasureNames(string watch, string category)
        {
            return (NameDescriptionList)CallMethodOnProxy(managementProxy, "GetServerWatcherMeasureNames", watch, category);
        }

        public float[] GetServerWatcherMeasureValues(string watch, string category, string instance, string[] measureNames)
        {
            return (float[])CallMethodOnProxy(managementProxy, "GetServerWatcherMeasureValues", watch, category, instance, measureNames);
        }

        #endregion

        #region general

        public ExtendedMetadata GetItemExtendedMetadata(ProcessingItem itemType)
        {
            return (ExtendedMetadata)CallMethodOnProxy(managementProxy, "GetItemExtendedMetadata", itemType);
        }

        public Dictionary<PropertyKey, EntityMetadata> GetItemDefaultMetadata(ProcessingItem itemType)
        {
            return GetItemDefaultMetadata(itemType, null);
        }

        public Dictionary<PropertyKey, EntityMetadata> GetItemDefaultMetadata(ProcessingItem itemType, string itemName)
        {
            return (Dictionary<PropertyKey, EntityMetadata>)CallMethodOnProxy(managementProxy, "GetItemDefaultMetadata", itemType, itemName);
        }

        public LogQueryResult GetItemLog(ProcessingItem itemType, string itemName, LogQuery query)
        {
            return (LogQueryResult)CallMethodOnProxy(managementProxy, "GetItemLog", itemType, itemName, query);
        }

        public Dictionary<ProcessingItem, IEnumerable<string>> GetLogSources()
        {
            return (Dictionary<ProcessingItem, IEnumerable<string>>)CallMethodOnProxy(managementProxy, "GetLogSources");
        }

        public IList<LastEvent> GetLastEvents(ProcessingItem itemType, string itemName)
        {
            return (IList<LastEvent>)CallMethodOnProxy(managementProxy, "GetLastEvents", itemType, itemName);

        }

        public void SetLastEventFilter(ProcessingItem itemType, string itemName, LastEventFilter filter)
        {
            CallMethodOnProxy(managementProxy, "SetLastEventFilter", itemType, itemName, filter);
        }

        public LastEventFilter GetLastEventFilter(ProcessingItem itemType, string itemName)
        {
            return (LastEventFilter)CallMethodOnProxy(managementProxy, "GetLastEventFilter", itemType, itemName);
        }

        #endregion

        #region sensorManagement

        public SensorDeviceEntity GetSensorDevice(string sensorName)
        {
            return (SensorDeviceEntity)CallMethodOnProxy(managementProxy, "GetSensorDevice", sensorName);
        }

        public ResponseEventArgs ExecuteCommand(string sensorName, string source, SensorCommand command)
        {
            return (ResponseEventArgs)CallMethodOnProxy(commandProxy, "Execute", sensorName, source, command);

        }

        public Kalitte.Sensors.Processing.Metadata.SensorDeviceEntity[] GetSensorDevicesForProvider(string providerName)
        {
            return (Kalitte.Sensors.Processing.Metadata.SensorDeviceEntity[])CallMethodOnProxy(managementProxy, "GetSensorDevicesForProvider", providerName);
        }

        public SensorDeviceEntity[] GetSensorDevices()
        {
            return (SensorDeviceEntity[])CallMethodOnProxy(managementProxy, "GetSensorDevices", new object[] { });
        }

        public SensorDeviceEntity CreateSensor(string sensorName, string sensorId, string description, ConnectionInformation connection, AuthenticationInformation authentication, ItemStartupType startup)
        {
            return (SensorDeviceEntity)CallMethodOnProxy(managementProxy, "CreateSensor", sensorName, sensorId, description, connection, authentication, startup);

        }

        public void DeleteSensor(string sensorName)
        {
            CallMethodOnProxy(managementProxy, "DeleteSensor", sensorName);
        }

        public void UpdateSensor(string sensorName, string sensorId, string description, SensorDeviceProperty properties)
        {
            CallMethodOnProxy(managementProxy, "UpdateSensor", sensorName, sensorId, description, properties);
        }

        public void ChangeSensorState(string sensorName, ItemState newState)
        {
            CallMethodOnProxy(managementProxy, "ChangeSensorState", sensorName, newState);
        }


        public Dictionary<string, PropertyList> GetSensorSources(string sensorName)
        {
            return (Dictionary<string, PropertyList>)CallMethodOnProxy(managementProxy, "GetSensorSources", sensorName);
        }

        public ProviderMetadata GetSensorProviderMetadata(string providerName)
        {
            return (ProviderMetadata)CallMethodOnProxy(managementProxy, "GetSensorProviderMetadata", providerName);
        }

        public Dictionary<PropertyKey, DevicePropertyMetadata> GetSensorMetadata(string providerName)
        {
            return (Dictionary<PropertyKey, DevicePropertyMetadata>)CallMethodOnProxy(managementProxy, "GetSensorMetadata", providerName);
        }

        public void UpdateSensorWithBindings(string sensorName, string sensorId, string description, SensorDeviceProperty properties, Logical2SensorBindingEntity[] bindings)
        {
            CallMethodOnProxy(managementProxy, "UpdateSensorWithBindings", sensorName, sensorId, description, properties, bindings);
        }

        #endregion

        #region sensorProviderManagement

        public void DeleteSensorProvider(string providerName)
        {
            CallMethodOnProxy(managementProxy, "DeleteSensorProvider", providerName);
        }


        public void UpdateSensorProvider(string providerName, string description, string type, SensorProviderProperty properties)
        {
            CallMethodOnProxy(managementProxy, "UpdateSensorProvider", providerName, description, type, properties);
        }

        public SensorProviderEntity GetSensorProvider(string providerName)
        {
            return (SensorProviderEntity)CallMethodOnProxy(managementProxy, "GetSensorProvider", providerName);
        }

        public void ChangeProviderState(string providerName, ItemState newState)
        {
            CallMethodOnProxy(managementProxy, "ChangeProviderState", providerName, newState);

        }

        public SensorProviderEntity[] GetSensorProviders()
        {
            return (SensorProviderEntity[])CallMethodOnProxy(managementProxy, "GetSensorProviders");
        }


        public SensorProviderEntity CreateSensorProvider(string name, string description, string type, ItemStartupType startup)
        {
            return (SensorProviderEntity)CallMethodOnProxy(managementProxy, "CreateSensorProvider", name, description, type, startup);
        }



        #endregion


        #region logicalSensorManagement

        public LogicalSensorEntity GetLogicalSensor(string logicalSensorName)
        {
            return (LogicalSensorEntity)CallMethodOnProxy(managementProxy, "GetLogicalSensor", logicalSensorName);
        }

        public LogicalSensorEntity[] GetLogicalSensors()
        {
            return (LogicalSensorEntity[])CallMethodOnProxy(managementProxy, "GetLogicalSensors");
        }

        public LogicalSensorEntity CreateLogicalSensor(string logicalSensorName, string description, ItemStartupType startup)
        {
            return (LogicalSensorEntity)CallMethodOnProxy(managementProxy, "CreateLogicalSensor", logicalSensorName, description, startup);
        }

        public void UpdateLogicalSensor(string logicalSensorName, string description, LogicalSensorProperty properties)
        {
            CallMethodOnProxy(managementProxy, "UpdateLogicalSensor", logicalSensorName, description, properties);
        }

        public void DeleteLogicalSensor(string logicalSensorName)
        {
            CallMethodOnProxy(managementProxy, "DeleteLogicalSensor", logicalSensorName);
        }

        public void ChangeLogicalSensorState(string logicalSensorName, ItemState newState)
        {
            CallMethodOnProxy(managementProxy, "ChangeLogicalSensorState", logicalSensorName, newState);
        }

        public Logical2SensorBindingEntity[] GetSensor2LogicalBindings(string logicalSensorName)
        {
            return (Logical2SensorBindingEntity[])CallMethodOnProxy(managementProxy, "GetSensor2LogicalBindings", logicalSensorName);
        }

        public Logical2SensorBindingEntity[] GetLogical2SensorBindings(string sensorDeviceName)
        {
            return (Logical2SensorBindingEntity[])CallMethodOnProxy(managementProxy, "GetLogical2SensorBindings", sensorDeviceName);
        }

        public void UpdateLogical2SensorBindings(string sensorDeviceName, Logical2SensorBindingEntity[] bindings)
        {
            CallMethodOnProxy(managementProxy, "UpdateLogical2SensorBindings", sensorDeviceName, bindings);
        }

        #endregion


        #region processorManagement


        public void SetEventModuleProfile(string processor, string module, PropertyList properties)
        {
            CallMethodOnProxy(managementProxy, "SetEventModuleProfile", processor, module, properties);
        }


        public ProcessorEntity[] GetProcessors()
        {
            return (ProcessorEntity[])CallMethodOnProxy(managementProxy, "GetProcessors");
        }

        public ProcessorEntity GetProcessor(string processorName)
        {
            return (ProcessorEntity)CallMethodOnProxy(managementProxy, "GetProcessor", processorName);
        }

        public void DeleteProcessor(string processorName)
        {
            CallMethodOnProxy(managementProxy, "DeleteProcessor", processorName);

        }

        public void UpdateProcessor(string processorName, string description, ProcessorProperty properties)
        {
            CallMethodOnProxy(managementProxy, "UpdateProcessor", processorName, description, properties);

        }

        public void ChangeProcessorState(string processorName, ItemState newState)
        {
            CallMethodOnProxy(managementProxy, "ChangeProcessorState", processorName, newState);
        }

        public ProcessorMetadata GetProcessorMetadata(string processorName)
        {
            return (ProcessorMetadata)CallMethodOnProxy(managementProxy, "GetProcessorMetadata", processorName);
        }

        public ProcessorEntity CreateProcessor(string processorName, string description, ItemStartupType startup)
        {
            return (ProcessorEntity)CallMethodOnProxy(managementProxy, "CreateProcessor", processorName, description, startup);
        }

        public Processor2ModuleBindingEntity[] GetProcessor2ModuleBindings(string processorName)
        {
            return (Processor2ModuleBindingEntity[])CallMethodOnProxy(managementProxy, "GetProcessor2ModuleBindings", processorName);

        }

        public void UpdateProcessor2ModuleBindings(string processorName, Processor2ModuleBindingEntity[] bindings)
        {
            CallMethodOnProxy(managementProxy, "UpdateProcessor2ModuleBindings", processorName, bindings);
        }

        public Logical2ProcessorBindingEntity[] GetProcessor2LogicalSensorBindings(string processorName)
        {
            return (Logical2ProcessorBindingEntity[])CallMethodOnProxy(managementProxy, "GetProcessor2LogicalSensorBindings", processorName);
        }

        public void UpdateProcessor2LogicalSensorBindings(string processorName, Logical2ProcessorBindingEntity[] bindings)
        {
            CallMethodOnProxy(managementProxy, "UpdateProcessor2LogicalSensorBindings", processorName, bindings);
        }

        public void UpdateProcessorWithBindings(string processorName, string description, ProcessorProperty properties, Processor2ModuleBindingEntity[] moduleBindings, Logical2ProcessorBindingEntity[] logicalSensorBindings)
        {
            CallMethodOnProxy(managementProxy, "UpdateProcessorWithBindings", processorName, description, properties, moduleBindings, logicalSensorBindings);
        }

        public void ChangeProcessorModuleState(string processorName, string moduleName, ItemState newState)
        {
            CallMethodOnProxy(managementProxy, "ChangeProcessorModuleState", processorName, moduleName, newState);
        }

        public void ChangeProcessorLogicalSensorBindingState(string processorName, string logicalSensorName, ItemState newState)
        {
            CallMethodOnProxy(managementProxy, "ChangeProcessorLogicalSensorBindingState", processorName, logicalSensorName, newState);
        }

        #endregion

        #region eventModuleManagement


        public EventModuleEntity[] GetEventModules()
        {
            return (EventModuleEntity[])CallMethodOnProxy(managementProxy, "GetEventModules");
        }

        public EventModuleEntity CreateEventModule(string eventModuleName, string description, string type, ItemStartupType startup)
        {
            return (EventModuleEntity)CallMethodOnProxy(managementProxy, "CreateEventModule", eventModuleName, description, type, startup);
        }

        public void DeleteEventModule(string eventModuleName)
        {
            CallMethodOnProxy(managementProxy, "DeleteEventModule", eventModuleName);

        }

        public void UpdateEventModule(string eventModuleName, string description, string type, EventModuleProperty properties)
        {
            CallMethodOnProxy(managementProxy, "UpdateEventModule", eventModuleName, description, type, properties);
        }

        public EventModuleMetadata GetEventModuleMetadata(string eventModuleName)
        {
            return (EventModuleMetadata)CallMethodOnProxy(managementProxy, "GetEventModuleMetadata", eventModuleName);
        }

        public EventModuleEntity GetEventModule(string eventModuleName)
        {
            return (EventModuleEntity)CallMethodOnProxy(managementProxy, "GetEventModule", eventModuleName);
        }

        public void ChangeEventModuleState(string eventModuleName, ItemState newState)
        {
            CallMethodOnProxy(managementProxy, "ChangeEventModuleState", eventModuleName, newState);
        }

        #endregion

        #region dispatcher

        public void SetDispatcherProfile(string dispatcher, PropertyList properties)
        {
            CallMethodOnProxy(managementProxy, "SetDispatcherProfile", dispatcher, properties);
        }

        public Dispatcher2ProcessorBindingEntity[] GetDispatcher2ProcessorBindings(string dispatcherName)
        {
            return (Dispatcher2ProcessorBindingEntity[])CallMethodOnProxy(managementProxy, "GetDispatcher2ProcessorBindings", dispatcherName);
        }

        public void UpdateDispatcher2ProcessorBindings(string dispatcherName, Dispatcher2ProcessorBindingEntity[] bindings)
        {
            CallMethodOnProxy(managementProxy, "UpdateDispatcher2ProcessorBindings", dispatcherName, bindings);
        }




        public DispatcherEntity[] GetDispatchers()
        {
            return (DispatcherEntity[])CallMethodOnProxy(managementProxy, "GetDispatchers");
        }

        public DispatcherEntity GetDispatcher(string dispatcherName)
        {
            return (DispatcherEntity)CallMethodOnProxy(managementProxy, "GetDispatcher", dispatcherName);
        }

        public void ChangeDispatcherState(string dispatcherName, ItemState newState)
        {
            CallMethodOnProxy(managementProxy, "ChangeDispatcherState", dispatcherName, newState);
        }

        public DispatcherEntity CreateDispatcher(string dispatcherName, string description, string type, ItemStartupType startup)
        {
            return (DispatcherEntity)CallMethodOnProxy(managementProxy, "CreateDispatcher", dispatcherName, description, type, startup);
        }

        public void DeleteDispatcher(string dispatcherName)
        {
            CallMethodOnProxy(managementProxy, "DeleteDispatcher", dispatcherName);
        }

        public void UpdateDispatcher(string dispatcherName, string description, string type, DispatcherProperty properties)
        {
            CallMethodOnProxy(managementProxy, "UpdateDispatcher", dispatcherName, description, type, properties);
        }

        public DispatcherMetadata GetDispatcherMetadata(string dispatcherName)
        {
            return (DispatcherMetadata)CallMethodOnProxy(managementProxy, "GetDispatcherMetadata", dispatcherName);
        }





        public void UpdateDispatcherWithBindings(string dispatcherName, string description, string type, DispatcherProperty properties, Dispatcher2ProcessorBindingEntity[] processorBindings)
        {
            CallMethodOnProxy(managementProxy, "UpdateDispatcherWithBindings", dispatcherName, description, type, properties, processorBindings);
        }

        public void ChangeDispatcherProcessorBindingState(string dispatcherName, string processorName, ItemState newState)
        {
            CallMethodOnProxy(managementProxy, "ChangeDispatcherProcessorBindingState", dispatcherName, processorName, newState);
        }

        #endregion

        public ServiceConfiguration Configuration { get; private set; }

        public SensorClient()
            : this(Dns.GetHostName())
        {
        }

        public SensorClient(ClientCredentials credentials)
            : this(Dns.GetHostName(), 0, credentials)
        {
        }

        public SensorClient(string host, ClientCredentials credentials)
            : this(host, 0, credentials)
        {
        }

        public SensorClient(string host)
            : this(host, 0)
        {
        }


        public SensorClient(string host, int port)
            : this(host, port, host, port, null)
        {

        }

        public SensorClient(string host, int port, ClientCredentials credentials)
            : this(host, port, host, port, credentials)
        {

        }

        public SensorClient(string host, int managementServicePort, int sensorCommandServicePort, ClientCredentials credentials) :
            this(host, managementServicePort, host, sensorCommandServicePort, credentials)
        {

        }


        public string CommandServiceHost
        {
            get
            {
                return this.commandProxy.Host;
            }
        }

        public string ManagementServiceHost
        {
            get
            {
                return this.managementProxy.Host;
            }
        }

        public int Timeout
        {
            get
            {
                return this.managementProxy.Timeout;
            }
            set
            {
                this.managementProxy.Timeout = value;
                this.commandProxy.Timeout = value;
            }
        }

        public SensorClient(string managementServiceHost, int managementServicePort, string commandServiceHost, int commandServicePort, ClientCredentials credentials)
        {
            this.Configuration = new ServiceConfiguration();
            if (managementServicePort > 0)
            {
                this.Configuration.ManagementServicePort = managementServicePort;
            }

            if (commandServicePort > 0)
            {
                Configuration.SensorCommandServicePort = commandServicePort;
            }
            commandProxy = new ProxyBase(commandServiceHost, this.Configuration.SensorCommandServicePort, Configuration, typeof(SensorCommandServiceClient), "CommandProcessor", credentials);
            managementProxy = new ProxyBase(managementServiceHost, this.Configuration.ManagementServicePort, Configuration, typeof(ManagementServiceClient), "Management", credentials);
        }

        public SensorClient(string managementServiceHost, string commandServiceHost, ServiceConfiguration configuration, ClientCredentials credentials)
        {
            this.Configuration = configuration;
            commandProxy = new ProxyBase(commandServiceHost, this.Configuration.SensorCommandServicePort, Configuration, typeof(SensorCommandServiceClient), "CommandProcessor", credentials);
            managementProxy = new ProxyBase(managementServiceHost, this.Configuration.ManagementServicePort, Configuration, typeof(ManagementServiceClient), "Management", credentials);
        }

        public SensorClient(ServiceConfiguration configuration, ClientCredentials credentials): this(Dns.GetHostName(), Dns.GetHostName(), configuration, credentials)
        {

        }


        public void SetSensorProfile(string sensorName, string source, PropertyList properties)
        {
            CallMethodOnProxy(managementProxy, "SetSensorProfile", sensorName, source, properties);

        }
 
    }
}
