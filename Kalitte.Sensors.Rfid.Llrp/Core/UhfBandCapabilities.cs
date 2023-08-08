namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class UhfBandCapabilities : LlrpTlvParameterBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.FrequencyInformation m_frequencyInformation;
        private Collection<UhfRFModeTable> m_rfModeTables;
        private Collection<TransmitPowerLevelTableEntry> m_transmitPowerTableEntries;

        internal UhfBandCapabilities(BitArray bitArray, ref int index) : base(LlrpParameterType.UhfBandCapabilities, bitArray, index)
        {
            LlrpParameterType type;
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Collection<TransmitPowerLevelTableEntry> transmitPowerTableEntries = new Collection<TransmitPowerLevelTableEntry>();
            Kalitte.Sensors.Rfid.Llrp.Core.FrequencyInformation frequencyInformation = null;
            Collection<UhfRFModeTable> rfModeTables = new Collection<UhfRFModeTable>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.TransmitPowerLevelTableEntry, bitArray, index, parameterEndLimit))
            {
                transmitPowerTableEntries.Add(new TransmitPowerLevelTableEntry(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.FrequencyInformation, bitArray, index, parameterEndLimit))
            {
                frequencyInformation = new Kalitte.Sensors.Rfid.Llrp.Core.FrequencyInformation(bitArray, ref index);
            }
            Collection<LlrpParameterType> expectedTypes = new Collection<LlrpParameterType>();
            expectedTypes.Add(LlrpParameterType.UhfC1G2RFModeTable);
            while (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, parameterEndLimit, out type))
            {
                switch (type)
                {
                    case LlrpParameterType.UhfC1G2RFModeTable:
                    {
                        rfModeTables.Add(new UhfC1G2RFModeTable(bitArray, ref index));
                        break;
                    }
                }
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(transmitPowerTableEntries, frequencyInformation, rfModeTables);
        }

        public UhfBandCapabilities(Collection<TransmitPowerLevelTableEntry> transmitPowerTableEntries, Kalitte.Sensors.Rfid.Llrp.Core.FrequencyInformation frequencyInformation, Collection<UhfRFModeTable> rfModeTables) : base(LlrpParameterType.UhfBandCapabilities)
        {
            this.Init(transmitPowerTableEntries, frequencyInformation, rfModeTables);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            Util.Encode<TransmitPowerLevelTableEntry>(this.TransmitPowerTableEntries, stream);
            Util.Encode(this.FrequencyInformation, stream);
            Util.Encode<UhfRFModeTable>(this.RFModeTable, stream);
        }

        private void Init(Collection<TransmitPowerLevelTableEntry> transmitPowerTableEntries, Kalitte.Sensors.Rfid.Llrp.Core.FrequencyInformation frequencyInformation, Collection<UhfRFModeTable> rfModeTables)
        {
            if ((transmitPowerTableEntries == null) || (transmitPowerTableEntries.Count == 0))
            {
                throw new ArgumentException("transmitPowerTableEntries");
            }
            if (frequencyInformation == null)
            {
                throw new ArgumentException("frequencyInformation");
            }
            if ((rfModeTables == null) || (rfModeTables.Count == 0))
            {
                throw new ArgumentException("rfModeTables");
            }
            Util.CheckCollectionForNonNullElement<UhfRFModeTable>(rfModeTables);
            this.m_transmitPowerTableEntries = transmitPowerTableEntries;
            this.m_frequencyInformation = frequencyInformation;
            this.m_rfModeTables = rfModeTables;
            this.ParameterLength = (ushort) ((Util.GetTotalBitLengthOfParam<TransmitPowerLevelTableEntry>(this.m_transmitPowerTableEntries) + Util.GetBitLengthOfParam(frequencyInformation)) + Util.GetTotalBitLengthOfParam<UhfRFModeTable>(this.m_rfModeTables));
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.FrequencyInformation FrequencyInformation
        {
            get
            {
                return this.m_frequencyInformation;
            }
        }

        public Collection<UhfRFModeTable> RFModeTable
        {
            get
            {
                return this.m_rfModeTables;
            }
        }

        public Collection<TransmitPowerLevelTableEntry> TransmitPowerTableEntries
        {
            get
            {
                return this.m_transmitPowerTableEntries;
            }
        }
    }
}
