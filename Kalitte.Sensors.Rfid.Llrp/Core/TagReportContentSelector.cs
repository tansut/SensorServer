namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class TagReportContentSelector : LlrpTlvParameterBase
    {
        private bool m_enableAccessSpecId;
        private bool m_enableAntennaId;
        private bool m_enableChannelIndex;
        private bool m_enableFirstSeenTimeStamp;
        private bool m_enableInventoryParameterSpecId;
        private bool m_enableLastSeenTimeStamp;
        private bool m_enablePeakRSSI;
        private bool m_enableROSpecId;
        private bool m_enableSpecIndex;
        private bool m_enableTagSeenCount;
        private Collection<AirProtocolSpecificEpcMemorySelectorParameter> m_memorySelector;

        internal TagReportContentSelector(BitArray bitArray, ref int index) : base(LlrpParameterType.TagReportContentSelector, bitArray, index)
        {
            LlrpParameterType type;
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            bool enableROSpecId = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool enableSpecIndex = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool enableInventoryParameterSpecId = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool enableAntennaId = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool enableChannelIndex = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool enablePeakRSSI = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool enableFirstSeenTimeStamp = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool enableLastSeenTimeStamp = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool enableTagSeenCount = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool enableAccessSpecId = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 6;
            Collection<LlrpParameterType> expectedTypes = new Collection<LlrpParameterType>();
            expectedTypes.Add(LlrpParameterType.C1G2EpcMemorySelector);
            Collection<AirProtocolSpecificEpcMemorySelectorParameter> memorySelector = new Collection<AirProtocolSpecificEpcMemorySelectorParameter>();
            while (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, parameterEndLimit, out type))
            {
                switch (type)
                {
                    case LlrpParameterType.C1G2EpcMemorySelector:
                    {
                        memorySelector.Add(new C1G2EpcMemorySelector(bitArray, ref index));
                        break;
                    }
                }
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enableROSpecId, enableSpecIndex, enableInventoryParameterSpecId, enableAntennaId, enableChannelIndex, enablePeakRSSI, enableFirstSeenTimeStamp, enableLastSeenTimeStamp, enableTagSeenCount, enableAccessSpecId, memorySelector);
        }

        public TagReportContentSelector(bool enableROSpecId, bool enableSpecIndex, bool enableInventoryParameterSpecId, bool enableAntennaId, bool enableChannelIndex, bool enablePeakRSSI, bool enableFirstSeenTimestamp, bool enableLastSeenTimestamp, bool enableTagSeenCount, bool enableAccessSpecId, Collection<AirProtocolSpecificEpcMemorySelectorParameter> memorySelector) : base(LlrpParameterType.TagReportContentSelector)
        {
            this.Init(enableROSpecId, enableSpecIndex, enableInventoryParameterSpecId, enableAntennaId, enableChannelIndex, enablePeakRSSI, enableFirstSeenTimestamp, enableLastSeenTimestamp, enableTagSeenCount, enableAccessSpecId, memorySelector);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append(this.EnableROSpecId, 1, true);
            stream.Append(this.EnableSpecIndex, 1, true);
            stream.Append(this.EnableInventoryParameterSpecId, 1, true);
            stream.Append(this.EnableAntennaId, 1, true);
            stream.Append(this.EnableChannelIndex, 1, true);
            stream.Append(this.EnablePeakRssi, 1, true);
            stream.Append(this.EnableFirstSeenTimestamp, 1, true);
            stream.Append(this.EnableLastSeenTimestamp, 1, true);
            stream.Append(this.EnableTagSeenCount, 1, true);
            stream.Append(this.EnableAccessSpecId, 1, true);
            stream.Append((ulong) 0L, 6, true);
            Util.Encode<AirProtocolSpecificEpcMemorySelectorParameter>(this.MemorySelector, stream);
        }

        private void Init(bool enableROSpecId, bool enableSpecIndex, bool enableInventoryParameterSpecId, bool enableAntennaId, bool enableChannelIndex, bool enablePeakRSSI, bool enableFirstSeenTimeStamp, bool enableLastSeenTimeStamp, bool enableTagSeenCount, bool enableAccessSpecId, Collection<AirProtocolSpecificEpcMemorySelectorParameter> memorySelector)
        {
            Util.CheckCollectionForNonNullElement<AirProtocolSpecificEpcMemorySelectorParameter>(memorySelector);
            this.m_enableROSpecId = enableROSpecId;
            this.m_enableSpecIndex = enableSpecIndex;
            this.m_enableInventoryParameterSpecId = enableInventoryParameterSpecId;
            this.m_enableAntennaId = enableAntennaId;
            this.m_enableChannelIndex = enableChannelIndex;
            this.m_enablePeakRSSI = enablePeakRSSI;
            this.m_enableFirstSeenTimeStamp = enableFirstSeenTimeStamp;
            this.m_enableLastSeenTimeStamp = enableLastSeenTimeStamp;
            this.m_enableTagSeenCount = enableTagSeenCount;
            this.m_enableAccessSpecId = enableAccessSpecId;
            this.m_memorySelector = memorySelector;
            this.ParameterLength = 0x10 + Util.GetTotalBitLengthOfParam<AirProtocolSpecificEpcMemorySelectorParameter>(this.MemorySelector);
        }

        public bool EnableAccessSpecId
        {
            get
            {
                return this.m_enableAccessSpecId;
            }
        }

        public bool EnableAntennaId
        {
            get
            {
                return this.m_enableAntennaId;
            }
        }

        public bool EnableChannelIndex
        {
            get
            {
                return this.m_enableChannelIndex;
            }
        }

        public bool EnableFirstSeenTimestamp
        {
            get
            {
                return this.m_enableFirstSeenTimeStamp;
            }
        }

        public bool EnableInventoryParameterSpecId
        {
            get
            {
                return this.m_enableInventoryParameterSpecId;
            }
        }

        public bool EnableLastSeenTimestamp
        {
            get
            {
                return this.m_enableLastSeenTimeStamp;
            }
        }

        public bool EnablePeakRssi
        {
            get
            {
                return this.m_enablePeakRSSI;
            }
        }

        public bool EnableROSpecId
        {
            get
            {
                return this.m_enableROSpecId;
            }
        }

        public bool EnableSpecIndex
        {
            get
            {
                return this.m_enableSpecIndex;
            }
        }

        public bool EnableTagSeenCount
        {
            get
            {
                return this.m_enableTagSeenCount;
            }
        }

        public Collection<AirProtocolSpecificEpcMemorySelectorParameter> MemorySelector
        {
            get
            {
                return this.m_memorySelector;
            }
        }
    }
}
