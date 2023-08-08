using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Events
{
    [Serializable, KnownType("GetSubTypes")]
    public sealed class LastEvent
    {
        private static IEnumerable<Type> GetSubTypes()
        {
            return TypesHelper.GetTypes(typeof(SensorEventBase));
        }

        public DateTime EventTime { get; private set; }
        public SensorEventBase Event { get; private set; }
        public string Source { get; private set; }

        public LastEvent(DateTime eventTime, string source, SensorEventBase sensorEvent)
        {
            this.EventTime = eventTime;
            this.Event = sensorEvent;
            this.Source = source;
        }

        public LastEvent(DateTime eventTime, SensorEventBase sensorEvent)
            : this(eventTime, null, sensorEvent)
        {
        }

        public string TypeName
        {
            get
            {
                return this.Event.GetType().Name;
            }
        }
    }
}
