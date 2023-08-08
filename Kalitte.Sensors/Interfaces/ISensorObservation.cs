using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Interfaces
{
    public interface ISensorObservation
    {
         string DeviceName { get; set; }
         string Source { get; set; }
    }
}
