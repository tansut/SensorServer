using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace Kalitte.Sensors.Configuration
{
    [Serializable, DataContract]
    public sealed class ProviderPropertyMetadata : EntityMetadata
    {
        // Fields
        [DataMember]
        private readonly bool isInitOnly;

        // Methods
        public ProviderPropertyMetadata(Type type, string description, object defaultValue, bool isInitOnly, bool isMandatory, bool requiresRestart)
            : base(type, description, defaultValue, true, isMandatory, false, requiresRestart)
        {
            if (requiresRestart && isInitOnly)
            {
                throw new ArgumentException("InitOnlyAndRequiresRestart");
            }
            this.isInitOnly = isInitOnly;
        }

        public ProviderPropertyMetadata(Type type, string description, object defaultValue, bool isInitOnly, bool isMandatory, bool requiresRestart, Collection<object> valueSet)
            : base(type, description, defaultValue, true, isMandatory, false, requiresRestart, valueSet)
        {
            if (requiresRestart && isInitOnly)
            {
                throw new ArgumentException("InitOnlyAndRequiresRestart");
            }
            this.isInitOnly = isInitOnly;
        }

        public ProviderPropertyMetadata(Type type, string description, object defaultValue, bool isInitOnly, bool isMandatory, bool requiresRestart, string xmlSchema)
            : base(type, description, defaultValue, true, isMandatory, false, requiresRestart, xmlSchema)
        {
            if (requiresRestart && isInitOnly)
            {
                throw new ArgumentException("InitOnlyAndRequiresRestart");
            }
            this.isInitOnly = isInitOnly;
        }

        public ProviderPropertyMetadata(Type type, string description, object defaultValue, bool isInitOnly, bool isMandatory, bool requiresRestart, Regex valueExpression)
            : base(type, description, defaultValue, true, isMandatory, false, requiresRestart, valueExpression)
        {
            if (requiresRestart && isInitOnly)
            {
                throw new ArgumentException("InitOnlyAndRequiresRestart");
            }
            this.isInitOnly = isInitOnly;
        }

        public ProviderPropertyMetadata(Type type, string description, object defaultValue, bool isInitOnly, bool isMandatory, bool requiresRestart, double lowerRange, double higherRange)
            : base(type, description, defaultValue, true, isMandatory, false, requiresRestart, lowerRange, higherRange)
        {
            if (requiresRestart && isInitOnly)
            {
                throw new ArgumentException("InitOnlyAndRequiresRestart");
            }
            this.isInitOnly = isInitOnly;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<ProviderPropertyMetadata>");
            builder.Append(base.ToString());
            builder.Append("<isInitOnly>");
            builder.Append(this.isInitOnly);
            builder.Append("</isInitOnly>");
            builder.Append("</ProviderPropertyMetadata>");
            return builder.ToString();
        }

        // Properties
        public bool IsInitOnly
        {
            get
            {
                return this.isInitOnly;
            }
        }
    }




}
