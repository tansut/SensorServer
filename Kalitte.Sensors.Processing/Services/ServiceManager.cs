using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Globalization;
using Kalitte.Sensors.Utilities;
using System.Xml;
using System.ServiceModel.Channels;
using Kalitte.Sensors.Processing.Services;
using System.ServiceModel.Description;
using System.Threading;
using Kalitte.Sensors.Processing.Metadata;
using System.Runtime.Serialization;
using Kalitte.Sensors.Configuration;
using System.Reflection;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Security;
using System.Security.Permissions;
using System.Security.Principal;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Processing.Core;
using Kalitte.Sensors.Service;
using System.ServiceModel.Activation;
using System.Configuration;

namespace Kalitte.Sensors.Processing.Services
{
    [ServiceBehavior(Namespace = "http://kalitte.sensorServer", InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    internal class ServiceManager : OperationManagerBase, ISensorCommandService, IManagementService
    {
        private ServiceHost host;
        private bool wcfStartupInProgress;
        private ManualResetEvent wcfOpenDoneEvent;
        private object wcfManagerLock;
        private int numberOfWcfOpens = 0;
        private System.Exception lastExceptionFromOpen = null;
        private readonly bool iisHosted;
        private volatile bool typesAddedToIssHost = false;

        public override IList<LastEvent> GetLastEvents(string itemName)
        {
            throw new InvalidOperationException("Doesnot support last events");
        }

        internal override void SetLastEventFilter(string itemName, LastEventFilter filter)
        {
            throw new InvalidOperationException("Doesnot support last events");

        }

        internal override LastEventFilter GetLastEventFilter(string itemName)
        {
            throw new InvalidOperationException("Doesnot support last events");

        }

        public override ProcessingItem ItemType
        {
            get { return ProcessingItem.None; }
        }

        public ServiceManager(ServerManager serverManager)
            : base(serverManager)
        {
            wcfOpenDoneEvent = new ManualResetEvent(false);
            wcfManagerLock = new object();
            iisHosted = false;
        }

        public ServiceManager()
            : this(null)
        {
            iisHosted = true;
            Kalitte.Sensors.Processing.Core.ServerManager.StartExternallyHosted(this);
            if (iisHosted != ServerConfiguration.Current.HostingConfiguration.IISHosted)
                throw new ConfigurationErrorsException("Please set IISHosting in configuration section");
        }


        private void addServiceEndPoint(Type implementedType, Binding binding, string address)
        {
            TypesHelper.AddKnownTypes(this.host.AddServiceEndpoint(implementedType, binding, address));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (iisHosted && this.ServerManager != null)
                    this.ServerManager.Shutdown();
            }
            base.Dispose(disposing);
        }


        private void createDefaultWcf()
        {
            Uri managementUri = new Uri(string.Format(CultureInfo.InvariantCulture, "net.tcp://localhost:{0}/sensor/service", ServerConfiguration.Current.ServiceConfiguration.ManagementServicePort));
            Uri commandUri = new Uri(string.Format(CultureInfo.InvariantCulture, "net.tcp://localhost:{0}/sensor/service", ServerConfiguration.Current.ServiceConfiguration.SensorCommandServicePort));
            host = new ServiceHost(this);
            Binding binding = ServiceBindingManager.GetDefaultBinding();

            if (ServerConfiguration.Current.ServiceConfiguration.EnableManagementService)
                this.addServiceEndPoint(typeof(IManagementService), binding, managementUri + "/Management");
            if (ServerConfiguration.Current.ServiceConfiguration.EnableSensorCommandService)
                this.addServiceEndPoint(typeof(ISensorCommandService), binding, commandUri + "/CommandProcessor");


            this.host.Description.Behaviors.Remove<ServiceCredentials>();
            ServiceCredentials credentials = new ServiceCredentials();
            credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new DefaultCredentialValidator();
            credentials.UserNameAuthentication.UserNamePasswordValidationMode = System.ServiceModel.Security.UserNamePasswordValidationMode.Custom;
            this.host.Description.Behaviors.Add(credentials);
            this.host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            ServiceSecurityAuditBehavior item = new ServiceSecurityAuditBehavior();
            item.AuditLogLocation = AuditLogLocation.Application;
            item.MessageAuthenticationAuditLevel = AuditLevel.Failure;
            item.ServiceAuthorizationAuditLevel = AuditLevel.None;
            item.SuppressAuditFailure = false;
            this.host.Description.Behaviors.Add(item);
            this.host.Description.Behaviors.Remove<ServiceAuthorizationBehavior>();
            var behavior2 = new ServiceAuthorizationBehavior();
            this.host.Description.Behaviors.Add(behavior2);
            behavior2.PrincipalPermissionMode = PrincipalPermissionMode.None;
            this.host.Description.Behaviors.Remove<ServiceThrottlingBehavior>();
            var behavior3 = new ServiceThrottlingBehavior();
            behavior3.MaxConcurrentSessions = 0x7fffffff;
            behavior3.MaxConcurrentCalls = 0x7fffffff;
            this.host.Description.Behaviors.Add(behavior3);
            this.host.CloseTimeout = TimeSpan.Zero;
        }

        private void CreateWcf()
        {
            if (iisHosted)
            {

                this.wcfOpenDoneEvent.Set();

            }
            else if (ServerConfiguration.Current.ServiceConfiguration.EnableManagementService ||
                ServerConfiguration.Current.ServiceConfiguration.EnableSensorCommandService)
            {
                try
                {
                    lock (wcfManagerLock)
                    {
                        try
                        {
                            if (this.host != null)
                            {
                                this.host.Close();
                                ((IDisposable)this.host).Dispose();
                            }
                        }
                        catch (System.Exception exc)
                        {
                            Logger.LogException("Exception while cleaning up sensorservicehost {0}", exc);
                        }

                        if (ServerConfiguration.Current.ServiceConfiguration.UseDefaultWcfSettings)
                        {
                            createDefaultWcf();
                        }
                        else host = new ServiceHost(this);

                        this.host.Faulted += new EventHandler(this.serviceHost_Closed);
                        this.wcfStartupInProgress = true;
                        this.wcfOpenDoneEvent.Reset();
                        this.host.BeginOpen(new AsyncCallback(this.serviceHost_Opened), null);
                    }
                }
                catch (System.Exception exc)
                {
                    Logger.LogException("Error starting wcf", exc);
                    throw;
                }
            }
            else
            {
                this.wcfOpenDoneEvent.Set();
            }
        }

        private void serviceHost_Opened(IAsyncResult ar)
        {
            lock (this.wcfManagerLock)
            {
                try
                {
                    this.host.EndOpen(ar);
                    this.wcfStartupInProgress = false;
                    numberOfWcfOpens++;
                    Logger.Info("Finished startup of Sensor WCF service");
                }
                catch (AddressAlreadyInUseException exception)
                {
                    lastExceptionFromOpen = new SensorException("PortOccupied WCF", exception);
                }
                catch (System.Exception exception2)
                {
                    lastExceptionFromOpen = new SensorException("Unkown WCF Exception", exception2);
                }
                finally
                {
                    this.wcfOpenDoneEvent.Set();
                }
                if (lastExceptionFromOpen != null)
                {
                    Logger.LogException("WcfStartupFailed", lastExceptionFromOpen);
                    this.host = null;
                    if (this.numberOfWcfOpens != 0)
                    {
                        Environment.FailFast(lastExceptionFromOpen.ToString());
                        throw lastExceptionFromOpen;
                    }
                }
            }
        }

        private void serviceHost_Closed(object sender, EventArgs e)
        {
            lock (this.wcfManagerLock)
            {
                if (this.wcfStartupInProgress || isShuttingDown)
                {
                    return;
                }
            }
            Logger.Warning("Sensorserver restarting");
            this.CreateWcf();
        }

        public override void Startup()
        {
            base.Startup();
            CreateWcf();
        }

        public override void Shutdown()
        {
            ShutdownWcf();
            base.Shutdown();
        }

        private void serviceHost_CloseFinished(IAsyncResult ar)
        {
            this.host.EndClose(ar);
        }

        private void ShutdownWcf()
        {
            lock (this.wcfManagerLock)
            {
                try
                {
                    if (this.host != null)
                    {
                        this.host.BeginClose(new AsyncCallback(this.serviceHost_CloseFinished), null);
                    }
                }
                catch (System.Exception exception)
                {
                    Logger.Error("Error shutting down wcf {0}", new object[] { exception });
                    throw;
                }
            }
        }

        public override void DelayedStartup()
        {
            try
            {
                this.wcfOpenDoneEvent.WaitOne();
                lock (this.wcfManagerLock)
                {
                    if (this.lastExceptionFromOpen != null)
                    {
                        throw this.lastExceptionFromOpen;
                    }
                }
            }
            catch (Exception exc)
            {
                throw new StartupException("Unable to create WCF", exc);
            }
            finally
            {
                base.DelayedStartup();
            }
        }

        private T callMethod<T>(object sourceObj, string methodName, params object[] args)
        {
            T result = (T)callMethod(sourceObj, methodName, args);
            return result;
        }

        private object callMethod(object sourceObj, string methodName, params object[] args)
        {
            if (sourceObj == null)
            {
                throw new ArgumentException("Cannot call method. Probably service is still initializing or got an exception. See log for details");
            }
            startupDone.WaitOne();
            AddKnownTypesIfRequired();
            MethodInfo method = sourceObj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            object result = null;
            System.Exception exc = null;
            try
            {
                result = method.Invoke(sourceObj, args);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    exc = ex.InnerException;
                else exc = ex;
            }

            catch (System.Exception ex)
            {
                exc = ex;
            }
            if (exc != null)
            {
                this.logger.Error("Error from method invocation {0}", new object[] { exc });
                SensorFault fault = SensorCommon.GetFaultFromException(exc, true);
                FaultException fExc = new FaultException<SensorFault>(fault, "Error in server");
                throw fExc;
            }
            else return result;
        }

        private void AddKnownTypesIfRequired()
        {
            if (iisHosted && !typesAddedToIssHost)
            {
                lock (this)
                {
                    if (!typesAddedToIssHost && OperationContext.Current != null && OperationContext.Current.InstanceContext != null &&
                        OperationContext.Current.InstanceContext.Host != null)
                    {
                        try
                        {
                            foreach (var endPoint in (OperationContext.Current.InstanceContext.Host).Description.Endpoints)
                            {
                                TypesHelper.AddKnownTypes(endPoint);
                            }
                        }
                        catch (Exception exc)
                        {
                            Logger.LogException("Unable to add knowntypes to endpoint", exc);
                        }
                        finally
                        {
                            typesAddedToIssHost = true;
                        }

                    }
                }
            }
        }

        #region ISensorCommandProcessor Members

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        public Commands.ResponseEventArgs Execute(string sensorName, string source, Commands.SensorCommand command)
        {
            return callMethod<Commands.ResponseEventArgs>(SensorManager, "ExecuteCommand", sensorName, source, command);
        }

        #endregion

        #region sensorProviderManagement

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public ProviderMetadata GetSensorProviderMetadata(string providerName)
        {
            return callMethod<ProviderMetadata>(ProviderManager, "GetMetadata", providerName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public IEnumerable<SensorProviderEntity> GetSensorProviders()
        {
            return callMethod<IEnumerable<SensorProviderEntity>>(ProviderManager, "GetCurrentEntityList");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public SensorProviderEntity CreateSensorProvider(string name, string description, string type, ItemStartupType startup)
        {
            return callMethod<SensorProviderEntity>(ProviderManager, "Create", name, description, type, startup);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void UpdateSensorProvider(string providerName, string description, string type, SensorProviderProperty properties)
        {
            callMethod(ProviderManager, "Update", providerName, description, type, properties);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void DeleteSensorProvider(string providerName)
        {
            callMethod(ProviderManager, "DeleteItem", providerName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public SensorProviderEntity GetSensorProvider(string providerName)
        {
            return callMethod<SensorProviderEntity>(ProviderManager, "GetEntity", providerName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void ChangeProviderState(string providerName, ItemState newState)
        {
            callMethod(ProviderManager, "ChangeState", providerName, newState);
        }

        #endregion

        #region sensorManagement

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public Dictionary<PropertyKey, DevicePropertyMetadata> GetSensorMetadata(string providerName)
        {
            return callMethod<Dictionary<PropertyKey, DevicePropertyMetadata>>(ProviderManager, "GetSensorMetadata", providerName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public SensorDeviceEntity GetSensorDevice(string sensorName)
        {
            return callMethod<SensorDeviceEntity>(SensorManager, "GetEntity", sensorName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public IEnumerable<SensorDeviceEntity> GetSensorDevicesForProvider(string providerName)
        {
            return callMethod<IEnumerable<SensorDeviceEntity>>(SensorManager, "GetSensorDevicesForProvider", providerName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void CreateSensor(string sensorName, string sensorId, string description, Communication.ConnectionInformation connInfo, Security.AuthenticationInformation authInfo, ItemStartupType startup)
        {
            Communication.ConnectionInformation connInfoCopy = (Communication.ConnectionInformation)SerializationHelper.CopiedObject(connInfo);
            Security.AuthenticationInformation authInfoCopy = (Security.AuthenticationInformation)SerializationHelper.CopiedObject(authInfo);
            callMethod<SensorDeviceEntity>(SensorManager, "CreateSensor", sensorName, sensorId, description, connInfoCopy, authInfoCopy, startup);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void DeleteSensor(string sensorName)
        {
            callMethod(SensorManager, "DeleteItem", sensorName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public IEnumerable<SensorDeviceEntity> GetSensorDevices()
        {
            return callMethod<IEnumerable<SensorDeviceEntity>>(SensorManager, "GetCurrentEntityList");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void UpdateSensor(string sensorName, string sensorId, string description, SensorDeviceProperty properties)
        {
            SensorDeviceProperty copied = (SensorDeviceProperty)SerializationHelper.CopiedObject(properties);
            callMethod(SensorManager, "UpdateSensor", sensorName, sensorId, description, copied);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void ChangeSensorState(string sensorName, ItemState newState)
        {
            callMethod(SensorManager, "ChangeState", sensorName, newState);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public Dictionary<string, Sensors.Configuration.PropertyList> GetSensorSources(string sensorName)
        {
            return callMethod<Dictionary<string, PropertyList>>(SensorManager, "GetSources", sensorName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public IEnumerable<Logical2SensorBindingEntity> GetLogical2SensorBindings(string sensorName)
        {
            return callMethod<IEnumerable<Logical2SensorBindingEntity>>(SensorManager, "GetLogical2SensorBindings", sensorName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void UpdateLogical2SensorBindings(string sensorName, Logical2SensorBindingEntity[] bindings)
        {
            callMethod(SensorManager, "UpdateLogical2SensorBindings", sensorName, bindings);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void UpdateSensorWithBindings(string sensorName, string sensorId, string description, SensorDeviceProperty properties,
            Logical2SensorBindingEntity[] bindings)
        {
            callMethod(SensorManager, "UpdateWithBindings", sensorName, sensorId, description, properties, bindings);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void SetSensorProfile(string sensorName, string source, PropertyList properties)
        {
            this.SensorManager.SetSensorProfile(sensorName,source,properties);
        }

        #endregion

        #region logicalSensorManagement

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public IEnumerable<LogicalSensorEntity> GetLogicalSensors()
        {
            return callMethod<IEnumerable<LogicalSensorEntity>>(LogicalManager, "GetCurrentEntityList");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public LogicalSensorEntity CreateLogicalSensor(string logicalSensorName, string description, ItemStartupType startup)
        {
            return callMethod<LogicalSensorEntity>(LogicalManager, "CreateLogicalSensor", logicalSensorName, description, startup);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public LogicalSensorEntity GetLogicalSensor(string logicalSensorName)
        {
            return callMethod<LogicalSensorEntity>(LogicalManager, "GetEntity", logicalSensorName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void ChangeLogicalSensorState(string logicalSensorName, ItemState newState)
        {
            callMethod(LogicalManager, "ChangeState", logicalSensorName, newState);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void UpdateLogicalSensor(string logicalSensorName, string description, LogicalSensorProperty properties)
        {
            callMethod(LogicalManager, "Update", logicalSensorName, description, properties);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void DeleteLogicalSensor(string logicalSensorName)
        {
            callMethod(LogicalManager, "DeleteItem", logicalSensorName);
        }

        public IEnumerable<Logical2SensorBindingEntity> GetSensor2LogicalBindings(string logicalSensorName)
        {
            return LogicalManager.GetSensor2LogicalBindings(logicalSensorName);
        }

        #endregion

        #region processorManagement

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public ProcessorEntity CreateProcessor(string processorName, string description, ItemStartupType startup)
        {
            return callMethod<ProcessorEntity>(ProcessManager, "CreateProcessor", processorName, description, startup);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public IEnumerable<ProcessorEntity> GetProcessors()
        {
            return callMethod<IEnumerable<ProcessorEntity>>(ProcessManager, "GetCurrentEntityList");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void DeleteProcessor(string processorName)
        {
            callMethod(ProcessManager, "DeleteItem", processorName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void UpdateProcessor(string processorName, string description, ProcessorProperty properties)
        {
            callMethod(ProcessManager, "Update", processorName, description, properties);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void ChangeProcessorState(string processorName, ItemState newState)
        {
            callMethod(ProcessManager, "ChangeState", processorName, newState);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public ProcessorEntity GetProcessor(string processorName)
        {
            return callMethod<ProcessorEntity>(ProcessManager, "GetEntity", processorName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public ProcessorMetadata GetProcessorMetadata(string processorName)
        {
            return callMethod<ProcessorMetadata>(ProcessManager, "GetMetadata", processorName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public IEnumerable<Processor2ModuleBindingEntity> GetProcessor2ModuleBindings(string processorName)
        {
            return callMethod<IEnumerable<Processor2ModuleBindingEntity>>(ProcessManager, "GetModuleBindings", processorName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void UpdateProcessor2ModuleBindings(string processorName, Processor2ModuleBindingEntity[] bindings)
        {
            callMethod(ProcessManager, "UpdateModuleBindings", processorName, bindings);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public IEnumerable<Logical2ProcessorBindingEntity> GetProcessor2LogicalSensorBindings(string processorName)
        {
            return callMethod<IEnumerable<Logical2ProcessorBindingEntity>>(ProcessManager, "GetLogicalSensorBindings", processorName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void UpdateProcessor2LogicalSensorBindings(string processorName, Logical2ProcessorBindingEntity[] bindings)
        {
            callMethod(ProcessManager, "UpdateLogicalSensorBindings", processorName, bindings);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void UpdateProcessorWithBindings(string processorName, string description, ProcessorProperty properties, Processor2ModuleBindingEntity[] moduleBindings, Logical2ProcessorBindingEntity[] logicalSensorBindings)
        {
            callMethod(ProcessManager, "UpdateWithBindings", processorName, description, properties, moduleBindings, logicalSensorBindings);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void ChangeProcessorModuleState(string processorName, string moduleName, ItemState newState)
        {
            callMethod(ProcessManager, "ChangeProcessorModuleState", processorName, moduleName, newState);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void ChangeProcessorLogicalSensorBindingState(string processorName, string logicalSensorName, ItemState newState)
        {
            callMethod(ProcessManager, "ChangeLogicalSensorBindingState", processorName, logicalSensorName, newState);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void SetEventModuleProfile(string processor, string module, PropertyList properties)
        {
            callMethod(ProcessManager, "SetModuleProfile", processor, module, properties);
        }

        #endregion

        #region eventModuleManagement

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public EventModuleEntity GetEventModule(string eventModuleName)
        {
            return callMethod<EventModuleEntity>(EventModuleManager, "GetEntity", eventModuleName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void ChangeEventModuleState(string eventModuleName, ItemState newState)
        {
            callMethod(EventModuleManager, "ChangeState", eventModuleName, newState);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public IEnumerable<EventModuleEntity> GetEventModules()
        {

            //(Thread.CurrentPrincipal.Identity as WindowsIdentity).Groups.ToList().ForEach(p => Console.WriteLine(p.Translate(typeof(NTAccount))));

            return callMethod<IEnumerable<EventModuleEntity>>(EventModuleManager, "GetCurrentEntityList");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public EventModuleEntity CreateEventModule(string eventModuleName, string description, string type, ItemStartupType startup)
        {
            return callMethod<EventModuleEntity>(EventModuleManager, "CreateEventModule", eventModuleName, description, type, startup);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void DeleteEventModule(string eventModuleName)
        {
            callMethod(EventModuleManager, "DeleteItem", eventModuleName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void UpdateEventModule(string eventModuleName, string description, string type, EventModuleProperty properties)
        {
            callMethod(EventModuleManager, "Update", eventModuleName, description, type, properties);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public EventModuleMetadata GetEventModuleMetadata(string eventModuleName)
        {
            return callMethod<EventModuleMetadata>(EventModuleManager, "GetMetadata", eventModuleName);
        }

        #endregion

        #region dispatcherManagement

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public IEnumerable<DispatcherEntity> GetDispatchers()
        {
            return callMethod<IEnumerable<DispatcherEntity>>(DispatcherManager, "GetCurrentEntityList");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public DispatcherEntity GetDispatcher(string dispatcherName)
        {
            //return this.DispatcherManager.GetEntity(dispatcherName);
            return callMethod<DispatcherEntity>(DispatcherManager, "GetEntity", dispatcherName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void ChangeDispatcherState(string dispatcherName, ItemState newState)
        {
            //this.DispatcherManager.ChangeState(dispatcherName, newState);
            callMethod(DispatcherManager, "ChangeState", dispatcherName, newState);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public DispatcherEntity CreateDispatcher(string dispatcherName, string description, string type, ItemStartupType startup)
        {
            return callMethod<DispatcherEntity>(DispatcherManager, "Create", dispatcherName, description, type, startup);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void DeleteDispatcher(string dispatcherName)
        {
            callMethod(DispatcherManager, "DeleteItem", dispatcherName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void UpdateDispatcher(string dispatcherName, string description, string type, DispatcherProperty properties)
        {
            callMethod(DispatcherManager, "Update", dispatcherName, description, type, properties);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public DispatcherMetadata GetDispatcherMetadata(string dispatcherName)
        {
            return callMethod<DispatcherMetadata>(DispatcherManager, "GetMetadata", dispatcherName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public IEnumerable<Dispatcher2ProcessorBindingEntity> GetDispatcher2ProcessorBindings(string dispatcherName)
        {
            return callMethod<IEnumerable<Dispatcher2ProcessorBindingEntity>>(DispatcherManager, "GetProcessorBindings", dispatcherName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void UpdateDispatcher2ProcessorBindings(string dispatcherName, Dispatcher2ProcessorBindingEntity[] bindings)
        {
            callMethod(DispatcherManager, "UpdateProcessorBindings", dispatcherName, bindings);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void UpdateDispatcherWithBindings(string dispatcherName, string description, string type, DispatcherProperty properties, Dispatcher2ProcessorBindingEntity[] processorBindings)
        {
            callMethod(DispatcherManager, "UpdateWithBindings", dispatcherName, description, type, properties, processorBindings);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void ChangeDispatcherProcessorBindingState(string dispatcherName, string processorName, ItemState newState)
        {
            callMethod(DispatcherManager, "ChangeProcessorBindingState", dispatcherName, processorName, newState);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void SetDispatcherProfile(string dispatcher, PropertyList properties)
        {
            callMethod(DispatcherManager, "SetDispatcherProfile", dispatcher, properties);
        }

        #endregion

        #region general

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public ExtendedMetadata GetItemExtendedMetadata(ProcessingItem itemType)
        {
            return callMethod<ExtendedMetadata>(ServerManager, "GetItemExtendedMetadata", itemType);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public Dictionary<PropertyKey, EntityMetadata> GetItemDefaultMetadata(ProcessingItem itemType, string itemName)
        {
            return callMethod<Dictionary<PropertyKey, EntityMetadata>>(ServerManager, "GetItemDefaultMetadata", itemType, itemName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public LogQueryResult GetItemLog(ProcessingItem itemType, string itemName, LogQuery logQuery)
        {
            return callMethod<LogQueryResult>(ServerManager, "GetItemLog", itemType, itemName, logQuery);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public IList<LastEvent> GetLastEvents(ProcessingItem itemType, string itemName)
        {
            return callMethod<IList<LastEvent>>(ServerManager, "GetLastEvents", itemType, itemName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        public void SetLastEventFilter(ProcessingItem itemType, string itemName, LastEventFilter filter)
        {
            callMethod(ServerManager, "SetLastEventFilter", itemType, itemName, filter);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public LastEventFilter GetLastEventFilter(ProcessingItem itemType, string itemName)
        {
            return callMethod<LastEventFilter>(ServerManager, "GetLastEventFilter", itemType, itemName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public Dictionary<ProcessingItem, IEnumerable<string>> GetLogSources()
        {
            return callMethod<Dictionary<ProcessingItem, IEnumerable<string>>>(this.ServerManager, "GetLogSources"); ;
        }

        #endregion

        #region watch

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public NameDescriptionList GetServerWatcherCategoryNames(string watch, ServerAnalyseItem related)
        {
            return callMethod<NameDescriptionList>(ServerManager, "GetServerWatcherCategoryNames", watch, related);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public NameDescriptionList GetServerWatcherNames()
        {
            return callMethod<NameDescriptionList>(ServerManager, "GetServerWatcherNames");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public NameDescriptionList GetServerWatcherCategories(string watch)
        {
            return callMethod<NameDescriptionList>(ServerManager, "GetServerWatcherCategories", watch);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public NameDescriptionList GetServerWatcherInstanceNames(string watch, string category)
        {
            return callMethod<NameDescriptionList>(ServerManager, "GetServerWatcherInstanceNames", watch, category);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public NameDescriptionList GetServerWatcherMeasureNames(string watch, string category)
        {
            return callMethod<NameDescriptionList>(ServerManager, "GetServerWatcherMeasureNames", watch, category);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.AdminGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.UserGroup)]
        [PrincipalPermission(SecurityAction.Demand, Role = SensorRoles.GuestGroup)]
        public float[] GetServerWatcherMeasureValues(string watch, string category, string instance, string[] measureNames)
        {
            return callMethod<float[]>(ServerManager, "GetServerWatcherMeasureValues", watch, category, instance, measureNames);
        }

        #endregion



        #region IManagementService Members


        public void SetSensorProviderProfile(string providerName, PropertyList properties)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
