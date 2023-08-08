using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Commands;

namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    internal sealed class ShutdownCommand : SensorCommand
    {
        // Fields
        private bool m_fIsReaderIntiatedConnection;

        // Methods
        internal ShutdownCommand(bool isReaderIntiatedConnection)
        {
            this.m_fIsReaderIntiatedConnection = isReaderIntiatedConnection;
        }

        public override string ToString()
        {
            return "<ShutdownCommand/>";
        }

        // Properties
        internal bool IsReaderIntiatedConnection
        {
            get
            {
                return this.m_fIsReaderIntiatedConnection;
            }
        }
    }




}
