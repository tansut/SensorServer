using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kalitte.Sensors.Communication;
using System.Runtime.Serialization;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.SensorDevices
{

    [Serializable]
    public abstract class SensorDeviceInformationBase
    {
        private readonly ConnectionInformation connectionInformation;
        private VendorData providerData;

        protected internal SensorDeviceInformationBase(ConnectionInformation connectionInformation, VendorData providerData)
        {
            this.connectionInformation = connectionInformation;
            this.ProviderData = providerData;
            this.ValidateParameters();
        }

        public override bool Equals(object obj)
        {
            SensorDeviceInformationBase base2 = obj as SensorDeviceInformationBase;
            if (base2 == null)
            {
                return false;
            }
            return (((this.connectionInformation != null) && this.connectionInformation.Equals(base2.connectionInformation)) && 
                CollectionsHelper.CompareDictionaries(this.providerData, base2.providerData));
        }

        public override int GetHashCode()
        {
            if (this.connectionInformation == null)
            {
                return 0;
            }
            return this.connectionInformation.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<deviceInformationBase>");
            builder.Append(this.connectionInformation);
            builder.Append("</deviceInformationBase>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.connectionInformation == null)
            {
                throw new ArgumentNullException("connectionInformation");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public ConnectionInformation ConnectionInformation
        {
            get
            {
                return this.connectionInformation;
            }
        }

        public VendorData ProviderData
        {
            get
            {
                return this.providerData;
            }
            set
            {
                this.providerData = value;
            }
        }
    }





}
