using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events.Management;
using Kalitte.Sensors.Core;



namespace Kalitte.Sensors.Events.Management
{
    [Serializable]
    public class ProviderManagementEvent : ManagementEvent
    {
        // Fields
        private string m_providerName;

        // Methods
        public ProviderManagementEvent(EventLevel eventLevel, EventType eventType, string description)
            : base(eventLevel, eventType, description)
        {
        }

        protected ProviderManagementEvent(EventLevel eventLevel, EventType eventType, string description, VendorData vendorData)
            : base(eventLevel, eventType, description, vendorData)
        {
        }

        // Properties
        public string ProviderName
        {
            get
            {
                return this.m_providerName;
            }
            set
            {
                this.m_providerName = value;
            }
        }
    }




}
