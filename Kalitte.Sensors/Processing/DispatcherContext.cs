using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing
{
    public abstract class DispatcherContext: SensorContext
    {
        public DispatcherContext(Dictionary<string, object> contextObjects, ILogger logger)
            : base(contextObjects, logger)
        {

        }

        public static new DispatcherContext Current
        {
            get
            {
                return (DispatcherContext)current;
            }
        }

        public abstract void Stop(string reasonMessage);
        public abstract void SetProperty(object sender, EntityProperty property);
    }
}
