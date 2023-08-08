using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Core
{
    [Serializable, KnownType("GetKnownTypes")]
    public sealed class VendorData : ICloneable
    {
        // Fields
        private Dictionary<string, object> dictionary = new Dictionary<string, object>();

        // Methods
        public void Add(string key, object value)
        {
            ValidateKey(key);
            this[key] = value;
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public object Clone()
        {
            VendorData information = new VendorData();
            foreach (KeyValuePair<string, object> pair in this.dictionary)
            {
                information.Add(pair.Key, pair.Value);
            }
            return information;
        }

        public bool ContainsKey(string key)
        {
            ValidateKey(key);
            return this.dictionary.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }

        public bool Remove(string key)
        {
            ValidateKey(key);
            return this.dictionary.Remove(key);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<VendorData>");
            builder.Append("<properties>");
            if (this.dictionary != null)
            {
                foreach (KeyValuePair<string, object> pair in this.dictionary)
                {
                    builder.Append("<property>");
                    builder.Append("<vendorKey>");
                    builder.Append(pair.Key.ToString());
                    builder.Append("</vendorKey>");
                    builder.Append("<value>");
                    builder.Append(pair.Value);
                    builder.Append("</value>");
                    builder.Append("</property>");
                }
            }
            builder.Append("</properties>");
            builder.Append("</VendorData>");
            return builder.ToString();
        }

        public bool TryGetValue(string key, out object value)
        {
            ValidateKey(key);
            return this.dictionary.TryGetValue(key, out value);
        }

        private static void ValidateKey(string key)
        {
            if ((key == null) || (key.Length == 0))
            {
                throw new ArgumentNullException("key");
            }
        }

        // Properties
        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }

        public object this[string key]
        {
            get
            {
                ValidateKey(key);
                return this.dictionary[key];
            }
            set
            {
                ValidateKey(key);
                if (!TypesHelper.IsKnownTypeObject(value))
                {
                    throw new ArgumentException("ObjectTypeNotSupported", "VendorSpecificInformation:Value");
                }
                this.dictionary[key] = value;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return this.dictionary.Keys;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return this.dictionary.Values;
            }
        }
    }





}
