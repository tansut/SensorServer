using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;


namespace Kalitte.Sensors.Events.Management
{
    [Serializable]
    public class DeviceManagementEvent : ManagementEvent
    {
        // Fields
        private string m_deviceName;

        // Methods
        public DeviceManagementEvent(EventLevel eventLevel, EventType eventType, string description)
            : base(eventLevel, eventType, description)
        {
        }

        protected DeviceManagementEvent(EventLevel eventLevel, EventType eventType, string description, VendorData vendorData)
            : base(eventLevel, eventType, description, vendorData)
        {
        }

        // Properties
        public string DeviceName
        {
            get
            {
                return this.m_deviceName;
            }
            set
            {
                this.m_deviceName = value;
            }
        }
    }



}
