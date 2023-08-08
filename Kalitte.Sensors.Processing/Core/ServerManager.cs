using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Security;
using System.IO;
using System.Threading;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Processing.Logging;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.Core.Sensor;
using Kalitte.Sensors.Processing.Core.Process;
using Kalitte.Sensors.Processing.Core.Dispatch;
using Kalitte.Sensors.Processing.ServerAnalyse;
using Kalitte.Sensors.Processing.Metadata;
using System.Xml;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Processing.Services;

namespace Kalitte.Sensors.Processing.Core
{
    public sealed class ServerManager
    {
        ManualResetEvent startupDone = new ManualResetEvent(false);
        private ItemMetadataManager metaDataManager;

        internal SensorManager SensorManager { get; private set; }
        internal SensorProviderManager SensorProviderManager { get; private set; }
        internal LogicalSensorManager LogicalSensorManager { get; private set; }
        internal ProcessorManager ProcessingManager { get; private set; }
        internal DispatcherManager DispatcherManager { get; private set; }
        internal ServiceManager ServiceManager { get; private set; }
        internal EventModuleManager EventModuleManager { get; private set; }
        internal ServerAnalyseManager ServerAnalyseManager { get; private set; }
        private Exception exceptionFromStartup;


        private Collection<OperationManagerBase> operationManagers;
        private Collection<OperationManagerBase> startedOperationManagers;

        internal SensorServerLogger Logger { get; private set; }

        private void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Exception exc = e.ExceptionObject as System.Exception;
            if (exc != null)
            {
                Console.WriteLine("Unhandled exception. {0}", exc.Message);
            }
            if (e.IsTerminating)
            {
                Console.WriteLine("Bad news. Terminating ...");
            }

        }
        public ServerManager()
            : this(false)
        {
        }

        private ServerManager(bool external)
        {
            this.Logger = new SensorServerLogger();
            this.metaDataManager = new ItemMetadataManager(this);
            this.ServerAnalyseManager = new ServerAnalyseManager();

            this.SensorManager = new SensorManager(this);
            this.SensorProviderManager = new SensorProviderManager(this);
            this.LogicalSensorManager = new LogicalSensorManager(this);
            this.ProcessingManager = new ProcessorManager(this);
            this.DispatcherManager = new DispatcherManager(this);
            if (!external)
                this.ServiceManager = new ServiceManager(this);
            this.EventModuleManager = new EventModuleManager(this);
            operationManagers = new Collection<OperationManagerBase>();
            startedOperationManagers = new Collection<OperationManagerBase>();
            operationManagers.Add(this.SensorProviderManager);
            operationManagers.Add(this.LogicalSensorManager);
            operationManagers.Add(this.EventModuleManager);
            operationManagers.Add(this.ProcessingManager);
            operationManagers.Add(this.DispatcherManager);
            operationManagers.Add(this.SensorManager);
            if (!external)
                operationManagers.Add(this.ServiceManager);
            Thread.CurrentThread.Name = "StartupServerThread";
        }

        internal static void StartExternallyHosted(Services.ServiceManager serviceManager)
        {
            ServerManager serverManager = new ServerManager(true);
            serverManager.ServiceManager = serviceManager;
            serviceManager.ServerManager = serverManager;
            serverManager.operationManagers.Add(serverManager.ServiceManager);
            serverManager.Startup(false);
        }


