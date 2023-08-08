using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Processing
{
    [Serializable]
    public enum ProcessingItem
    {
        SensorProvider,
        SensorDevice,
        LogicalSensor,
        Processor,
        EventModule,
        Dispatcher,
        Server,
        None
    }
}
