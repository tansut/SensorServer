using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.SensorDevices
{

    [Serializable]
    public sealed class SensorDeviceInformation : SensorDeviceInformationBase
    {
        // Fields
        private readonly string deviceId;

        // Methods
        public SensorDeviceInformation(string deviceId, ConnectionInformation connectionInformation, VendorData providerData)
            : base(connectionInformation, providerData)
        {
            this.deviceId = deviceId;
            this.ValidateParameters();
        }

        public override bool Equals(object obj)
        {
            SensorDeviceInformation information = obj as SensorDeviceInformation;
            if (information == null)
            {
                return false;
            }
            return ((base.Equals(information) && (this.deviceId != null)) && this.deviceId.Equals(information.deviceId));
        }

        public override int GetHashCode()
        {
            if (this.deviceId == null)
            {
                return 0;
            }
            return (this.deviceId.GetHashCode() * base.GetHashCode());
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<sensorDeviceInformation>");
            builder.Append("<deviceId>");
            builder.Append(this.deviceId);
            builder.Append("</deviceId>");
            builder.Append(base.ToString());
            builder.Append("</sensorDeviceInformation>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.deviceId == null)
            {
                throw new ArgumentNullException("deviceId");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public string DeviceId
        {
            get
            {
                return this.deviceId;
            }
        }
    }







}
