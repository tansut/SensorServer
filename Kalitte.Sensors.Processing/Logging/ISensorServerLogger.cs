using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Exceptions;

namespace Kalitte.Sensors.Processing.Logging
{
    internal interface ISensorServerLogger
    {
        // Methods
        ILogger GetLogger(string name);
        void Report(SensorException sensorException);
        void Report(System.Exception exception);
    }

 

}
