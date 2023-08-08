using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Events
{
    [Serializable]
    public class GenericSensorEvent : SensorEventBase
    {
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<genericSensorEvent>");
            builder.Append(base.ToString());
            builder.Append("</genericSensorEvent>");
            return builder.ToString();
        }
    }
}
