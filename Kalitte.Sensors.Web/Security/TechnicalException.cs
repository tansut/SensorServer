using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Web.Security
{
    [Serializable]
    public class TechnicalException : ApplicationException
    {
        public TechnicalException() { }
        public TechnicalException(string message) : base(message) { }
        public TechnicalException(string message, Exception inner) : base(message, inner) { }
        protected TechnicalException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
