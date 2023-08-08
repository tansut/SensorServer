using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Commands;

namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    internal sealed class StopInventoryCommand : SensorCommand
    {
        // Methods
        internal StopInventoryCommand()
        {
        }

        public override string ToString()
        {
            return "<StopInventoryCommand/>";
        }
    }

 

}
