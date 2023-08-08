using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.SetupConfiguration.Core.Exception
{
    [Serializable]
    public class UserException : System.Exception
    {
        public string Detail { get; set; }
        public UserException() { }
        public UserException(string message) : base(message) { }
        public UserException(string message,string detail) : base(message) 
        {
            Detail = detail;
        }
        public UserException(string message, System.Exception inner) : base(message, inner) { }
        protected UserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
