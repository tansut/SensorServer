namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class FrequencyHopTable : LlrpTlvParameterBase
    {
        private Collection<uint> m_frequencies;
        private byte m_hopTableId;

        public FrequencyHopTable(byte hopTableId, Collection<uint> frequencies) : base(LlrpParameterType.FrequencyHopTable)
        {
            this.Init(hopTableId, frequencies);
        }

        internal FrequencyHopTable(BitArray bitArray, ref int index) : base(LlrpParameterType.FrequencyHopTable, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Collection<uint> frequencies = new Collection<uint>();
            byte hopTableId = (byte) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8);
            index += 8;
            ushort num3 = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            for (int i = 0; i < num3; i++)
            {
                frequencies.Add((uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(hopTableId, frequencies);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.HopTableId, 8, true);
            stream.Append((ulong) 0L, 8, true);
            ushort num = (this.Frequencies != null) ? ((ushort) this.Frequencies.Count) : ((ushort) 0);
            stream.Append((long) num, 0x10, true);
            if (num > 0)
            {
                foreach (uint num2 in this.Frequencies)
                {
                    stream.Append((long) num2, 0x20, true);
                }
            }
        }

        private void Init(byte hopTableId, Collection<uint> frequencies)
        {
            if ((frequencies == null) || (frequencies.Count == 0))
            {
                throw new ArgumentException("frequencies");
            }
            this.m_hopTableId = hopTableId;
            this.m_frequencies = frequencies;
            this.ParameterLength = (ushort) (0x20 + ((this.m_frequencies != null) ? (this.m_frequencies.Count * 0x20) : 0));
        }

        public Collection<uint> Frequencies
        {
            get
            {
                return this.m_frequencies;
            }
        }

        public byte HopTableId
        {
            get
            {
                return this.m_hopTableId;
            }
        }
    }
}
