using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Security;
using System.Globalization;

namespace Kalitte.Sensors.Configuration
{

    [Serializable, DataContract]
    public sealed class DevicePropertyMetadata : EntityMetadata
    {
        // Fields
        [DataMember]
        private readonly SensorPropertyRelation propertyTargets;

        // Methods
        public DevicePropertyMetadata(Type type, string description, SensorPropertyRelation propertyTargets, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart)
        {
            this.propertyTargets = propertyTargets;
        }

        public DevicePropertyMetadata(Type type, string description, SensorPropertyRelation propertyTargets, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, Collection<object> valueSet)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart, valueSet)
        {
            this.propertyTargets = propertyTargets;
        }

        public DevicePropertyMetadata(Type type, string description, SensorPropertyRelation propertyTargets, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, string xmlSchema)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart, xmlSchema)
        {
            this.propertyTargets = propertyTargets;
        }

        public DevicePropertyMetadata(Type type, string description, SensorPropertyRelation propertyTargets, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, Regex valueExpression)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart, valueExpression)
        {
            this.propertyTargets = propertyTargets;
        }

        public DevicePropertyMetadata(Type type, string description, SensorPropertyRelation propertyTargets, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, double lowerRange, double higherRange)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart, lowerRange, higherRange)
        {
            this.propertyTargets = propertyTargets;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<sensorDevicePropertyMetadata>");
            builder.Append(base.ToString());
            builder.Append("<propertyTargets>");
            builder.Append(this.propertyTargets);
            builder.Append("</propertyTargets>");
            builder.Append("</sensorDevicePropertyMetadata>");
            return builder.ToString();
        }

        // Properties
        public SensorPropertyRelation PropertyTargets
        {
            get
            {
                return this.propertyTargets;
            }
        }
    }







}
