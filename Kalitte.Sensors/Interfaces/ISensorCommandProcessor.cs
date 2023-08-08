using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Commands;
using System.ServiceModel;

namespace Kalitte.Sensors.Interfaces
{
    [ServiceContract(Namespace = "http://kalitte.sensors/")]
    public interface ISensorCommandProcessor
    {
        [OperationContract]
        ResponseEventArgs Execute(string sensorName, string source, SensorCommand command);
    }
}
