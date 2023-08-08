namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class UhfC1G2RFModeTable : UhfRFModeTable
    {
        private Collection<UhfC1G2RFModeTableEntry> m_tableEntries;

        public UhfC1G2RFModeTable(Collection<UhfC1G2RFModeTableEntry> tableEntries) : base(LlrpParameterType.UhfC1G2RFModeTable)
        {
            this.Init(tableEntries);
        }

        internal UhfC1G2RFModeTable(BitArray bitArray, ref int index) : base(LlrpParameterType.UhfC1G2RFModeTable, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Collection<UhfC1G2RFModeTableEntry> tableEntries = new Collection<UhfC1G2RFModeTableEntry>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.UhfC1G2RFModeTableEntry, bitArray, index, parameterEndLimit))
            {
                tableEntries.Add(new UhfC1G2RFModeTableEntry(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(tableEntries);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            Util.Encode<UhfC1G2RFModeTableEntry>(this.TableEntries, stream);
        }

        private void Init(Collection<UhfC1G2RFModeTableEntry> tableEntries)
        {
            if ((tableEntries == null) || (tableEntries.Count == 0))
            {
                throw new ArgumentException("tableEntries");
            }
            Util.CheckCollectionForNonNullElement<UhfC1G2RFModeTableEntry>(tableEntries);
            this.m_tableEntries = tableEntries;
            this.ParameterLength = (ushort) Util.GetTotalBitLengthOfParam<UhfC1G2RFModeTableEntry>(tableEntries);
        }

        public Collection<UhfC1G2RFModeTableEntry> TableEntries
        {
            get
            {
                return this.m_tableEntries;
            }
        }
    }
}
