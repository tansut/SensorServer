using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Processing
{
    public interface ISensorContext
    {
        ILogger Logger { get; }
        //public ILogger GetLogger(string name);
    }
}
