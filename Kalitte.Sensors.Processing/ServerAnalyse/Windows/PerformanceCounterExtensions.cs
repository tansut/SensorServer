using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Kalitte.Sensors.Processing.ServerAnalyse.Windows
{
    public static class PerformanceCounterExtensions
    {
        public static long IncrementQuick(this PerformanceCounter instance)
        {
            return ++instance.RawValue;
        }

        public static long IncrementByQuick(this PerformanceCounter instance, long delta)
        {
            return instance.RawValue+=delta;
        }
    }
}
