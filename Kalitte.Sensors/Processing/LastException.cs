using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Exceptions;
using System.ServiceModel;

namespace Kalitte.Sensors.Processing
{
    [Serializable]
    public class LastException
    {
        [DataMember]
        public DateTime TimeExceptionThrown { get; private set; }

        [DataMember]
        public string ExceptionType { get; private set; }

        [DataMember]
        public string Message { get; private set; }

        [DataMember]
        public string MessageWithDetails { get; private set; }

        public LastException(Exception exc)
        {
            this.ExceptionType = exc.GetType().FullName;
            TimeExceptionThrown = DateTime.Now;
            Message = exc.Message;
            string allMessages;
            SensorCommon.GetDetailedErrorMessage(exc, true, out allMessages);
            MessageWithDetails = allMessages;
        }
    }
}
