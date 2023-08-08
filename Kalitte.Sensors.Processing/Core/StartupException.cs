using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Processing.Core
{
    [Serializable]
    public class StartupException : Exception
    {
        public StartupException() { }
        public StartupException(string message) : base(message) { }
        public StartupException(string message, Exception inner) : base(message, inner) { }
        protected StartupException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
