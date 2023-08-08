﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace Kalitte.Sensors.Configuration
{
        [Serializable, DataContract]
    public class DispatcherPropertyMetadata: EntityMetadata
    {
            public DispatcherPropertyMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart)
        {
        }

        public DispatcherPropertyMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, Collection<object> valueSet)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart, valueSet)
        {
        }

        public DispatcherPropertyMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, string xmlSchema)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart, xmlSchema)
        {
        }

        public DispatcherPropertyMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, Regex valueExpression)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart, valueExpression)
        {
        }

        public DispatcherPropertyMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, double lowerRange, double higherRange)
            : base(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart, lowerRange, higherRange)
        {

        }
    }
}
