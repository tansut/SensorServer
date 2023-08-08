using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events;
using System.Reflection;
using System.Threading;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Core
{
    internal abstract class VirtualRunnableBase : IDisposable
    {
        private bool m_disposed;

        // Methods
        public void Dispose()
        {
            this.Dispose(true);
        }




        public abstract void Notify(string source, SensorEventBase evt);

        protected virtual void Dispose(bool disposing)
        {
            this.m_disposed = true;
        }

        public abstract void Start();

        public abstract SensorException Stop();

        public abstract void SetModuleProperties(string module, PropertyList properties);


    }
}
