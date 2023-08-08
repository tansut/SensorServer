using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Processing.Metadata;
using System.Threading;
using Kalitte.Sensors.Exceptions;
using System.Diagnostics;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Processing.Core.Process;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.ServerAnalyse;
using System.Collections;

namespace Kalitte.Sensors.Processing.Core
{
    public abstract class RunnableMarshall<E, M, R, MR> : RunnableMarshallBase
        where E : PersistEntityBase, ISupportsLog
        where M : PersistEntityBase, ICanInstanceCreate
        where R : PersistEntityBase, IEntityPropertyProvider
        where MR : VirtualModuleBase
    {
        public class SensorEventInfo
        {
            public string Source { get; private set; }
            public SensorEventBase Event { get; private set; }
            public object SourceModule { get; private set; }

            public SensorEventInfo(string source, SensorEventBase evt)
                : this(source, evt, null)
            {
            }

            public SensorEventInfo(string source, SensorEventBase evt, object sourceModule)
            {
                this.Source = source;
                this.Event = evt;
                this.SourceModule = sourceModule;
            }
        }

        protected SafeDictionary<MR> Modules;
        protected ArrayList NotificationThreads = ArrayList.Synchronized(new ArrayList());
        protected internal E Entity { get; private set; }
        protected AutoResetEvent ThreadWaitOnNotification;
        protected long threadCountInNotificationPipe;
        protected volatile bool abortingNotificationThreads;
        protected volatile bool abortRequestedFromNotification;
        protected volatile bool domaninUnhandledExceptionReceived = false;
        protected Dictionary<string, object> InitItems;
        public bool WatchModuleNotification { get; protected set; }

        private List<MR> startedModules;

        public RunnableMarshall(E entity, Dictionary<string, object> initItems, ServerConfiguration configuration)
            : base()
        {
            this.Entity = entity;
            this.InitItems = initItems;
            Modules = new SafeDictionary<MR>();
            ThreadWaitOnNotification = new AutoResetEvent(false);
            abortingNotificationThreads = false;
            abortRequestedFromNotification = false;
            startedModules = new List<MR>();
            InitializeContext(configuration);
        }

        protected virtual void DoModuleSetPropertyHandler(object sender, R relation, EntityProperty property)
        {
        }

        internal virtual void SetPropertyFromModule(object sender, EntityProperty property)
        {
            var modules = Modules.GetCopiedList();
            R entity = null;
            foreach (var item in modules)
            {
                entity = (R)item.TryGetModuleNameFromInstance(sender);
                if (entity != null)
                    break;
            }

            if (entity != null)
                DoModuleSetPropertyHandler(sender, entity, property);
                
        }

        public ServerAnalyseLevel GetWatchLevel(ServerAnalyseItem item)
        {
            return ServerConfiguration.Current.WatchConfiguration.GetLevel(item);
        }

        public override void StopInternal()
        {
            AppContext.Logger.Info("Stopping runnable {0}. Waiting for notification pipe exit.", Entity.Name);
            bool pipeExit = ThreadWaitOnNotification.WaitOne(ServerConfiguration.Current.EventPipeTimeout);

            if (!pipeExit)
            {
                AbortNotificationThreads();
            }

            MultipleInnerException exc = null;

            foreach (KeyValuePair<string, MR> item in Modules)
            {
                try
                {
                    AppContext.Logger.Info("Stopping module {0}", item.Key);
                    TimeSpan span = StopModule(item.Value);
                    AppContext.Logger.Info("Stopped module {0} in {1}.", item.Key, span);
                }
                catch (System.Exception excThrown)
                {
                    AppContext.Logger.Error("Exception while stopping module {0}. {1}", item.Key, excThrown);
                    if (exc == null)
                        exc = new MultipleInnerException("Error in stopping modules. Forced dispose.");
                    exc.DetailedErrors.Add(CreateException("Stop exception", excThrown));
                }
            }

            if (!pipeExit)
            {
                if (exc == null)
                    exc = new MultipleInnerException("Timeout while stopping");
                exc.DetailedErrors.Add(CreateException("Stop forced for Notifications."));
            }

            if (exc != null)
            {
                AppContext.Logger.Warning("Got exception while trying to stop {0}. Throwing.", Entity.Name);
                throw exc;
            }
            else AppContext.Logger.Info("Stopped runnable {0}.", Entity.Name);
        }

        protected virtual TimeSpan StartModule(MR module)
        {
            module.Start();
            return TimeSpan.Zero;
        }

        protected virtual TimeSpan StopModule(MR module)
        {
            module.Stop();
            return TimeSpan.Zero;
        }

        public override void StartInternal()
        {
            AppContext.Logger.Info("Starting runnable {0}.", Entity.Name);
            abortingNotificationThreads = false;
            NotificationThreads.Clear();
            threadCountInNotificationPipe = 0;
            abortingNotificationThreads = false;
            abortRequestedFromNotification = false;
            ThreadWaitOnNotification.Reset();
            ThreadWaitOnNotification.Set();
            InitializeModules();
            MultipleInnerException mExc = null;
            try
            {
                foreach (KeyValuePair<string, MR> item in Modules)
                {
                    AppContext.Logger.Info("Starting module {0}", item.Key);
                    TimeSpan span = StartModule(item.Value);
                    AppContext.Logger.Info("Started module {0} in {1}.", item.Key, span);
                    startedModules.Add(item.Value);
                }
            }
            catch (System.Exception exc)
            {
                AppContext.Logger.Error("Exception while starting. Stopping already started {0} modules. Exception is: {1}", startedModules.Count, exc);
                foreach (var module in startedModules)
                {
                    try
                    {
                        module.Stop();
                    }
                    catch (System.Exception exc2)
                    {
                        AppContext.Logger.LogException("Error while stopping module. ", exc2);
                    }
                }
                if (mExc == null)
                    mExc = new MultipleInnerException("Exceptions while starting.");
                mExc.DetailedErrors.Add(CreateException("Start Exception", exc));
            }
            finally
            {
                startedModules.Clear();
            }
            if (mExc != null)
                throw mExc;
            AppContext.Logger.Info("Started runnable {0}.", Entity.Name);
        }


        protected void DoExceptionAsync(object state)
        {
            Thread.CurrentThread.Name = "MarshalException";
            System.Exception exc = (System.Exception)state;
            abortingNotificationThreads = true;
            DoException(exc);
        }

        private void AbortNotificationThreads()
        {
            abortingNotificationThreads = true;
            Array threads = NotificationThreads.ToArray(typeof(Thread));
            AppContext.Logger.Warning("Notification threads timeout to stop. Aborting {0} threads.", threadCountInNotificationPipe);
            foreach (var thread in threads)
            {
                try
                {

                    ((Thread)thread).Abort();
                }
                catch (System.Exception exc)
                {
                    AppContext.Logger.Warning("Got exception while aborting. {0}", exc);
                }
            }
            //NotificationThreads.Clear();

            AppContext.Logger.Info("Tried to abort. Current count of items in NotificationThreads: {0}", threadCountInNotificationPipe);
        }

        public abstract ProcessingItem ItemType { get; }


        protected virtual void InitializeContext(ServerConfiguration configuration)
        {
            ServerConfiguration.Init(configuration);
            AppContext.Initialize(ServerConfiguration.GetItemLogPath(ItemType, Entity.Name),
                Entity.Name, ServerConfiguration.Current, Entity.LogLevel.Inherit ? ServerConfiguration.Current.LogConfiguration.Level :
                Entity.LogLevel.Level);
        }

        protected virtual MR CreateVirtualModuleInstance(M module, R relation)
        {
            Type moduleType = typeof(MR);
            return (MR)Activator.CreateInstance(moduleType, module, relation);
        }

        protected virtual MR CreateVirtualModule(M module, R relation)
        {
            MR moduleInstance;
            try
            {
                moduleInstance = CreateVirtualModuleInstance(module, relation);
                moduleInstance.CreateModuleInstance();
            }
            catch (System.Exception)
            {
                throw;
            }

            Modules.Add(relation.Name, moduleInstance);
            return moduleInstance;
        }

        protected abstract void Notify(object state);

        protected void Notification(object state)
        {
            System.Exception exceptionInNotification = null;
            if (!abortingNotificationThreads)
            {
                Thread.BeginCriticalRegion();
                Interlocked.Increment(ref threadCountInNotificationPipe);
                NotificationThreads.Add(Thread.CurrentThread);
                Thread.EndCriticalRegion();
                try
                {
                    Notify(state);
                }
                catch (ThreadAbortException)
                {

                }
                catch (System.Exception exc)
                {
                    exceptionInNotification = exc;
                }
                finally
                {
                    Thread.BeginCriticalRegion();
                    NotificationThreads.Remove(Thread.CurrentThread);
                    if (Interlocked.Decrement(ref threadCountInNotificationPipe) == 0)
                        ThreadWaitOnNotification.Set();
                    Thread.EndCriticalRegion();
                    if (exceptionInNotification != null)
                    {
                        abortingNotificationThreads = true;
                        if (!abortRequestedFromNotification)
                        {
                            abortRequestedFromNotification = true;
                            if (!changingState)
                                ThreadPool.QueueUserWorkItem(DoExceptionAsync, exceptionInNotification);
                        }
                    }
                }
            }
        }

        public virtual void Stop(string reasonMessage)
        {
            SensorException stopException = CreateException(reasonMessage);
            ThreadPool.QueueUserWorkItem(DoExceptionAsync, stopException);
        }

        protected void QueNotification(object state)
        {
            if (!abortingNotificationThreads)
            {
                ThreadWaitOnNotification.Reset();
                Notification(state);
                //ThreadPool.QueueUserWorkItem(Notification, state);
            }
        }



        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    foreach (KeyValuePair<string, MR> item in Modules)
                        if (item.Value is IDisposable)
                            ((IDisposable)item.Value).Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }




        internal override void SetModuleProperties(string name, PropertyList properties)
        {
              var module = Modules.TryGetItem(name);
              if (module != null)
              {
                  foreach (var item in properties)
                  {
                      module.SetModuleProperty(new EntityProperty(item.Key, item.Value));
                  }
              }

                 
        }


    }
}
