using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Commands;

namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    internal sealed class GetDeviceIdCommand : SensorCommand
    {
        // Fields
        private GetDeviceIdResponse m_response;

        // Methods
        internal GetDeviceIdCommand()
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<GetDeviceIdCommand>");
            if (this.Response != null)
            {
                builder.Append(this.Response.ToString());
            }
            builder.Append("</GetDeviceIdCommand>");
            return builder.ToString();
        }

        // Properties
        internal GetDeviceIdResponse Response
        {
            get
            {
                return this.m_response;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Response");
                }
                this.m_response = value;
            }
        }
    }



}
