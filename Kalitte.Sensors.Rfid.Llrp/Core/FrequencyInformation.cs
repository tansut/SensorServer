namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class FrequencyInformation : LlrpTlvParameterBase
    {
        private FixedFrequencyTable m_fixedTable;
        private Collection<FrequencyHopTable> m_hopTables;
        private bool m_isHopping;

        public FrequencyInformation(Collection<FrequencyHopTable> hopTables) : base(LlrpParameterType.FrequencyInformation)
        {
            this.Init(true, null, hopTables);
        }

        public FrequencyInformation(FixedFrequencyTable fixedTable) : base(LlrpParameterType.FrequencyInformation)
        {
            this.Init(false, fixedTable, null);
        }

        internal FrequencyInformation(BitArray bitArray, ref int index) : base(LlrpParameterType.FrequencyInformation, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Collection<FrequencyHopTable> hopTables = new Collection<FrequencyHopTable>();
            FixedFrequencyTable fixedTable = null;
            bool isHopping = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 7;
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.FrequencyHopTable, bitArray, index, parameterEndLimit))
            {
                hopTables.Add(new FrequencyHopTable(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.FixedFrequencyTable, bitArray, index, parameterEndLimit))
            {
                fixedTable = new FixedFrequencyTable(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(isHopping, fixedTable, hopTables);
        }

        private void CalculateLength()
        {
            this.ParameterLength = (ushort) ((8 + Util.GetTotalBitLengthOfParam<FrequencyHopTable>(this.m_hopTables)) + Util.GetBitLengthOfParam(this.m_fixedTable));
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append(this.IsHopping, 1, true);
            stream.Append((ulong) 0L, 7, true);
            if (this.IsHopping)
            {
                Util.Encode<FrequencyHopTable>(this.HopTables, stream);
            }
            else
            {
                Util.Encode(this.FixedTable, stream);
            }
        }

        private void Init(bool isHopping, FixedFrequencyTable fixedTable, Collection<FrequencyHopTable> hopTables)
        {
            if (!isHopping && (fixedTable == null))
            {
                throw new ArgumentException("fixedTable");
            }
            if (isHopping && ((hopTables == null) || (hopTables.Count == 0)))
            {
                throw new ArgumentException("hopTables");
            }
            if (((fixedTable != null) && (hopTables != null)) && (hopTables.Count > 0))
            {
                throw new ArgumentException("isHopping");
            }
            Util.CheckCollectionForNonNullElement<FrequencyHopTable>(hopTables);
            this.m_isHopping = isHopping;
            this.m_hopTables = hopTables;
            this.m_fixedTable = fixedTable;
            this.CalculateLength();
        }

        public FixedFrequencyTable FixedTable
        {
            get
            {
                return this.m_fixedTable;
            }
        }

        public Collection<FrequencyHopTable> HopTables
        {
            get
            {
                return this.m_hopTables;
            }
        }

        public bool IsHopping
        {
            get
            {
                return this.m_isHopping;
            }
        }
    }
}
