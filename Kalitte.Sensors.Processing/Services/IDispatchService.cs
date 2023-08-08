using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Interfaces;
using System.ServiceModel;

namespace Kalitte.Sensors.Processing.Services
{
    [ServiceContract(Namespace = "http://kalitte.sensors/")]
    public interface IDispatchService: IEventDispatcher
    {
    }
}
