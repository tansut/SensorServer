namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class C1G2EpcMemorySelector : AirProtocolSpecificEpcMemorySelectorParameter
    {
        private bool m_enableCRC;
        private bool m_enablePC;

        public C1G2EpcMemorySelector(bool enableCRC, bool enablePC) : base(LlrpParameterType.C1G2EpcMemorySelector)
        {
            this.Init(enableCRC, enablePC);
        }

        internal C1G2EpcMemorySelector(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2EpcMemorySelector, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            bool enableCRC = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool enablePC = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 6;
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enableCRC, enablePC);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append(this.EnableCrc, 1, true);
            stream.Append(this.EnablePC, 1, true);
            stream.Append((ulong) 0L, 6, true);
        }

        private void Init(bool enableCRC, bool enablePC)
        {
            this.m_enableCRC = enableCRC;
            this.m_enablePC = enablePC;
            this.ParameterLength = 8;
        }

        public bool EnableCrc
        {
            get
            {
                return this.m_enableCRC;
            }
        }

        public bool EnablePC
        {
            get
            {
                return this.m_enablePC;
            }
        }
    }
}
