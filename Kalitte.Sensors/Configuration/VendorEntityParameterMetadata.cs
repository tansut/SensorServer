using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Collections.ObjectModel;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public sealed class VendorEntityParameterMetadata : EntityMetadata
    {
        // Methods
        public VendorEntityParameterMetadata(Type type, string description, object defaultValue, bool isMandatory)
            : base(type, description, defaultValue, true, isMandatory, true, false)
        {
            this.Validate();
        }

        public VendorEntityParameterMetadata(Type type, string description, object defaultValue, bool isMandatory, Collection<object> valueSet)
            : base(type, description, defaultValue, true, isMandatory, true, false, valueSet)
        {
            this.Validate();
        }

        public VendorEntityParameterMetadata(Type type, string description, object defaultValue, bool isMandatory, double lower, double upper)
            : base(type, description, defaultValue, true, isMandatory, true, false, lower, upper)
        {
            this.Validate();
        }

        private void Validate()
        {
            if ((base.Type == typeof(SecureString)) && (base.DefaultValue != null))
            {
                throw new NotSupportedException("DefaultValueForSecureStringIsNotSupported");
            }
        }

        // Properties
        public bool HasDefault
        {
            get
            {
                return !base.IsMandatory;
            }
        }
    }





}
