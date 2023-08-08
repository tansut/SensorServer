using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.Core.Process;
using Kalitte.Sensors.Processing.Core.Sensor;
using Kalitte.Sensors.Processing.Core.Dispatch;
using Kalitte.Sensors.Processing.ServerAnalyse;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Processing.Core
{
    internal abstract class OperationManagerBase : IDisposable
    {
        protected ManualResetEvent startupDone = new ManualResetEvent(false);
        protected volatile bool isShuttingDown;
        protected ILogger logger;
        protected internal ItemMetadataManager itemMetadataManager;
        private object m_lock = new object();


        public ServerAnalyseManager WatchManager
        {
            get
            {
                return ServerManager.ServerAnalyseManager;
            }
        }

        public ServerManager ServerManager { get; internal set; }

        protected OperationManagerBase(ServerManager serverManager)
        {
            this.ServerManager = serverManager;
            this.itemMetadataManager = new ItemMetadataManager(serverManager);
        }

        public abstract ProcessingItem ItemType { get; }

        public abstract IList<LastEvent> GetLastEvents(string itemName);


        protected internal virtual Dictionary<PropertyKey, EntityMetadata> GetDefaultMetadata(string entityName)
        {
            return null;
        }


        protected internal SensorManager SensorManager
        {
            get
            {
                return ServerManager.SensorManager;
            }
        }

        protected internal SensorProviderManager ProviderManager
        {
            get
            {
                return ServerManager.SensorProviderManager;
            }
        }

        protected internal LogicalSensorManager LogicalManager
        {
            get
            {
                return ServerManager.LogicalSensorManager;
            }
        }

        protected internal ProcessorManager ProcessManager
        {
            get
            {
                return ServerManager.ProcessingManager;
            }
        }

        protected internal EventModuleManager EventModuleManager
        {
            get
            {
                return ServerManager.EventModuleManager;
            }
        }

        protected internal DispatcherManager DispatcherManager
        {
            get
            {
                return ServerManager.DispatcherManager;
            }
        }

        public virtual void Startup()
        {
            logger = ServerManager.Logger.GetLogger(this.GetType().Name);
            Logger.Verbose("{0} done startup.", this.GetType().Name);
        }

        public virtual void DelayedStartup()
        {
            startupDone.Set();

        }

        public ILogger Logger
        {
            get
            {
                return this.logger;
            }
        }

        public virtual void Shutdown()
        {
            Logger.Info("Shutdown {0}.", this.GetType().Name);
            startupDone.WaitOne();
            isShuttingDown = true;

        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.m_lock = null;
            }
        }

        ~OperationManagerBase()
        {
            this.Dispose(false);
        }

        #endregion

        internal abstract LastEventFilter GetLastEventFilter(string itemName);

        internal abstract void SetLastEventFilter(string itemName, LastEventFilter filter);
    }
}
