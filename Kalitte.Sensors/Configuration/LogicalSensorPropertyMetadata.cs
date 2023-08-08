using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Kalitte.Sensors.Configuration
{
    [Serializable, DataContract]
    public class LogicalSensorPropertyMetadata : EntityMetadata
    {
        public LogicalSensorPropertyMetadata(Type type, string description, object defaultValue, bool isMandatory)
            : base(type, description, defaultValue, true, isMandatory, true, false)
        {

        }

        public LogicalSensorPropertyMetadata(Type type, string description, object defaultValue, bool isMandatory, Collection<object> valueSet)
            : base(type, description, defaultValue, true, isMandatory, true, false, valueSet)
        {

        }

        public LogicalSensorPropertyMetadata(Type type, string description, object defaultValue, bool isMandatory, double lower, double upper)
            : base(type, description, defaultValue, true, isMandatory, true, false, lower, upper)
        {

        }
    }
}
