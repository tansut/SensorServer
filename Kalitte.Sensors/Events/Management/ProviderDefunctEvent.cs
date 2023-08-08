using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events;


namespace Kalitte.Sensors.Events.Management
{
    [Serializable]
    public sealed class ProviderDefunctEvent : ProviderManagementEvent
    {
        // Methods
        public ProviderDefunctEvent(string description)
            : base(EventLevel.Fatal, EventType.ProviderDefunct, description)
        {
        }
    }

 

}
