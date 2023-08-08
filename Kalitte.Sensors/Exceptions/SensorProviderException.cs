using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Exceptions
{
    [Serializable, StrongNameIdentityPermission(SecurityAction.InheritanceDemand)]
    public class SensorProviderException : RemoteObjectException
    {
        // Methods
        public SensorProviderException()
        {
        }

        public SensorProviderException(string message)
            : base(message)
        {
        }

        protected SensorProviderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public SensorProviderException(string message, Exception innerException)
            : base(message, innerException, null, null)
        {
        }

        public SensorProviderException(string message, string errorCode, params object[] parameters)
            : base(message, errorCode, parameters)
        {
        }

        public SensorProviderException(string message, Exception innerException, string errorCode, params object[] parameters)
            : base(message, innerException, errorCode, parameters)
        {
        }
    }




}
