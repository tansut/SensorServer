using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Exceptions
{


    [Serializable, StrongNameIdentityPermission(SecurityAction.InheritanceDemand)]
    public class DispatcherException : RemoteObjectException
    {

        public DispatcherException()
        {

        }



        public DispatcherException(string message)
            : base(message)
        {
        }



        protected DispatcherException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public DispatcherException(string message, Exception innerException)
            : base(message, innerException, null, null)
        {
        }

        public DispatcherException(string message, string errorCode, params object[] parameters)
            : base(message, errorCode, parameters)
        {
        }


    }

}
