namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class GpiPortCurrentState : LlrpTlvParameterBase
    {
        private bool m_enabled;
        private ushort m_portNum;
        private Kalitte.Sensors.Rfid.Llrp.Core.GpiState m_state;

        internal GpiPortCurrentState(BitArray bitArray, ref int index) : base(LlrpParameterType.GpiPortCurrentState, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort portnumber = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            bool enabled = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 7;
            Kalitte.Sensors.Rfid.Llrp.Core.GpiState enumInstance = BitHelper.GetEnumInstance<Kalitte.Sensors.Rfid.Llrp.Core.GpiState>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(portnumber, enabled, enumInstance);
        }

        public GpiPortCurrentState(ushort portNumber, bool enabled, Kalitte.Sensors.Rfid.Llrp.Core.GpiState state) : base(LlrpParameterType.GpiPortCurrentState)
        {
            this.Init(portNumber, enabled, state);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.PortNumber, 0x10, true);
            stream.Append(this.Enabled, 1, true);
            stream.Append((ulong) 0L, 7, true);
            stream.Append((long) ((byte) this.GpiState), 8, true);
        }

        private void Init(ushort portnumber, bool enabled, Kalitte.Sensors.Rfid.Llrp.Core.GpiState state)
        {
            if (portnumber == 0)
            {
                throw new ArgumentOutOfRangeException("portnumber");
            }
            this.m_portNum = portnumber;
            this.m_enabled = enabled;
            this.m_state = state;
            this.ParameterLength = 0x20;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<GPI Port Current State>");
            builder.Append(base.ToString());
            builder.Append("<Port Number>");
            builder.Append(this.PortNumber);
            builder.Append("</Port Number>");
            builder.Append("<Enabled>");
            builder.Append(this.Enabled);
            builder.Append("</Enabled>");
            builder.Append("<State>");
            builder.Append(this.GpiState.ToString());
            builder.Append("</State>");
            builder.Append("</GPI Port Current State>");
            return builder.ToString();
        }

        public bool Enabled
        {
            get
            {
                return this.m_enabled;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.GpiState GpiState
        {
            get
            {
                return this.m_state;
            }
        }

        public ushort PortNumber
        {
            get
            {
                return this.m_portNum;
            }
        }
    }
}
