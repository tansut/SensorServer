using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Configuration
{
    [Serializable, DataContract]
    public class VendorEntityKey : IEquatable<VendorEntityKey>
    {
        // Fields
        [DataMember]
        private readonly EntityType entityType;
        [DataMember]
        private readonly string name;
        private Type type;

        // Methods
        public VendorEntityKey(Type type, EntityType entityType)
            : this(type, null, entityType)
        {
        }

        public VendorEntityKey(Type type, string name, EntityType entityType)
        {
            this.entityType = entityType;
            this.type = type;
            this.name = name;
            this.ValidateParameters();
        }

        public bool Equals(VendorEntityKey other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }
            return (((((this.name == null) && (other.name == null)) || ((this.name != null) && this.name.Equals(other.name, StringComparison.CurrentCulture))) && (((this.type == null) && (other.type == null)) || ((this.type != null) && this.type.Equals(other.type)))) && this.entityType.Equals(other.entityType));
        }

        public override bool Equals(object obj)
        {
            return ((obj is VendorEntityKey) && this.Equals((VendorEntityKey)obj));
        }

        private static bool Equals(VendorEntityKey key1, VendorEntityKey key2)
        {
            return (object.ReferenceEquals(key1, key2) || (((key1 != null) && (key2 != null)) && key1.Equals(key2)));
        }

        public override int GetHashCode()
        {
            return ((((this.name != null) ? (this.name.GetHashCode() * 0x3e5) : 0) + ((this.type != null) ? (this.type.GetHashCode() * 0x3e5) : 0)) + this.entityType.Value);
        }

        public static bool operator ==(VendorEntityKey key1, VendorEntityKey key2)
        {
            return Equals(key1, key2);
        }

        public static bool operator !=(VendorEntityKey key1, VendorEntityKey key2)
        {
            return !Equals(key1, key2);
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "[", this.name, ":", this.entityType, "]" });
        }

        private void ValidateParameters()
        {
            if (EntityType.Uninitialized.Value >= this.entityType.Value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            if (this.type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (!TypesHelper.IsKnownType(this.type) && (this.type.Assembly != base.GetType().Assembly))
            {
                throw new ArgumentException("ObjectTypeNotSupported:" + this.type.Name, "type");
            }
            if (EntityType.baseType.ContainsKey(this.entityType.Value))
            {
                Type type = EntityType.baseType[this.entityType.Value];
                if (!type.IsAssignableFrom(this.type))
                {
                    throw new ArgumentException("EntityTypeAndObjectTypeMismatch:" + this.entityType.Description, "type");
                }
                if (((this.entityType == EntityType.PrintTemplateField) || (this.type == EntityType.extensionType[this.entityType.Value])) && ((this.name == null) || (this.name.Length == 0)))
                {
                    throw new ArgumentNullException("NameMustBeNonNullForExtensionPoint:" + this.type, "name");
                }
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public EntityType EntityType
        {
            get
            {
                return this.entityType;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
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
            }
        }
    }





}
