using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public sealed class VendorEntityMetadata
    {
        // Fields
        private readonly string description;
        private Dictionary<string, VendorEntityParameterMetadata> subEntities;

        // Methods
        public VendorEntityMetadata(string description, Dictionary<string, VendorEntityParameterMetadata> subEntities)
        {
            if ((description == null) || (description.Length == 0))
            {
                throw new ArgumentNullException("desription");
            }
            this.description = description;
            this.subEntities = subEntities;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<vendorEntityMetaData>");
            builder.Append("<description>");
            builder.Append(this.description);
            builder.Append("</description>");
            builder.Append("<subEntities>");
            if (this.subEntities != null)
            {
                foreach (KeyValuePair<string, VendorEntityParameterMetadata> pair in this.subEntities)
                {
                    builder.Append("<subEntity>");
                    builder.Append("<key>");
                    builder.Append(pair.Key);
                    builder.Append("</key>");
                    builder.Append("<value>");
                    builder.Append(pair.Value);
                    builder.Append("</value>");
                    builder.Append("</subEntity>");
                }
            }
            builder.Append("</subEntities>");
            builder.Append("</vendorEntityMetaData>");
            return builder.ToString();
        }

        // Properties
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public Dictionary<string, VendorEntityParameterMetadata> SubEntities
        {
            get
            {
                return this.subEntities;
            }
        }
    }





}
