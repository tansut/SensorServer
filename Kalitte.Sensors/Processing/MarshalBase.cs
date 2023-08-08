using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Runtime.Remoting.Lifetime;

namespace Kalitte.Sensors.Processing
{
    public class MarshalBase : MarshalByRefObject, IDisposable, ISponsor
    {
        // Fields
        private bool m_disposed;

        // Methods
        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.m_disposed = true;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (LeaseState.Initial == lease.CurrentState)
            {
                lease.Register(this);
            }
            return lease;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public TimeSpan Renewal(ILease lease)
        {
            if (lease != null)
            {
                if (!this.m_disposed)
                {
                    return lease.InitialLeaseTime;
                }
                lease.Unregister(this);
            }
            return TimeSpan.Zero;
        }
    }



}
