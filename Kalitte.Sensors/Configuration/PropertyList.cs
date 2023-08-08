namespace Kalitte.Sensors.Configuration
{

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Utilities;

    [Serializable, KnownType("GetKnownTypes")]
    public sealed class PropertyList
    {
        private Dictionary<PropertyKey, object> dictionary;
        private readonly string name;

        public PropertyList()
        {
            this.name = "Noname";
            this.dictionary = new Dictionary<PropertyKey, object>();
        }

        public PropertyList(string name)
        {
            this.name = name;
            this.dictionary = new Dictionary<PropertyKey, object>();
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentNullException("name");
            }
            this.name = name;
        }

        public void Add(PropertyKey key, object value)
        {
            this[key] = value;
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public bool ContainsKey(PropertyKey key)
        {
            return this.dictionary.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<PropertyKey, object>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }

        public bool Remove(PropertyKey key)
        {
            return this.dictionary.Remove(key);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<propertyList>");
            builder.Append("<name>");
            builder.Append(this.name);
            builder.Append("</name>");
            builder.Append("<properties>");
            if (this.dictionary != null)
            {
                foreach (KeyValuePair<PropertyKey, object> pair in this.dictionary)
                {
                    builder.Append("<property>");
                    builder.Append("<groupName>");
                    builder.Append(pair.Key.GroupName);
                    builder.Append("</groupName>");
                    builder.Append("<name>");
                    builder.Append(pair.Key.PropertyName);
                    builder.Append("</name>");
                    builder.Append("<value>");
                    builder.Append(pair.Value);
                    builder.Append("</value>");
                    builder.Append("</property>");
                }
            }
            builder.Append("</properties>");
            builder.Append("</propertyList>");
            return builder.ToString();
        }

        public bool TryGetValue(PropertyKey key, out object value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }

        public object this[PropertyKey key]
        {
            get
            {
                return this.dictionary[key];
            }
            set
            {
                if (!TypesHelper.IsKnownTypeObject(value))
                {
                    throw new ArgumentException("ObjectTypeNotSupported(value.GetType().Name)", "PropertyProfile:Value");
                }
                this.dictionary[key] = value;
            }
        }

        public ICollection<PropertyKey> Keys
        {
            get
            {

                return this.dictionary.Keys;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return this.dictionary.Values;
            }
        }

        public PropertyList Clone()
        {
            var result = new PropertyList();
            result.dictionary = new Dictionary<PropertyKey, object>(this.dictionary);
            return result;
        }
    }
}
