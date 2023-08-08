using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Interfaces;

namespace Kalitte.Sensors.Processing
{
    public abstract class SensorEventModule : IDisposable
    {
        private object m_lock = new object();

        protected SensorEventModule()
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

        ~SensorEventModule()
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


        public abstract void Startup(ProcessorContext context, string name, EventModuleInformation moduleInformation);
        public abstract void SetProperty(EntityProperty property);
        public abstract void Shutdown();
    }
}
