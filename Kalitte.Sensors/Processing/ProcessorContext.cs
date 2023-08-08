using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Interfaces;
using Kalitte.Sensors.Security;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing
{
    public abstract class ProcessorContext : SensorContext, IProcessorDispatcher
    {
        public ProcessorContext(Dictionary<string, object> contextObjects, ILogger logger)
            : base(contextObjects, logger)
        {

        }

        public static new ProcessorContext Current
        {
            get
            {
                lock (currentLock)
                {
                    return (ProcessorContext)current;
                }
            }
        }


        public abstract void Notify(string source, SensorEventBase sensorEvent);
        public abstract void AddEventToNextPipe(object sender, string source, SensorEventBase sensorEvent);
        public abstract void NotifyImmediate(string source, SensorEventBase sensorEvent);
        public abstract void Stop(string reasonMessage);
        public abstract void SetProperty(object sender, EntityProperty property);
    }
}
