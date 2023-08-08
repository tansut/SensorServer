using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Kalitte.Sensors.Configuration
{
    [Serializable, DataContract]
    public class EventModulePropertyMetadata : EntityMetadata
    {

        public EventModulePropertyMetadata(Type type, string description, object defaultValue, bool isMandatory)
            : base(type, description, defaultValue, true, isMandatory, true, false)
        {

        }

        public EventModulePropertyMetadata(Type type, string description, object defaultValue, bool isMandatory, bool writable)
            : base(type, description, defaultValue, writable, isMandatory, true, false)
        {

        }

        public EventModulePropertyMetadata(Type type, string description, object defaultValue, bool isMandatory, Collection<object> valueSet)
            : base(type, description, defaultValue, true, isMandatory, true, false, valueSet)
        {

        }

        public EventModulePropertyMetadata(Type type, string description, object defaultValue, bool isMandatory, double lower, double upper)
            : base(type, description, defaultValue, true, isMandatory, true, false, lower, upper)
        {

        }

        public EventModulePropertyMetadata(Type type, string description, object defaultValue, bool isMandatory, double lower, double upper, bool writable)
            : base(type, description, defaultValue, writable, isMandatory, true, false, lower, upper)
        {

        }



    }
}
