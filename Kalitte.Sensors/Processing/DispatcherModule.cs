using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Interfaces;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Processing
{
    public abstract class DispatcherModule : IEventDispatcher, IDisposable
    {
        private object m_lock = new object();

        protected DispatcherModule()
        {
        }

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

        ~DispatcherModule()
        {
            this.Dispose(false);
        }



        public virtual object SyncRoot
        {
            get
            {
                return this.m_lock;
            }
        }

        public abstract void Startup(DispatcherContext context, string name, DispatcherModuleInformation dispatcherInformation);
        public abstract void SetProperty(EntityProperty property);
        public abstract void Shutdown();

        public abstract void Notify(string source, SensorEventBase sensorEvent);
    }
}
