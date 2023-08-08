using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Rfid.VirtualProvider.Communication;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Events;

namespace Kalitte.Sensors.Rfid.VirtualProvider.Commands
{
    internal class GetActivePropertyListCommandHandler : CommandHandler
    {
        internal override Sensors.Commands.ResponseEventArgs ExecuteCommand()
        {
            //TagReadEvent tagRead = new TagReadEvent(
            //QueryTagsResponse response = new QueryTagsResponse();
            return null;
        }

        public GetActivePropertyListCommandHandler(string source, SensorCommand command, VirtualDevice device, VirtualDeviceState state, ILogger logger) :
            base(source, command, device, state, logger)
        {



        }
    }
}
