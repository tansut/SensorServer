using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Processing.Metadata
{
    public interface ISupportsLog
    {
        LogLevelInformation LogLevel { get; }
    }
}
