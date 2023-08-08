using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Kalitte.Sensors.Rfid.Llrp.Core;

namespace Kalitte.Sensors.Rfid.Llrp.PhysicalDevices
{
    internal class PendingMessageInformation : IDisposable
    {
        // Fields
        private ManualResetEvent m_manualResetEvent;
        private LlrpMessageBase m_requestMessage;

        // Methods
        internal PendingMessageInformation(LlrpMessageBase requestMessage, ManualResetEvent resetEvent)
        {
            this.m_manualResetEvent = resetEvent;
            this.m_requestMessage = requestMessage;
        }

        public void Dispose()
        {
            try
            {
                if (this.m_manualResetEvent != null)
                {
                    this.m_manualResetEvent.Close();
                    this.m_manualResetEvent = null;
                }
            }
            catch
            {
            }
        }

        // Properties
        internal LlrpMessageBase RequestMessage
        {
            get
            {
                return this.m_requestMessage;
            }
        }

        internal ManualResetEvent ResetEvent
        {
            get
            {
                return this.m_manualResetEvent;
            }
        }
    }



}
