using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Exceptions
{
    [Serializable, StrongNameIdentityPermission(SecurityAction.InheritanceDemand)]
    public abstract class RemoteObjectException : SensorException
    {
        // Methods
        public RemoteObjectException()
        {
        }

        public RemoteObjectException(string message)
            : base(message)
        {
        }

        protected RemoteObjectException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public RemoteObjectException(string message, Exception innerException)
            : this(message, innerException, null, null)
        {
        }

        public RemoteObjectException(string message, string errorCode, params object[] parameters)
            : base(message, errorCode, parameters)
        {
        }

        public RemoteObjectException(string message, Exception innerException, string errorCode, params object[] parameters)
            : base(message, innerException, errorCode, parameters)
        {
            this.SetDetailedErrorMessage();
            base.ClearInnerException();
        }

        protected void SetDetailedErrorMessage()
        {
            string detailedErrors;
            SensorCommon.GetDetailedErrorMessage(this, true, out detailedErrors);
            base.SetMessage(detailedErrors);
        }
    }

}
