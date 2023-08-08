using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Communication;

namespace Kalitte.Sensors.Rfid.Llrp.Events
{

    internal class ConnectionCloseEventArgs : EventArgs
    {
        // Fields
        private ConnectionInformation m_connectionInformation;

        // Methods
        public ConnectionCloseEventArgs(ConnectionInformation connectionInformation)
        {
            this.m_connectionInformation = connectionInformation;
        }

        // Properties
        public ConnectionInformation ConnectionInformation
        {
            get
            {
                return this.m_connectionInformation;
            }
        }
    }

}



