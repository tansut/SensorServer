using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Communication
{
    [Serializable]
    public sealed class VendorTransportSettings : TransportSettings
    {
        // Fields
        private readonly string name;

        // Methods
        public VendorTransportSettings(string name, VendorData vendorSpecificData)
        {
            this.name = name;
            base.VendorSpecificData = vendorSpecificData;
            this.ValidateParameters();
        }

        public override bool Equals(object obj)
        {
            VendorTransportSettings settings = obj as VendorTransportSettings;
            if (settings == null)
            {
                return false;
            }
            return ((base.Equals(settings) && (this.name != null)) && this.name.Equals(settings.name));
        }

        public override int GetHashCode()
        {
            if (this.name == null)
            {
                return 0;
            }
            return this.name.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<vendorDefinedTransportSettings>");
            builder.Append("<name>");
            builder.Append(this.name);
            builder.Append("</name>");
            builder.Append("</vendorDefinedTransportSettings>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.name == null) || (this.name.Length == 0))
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
        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }





}
