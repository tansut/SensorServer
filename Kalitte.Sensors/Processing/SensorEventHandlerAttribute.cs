using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Processing
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class SensorEventHandlerAttribute : Attribute
    {
        readonly bool exactTypeMatch;

        public SensorEventHandlerAttribute(bool exactTypeMatch = false)
        {
            this.exactTypeMatch = exactTypeMatch;
        }

        public bool ExactTypeMatch
        {
            get
            {
                return exactTypeMatch;
            }
        }
    }
}
