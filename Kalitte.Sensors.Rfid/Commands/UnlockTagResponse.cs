using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Commands;

namespace Kalitte.Sensors.Rfid.Commands
{
    [Serializable]
    public sealed class UnlockTagResponse : Response
    {
        // Methods
        public override string ToString()
        {
            return "<unLockTagResponse/>";
        }
    }

 

}
