namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class C1G2LlrpCapabilities : AirProtocolLlrpCapabilities
    {
        private bool m_canSupportBlockErase;
        private bool m_canSupportBlockWrite;
        private ushort m_maxNumberSelectFiltersPerQuery;

        internal C1G2LlrpCapabilities(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2LlrpCapabilities, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            bool canSupportBlockErase = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool canSupportBlockWrite = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 6;
            ushort maxNumberSelectFiltersPerQuery = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(canSupportBlockErase, canSupportBlockWrite, maxNumberSelectFiltersPerQuery);
        }

        public C1G2LlrpCapabilities(bool canSupportBlockErase, bool canSupportBlockWrite, ushort maxNumberSelectFiltersPerQuery) : base(LlrpParameterType.C1G2LlrpCapabilities)
        {
            this.Init(canSupportBlockErase, canSupportBlockWrite, maxNumberSelectFiltersPerQuery);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append(this.CanSupportBlockErase, 1, true);
            stream.Append(this.CanSupportBlockWrite, 1, true);
            stream.Append((ulong) 0L, 6, true);
            stream.Append((long) this.MaximumNumberSelectFiltersPerQuery, 0x10, true);
        }

        private void Init(bool canSupportBlockErase, bool canSupportBlockWrite, ushort maxNumberSelectFiltersPerQuery)
        {
            this.m_canSupportBlockErase = canSupportBlockErase;
            this.m_canSupportBlockWrite = canSupportBlockWrite;
            this.m_maxNumberSelectFiltersPerQuery = maxNumberSelectFiltersPerQuery;
            this.ParameterLength = 0x18;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<C1G2 Llrp Capabilities>");
            builder.Append(base.ToString());
            builder.Append("<Supports Block Erase>");
            builder.Append(this.CanSupportBlockErase);
            builder.Append("</Supports Block Erase>");
            builder.Append("<Supports Block Write>");
            builder.Append(this.CanSupportBlockWrite);
            builder.Append("</Supports Block Write>");
            builder.Append("</C1G2 Llrp Capabilities>");
            return builder.ToString();
        }

        public bool CanSupportBlockErase
        {
            get
            {
                return this.m_canSupportBlockErase;
            }
        }

        public bool CanSupportBlockWrite
        {
            get
            {
                return this.m_canSupportBlockWrite;
            }
        }

        public ushort MaximumNumberSelectFiltersPerQuery
        {
            get
            {
                return this.m_maxNumberSelectFiltersPerQuery;
            }
        }
    }
}
