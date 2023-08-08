using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Interfaces
{
    //[Serializable]
    //public enum ProcessorDispatchLevel
    //{
    //    AllPipeline,
    //    NextPipeline
    //}

    [ServiceContract(Namespace = "http://kalitte.sensors/")]
    public interface IProcessorDispatcher: IEventDispatcher
    {
        [OperationContract(IsOneWay = true)]
        void AddEventToNextPipe(object sender, string source, SensorEventBase sensorEvent);
    }
}
