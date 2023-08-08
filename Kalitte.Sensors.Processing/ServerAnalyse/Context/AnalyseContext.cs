using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Processing.ServerAnalyse.Context
{
    [Serializable]
    public abstract class AnalyseContext
    {
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceCounter(out long perfcount);
        public abstract void Done();
    }
}
