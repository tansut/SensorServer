using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Security;
using System.Globalization;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.UI;

namespace Kalitte.Sensors.Configuration
{
    [Serializable, KnownType("GetKnownTypes"), DataContract]
    public class EntityMetadata
    {
        // Fields
        [DataMember]
        private readonly object defaultValue;
        [DataMember]
        private readonly string description;
        [DataMember]
        private readonly double higherRange;
        [DataMember]
        private readonly bool isMandatory;
        [DataMember]
        private readonly bool isPersistent;
        [DataMember]
        private readonly bool isWritable;
        [DataMember]
        private readonly double lowerRange;
        [DataMember]
        private readonly bool requiresRestart;
        private Type type;
        [DataMember]
        private readonly Regex valueExpression;
        [DataMember]
        private readonly Collection<object> valueSet;
        [DataMember]
        private readonly string xmlSchema;

        // Methods
        public EntityMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart)
        {
            this.lowerRange = double.MinValue;
            this.higherRange = double.MaxValue;
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            this.ValidateType(type);
            this.type = type;
            this.description = description;
            if (defaultValue != null)
            {
                validateDefaultOrThrow(defaultValue, type);
            }
            if ((defaultValue != null) && isMandatory)
            {
                throw new ArgumentException("MandatoryAndDefault");
            }
            this.defaultValue = defaultValue;
            if (((defaultValue == null) && !isMandatory) && type.IsValueType)
            {
                this.defaultValue = Activator.CreateInstance(type);
            }
            this.isWritable = writable;
            this.isMandatory = isMandatory;
            this.isPersistent = isPersistent;
            this.requiresRestart = requiresRestart;
        }

