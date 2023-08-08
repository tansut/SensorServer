using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Events;

using System.Threading;
using System.Reflection;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Processing.ServerAnalyse;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;
using Kalitte.Sensors.Processing.ServerAnalyse.Context;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Core
{
    internal abstract class VirtualRunnable<E, T> : VirtualRunnableBase
        where E : PersistEntityBase
        where T : RunnableMarshallBase
    {
        public AppDomain Domain;
        protected abstract string DomainFriendlyName { get; }
        protected E Entity { get; private set; }
        T marshallObj;
        protected RunnableEventHandler eventMarshall;
        INotificationErrorHandler ErrorHandler;
        OperationManagerBase manager;


        protected virtual object[] GetConstructorParamsOfMarshall()
        {
            return new object[] { Entity };
        }

        protected virtual T CreateMarshall()
        {
            Type t = typeof(T);

            var obj = (T)Domain.CreateInstanceAndUnwrap(
                t.Assembly.FullName,
                t.FullName, false, System.Reflection.BindingFlags.CreateInstance,
                null, GetConstructorParamsOfMarshall(), null, null);

            return obj;
        }



        public virtual void CreateMarshallObject()
        {
            this.marshallObj = CreateMarshall();
            this.marshallObj.ExceptionEvent += new EventHandler<ExceptionEventArgs>(this.eventMarshall.ExceptionEvent);
            this.marshallObj.ShortLogEvent+=new EventHandler<ShortLogEventArgs>(this.eventMarshall.ShortLogEvent);
            this.marshallObj.ModuleStartEvent += new EventHandler<ModuleStartEventArgs>(this.eventMarshall.ModuleStartEvent);
            this.marshallObj.ModuleNotificationEvent+=new EventHandler<ModuleNotifyEventArgs>(this.eventMarshall.ModuleNotifyEvent);
            this.marshallObj.ModuleStopEvent += new EventHandler<ModuleStopEventArgs>(this.eventMarshall.ModuleStopEvent);
            this.marshallObj.SetModulePropertyFromModuleEvent += new EventHandler<SetPropertyEventArgs>(this.eventMarshall.SetModulePropertyEvent);
        }



        public VirtualRunnable(E entity, INotificationErrorHandler ErrorHandler, OperationManagerBase manager)
        {
            this.manager = manager;
            this.Entity = entity;
            this.ErrorHandler = ErrorHandler;
            this.Domain = MarshalHelper.CreateAppDomanin(DomainFriendlyName);
            EventHandler<ExceptionEventArgs> exceptionCallback = new EventHandler<ExceptionEventArgs>(this.OnRunnebleException);
            EventHandler<ShortLogEventArgs> shortLogCallback = new EventHandler<ShortLogEventArgs>(this.OnShortLog);
            EventHandler<ModuleNotifyEventArgs> moduleNotifyCallback = new EventHandler<ModuleNotifyEventArgs>(this.OnModuleNotify);
            EventHandler<ModuleStartEventArgs> moduleStartCallback = new EventHandler<ModuleStartEventArgs>(this.OnModuleStart);
            EventHandler<ModuleStopEventArgs> moduleStopCallback = new EventHandler<ModuleStopEventArgs>(this.OnModuleStop);
            EventHandler<SetPropertyEventArgs> setpropertyCallBack = new EventHandler<SetPropertyEventArgs>(this.OnModuleSetProperty);
            this.eventMarshall = new RunnableEventHandler(exceptionCallback, shortLogCallback, moduleStartCallback, moduleNotifyCallback, moduleStopCallback, setpropertyCallBack);
        }

        public virtual void OnShortLog(object sender, ShortLogEventArgs e)
        {

        }

        public virtual void OnModuleNotify(object sender, ModuleNotifyEventArgs e)
        {
           
        }

        public virtual void OnModuleStart(object sender, ModuleStartEventArgs e)
        {

        }

        public virtual void OnModuleStop(object sender, ModuleStopEventArgs e)
        {

        }

        public virtual void OnModuleSetProperty(object sender, SetPropertyEventArgs e)
        {

        }

        public virtual void OnRunnebleException(object sender, ExceptionEventArgs args)
        {
            ErrorHandler.HandleError(sender, args);
        }

        protected virtual void StartDone(AnalyseContext context)
        {
        }

        protected virtual void StopDone(AnalyseContext context)
        {
        }

        protected virtual void NotificationDone(string source, SensorEventBase evt, AnalyseContext context)
        {
        }

        public override void Start()
        {
            if (marshallObj != null)
            {
                var watchContext = ServerAnalyseManager.CreateContext<DurationAnalyseContext>();
                marshallObj.Start();
                StartDone(watchContext);
                

            }
        }

        public override void SetModuleProperties(string module, PropertyList properties)
        {
            marshallObj.SetModuleProperties(module, properties);
        }

        public override SensorException Stop()
        {
            if (marshallObj != null)
            {
                var watchContext = ServerAnalyseManager.CreateContext<DurationAnalyseContext>();
                SensorException exc = marshallObj.Stop();
                StopDone(watchContext);
                return exc;
            }
            else return null;
        }

        public override void Notify(string source, SensorEventBase evt)
        {
            if (marshallObj != null)
            {
                var watchContext = ServerAnalyseManager.CreateContext<DurationAnalyseContext>();
                marshallObj.Notify(source, evt);
                NotificationDone(source, evt, watchContext);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (marshallObj != null)
                        marshallObj.Dispose();
                    marshallObj = null;

                    int tryCount = 0;
                    while (++tryCount < 5)
                    {
                        try
                        {
                            AppDomain.Unload(Domain);
                            break;
                        }
                        catch (CannotUnloadAppDomainException exc)
                        {
                            if (tryCount == 4)
                                throw;
                            manager.Logger.Error("Unable to unload appdomain {0}. Try count: {1}. Exception: {2}", 
                                AppDomain.CurrentDomain.FriendlyName, tryCount, exc);
                            Thread.Sleep(1000);
                        }
                    }

                    //throw new CannotUnloadAppDomainException();
                    GC.Collect(); 
                    GC.WaitForPendingFinalizers(); 
                    GC.Collect();
                }
                catch (ThreadAbortException)
                {
                }
            }
            base.Dispose(disposing);
        }
    }
}
