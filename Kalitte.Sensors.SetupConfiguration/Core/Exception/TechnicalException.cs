using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.SetupConfiguration.Core.Exception
{
    [Serializable]
    public class TechnicalException : System.Exception
    {
        public TechnicalException() { }
        public TechnicalException(string message) : base(message) { }
        public TechnicalException(string message, System.Exception inner) : base(message, inner) { }
        protected TechnicalException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
