using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events;
using System.ServiceModel;

namespace Kalitte.Sensors.Interfaces
{
    [ServiceContract(Namespace = "http://kalitte.sensors/")]
    public interface IEventDispatcher
    {
        [OperationContract(IsOneWay = true)]
        void Notify(string source, SensorEventBase sensorEvent);
    }
}
