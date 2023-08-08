using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Exceptions
{
    [Serializable]
    public sealed class ConnectionFailedException : SensorProviderException
    {
        // Methods
        public ConnectionFailedException()
        {
        }

        public ConnectionFailedException(string message)
            : base(message)
        {
        }

        private ConnectionFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ConnectionFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ConnectionFailedException(string message, string errorCode, params object[] parameters)
            : base(message, errorCode, parameters)
        {
        }

        public ConnectionFailedException(string message, Exception innerException, string errorCode, params object[] parameters)
            : base(message, innerException, errorCode, parameters)
        {
        }
    }



}
