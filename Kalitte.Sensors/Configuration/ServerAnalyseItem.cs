using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public enum ServerAnalyseItem
    {
        SensorDevice,
        SensorProvider,
        LogicalSensor,
        Processor,
        EventModule,
        Dispatcher,
        EventType,
        ProcessorQue,
        DispatcherQue,
        System
    }
}
