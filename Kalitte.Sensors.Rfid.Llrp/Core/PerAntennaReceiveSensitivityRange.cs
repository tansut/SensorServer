namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class PerAntennaReceiveSensitivityRange : LlrpTlvParameterBase
    {
        private ushort m_antennaId;
        private ushort m_indexMax;
        private ushort m_indexMin;

        internal PerAntennaReceiveSensitivityRange(BitArray bitArray, ref int index) : base(LlrpParameterType.PerAntennaReceiveSensitivityRange, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort antennaId = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort minIndex = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort maxIndex = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(antennaId, minIndex, maxIndex);
        }

        public PerAntennaReceiveSensitivityRange(ushort antennaId, ushort minIndex, ushort maxIndex) : base(LlrpParameterType.PerAntennaReceiveSensitivityRange)
        {
            this.Init(antennaId, minIndex, maxIndex);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.AntennaId, 0x10, true);
            stream.Append((long) this.MinimumIndex, 0x10, true);
            stream.Append((long) this.MaximumIndex, 0x10, true);
        }

        private void Init(ushort antennaId, ushort minIndex, ushort maxIndex)
        {
            if (minIndex > maxIndex)
            {
                throw new ArgumentOutOfRangeException("minIndex");
            }
            this.m_antennaId = antennaId;
            this.m_indexMin = minIndex;
            this.m_indexMax = maxIndex;
            this.ParameterLength = 0x30;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Per Antenna Receive Sensitivity Range>");
            builder.Append(base.ToString());
            builder.Append("<Antenna Id>");
            builder.Append(this.AntennaId);
            builder.Append("</Antenna Id>");
            builder.Append("<Index Minimum>");
            builder.Append(this.MinimumIndex);
            builder.Append("</Index Minimum>");
            builder.Append("<Index Maximum>");
            builder.Append(this.MaximumIndex);
            builder.Append("</Index Maximum>");
            builder.Append("</Per Antenna Receive Sensitivity Range>");
            return builder.ToString();
        }

        public ushort AntennaId
        {
            get
            {
                return this.m_antennaId;
            }
        }

        public ushort MaximumIndex
        {
            get
            {
                return this.m_indexMax;
            }
        }

        public ushort MinimumIndex
        {
            get
            {
                return this.m_indexMin;
            }
        }
    }
}
