using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Security;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable]
    public class LogLevelInformation
    {
        public bool Inherit { get;  set; }
        public LogLevel Level { get;  set; }

        public LogLevelInformation(bool inherit, LogLevel level)
        {
            this.Inherit = inherit;
            this.Level = level;
        }
    }
}
