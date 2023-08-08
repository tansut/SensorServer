using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Processing.ServerAnalyse.Context
{
    [Serializable]
    public class DurationAnalyseContext : AnalyseContext
    {
        long startTick, endTick;
        public DurationAnalyseContext()
            : base()
        {
            //startTick = DateTime.Now.Ticks;
            QueryPerformanceCounter(out startTick);
            //startTick = Environment.TickCount;
        }


        public override void Done()
        {
            QueryPerformanceCounter(out endTick);
            //endTick = DateTime.Now.Ticks;
            //endTick = Environment.TickCount;
        }

        public long Ticks
        {
            get
            {
                return endTick - startTick;
            }
        }

        public long Seconds
        {
            get
            {
                return Convert.ToInt64(Duration.TotalSeconds);
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromTicks(Ticks);
            }
        }

        public long MiliSeconds
        {
            get
            {
                return Convert.ToInt64(Duration.TotalMilliseconds);
            }
        }

    }
}
