namespace Kalitte.Sensors.Configuration
{

    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Utilities;

    [Serializable, KnownType("GetKnownTypes")]
    public sealed class EntityProperty
    {
        private readonly PropertyKey propertyKey;
        private readonly object propertyValue;

        public void SetObjectPropertyValue(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            var properties = t.GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(PropertyKeyBindingAttribute), true);
                if (attributes.Length > 0)
                {
                    PropertyKeyBindingAttribute attribute = attributes[0] as PropertyKeyBindingAttribute;
                    if (attribute.BindingKey == this.propertyKey)
                        property.SetValue(obj, this.propertyValue, null);
                }
                else throw new ArgumentException(string.Format("Type {0} doesnot have any properties bindable.", obj.GetType()));
            }
        }

        public EntityProperty(PropertyKey propertyKey, object propertyValue)
        {
            this.propertyKey = propertyKey;
            this.propertyValue = propertyValue;
            this.ValidateParameters();
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<entityProperty>");
            builder.Append("<propertyKey>");
            builder.Append(this.propertyKey);
            builder.Append("</propertyKey>");
            builder.Append("<propertyValue>");
            builder.Append(this.propertyValue);
            builder.Append("</propertyValue>");
            builder.Append("</entityProperty>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (null == this.propertyKey)
            {
                throw new ArgumentNullException("propertyKey.PropertyName");
            }
            if (!TypesHelper.IsKnownTypeObject(this.propertyValue))
            {
                throw new ArgumentException("ObjectTypeNotSupported(this.propertyValue.GetType().Name)", "propertyValue");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public PropertyKey Key
        {
            get
            {
                return this.propertyKey;
            }
        }

        public object PropertyValue
        {
            get
            {
                return this.propertyValue;
            }
        }
    }
}
