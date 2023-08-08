namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class PeriodicTrigger : LlrpTlvParameterBase
    {
        private uint m_offset;
        private uint m_period;
        private Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp m_utcTimeStamp;

        internal PeriodicTrigger(BitArray bitArray, ref int index) : base(LlrpParameterType.PeriodicTriggerValue, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp timeStamp = null;
            uint offset = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint period = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.UtcTimestamp, bitArray, index, parameterEndLimit))
            {
                timeStamp = new Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(offset, period, timeStamp);
        }

        public PeriodicTrigger(uint offset, uint period, Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp timestamp) : base(LlrpParameterType.PeriodicTriggerValue)
        {
            this.Init(offset, period, timestamp);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.Offset, 0x20, true);
            stream.Append((long) this.Period, 0x20, true);
            Util.Encode(this.UtcTimestamp, stream);
        }

        private void Init(uint offset, uint period, Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp timeStamp)
        {
            this.m_offset = offset;
            this.m_period = period;
            this.m_utcTimeStamp = timeStamp;
            this.ParameterLength = 0x40 + Util.GetBitLengthOfParam(this.UtcTimestamp);
        }

        public uint Offset
        {
            get
            {
                return this.m_offset;
            }
        }

        public uint Period
        {
            get
            {
                return this.m_period;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp UtcTimestamp
        {
            get
            {
                return this.m_utcTimeStamp;
            }
        }
    }
}
