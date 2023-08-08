namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    public sealed class TagReportData : LlrpTlvParameterBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.AccessSpecId m_accessSpecId;
        private Collection<AirProtocolSpecificOPSpecResult> m_airProtocolOpSpec;
        private Collection<Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolTagData> m_airProtocolTagData;
        private Kalitte.Sensors.Rfid.Llrp.Core.AntennaId m_antennaId;
        private Kalitte.Sensors.Rfid.Llrp.Core.ChannelIndex m_channelIndex;
        private Collection<ClientRequestOPSpecResult> m_clientRequestOpSpec;
        private Collection<CustomParameterBase> m_customParams;
        private Kalitte.Sensors.Rfid.Llrp.Core.EPC96 m_epc96;
        private Kalitte.Sensors.Rfid.Llrp.Core.EpcData m_epcData;
        private Kalitte.Sensors.Rfid.Llrp.Core.FirstSeenTimestampUptime m_firstSeenUptime;
        private Kalitte.Sensors.Rfid.Llrp.Core.FirstSeenTimestampUtc m_firstSeenUTC;
        private Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpecId m_inventoryParamSpecId;
        private Kalitte.Sensors.Rfid.Llrp.Core.LastSeenTimestampUptime m_lastSeenUptime;
        private Kalitte.Sensors.Rfid.Llrp.Core.LastSeenTimestampUtc m_lastSeenUTC;
        private Kalitte.Sensors.Rfid.Llrp.Core.PeakRssi m_peakRSSI;
        private Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId m_roSpecId;
        private Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex m_specIndex;
        private Kalitte.Sensors.Rfid.Llrp.Core.TagSeenCount m_tagSeenCount;

        internal TagReportData(BitArray bitArray, ref int index) : base(LlrpParameterType.TagReportData, bitArray, index)
        {
            LlrpParameterType type;
            Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId id;
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Collection<LlrpParameterType> expectedTypes = new Collection<LlrpParameterType>();
            Kalitte.Sensors.Rfid.Llrp.Core.EPC96 epc = null;
            Kalitte.Sensors.Rfid.Llrp.Core.EpcData epcData = null;
            expectedTypes.Add(LlrpParameterType.EPC96);
            expectedTypes.Add(LlrpParameterType.EpcData);
            if (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, parameterEndLimit, out type))
            {
                switch (type)
                {
                    case LlrpParameterType.EPC96:
                        epc = new Kalitte.Sensors.Rfid.Llrp.Core.EPC96(bitArray, ref index);
                        break;

                    case LlrpParameterType.EpcData:
                        epcData = new Kalitte.Sensors.Rfid.Llrp.Core.EpcData(bitArray, ref index);
                        goto Label_0069;
                }
            }
        Label_0069:
            id = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ROSpecId, bitArray, index, parameterEndLimit))
            {
                id = new Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex specIndex = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.SpecIndex, bitArray, index, parameterEndLimit))
            {
                specIndex = new Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpecId inventoryId = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.InventoryParameterSpecId, bitArray, index, parameterEndLimit))
            {
                inventoryId = new Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpecId(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.AntennaId antennaId = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AntennaId, bitArray, index, parameterEndLimit))
            {
                antennaId = new Kalitte.Sensors.Rfid.Llrp.Core.AntennaId(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.PeakRssi peakRSSI = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.PeakRssi, bitArray, index, parameterEndLimit))
            {
                peakRSSI = new Kalitte.Sensors.Rfid.Llrp.Core.PeakRssi(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.ChannelIndex channelIndex = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ChannelIndex, bitArray, index, parameterEndLimit))
            {
                channelIndex = new Kalitte.Sensors.Rfid.Llrp.Core.ChannelIndex(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.FirstSeenTimestampUtc firstUTC = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.FirstSeenTimestampUtc, bitArray, index, parameterEndLimit))
            {
                firstUTC = new Kalitte.Sensors.Rfid.Llrp.Core.FirstSeenTimestampUtc(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.FirstSeenTimestampUptime firstUptime = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.FirstSeenTimestampUptime, bitArray, index, parameterEndLimit))
            {
                firstUptime = new Kalitte.Sensors.Rfid.Llrp.Core.FirstSeenTimestampUptime(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.LastSeenTimestampUtc lastUTC = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.LastSeenTimestampUtc, bitArray, index, parameterEndLimit))
            {
                lastUTC = new Kalitte.Sensors.Rfid.Llrp.Core.LastSeenTimestampUtc(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.LastSeenTimestampUptime lastUptime = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.LastSeenTimestampUptime, bitArray, index, parameterEndLimit))
            {
                lastUptime = new Kalitte.Sensors.Rfid.Llrp.Core.LastSeenTimestampUptime(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.TagSeenCount tagSeenCount = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.TagSeenCount, bitArray, index, parameterEndLimit))
            {
                tagSeenCount = new Kalitte.Sensors.Rfid.Llrp.Core.TagSeenCount(bitArray, ref index);
            }
            Collection<Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolTagData> airProtocolTagData = new Collection<Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolTagData>();
            expectedTypes.Clear();
            expectedTypes.Add(LlrpParameterType.C1G2PC);
            expectedTypes.Add(LlrpParameterType.C1G2Crc);
            while (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, parameterEndLimit, out type))
            {
                switch (type)
                {
                    case LlrpParameterType.C1G2Crc:
                        airProtocolTagData.Add(new C1G2Crc(bitArray, ref index));
                        break;

                    case LlrpParameterType.C1G2PC:
                        airProtocolTagData.Add(new C1G2PC(bitArray, ref index));
                        break;
                }
            }
            Kalitte.Sensors.Rfid.Llrp.Core.AccessSpecId accessSpecId = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AccessSpecId, bitArray, index, parameterEndLimit))
            {
                accessSpecId = new Kalitte.Sensors.Rfid.Llrp.Core.AccessSpecId(bitArray, ref index);
            }
            Collection<AirProtocolSpecificOPSpecResult> airProtocolOpSpecs = new Collection<AirProtocolSpecificOPSpecResult>();
            Collection<ClientRequestOPSpecResult> clientRequestOpSpecsResult = new Collection<ClientRequestOPSpecResult>();
            expectedTypes.Clear();
            expectedTypes.Add(LlrpParameterType.C1G2ReadOPSpecResult);
            expectedTypes.Add(LlrpParameterType.C1G2WriteOPSpecResult);
            expectedTypes.Add(LlrpParameterType.C1G2KillOPSpecResult);
            expectedTypes.Add(LlrpParameterType.C1G2LockOPSpecResult);
            expectedTypes.Add(LlrpParameterType.C1G2BlockEraseOPSpecResult);
            expectedTypes.Add(LlrpParameterType.C1G2BlockWriteOPSpecResult);
            expectedTypes.Add(LlrpParameterType.ClientOperationOPSpecResult);
            while (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, parameterEndLimit, out type))
            {
                switch (type)
                {
                    case LlrpParameterType.C1G2ReadOPSpecResult:
                    {
                        airProtocolOpSpecs.Add(new C1G2ReadOPSpecResult(bitArray, ref index));
                        continue;
                    }
                    case LlrpParameterType.C1G2WriteOPSpecResult:
                    {
                        airProtocolOpSpecs.Add(new C1G2WriteOPSpecResult(bitArray, ref index));
                        continue;
                    }
                    case LlrpParameterType.C1G2KillOPSpecResult:
                    {
                        airProtocolOpSpecs.Add(new C1G2KillOPSpecResult(bitArray, ref index));
                        continue;
                    }
                    case LlrpParameterType.C1G2LockOPSpecResult:
                    {
                        airProtocolOpSpecs.Add(new C1G2LockOPSpecResult(bitArray, ref index));
                        continue;
                    }
                    case LlrpParameterType.C1G2BlockEraseOPSpecResult:
                    {
                        airProtocolOpSpecs.Add(new C1G2BlockEraseOPSpecResult(bitArray, ref index));
                        continue;
                    }
                    case LlrpParameterType.C1G2BlockWriteOPSpecResult:
                    {
                        airProtocolOpSpecs.Add(new C1G2BlockWriteOPSpecResult(bitArray, ref index));
                        continue;
                    }
                    case LlrpParameterType.ClientOperationOPSpecResult:
                        break;

                    default:
                    {
                        continue;
                    }
                }
                clientRequestOpSpecsResult.Add(new ClientRequestOPSpecResult(bitArray, ref index));
            }
            Collection<CustomParameterBase> customParameters = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index, parameterEndLimit))
            {
                customParameters.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(epc, epcData, id, specIndex, inventoryId, antennaId, peakRSSI, channelIndex, firstUTC, firstUptime, lastUTC, lastUptime, tagSeenCount, airProtocolTagData, accessSpecId, airProtocolOpSpecs, clientRequestOpSpecsResult, customParameters);
        }

        public TagReportData(Kalitte.Sensors.Rfid.Llrp.Core.EPC96 epc96, Kalitte.Sensors.Rfid.Llrp.Core.EpcData epcData, Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId roSpecId, Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex specIndex, Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpecId inventoryId, Kalitte.Sensors.Rfid.Llrp.Core.AntennaId antennaId, Kalitte.Sensors.Rfid.Llrp.Core.PeakRssi peakRSSI, Kalitte.Sensors.Rfid.Llrp.Core.ChannelIndex channelIndex, Kalitte.Sensors.Rfid.Llrp.Core.FirstSeenTimestampUtc firstUTC, Kalitte.Sensors.Rfid.Llrp.Core.FirstSeenTimestampUptime firstUptime, Kalitte.Sensors.Rfid.Llrp.Core.LastSeenTimestampUtc lastUTC, Kalitte.Sensors.Rfid.Llrp.Core.LastSeenTimestampUptime lastUptime, Kalitte.Sensors.Rfid.Llrp.Core.TagSeenCount tagSeenCount, Collection<Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolTagData> airProtocolTagData, Kalitte.Sensors.Rfid.Llrp.Core.AccessSpecId accessSpecId, Collection<AirProtocolSpecificOPSpecResult> airProtocolOpSpecs, Collection<ClientRequestOPSpecResult> clientRequestOpSpecsResult, Collection<CustomParameterBase> customParameters) : base(LlrpParameterType.TagReportData)
        {
            this.Init(epc96, epcData, roSpecId, specIndex, inventoryId, antennaId, peakRSSI, channelIndex, firstUTC, firstUptime, lastUTC, lastUptime, tagSeenCount, airProtocolTagData, accessSpecId, airProtocolOpSpecs, clientRequestOpSpecsResult, customParameters);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            Util.Encode(this.m_epc96, stream);
            Util.Encode(this.m_epcData, stream);
            Util.Encode(this.m_roSpecId, stream);
            Util.Encode(this.m_specIndex, stream);
            Util.Encode(this.m_inventoryParamSpecId, stream);
            Util.Encode(this.m_antennaId, stream);
            Util.Encode(this.m_peakRSSI, stream);
            Util.Encode(this.m_channelIndex, stream);
            Util.Encode(this.m_firstSeenUTC, stream);
            Util.Encode(this.m_firstSeenUptime, stream);
            Util.Encode(this.m_lastSeenUTC, stream);
            Util.Encode(this.m_lastSeenUptime, stream);
            Util.Encode(this.m_tagSeenCount, stream);
            Util.Encode<Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolTagData>(this.m_airProtocolTagData, stream);
            Util.Encode(this.m_accessSpecId, stream);
            Util.Encode<AirProtocolSpecificOPSpecResult>(this.m_airProtocolOpSpec, stream);
            Util.Encode<ClientRequestOPSpecResult>(this.m_clientRequestOpSpec, stream);
            Util.Encode<CustomParameterBase>(this.m_customParams, stream);
        }

        private void Init(Kalitte.Sensors.Rfid.Llrp.Core.EPC96 epc96, Kalitte.Sensors.Rfid.Llrp.Core.EpcData epcData, Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId roSpecId, Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex specIndex, Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpecId inventoryId, Kalitte.Sensors.Rfid.Llrp.Core.AntennaId antennaId, Kalitte.Sensors.Rfid.Llrp.Core.PeakRssi peakRSSI, Kalitte.Sensors.Rfid.Llrp.Core.ChannelIndex channelIndex, Kalitte.Sensors.Rfid.Llrp.Core.FirstSeenTimestampUtc firstUTC, Kalitte.Sensors.Rfid.Llrp.Core.FirstSeenTimestampUptime firstUptime, Kalitte.Sensors.Rfid.Llrp.Core.LastSeenTimestampUtc lastUTC, Kalitte.Sensors.Rfid.Llrp.Core.LastSeenTimestampUptime lastUptime, Kalitte.Sensors.Rfid.Llrp.Core.TagSeenCount tagSeenCount, Collection<Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolTagData> airProtocolTagData, Kalitte.Sensors.Rfid.Llrp.Core.AccessSpecId accessSpecId, Collection<AirProtocolSpecificOPSpecResult> airProtocolOpSpecs, Collection<ClientRequestOPSpecResult> clientRequestOpSpecsResult, Collection<CustomParameterBase> customParameters)
        {
            if ((epc96 == null) && (epcData == null))
            {
                throw new ArgumentException(LlrpResources.NoEPCDataFound);
            }
            if ((epc96 != null) && (epcData != null))
            {
                throw new ArgumentException(LlrpResources.BothEPCDataPresent);
            }
            Util.CheckCollectionForNonNullElement<Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolTagData>(airProtocolTagData);
            Util.CheckCollectionForNonNullElement<AirProtocolSpecificOPSpecResult>(airProtocolOpSpecs);
            Util.CheckCollectionForNonNullElement<ClientRequestOPSpecResult>(clientRequestOpSpecsResult);
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customParameters);
            this.m_epc96 = epc96;
            this.m_epcData = epcData;
            this.m_roSpecId = roSpecId;
            this.m_specIndex = specIndex;
            this.m_inventoryParamSpecId = inventoryId;
            this.m_antennaId = antennaId;
            this.m_peakRSSI = peakRSSI;
            this.m_channelIndex = channelIndex;
            this.m_firstSeenUTC = firstUTC;
            this.m_firstSeenUptime = firstUptime;
            this.m_lastSeenUTC = lastUTC;
            this.m_lastSeenUptime = lastUptime;
            this.m_tagSeenCount = tagSeenCount;
            this.m_airProtocolTagData = airProtocolTagData;
            this.m_accessSpecId = accessSpecId;
            this.m_airProtocolOpSpec = airProtocolOpSpecs;
            this.m_clientRequestOpSpec = clientRequestOpSpecsResult;
            this.m_customParams = customParameters;
            this.ParameterLength = ((((((((((((((((Util.GetBitLengthOfParam(this.m_epc96) + Util.GetBitLengthOfParam(this.m_epcData)) + Util.GetBitLengthOfParam(this.m_roSpecId)) + Util.GetBitLengthOfParam(this.m_specIndex)) + Util.GetBitLengthOfParam(this.m_inventoryParamSpecId)) + Util.GetBitLengthOfParam(this.m_antennaId)) + Util.GetBitLengthOfParam(this.m_peakRSSI)) + Util.GetBitLengthOfParam(this.m_channelIndex)) + Util.GetBitLengthOfParam(this.m_firstSeenUTC)) + Util.GetBitLengthOfParam(this.m_firstSeenUptime)) + Util.GetBitLengthOfParam(this.m_lastSeenUTC)) + Util.GetBitLengthOfParam(this.m_lastSeenUptime)) + Util.GetBitLengthOfParam(this.m_tagSeenCount)) + Util.GetTotalBitLengthOfParam<Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolTagData>(this.m_airProtocolTagData)) + Util.GetBitLengthOfParam(this.m_accessSpecId)) + Util.GetTotalBitLengthOfParam<AirProtocolSpecificOPSpecResult>(this.m_airProtocolOpSpec)) + Util.GetTotalBitLengthOfParam<ClientRequestOPSpecResult>(this.m_clientRequestOpSpec)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.m_customParams);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Tag Report Data>");
            strBuilder.Append(base.ToString());
            Util.ToString(this.EpcData, strBuilder);
            Util.ToString(this.EPC96, strBuilder);
            Util.ToString(this.SpecIndex, strBuilder);
            Util.ToString(this.InventoryParameterSpecId, strBuilder);
            Util.ToString(this.AntennaId, strBuilder);
            Util.ToString(this.PeakRssi, strBuilder);
            Util.ToString(this.ChannelIndex, strBuilder);
            Util.ToString(this.FirstSeenTimestampUtc, strBuilder);
            Util.ToString(this.FirstSeenTimestampUptime, strBuilder);
            Util.ToString(this.LastSeenTimestampUtc, strBuilder);
            Util.ToString(this.LastSeenTimestampUptime, strBuilder);
            Util.ToString(this.TagSeenCount, strBuilder);
            Util.ToString<Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolTagData>(this.AirProtocolTagData, strBuilder);
            Util.ToString(this.AccessSpecId, strBuilder);
            Util.ToString<AirProtocolSpecificOPSpecResult>(this.AirProtocolSpecificOPSpecResults, strBuilder);
            Util.ToString<ClientRequestOPSpecResult>(this.ClientRequestOPSpecResults, strBuilder);
            Util.ToString<CustomParameterBase>(this.CustomParameters, strBuilder);
            strBuilder.Append("</Tag Report Data>");
            return strBuilder.ToString();
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.AccessSpecId AccessSpecId
        {
            get
            {
                return this.m_accessSpecId;
            }
        }

        public Collection<AirProtocolSpecificOPSpecResult> AirProtocolSpecificOPSpecResults
        {
            get
            {
                return this.m_airProtocolOpSpec;
            }
        }

        public Collection<Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolTagData> AirProtocolTagData
        {
            get
            {
                return this.m_airProtocolTagData;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.AntennaId AntennaId
        {
            get
            {
                return this.m_antennaId;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.ChannelIndex ChannelIndex
        {
            get
            {
                return this.m_channelIndex;
            }
        }

        public Collection<ClientRequestOPSpecResult> ClientRequestOPSpecResults
        {
            get
            {
                return this.m_clientRequestOpSpec;
            }
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customParams;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.EPC96 EPC96
        {
            get
            {
                return this.m_epc96;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.EpcData EpcData
        {
            get
            {
                return this.m_epcData;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.FirstSeenTimestampUptime FirstSeenTimestampUptime
        {
            get
            {
                return this.m_firstSeenUptime;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.FirstSeenTimestampUtc FirstSeenTimestampUtc
        {
            get
            {
                return this.m_firstSeenUTC;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpecId InventoryParameterSpecId
        {
            get
            {
                return this.m_inventoryParamSpecId;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.LastSeenTimestampUptime LastSeenTimestampUptime
        {
            get
            {
                return this.m_lastSeenUptime;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.LastSeenTimestampUtc LastSeenTimestampUtc
        {
            get
            {
                return this.m_lastSeenUTC;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.PeakRssi PeakRssi
        {
            get
            {
                return this.m_peakRSSI;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId ROSpecId
        {
            get
            {
                return this.m_roSpecId;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex SpecIndex
        {
            get
            {
                return this.m_specIndex;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.TagSeenCount TagSeenCount
        {
            get
            {
                return this.m_tagSeenCount;
            }
        }
    }
}
