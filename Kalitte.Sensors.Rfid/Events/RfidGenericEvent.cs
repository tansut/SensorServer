using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Rfid.Events
{
    [Serializable]
    public class RfidGenericEvent : RfidEventBase
    {
        // Methods
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<rfidGenericEvent>");
            builder.Append(base.ToString());
            builder.Append("</rfidGenericEvent>");
            return builder.ToString();
        }
    }

 

}