        public EntityMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, Collection<object> valueSet)
            : this(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart)
        {
            if ((valueSet == null) || (valueSet.Count == 0))
            {
                throw new ArgumentNullException("valueSet");
            }
            foreach (object obj2 in valueSet)
            {
                if (obj2 == null)
                {
                    if (type.IsPrimitive)
                    {
                        throw new ArgumentException("ValueSetForPrimitiveContainsNull");
                    }
                }
                else
                {
                    validateValueOrThrow(obj2, type);
                }
            }
            if ((defaultValue != null) && !valueSet.Contains(defaultValue))
            {
                throw new ArgumentException("ValueSetNotContainsDefault");
            }
            this.valueSet = valueSet;
        }

        public EntityMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, string xmlSchema)
            : this(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart)
        {
            if (xmlSchema == null)
            {
                throw new ArgumentNullException("xmlSchema");
            }
            this.xmlSchema = xmlSchema;
        }

        public EntityMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, Regex valueExpression)
            : this(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart)
        {
            if (valueExpression == null)
            {
                throw new ArgumentNullException("valueExpression");
            }
            this.valueExpression = valueExpression;
        }

        public EntityMetadata(Type type, string description, object defaultValue, bool writable, bool isMandatory, bool isPersistent, bool requiresRestart, double lowerRange, double higherRange)
            : this(type, description, defaultValue, writable, isMandatory, isPersistent, requiresRestart)
        {
            if (lowerRange >= higherRange)
            {
                throw new ArgumentException("InvalidLowerRangeHigherRange");
            }
            if (defaultValue != null)
            {

                if (!TypesHelper.IsNumericType(defaultValue.GetType()))
                {
                    throw new ArgumentException("NoRangeForNonnumericDefaultValue");
                }
                double num = double.Parse(defaultValue.ToString(), CultureInfo.CurrentCulture);
                if ((num < lowerRange) || (num > higherRange))
                {
                    throw new ArgumentException("DefaultValueNotInRange");
                }
            }
            this.lowerRange = lowerRange;
            this.higherRange = higherRange;
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<entityMetaData>");
            builder.Append("<description>");
            builder.Append((this.description == null) ? "" : this.description);
            builder.Append("</description>");
            builder.Append("<type>");
            builder.Append(this.type);
            builder.Append("</type>");
            builder.Append("<lowerRange>");
            builder.Append(this.lowerRange);
            builder.Append("</lowerRange>");
            builder.Append("<higherRange>");
            builder.Append(this.higherRange);
            builder.Append("</higherRange>");
            builder.Append("<valueExpression>");
            builder.Append(this.valueExpression);
            builder.Append("</valueExpression>");
            builder.Append("<valueSet>");
            if (this.valueSet != null)
            {
                foreach (object obj2 in this.valueSet)
                {
                    builder.Append("<value>");
                    builder.Append(obj2);
                    builder.Append("</value>");
                }
            }
            builder.Append("</valueSet>");
            builder.Append("<isWritable>");
            builder.Append(this.isWritable);
            builder.Append("</isWritable>");
            builder.Append("<isMandatory>");
            builder.Append(this.isMandatory);
            builder.Append("</isMandatory>");
            builder.Append("<requiresRestart>");
            builder.Append(this.requiresRestart);
            builder.Append("</requiresRestart>");
            builder.Append("<isPersistent>");
            builder.Append(this.isPersistent);
            builder.Append("</isPersistent>");
            builder.Append("<xmlSchema>");
            builder.Append(this.xmlSchema);
            builder.Append("</xmlSchema>");
            builder.Append("<defaultValue>");
            builder.Append(this.defaultValue);
            builder.Append("</defaultValue>");
            builder.Append("</entityMetaData>");
            return builder.ToString();
        }

        private static void validateDefaultOrThrow(object value, Type type)
        {
            if (!type.IsAssignableFrom(value.GetType()))
            {
                throw new ArgumentException("InvalidObjectType:" + value.GetType().FullName, "type");
            }
        }

        protected virtual void ValidateType(Type type)
        {
            if (!TypesHelper.IsKnownType(type) && (typeof(SecureString) != type))
            {
                throw new ArgumentException("ObjectTypeNotSupported(" + type.Name + ")", "type");
            }
        }

        private static void validateValueOrThrow(object value, Type type)
        {
            if (type.IsArray && !type.GetElementType().IsAssignableFrom(value.GetType()))
            {
                throw new ArgumentException("InvalidElementType(value.GetType().FullName, type.GetElementType())");
            }
            if (!type.IsArray && !type.IsAssignableFrom(value.GetType()))
            {
                throw new ArgumentException("InvalidObjectType(value.GetType().FullName, type)");
            }
        }

        public bool HasCustomPropertyEditor()
        {
            return GetPropertyEditorAttribute() != null;
        }

        public PropertyEditorAttribute GetPropertyEditorAttribute()
        {
            if (this.Type != null)
            {
                var list = type.GetCustomAttributes(typeof(PropertyEditorAttribute), true);
                if (list.Length > 0)
                    return (PropertyEditorAttribute)list[0];
                else return null;
            }
            else return null;
        }

        // Properties
        public object DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public double HigherRange
        {
            get
            {
                return this.higherRange;
            }
        }

        public bool IsMandatory
        {
            get
            {
                return this.isMandatory;
            }
        }

        public bool IsPersistent
        {
            get
            {
                return this.isPersistent;
            }
        }

        public bool IsWritable
        {
            get
            {
                return this.isWritable;
            }
        }

        public double LowerRange
        {
            get
            {
                return this.lowerRange;
            }
        }

        public bool RequiresRestart
        {
            get
            {
                return this.requiresRestart;
            }
        }

        public Type Type
        {
            get
            {
                return this.type;
            }
        }

        [DataMember]
        private string TypeForXml
        {
            get
            {
                return this.type.FullName;
            }
            set
            {
                this.type = Type.GetType(value);
                if (type == null)
                    type = TypesHelper.GetType(value);
            }
        }

        public Regex ValueExpression
        {
            get
            {
                return this.valueExpression;
            }
        }

        public ReadOnlyCollection<object> ValueSet
        {
            get
            {
                if (this.valueSet != null)
                {
                    return new ReadOnlyCollection<object>(this.valueSet);
                }
                return null;
            }
        }

        public string XmlSchema
        {
            get
            {
                return this.xmlSchema;
            }
        }
    }
}
