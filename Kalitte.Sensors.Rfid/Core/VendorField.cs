using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Rfid.Core
{
    [Serializable]
    public sealed class VendorField : PrintTemplateField
    {
        // Fields
        private readonly string m_fieldType;

        // Methods
        public VendorField(string name, string fieldDescription, string fieldType, VendorData vendorData)
            : base(name, fieldDescription, vendorData)
        {
            this.m_fieldType = fieldType;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<vendorDefinedField>");
            builder.Append(base.ToString());
            builder.Append("<fieldType>");
            builder.Append(this.FieldType);
            builder.Append("</fieldType>");
            builder.Append("</vendorDefinedField>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (string.IsNullOrEmpty(this.m_fieldType))
            {
                throw new ArgumentNullException("fieldType");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public string FieldType
        {
            get
            {
                return this.m_fieldType;
            }
        }
    }




}
