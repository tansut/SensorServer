using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Security;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Core;
using System.Security.Permissions;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Processing.Services
{
    [ServiceContract(Namespace = "http://kalitte.sensors/")]
    public interface IManagementService
    {
        #region general

        [OperationContract, FaultContract(typeof(SensorFault))]
        ExtendedMetadata GetItemExtendedMetadata(ProcessingItem itemType);

        [OperationContract, FaultContract(typeof(SensorFault))]
        Dictionary<PropertyKey, EntityMetadata> GetItemDefaultMetadata(ProcessingItem itemType, string itemName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        LogQueryResult GetItemLog(ProcessingItem itemType, string itemName, LogQuery query);

        [OperationContract, FaultContract(typeof(SensorFault))]
        Dictionary<ProcessingItem, IEnumerable<string>> GetLogSources();

        [OperationContract, FaultContract(typeof(SensorFault))]        
        IList<LastEvent> GetLastEvents(ProcessingItem itemType, string itemName);

        [OperationContract, FaultContract(typeof(SensorFault))]        
        void SetLastEventFilter(ProcessingItem itemType, string itemName, LastEventFilter filter);

        [OperationContract, FaultContract(typeof(SensorFault))]        
        LastEventFilter GetLastEventFilter(ProcessingItem itemType, string itemName);
        #endregion

        #region watch
        [OperationContract, FaultContract(typeof(SensorFault))]
        NameDescriptionList GetServerWatcherCategoryNames(string watch, ServerAnalyseItem related);

        [OperationContract, FaultContract(typeof(SensorFault))]
        NameDescriptionList GetServerWatcherNames();

        [OperationContract, FaultContract(typeof(SensorFault))]
        NameDescriptionList GetServerWatcherCategories(string watch);

        [OperationContract, FaultContract(typeof(SensorFault))]
        NameDescriptionList GetServerWatcherInstanceNames(string watch, string category);

        [OperationContract, FaultContract(typeof(SensorFault))]
        NameDescriptionList GetServerWatcherMeasureNames(string watch, string category);

        [OperationContract, FaultContract(typeof(SensorFault))]
        float[] GetServerWatcherMeasureValues(string watch, string category, string instance, string[] measureNames);
        #endregion

        #region ProviderManagement
        [OperationContract, FaultContract(typeof(SensorFault))]
        IEnumerable<SensorProviderEntity> GetSensorProviders();

        [OperationContract, FaultContract(typeof(SensorFault))]
        void UpdateSensorProvider(string providerName, string description, string type, SensorProviderProperty properties);

        [OperationContract, FaultContract(typeof(SensorFault))]
        ProviderMetadata GetSensorProviderMetadata(string providerName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        SensorProviderEntity CreateSensorProvider(string name, string description, string type, ItemStartupType startup);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void DeleteSensorProvider(string providerName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        SensorProviderEntity GetSensorProvider(string providerName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void ChangeProviderState(string providerName, ItemState newState);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void SetSensorProviderProfile(string providerName, PropertyList properties);

        #endregion

        #region sensorManagement

        [OperationContract, FaultContract(typeof(SensorFault))]
        SensorDeviceEntity GetSensorDevice(string sensorName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        IEnumerable<SensorDeviceEntity> GetSensorDevices();

        [OperationContract, FaultContract(typeof(SensorFault))]
        IEnumerable<SensorDeviceEntity> GetSensorDevicesForProvider(string providerName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void CreateSensor(string sensorName, string sensorId, string description,
            ConnectionInformation connInfo,
            AuthenticationInformation authInfo,
            ItemStartupType startup);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void DeleteSensor(string sensorName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void ChangeSensorState(string sensorName, ItemState newState);


        [OperationContract, FaultContract(typeof(SensorFault))]
        void UpdateSensor(string sensorName, string sensorId, string description, SensorDeviceProperty properties);

        [OperationContract, FaultContract(typeof(SensorFault))]
        Dictionary<string, PropertyList> GetSensorSources(string sensorName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        Dictionary<PropertyKey, DevicePropertyMetadata> GetSensorMetadata(string providerName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void UpdateSensorWithBindings(string sensorName, string sensorId, string description, SensorDeviceProperty properties,
        Logical2SensorBindingEntity[] bindings);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void SetSensorProfile(string sensorName, string source, PropertyList properties);

        #endregion

        #region logicalSensorManagement

        [OperationContract, FaultContract(typeof(SensorFault))]
        IEnumerable<LogicalSensorEntity> GetLogicalSensors();

        [OperationContract, FaultContract(typeof(SensorFault))]
        LogicalSensorEntity GetLogicalSensor(string logicalSensorName);



        [OperationContract, FaultContract(typeof(SensorFault))]
        IEnumerable<Logical2SensorBindingEntity> GetLogical2SensorBindings(string sensorName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void UpdateLogical2SensorBindings(string sensorName, Logical2SensorBindingEntity[] bindings);


        [OperationContract, FaultContract(typeof(SensorFault))]
        LogicalSensorEntity CreateLogicalSensor(string logicalSensorName, string description, ItemStartupType startup);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void ChangeLogicalSensorState(string logicalSensorName, ItemState newState);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void UpdateLogicalSensor(string logicalSensorName, string description, LogicalSensorProperty properties);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void DeleteLogicalSensor(string logicalSensorName);


        [OperationContract, FaultContract(typeof(SensorFault))]
        IEnumerable<Logical2SensorBindingEntity> GetSensor2LogicalBindings(string logicalSensorName);


        #endregion

        #region processorManagement


        [OperationContract, FaultContract(typeof(SensorFault))]
        ProcessorEntity CreateProcessor(string processorName, string description, ItemStartupType startup);

        [OperationContract, FaultContract(typeof(SensorFault))]
        IEnumerable<ProcessorEntity> GetProcessors();

        [OperationContract, FaultContract(typeof(SensorFault))]
        ProcessorEntity GetProcessor(string processorName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void DeleteProcessor(string processorName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void UpdateProcessor(string processorName, string description, ProcessorProperty properties);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void ChangeProcessorState(string processorName, ItemState newState);

        [OperationContract, FaultContract(typeof(SensorFault))]
        ProcessorMetadata GetProcessorMetadata(string processorName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        IEnumerable<Processor2ModuleBindingEntity> GetProcessor2ModuleBindings(string processorName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void UpdateProcessor2ModuleBindings(string processorName, Processor2ModuleBindingEntity[] bindings);

        [OperationContract, FaultContract(typeof(SensorFault))]
        IEnumerable<Logical2ProcessorBindingEntity> GetProcessor2LogicalSensorBindings(string processorName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void UpdateProcessor2LogicalSensorBindings(string processorName, Logical2ProcessorBindingEntity[] bindings);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void UpdateProcessorWithBindings(string processorName, string description, ProcessorProperty properties,
            Processor2ModuleBindingEntity[] moduleBindings, Logical2ProcessorBindingEntity[] logicalSensorBindings);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void ChangeProcessorModuleState(string processorName, string moduleName, ItemState newState);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void ChangeProcessorLogicalSensorBindingState(string processorName, string logicalSensorName, ItemState newState);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void SetEventModuleProfile(string processor, string module, PropertyList properties);

        #endregion

        #region eventModule

        [OperationContract, FaultContract(typeof(SensorFault))]
        IEnumerable<EventModuleEntity> GetEventModules();

        [OperationContract, FaultContract(typeof(SensorFault))]
        EventModuleEntity GetEventModule(string eventModuleName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void ChangeEventModuleState(string eventModuleName, ItemState newState);

        [OperationContract, FaultContract(typeof(SensorFault))]
        EventModuleEntity CreateEventModule(string eventModuleName,
            string description,
            string type,
            ItemStartupType startup);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void DeleteEventModule(string eventModuleName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void UpdateEventModule(string eventModuleName, string description, string type, EventModuleProperty properties);

        [OperationContract, FaultContract(typeof(SensorFault))]
        EventModuleMetadata GetEventModuleMetadata(string eventModuleName);

        #endregion

        #region dispatcher

        [OperationContract, FaultContract(typeof(SensorFault))]
        IEnumerable<DispatcherEntity> GetDispatchers();

        [OperationContract, FaultContract(typeof(SensorFault))]
        DispatcherEntity GetDispatcher(string dispatcherName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void ChangeDispatcherState(string dispatcherName, ItemState newState);

        [OperationContract, FaultContract(typeof(SensorFault))]
        DispatcherEntity CreateDispatcher(string dispatcherName,
            string description,
            string type,
            ItemStartupType startup);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void DeleteDispatcher(string dispatcherName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void UpdateDispatcher(string dispatcherName, string description, string type, DispatcherProperty properties);

        [OperationContract, FaultContract(typeof(SensorFault))]
        DispatcherMetadata GetDispatcherMetadata(string dispatcherName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        IEnumerable<Dispatcher2ProcessorBindingEntity> GetDispatcher2ProcessorBindings(string dispatcherName);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void UpdateDispatcher2ProcessorBindings(string dispatcherName, Dispatcher2ProcessorBindingEntity[] bindings);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void UpdateDispatcherWithBindings(string dispatcherName, string description, string type, DispatcherProperty properties,
            Dispatcher2ProcessorBindingEntity[] processorBindings);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void ChangeDispatcherProcessorBindingState(string dispatcherName, string processorName, ItemState newState);

        [OperationContract, FaultContract(typeof(SensorFault))]
        void SetDispatcherProfile(string dispatcher, PropertyList properties);

        #endregion
    }
}
