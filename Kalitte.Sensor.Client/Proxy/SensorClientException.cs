using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Client.Proxy
{
    [Serializable]
    public class SensorClientException : Exception
    {
        public SensorClientException() { }
        public SensorClientException(string message) : base(message) { }
        public SensorClientException(string message, Exception inner) : base(message, inner) { }
        protected SensorClientException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
