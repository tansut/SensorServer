namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class FixedFrequencyTable : LlrpTlvParameterBase
    {
        private Collection<uint> m_frequencies;

        public FixedFrequencyTable(Collection<uint> frequencies) : base(LlrpParameterType.FixedFrequencyTable)
        {
            this.Init(frequencies);
        }

        internal FixedFrequencyTable(BitArray bitArray, ref int index) : base(LlrpParameterType.FixedFrequencyTable, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Collection<uint> frequencies = new Collection<uint>();
            ushort num2 = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            for (int i = 0; i < num2; i++)
            {
                frequencies.Add((uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(frequencies);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            ushort count = 0;
            if (this.Frequencies != null)
            {
                count = (ushort) this.Frequencies.Count;
            }
            stream.Append((long) count, 0x10, true);
            if (count > 0)
            {
                foreach (uint num2 in this.Frequencies)
                {
                    stream.Append((long) num2, 0x20, true);
                }
            }
        }

        private void Init(Collection<uint> frequencies)
        {
            if ((frequencies == null) || (frequencies.Count == 0))
            {
                throw new ArgumentException("frequencies");
            }
            this.m_frequencies = frequencies;
            this.ParameterLength = (ushort) (0x10 + ((this.m_frequencies != null) ? (this.m_frequencies.Count * 0x20) : 0));
        }

        public Collection<uint> Frequencies
        {
            get
            {
                return this.m_frequencies;
            }
        }
    }
}
