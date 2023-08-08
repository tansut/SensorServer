using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Rfid.VirtualProvider.Events;

namespace Kalitte.Sensors.Rfid.VirtualProvider.Communication
{
    internal class VirtualDevice
    {
        public string DeviceName { get; private set; }
        private ConnectionInformation connectionInformation;
        private ILogger logger;

        public EventHandler<MessageEventArgs> MessageReceivedEvent;

        internal VirtualDevice(ConnectionInformation connectionInformation, ILogger logger)            
        {
            this.logger = logger;
            this.connectionInformation = connectionInformation;
            this.DeviceName = "";
        }
    }
}
