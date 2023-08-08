using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public sealed class ProviderInformation
    {
        // Fields
        private readonly string description;
        private readonly string id;
        private readonly string version;

        // Methods
        public ProviderInformation(string id, string description, string version)
        {
            this.id = id;
            this.description = description;
            this.version = version;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<providerInformation>");
            builder.Append("<id>");
            builder.Append(this.id);
            builder.Append("</id>");
            builder.Append("<description>");
            builder.Append(this.description);
            builder.Append("</description>");
            builder.Append("<version>");
            builder.Append(this.version);
            builder.Append("</version>");
            builder.Append("</providerInformation>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.id == null) || (this.id.Length == 0))
            {
                throw new ArgumentNullException("id");
            }
            if ((this.description == null) || (this.description.Length == 0))
            {
                throw new ArgumentNullException("description");
            }
            if ((this.version == null) || (this.version.Length == 0))
            {
                throw new ArgumentNullException("version");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }
        }
    }





}