        public void Startup(bool waitForFullStartup = false)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.HandleUnhandledException);
            exceptionFromStartup = null;
            bool logStarted = false;
            try
            {
                try
                {
                    SensorServerConfigurationSection.Init();
                    var licenseManager = new SensorLicenseManager();
                    licenseManager.Validate();
                    Logger.Startup();
                    logStarted = true;
                }
                catch (Exception exc)
                {
                    throw new StartupException("Unable to start server", exc);
                }
                Logger.Info("Starting up ServerManager.");
                Logger.Info("Starting server watch managers");
                this.ServerAnalyseManager.Startup(Logger, ServerConfiguration.Current.WatchConfiguration);
                Logger.Info("Starting operation managers");
                foreach (var item in operationManagers)
                {
                    try
                    {
                        Logger.Info("Starting {0}", item.GetType().Name);
                        item.Startup();
                    }
                    catch (System.Exception exc)
                    {
                        Logger.LogException("Error starting operationbase {0}.", exc, item.GetType().Name);
                        throw new StartupException("Unable to start operation managers", exc);
                    }
                }
                Logger.Info("Startup done. Processing delayed startup.");
                new Thread(DelayedStartup).Start();
                if (waitForFullStartup)
                    startupDone.WaitOne();
                if (exceptionFromStartup != null)
                    throw exceptionFromStartup;
            }
            catch (Exception exc)
            {
                ExceptionManager.SaveExceptionToLog(exc);
                if (logStarted)
                    Logger.Shutdown();
                throw;
            }
        }

        private void DelayedStartup()
        {
            try
            {
                Thread.CurrentThread.Name = "ServerManagerDelayedStartup";
                foreach (var item in operationManagers)
                {
                    try
                    {
                        Logger.Info("Delayed starting {0}", item.GetType().Name);
                        item.DelayedStartup();
                        startedOperationManagers.Add(item);
                    }
                    catch (StartupException)
                    {
                        throw;
                    }
                    catch (System.Exception exc)
                    {
                        Logger.LogException("Error in delayed startup of {0}.", exc, item.GetType().Name);
                    }
                }
                Logger.Info("Server is ready.");
            }
            catch (StartupException exc)
            {
                ExceptionManager.SaveExceptionToLog(exc); 
                exceptionFromStartup = exc;
                startupDone.Set();
                Shutdown();
            }
            finally
            {
                startupDone.Set();
            }
        }

        public void Shutdown()
        {
            Logger.Info("Shutdown started for ServerManager.");
            startupDone.WaitOne();
            try
            {
                for (int i = startedOperationManagers.Count - 1; i >= 0; i--)
                {
                    var item = startedOperationManagers[i];
                    try
                    {
                        Logger.Info("Shutting down {0}", item.GetType().Name);
                        item.Shutdown();
                    }
                    catch (System.Exception exc)
                    {
                        Logger.LogException("Error shutdown operation base {0}.", exc, item.GetType().Name);
                    }
                }
                Logger.Info("Shutdown ServerManager done.");
                this.ServerAnalyseManager.Shutdown();
                this.Logger.Shutdown();
            }
            catch (Exception exc)
            {
                ExceptionManager.SaveExceptionToLog(exc);
            }
        }

        internal ExtendedMetadata GetItemExtendedMetadata(ProcessingItem itemType)
        {
            return metaDataManager.GetExtendedMetadata(itemType);
        }

        internal Dictionary<PropertyKey, EntityMetadata> GetItemDefaultMetadata(ProcessingItem itemType, string itemName)
        {
            foreach (var item in operationManagers)
            {
                if (item.ItemType == itemType)
                    return item.GetDefaultMetadata(itemName);
            }
            return null;
        }

        internal LogQueryResult GetItemLog(ProcessingItem itemType, string itemName, LogQuery query)
        {
            string itemLogPath = ServerConfiguration.GetItemLogPath(itemType, itemName);
            LogFileParser parser = new LogFileParser(itemLogPath, query);
            return parser.GetResult();


        }

        internal IList<LastEvent> GetLastEvents(ProcessingItem itemType, string itemName)
        {
            foreach (var item in operationManagers)
            {
                if (item.ItemType == itemType)
                    return item.GetLastEvents(itemName);
            }
            return null;
        }

        public void SetLastEventFilter(ProcessingItem itemType, string itemName, LastEventFilter filter)
        {
            foreach (var item in operationManagers)
            {
                if (item.ItemType == itemType)
                    item.SetLastEventFilter(itemName, filter);
            }
        }

        public LastEventFilter GetLastEventFilter(ProcessingItem itemType, string itemName)
        {
            foreach (var item in operationManagers)
            {
                if (item.ItemType == itemType)
                    return item.GetLastEventFilter(itemName);
            }
            return null;
        }

        internal Dictionary<ProcessingItem, IEnumerable<string>> GetLogSources()
        {
            var result = new Dictionary<ProcessingItem, IEnumerable<string>>();
            result.Add(ProcessingItem.Server, new List<string>());
            result.Add(ProcessingItem.SensorProvider, this.SensorProviderManager.GetCurrentEntityList().Select(p => p.Name).ToList());
            result.Add(ProcessingItem.Processor, this.ProcessingManager.GetCurrentEntityList().Select(p => p.Name).ToList());
            result.Add(ProcessingItem.Dispatcher, this.DispatcherManager.GetCurrentEntityList().Select(p => p.Name).ToList());
            return result;
        }

        public NameDescriptionList GetServerWatcherCategoryNames(string watch, ServerAnalyseItem related)
        {
            var watchInstance = this.ServerAnalyseManager.ValidateAndGet(watch);
            return watchInstance.GetCategoryNames(related);
        }

        public NameDescriptionList GetServerWatcherNames()
        {
            return this.ServerAnalyseManager.GetProviderNames();
        }

        public NameDescriptionList GetServerWatcherCategories(string watch)
        {
            var watchInstance = this.ServerAnalyseManager.ValidateAndGet(watch);
            return watchInstance.GetCategories();
        }

        public NameDescriptionList GetServerWatcherInstanceNames(string watch, string category)
        {
            var watchInstance = this.ServerAnalyseManager.ValidateAndGet(watch);
            return watchInstance.GetInstanceNames(category);
        }

        public NameDescriptionList GetServerWatcherMeasureNames(string watch, string category)
        {
            var watchInstance = this.ServerAnalyseManager.ValidateAndGet(watch);
            return watchInstance.GetMeasureNames(category);
        }

        public float[] GetServerWatcherMeasureValues(string watch, string category, string instance, string[] measureNames)
        {
            var watchInstance = this.ServerAnalyseManager.ValidateAndGet(watch);
            return watchInstance.GetMeasureValues(category, instance, measureNames);
        }





    }
}
