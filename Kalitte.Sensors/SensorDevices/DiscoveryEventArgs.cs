using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;


namespace Kalitte.Sensors.SensorDevices
{
    [Serializable]
    public sealed class DiscoveryEventArgs : EventArgs
    {
        // Fields
        private readonly SensorDeviceInformation deviceInfo;
        private readonly PropertyList propertyProfile;

        // Methods
        public DiscoveryEventArgs(SensorDeviceInformation deviceInfo)
        {
            if (deviceInfo == null)
            {
                throw new ArgumentNullException("deviceInfo");
            }
            this.deviceInfo = deviceInfo;
        }

        public DiscoveryEventArgs(SensorDeviceInformation deviceInfo, PropertyList propertyProfile)
            : this(deviceInfo)
        {
            if (propertyProfile == null)
            {
                throw new ArgumentNullException("propertyProfile");
            }
            this.propertyProfile = propertyProfile;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<discoveryEventArgs>");
            builder.Append("<deviceInfo>");
            builder.Append(this.deviceInfo);
            builder.Append("</deviceInfo>");
            builder.Append("<propertyProfile>");
            builder.Append(this.propertyProfile);
            builder.Append("</propertyProfile>");
            builder.Append("</discoveryEventArgs>");
            return builder.ToString();
        }

        // Properties
        public SensorDeviceInformation DeviceInfo
        {
            get
            {
                return this.deviceInfo;
            }
        }

        public PropertyList PropertyProfile
        {
            get
            {
                return this.propertyProfile;
            }
        }
    }




}
