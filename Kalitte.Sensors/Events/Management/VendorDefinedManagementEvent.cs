using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Instrumentation;
using System.Runtime.Serialization;
using Kalitte.Sensors.Core;


namespace Kalitte.Sensors.Events.Management
{
    [Serializable]
    public sealed class VendorDefinedManagementEvent : DeviceManagementEvent
    {
        // Fields
        [IgnoreMember]
        private readonly string name;

        // Methods
        public VendorDefinedManagementEvent(EventLevel eventLevel, EventType eventType, string description, string name, VendorData vendorData)
            : base(eventLevel, eventType, description, vendorData)
        {
            this.name = name;
            this.ValidateParameters();
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
