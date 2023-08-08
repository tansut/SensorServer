namespace Kalitte.Sensors.Configuration
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class PropertyKey : IEquatable<PropertyKey>
    {
        private readonly string groupName;
        private readonly string propertyName;

        public PropertyKey(string groupName, string propertyName)
        {
            this.groupName = groupName;
            this.propertyName = propertyName;
            this.ValidateParameters();
        }

        public bool Equals(PropertyKey other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }
            return ((string.Compare(this.groupName, other.groupName, StringComparison.OrdinalIgnoreCase) == 0) && (0 == string.Compare(this.propertyName, other.propertyName, StringComparison.OrdinalIgnoreCase)));
        }

        public override bool Equals(object obj)
        {
            return ((obj is PropertyKey) && this.Equals((PropertyKey) obj));
        }

        private static bool Equals(PropertyKey key1, PropertyKey key2)
        {
            return (object.ReferenceEquals(key1, key2) || (((!object.ReferenceEquals(key1, null)) && (!object.ReferenceEquals(key1, null))) && key1.Equals(key2)));
        }

        public override int GetHashCode()
        {
            int num = 0;
            num += (this.groupName != null) ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(this.groupName) : 0;
            return (num + ((this.propertyName != null) ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(this.propertyName) : 0));
        }

        public static bool operator ==(PropertyKey key1, PropertyKey key2)
        {
            return Equals(key1, key2);
        }

        public static bool operator !=(PropertyKey key1, PropertyKey key2)
        {
            return !Equals(key1, key2);
        }

        public override string ToString()
        {
            return ("[" + this.groupName + ":" + this.propertyName + "]");
        }

        private void ValidateParameters()
        {
            if ((this.groupName == null) || (this.groupName.Length == 0))
            {
                throw new ArgumentNullException("groupName");
            }
            if ((this.propertyName == null) || (this.propertyName.Length == 0))
            {
                throw new ArgumentNullException("propertyName");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public string GroupName
        {
            get
            {
                return this.groupName;
            }
        }

        public string PropertyName
        {
            get
            {
                return this.propertyName;
            }
        }
    }
}
