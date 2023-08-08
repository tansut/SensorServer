using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public sealed class ProviderMetadata
    {
        // Fields
        private Dictionary<PropertyKey, DevicePropertyMetadata> devicePropertyMetadata;
        private readonly Collection<ProviderCapability> providerCapabilities;
        private readonly ProviderInformation providerInformation;
        private Dictionary<PropertyKey, ProviderPropertyMetadata> providerPropertyMetadata;
        private Dictionary<VendorEntityKey, VendorEntityMetadata> vendorExtensionsEntityMetadata;

        // Methods
        public ProviderMetadata(ProviderInformation providerInformation, Collection<ProviderCapability> capabilities, Dictionary<PropertyKey, ProviderPropertyMetadata> providerPropertyMetadata, Dictionary<VendorEntityKey, VendorEntityMetadata> vendorExtensionsEntityMetadata, Dictionary<PropertyKey, DevicePropertyMetadata> devicePropertyMetadata)
        {
            this.providerCapabilities = capabilities;
            this.providerInformation = providerInformation;
            this.providerPropertyMetadata = providerPropertyMetadata;
            this.vendorExtensionsEntityMetadata = vendorExtensionsEntityMetadata;
            this.devicePropertyMetadata = devicePropertyMetadata;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<providerMetadata>");
            if (this.providerCapabilities != null)
            {
                builder.Append("<providerCapabilities>");
                foreach (ProviderCapability capability in this.providerCapabilities)
                {
                    builder.Append("<providerCapability>");
                    builder.Append(capability);
                    builder.Append("</providerCapability>");
                }
                builder.Append("</providerCapabilities>");
            }
            builder.Append("<providerInformation>");
            builder.Append(this.providerInformation);
            builder.Append("</providerInformation>");
            if (this.providerPropertyMetadata != null)
            {
                builder.Append("<providerPropertyMetadata>");
                foreach (KeyValuePair<PropertyKey, ProviderPropertyMetadata> pair in this.providerPropertyMetadata)
                {
                    builder.Append("<propertyMetadata>");
                    builder.Append("<propertyKey>");
                    builder.Append(pair.Key);
                    builder.Append("</propertyKey>");
                    builder.Append("<propertyMetadata>");
                    builder.Append(pair.Value);
                    builder.Append("</propertyMetadata>");
                    builder.Append("</propertyMetadata>");
                }
                builder.Append("</providerPropertyMetadata>");
            }
            if (this.vendorExtensionsEntityMetadata != null)
            {
                builder.Append("<vendorExtensionsEntityMetadata>");
                foreach (KeyValuePair<VendorEntityKey, VendorEntityMetadata> pair2 in this.vendorExtensionsEntityMetadata)
                {
                    builder.Append("<vendorEntityMetadata>");
                    builder.Append("<vendorEntityKey>");
                    builder.Append(pair2.Key);
                    builder.Append("</vendorEntityKey>");
                    builder.Append("<vendorEntityMetadata>");
                    builder.Append(pair2.Value);
                    builder.Append("</vendorEntityMetadata>");
                    builder.Append("</vendorEntityMetadata>");
                }
                builder.Append("</vendorExtensionsEntityMetadata>");
            }
            if (this.devicePropertyMetadata != null)
            {
                builder.Append("<devicePropertyMetadata>");
                foreach (KeyValuePair<PropertyKey, DevicePropertyMetadata> pair3 in this.devicePropertyMetadata)
                {
                    builder.Append("<propertyMetadata>");
                    builder.Append("<propertyKey>");
                    builder.Append(pair3.Key);
                    builder.Append("</propertyKey>");
                    builder.Append("<propertyMetadata>");
                    builder.Append(pair3.Value);
                    builder.Append("</propertyMetadata>");
                    builder.Append("</propertyMetadata>");
                }
                builder.Append("</devicePropertyMetadata>");
            }
            builder.Append("</providerMetadata>");
            return builder.ToString();
        }

        // Properties
        public Dictionary<PropertyKey, DevicePropertyMetadata> DevicePropertyMetadata
        {
            get
            {
                return this.devicePropertyMetadata;
            }
        }

        public ReadOnlyCollection<ProviderCapability> ProviderCapabilities
        {
            get
            {
                if (this.providerCapabilities != null)
                {
                    return new ReadOnlyCollection<ProviderCapability>(this.providerCapabilities);
                }
                return null;
            }
        }

        public ProviderInformation ProviderInformation
        {
            get
            {
                return this.providerInformation;
            }
        }

        public Dictionary<PropertyKey, ProviderPropertyMetadata> ProviderPropertyMetadata
        {
            get
            {
                return this.providerPropertyMetadata;
            }
        }

        public Dictionary<VendorEntityKey, VendorEntityMetadata> VendorExtensionsEntityMetadata
        {
            get
            {
                return this.vendorExtensionsEntityMetadata;
            }
        }
    }




}
