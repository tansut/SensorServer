using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Commands;

namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    internal sealed class GetDeviceIdResponse : Response
    {
        // Fields
        private string m_deviceId;

        // Methods
        internal GetDeviceIdResponse(string deviceId)
        {
            this.m_deviceId = deviceId;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<GetDeviceIdResponse>");
            builder.Append(this.m_deviceId);
            builder.Append("</GetDeviceIdResponse>");
            return builder.ToString();
        }

        // Properties
        internal string DeviceId
        {
            get
            {
                return this.m_deviceId;
            }
        }
    }




}
