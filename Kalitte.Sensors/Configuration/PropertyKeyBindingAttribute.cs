using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Configuration
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class PropertyKeyBindingAttribute : Attribute
    {
        public readonly PropertyKey BindingKey;

        public PropertyKeyBindingAttribute(string groupName, string propertyName)
        {
            this.BindingKey = new PropertyKey(groupName, propertyName);
        }

    }

}
