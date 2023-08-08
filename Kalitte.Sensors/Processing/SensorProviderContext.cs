using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Processing;

namespace Kalitte.Sensors.Processing
{

    public sealed class SensorProviderContext : SensorContext
    {
        public SensorProviderContext(Dictionary<string, object> contextObjects, ILogger logger)
            : base(contextObjects, logger)
        {

        }

        public static new SensorProviderContext Current
        {
            get
            {
                lock (currentLock)
                {
                    return (SensorProviderContext)current;
                }
            }
        }
    }
}
