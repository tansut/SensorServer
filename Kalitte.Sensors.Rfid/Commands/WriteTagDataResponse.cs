﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Commands;

namespace Kalitte.Sensors.Rfid.Commands
{
    [Serializable]
    public sealed class WriteTagDataResponse : Response
    {
        // Methods
        public override string ToString()
        {
            return "<writeTagDataResponse/>";
        }
    }

 

}