using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events;
using System.Threading;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Interfaces;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Processing.ServerAnalyse;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Core
{
    public abstract class RunnableMarshallBase : MarshalBase, IEventDispatcher
    {
        public event EventHandler<ExceptionEventArgs> ExceptionEvent;
        public event EventHandler<ShortLogEventArgs> ShortLogEvent;
        public event EventHandler<ModuleStartEventArgs> ModuleStartEvent;
        public event EventHandler<ModuleStopEventArgs> ModuleStopEvent;
        public event EventHandler<ModuleNotifyEventArgs> ModuleNotificationEvent;
        public event EventHandler<SetPropertyEventArgs> SetModulePropertyFromModuleEvent;


        protected volatile ItemState CurrentState;
        protected volatile bool changingState = false;

        protected abstract SensorException CreateException(string message, System.Exception exc, string relatedModule);

        protected SensorException CreateException(string message, string relatedModule)
        {
            return CreateException(message, null, relatedModule);
        }
        protected SensorException CreateException(string message)
        {
            return CreateException(message, null, null);
        }
        protected SensorException CreateException(string message, System.Exception exc)
        {
            return CreateException(message, exc, null);
        }

        public virtual void DoNotification(object sender, ModuleNotifyEventArgs e)
        {
            if (ModuleNotificationEvent != null)
            {
                ModuleNotificationEvent(null, e);
            }
        }

        internal abstract void SetModuleProperties(string module, PropertyList properties);


        public virtual void DoModuleStart(object sender, ModuleStartEventArgs e)
        {
            if (ModuleStartEvent != null)
            {
                ModuleStartEvent(null, e);
            }
        }

        public virtual void DoModuleStop(object sender, ModuleStopEventArgs e)
        {
            if (ModuleStopEvent != null)
            {
                ModuleStopEvent(null, e);
            }
        }

        public virtual void DoModuleSetProperty(object sender, SetPropertyEventArgs e)
        {
            if (SetModulePropertyFromModuleEvent != null)
            {
                SetModulePropertyFromModuleEvent(null, e);
            }
        }

        protected virtual void DoException(System.Exception exc)
        {
            if (ExceptionEvent != null)
            {
                SensorException ex = CreateException("Unhandled exception", exc);
                ExceptionEvent(null, new ExceptionEventArgs(ex));
            }
        }



        public abstract void Notify(string source, SensorEventBase evt);
        public abstract void StartInternal();
        public abstract void StopInternal();

        public void Start()
        {
            if (changingState)
                throw CreateException("There is already ongoing change state");
            changingState = true;
            try
            {
                if (CurrentState == ItemState.Stopped)
                {
                    this.StartInternal();
                }
                CurrentState = ItemState.Running;
            }

            finally
            {
                changingState = false;
            }
        }

        protected abstract void InitializeModules();

        public SensorException Stop()
        {
            if (changingState)
                throw CreateException("There is already ongoing  state change.");
            changingState = true;
            try
            {
                if (CurrentState == ItemState.Running)
                {
                    this.StopInternal();
                }
                CurrentState = ItemState.Stopped;
                return null;
            }
            catch (SensorException exc)
            {
                return exc;
            }
            catch (Exception exc)
            {
                return CreateException("Unknown stop exception {0}", exc);
            }
            finally
            {
                changingState = false;
            }

        }


    }
}
