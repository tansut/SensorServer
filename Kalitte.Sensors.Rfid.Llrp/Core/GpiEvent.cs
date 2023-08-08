namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Events;

    public sealed class GpiEvent : LlrpEvent
    {
        private bool m_enabled;
        private ushort m_gpiPortNumber;

        internal GpiEvent(BitArray bitArray, ref int index) : base(bitArray, index, LlrpParameterType.GpiEvent)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort port = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            bool enabled = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 7;
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(port, enabled);
        }

        internal override Notification ConvertToRfidNotification()
        {
            byte num = this.Enabled ? ((byte)1) : ((byte)0);
            return new Notification(new IOPortValueChangedEvent(Util.GetGpiName(this.Port), new byte[] { num }));
        }

 

 


        public GpiEvent(ushort port, bool enabled) : base(LlrpParameterType.GpiEvent)
        {
            this.Init(port, enabled);
        }



        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.Port, 0x10, true);
            stream.Append(this.Enabled, 1, true);
            stream.Append((ulong) 0L, 7, true);
        }

        private void Init(ushort port, bool enabled)
        {
            this.m_gpiPortNumber = port;
            this.m_enabled = enabled;
            this.ParameterLength = 0x18;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<GPI event>");
            builder.Append(base.ToString());
            builder.Append("<Port>");
            builder.Append(this.Port);
            builder.Append("</Port>");
            builder.Append("<Enabled>");
            builder.Append(this.Enabled);
            builder.Append("</Enabled>");
            builder.Append("</GPI event>");
            return builder.ToString();
        }

        public bool Enabled
        {
            get
            {
                return this.m_enabled;
            }
        }

        public ushort Port
        {
            get
            {
                return this.m_gpiPortNumber;
            }
        }
    }
}
