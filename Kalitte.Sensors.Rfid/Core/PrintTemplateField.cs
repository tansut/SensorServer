using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Rfid.Utilities;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Rfid.Core
{
    [Serializable, KnownType("GetPrintTemplateFieldSubTypes")]
    public abstract class PrintTemplateField
    {
        // Fields
        private readonly string m_fieldDescription;
        private readonly string m_fieldName;
        private VendorData m_vendorDefinedParameters;

        // Methods
        protected PrintTemplateField(string name, string description, VendorData vendorData)
        {
            this.m_fieldName = name;
            this.m_fieldDescription = description;
            this.VendorDefinedParameters = vendorData;
            this.ValidateParameters();
        }

        private static IEnumerable<Type> GetPrintTemplateFieldSubTypes()
        {
            return TypesHelper.GetCurrentAssemblyTypes(typeof(PrintTemplateField));
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<printTemplateField>");
            builder.Append("<fieldName>");
            builder.Append(this.Name);
            builder.Append("</fieldName>");
            builder.Append("<fieldDescription>");
            builder.Append(this.FieldDescription);
            builder.Append("</fieldDescription>");
            builder.Append("</printTemplateField>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.m_fieldName == null)
            {
                throw new ArgumentNullException("name");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public string FieldDescription
        {
            get
            {
                return this.m_fieldDescription;
            }
        }

        public string Name
        {
            get
            {
                return this.m_fieldName;
            }
        }

        public VendorData VendorDefinedParameters
        {
            get
            {
                return this.m_vendorDefinedParameters;
            }
            set
            {
                this.m_vendorDefinedParameters = value;
            }
        }
    }



}
