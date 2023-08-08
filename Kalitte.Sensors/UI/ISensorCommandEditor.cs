using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Commands;

namespace Kalitte.Sensors.UI
{
    public interface ISensorCommandEditor
    {
        SensorCommand CreateCommand(string sensorName, string source);
        void ShowResponse(ResponseEventArgs e);
    }
}
