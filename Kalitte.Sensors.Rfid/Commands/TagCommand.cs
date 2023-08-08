using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Utilities;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Rfid.Commands
{
    [Serializable]
    public abstract class TagCommand : SensorCommand
    {
        // Fields
        private readonly byte[] passCode;

        // Methods
        protected internal TagCommand(byte[] passCode)
        {
            this.passCode = passCode;
        }

        public byte[] GetPassCode()
        {
            return CollectionsHelper.CloneByte(this.passCode);
        }
    }





}
