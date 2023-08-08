namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class DisableAccessSpecMessage : LlrpMessageRequestBase
    {
        private uint m_accessSpecId;

        internal DisableAccessSpecMessage(BitArray bitArray) : base(LlrpMessageType.DisableAccessSpec, bitArray)
        {
            int startingIndex = 80;
            uint accessSpecId = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 0x20);
            BitHelper.ValidateEndOfParameterOrMessage(startingIndex, (uint) bitArray.Count, base.GetType().FullName);
            this.Init(accessSpecId);
        }

        public DisableAccessSpecMessage(uint accessSpecId) : base(LlrpMessageType.DisableAccessSpec)
        {
            this.Init(accessSpecId);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = this.CreateHeaderStream();
            stream.Append((long) this.AccessSpecId, 0x20, true);
            return stream.Merge();
        }

        private void Init(uint accessSpecId)
        {
            this.m_accessSpecId = accessSpecId;
            this.MessageLength = 0x20L;
        }

        public uint AccessSpecId
        {
            get
            {
                return this.m_accessSpecId;
            }
        }
    }
}
