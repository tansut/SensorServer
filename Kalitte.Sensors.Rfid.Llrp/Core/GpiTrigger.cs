namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class GpiTrigger : LlrpTlvParameterBase
    {
        private bool m_enableGPIEventState;
        private ushort m_portNumber;
        private uint m_timeout;

        internal GpiTrigger(BitArray bitArray, ref int index) : base(LlrpParameterType.GpiTriggerValue, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort portNumber = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            bool triggerState = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 7;
            uint timeout = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(triggerState, portNumber, timeout);
        }

        public GpiTrigger(bool triggerState, ushort portNumber, uint timeout) : base(LlrpParameterType.GpiTriggerValue)
        {
            this.Init(triggerState, portNumber, timeout);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.PortNumber, 0x10, true);
            stream.Append(this.EnableGpiEventState, 1, true);
            stream.Append((ulong) 0L, 7, true);
            stream.Append((long) this.Timeout, 0x20, true);
        }

        private void Init(bool triggerState, ushort portNumber, uint timeout)
        {
            this.m_enableGPIEventState = triggerState;
            this.m_portNumber = portNumber;
            this.m_timeout = timeout;
            this.ParameterLength = 0x38;
        }

        public bool EnableGpiEventState
        {
            get
            {
                return this.m_enableGPIEventState;
            }
        }

        public ushort PortNumber
        {
            get
            {
                return this.m_portNumber;
            }
        }

        public uint Timeout
        {
            get
            {
                return this.m_timeout;
            }
        }
    }
}
