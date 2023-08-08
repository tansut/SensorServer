using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Events;
using System.Threading;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Core
{
    public abstract class VirtualModuleBase: IDisposable
    {
        protected string TypeQ { get; private set; }

        protected object actualModuleInstance;
        protected object relationObj;

        public VirtualModuleBase(string type, object relation)
        {
            this.TypeQ = type;
            this.relationObj = relation;
        }

        public virtual void SetModuleProperty(EntityProperty property)
        {

        }

        internal object TryGetModuleNameFromInstance(object sender)
        {
            if (actualModuleInstance == sender)
                return relationObj;
            else return null;
        }

        public virtual void CreateModuleInstance()
        {
            Type actualModuletype = Type.GetType(TypeQ);
            if (actualModuletype == null)
                throw new SensorException(string.Format("Unable to get type {0}", TypeQ));
            try
            {
                actualModuleInstance = Activator.CreateInstance(actualModuletype);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        protected abstract internal void Stop();
        protected abstract internal void Start();

        #region IDisposable Members

        public void Dispose()
        {
            if (actualModuleInstance is IDisposable)
                ((IDisposable)actualModuleInstance).Dispose();
        }

        #endregion
    }
}
