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
    public class ServerConfigurationPropertyMetadata : EntityMetadata
    {
        public ServerConfigurationPropertyMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart)
        {
        }

        public ServerConfigurationPropertyMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, Collection<object> valueSet)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart, valueSet)
        {
        }

        public ServerConfigurationPropertyMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, string xmlSchema)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart, xmlSchema)
        {
        }

        public ServerConfigurationPropertyMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, Regex valueExpression)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart, valueExpression)
        {
        }

        public ServerConfigurationPropertyMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, double lowerRange, double higherRange)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart, lowerRange, higherRange)
        {
        }
    }
}
