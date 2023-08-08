namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class RFSurveyReportData : LlrpTlvParameterBase
    {
        private Collection<CustomParameterBase> m_customParams;
        private Collection<FrequencyRssiLevelEntry> m_frequencyRSSILevenEntries;
        private Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId m_roID;
        private Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex m_specIndex;

        internal RFSurveyReportData(BitArray bitArray, ref int index) : base(LlrpParameterType.RFSurveyReportData, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId roId = null;
            Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex specIndex = null;
            Collection<FrequencyRssiLevelEntry> frequencyRSSILevelEnteries = new Collection<FrequencyRssiLevelEntry>();
            Collection<CustomParameterBase> customParameters = new Collection<CustomParameterBase>();
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ROSpecId, bitArray, index, parameterEndLimit))
            {
                roId = new Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId(bitArray, ref index);
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.SpecIndex, bitArray, index, parameterEndLimit))
            {
                specIndex = new Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex(bitArray, ref index);
            }
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.FrequencyRssiLevelEntry, bitArray, index, parameterEndLimit))
            {
                frequencyRSSILevelEnteries.Add(new FrequencyRssiLevelEntry(bitArray, ref index));
            }
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index, parameterEndLimit))
            {
                customParameters.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(roId, specIndex, frequencyRSSILevelEnteries, customParameters);
        }

        public RFSurveyReportData(Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId roSpecId, Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex specIndex, Collection<FrequencyRssiLevelEntry> frequencyRSSILevelEntries, Collection<CustomParameterBase> customParameters) : base(LlrpParameterType.RFSurveyReportData)
        {
            this.Init(roSpecId, specIndex, frequencyRSSILevelEntries, customParameters);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            Util.Encode(this.ROSpecId, stream);
            Util.Encode(this.SpecIndex, stream);
            Util.Encode<FrequencyRssiLevelEntry>(this.FrequencyRssiLevelEntries, stream);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
        }

        private void Init(Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId roId, Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex specIndex, Collection<FrequencyRssiLevelEntry> frequencyRSSILevelEnteries, Collection<CustomParameterBase> customParameters)
        {
            if ((frequencyRSSILevelEnteries == null) || (frequencyRSSILevelEnteries.Count == 0))
            {
                throw new ArgumentNullException("frequencyRSSILevelEnteries");
            }
            Util.CheckCollectionForNonNullElement<FrequencyRssiLevelEntry>(frequencyRSSILevelEnteries);
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customParameters);
            this.m_roID = roId;
            this.m_specIndex = specIndex;
            this.m_frequencyRSSILevenEntries = frequencyRSSILevelEnteries;
            this.m_customParams = customParameters;
            this.ParameterLength = ((Util.GetBitLengthOfParam(this.m_roID) + Util.GetBitLengthOfParam(this.m_specIndex)) + Util.GetTotalBitLengthOfParam<FrequencyRssiLevelEntry>(this.m_frequencyRSSILevenEntries)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.m_customParams);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<RF Survey Report Data>");
            strBuilder.Append(base.ToString());
            strBuilder.Append("<RO Spec Id>");
            strBuilder.Append(this.ROSpecId);
            strBuilder.Append("</RO Spec Id>");
            strBuilder.Append("<Spec Index>");
            strBuilder.Append(this.SpecIndex);
            strBuilder.Append("<Spec Index>");
            Util.ToString<FrequencyRssiLevelEntry>(this.FrequencyRssiLevelEntries, strBuilder);
            Util.ToString<CustomParameterBase>(this.CustomParameters, strBuilder);
            return strBuilder.ToString();
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customParams;
            }
        }

        public Collection<FrequencyRssiLevelEntry> FrequencyRssiLevelEntries
        {
            get
            {
                return this.m_frequencyRSSILevenEntries;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId ROSpecId
        {
            get
            {
                return this.m_roID;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex SpecIndex
        {
            get
            {
                return this.m_specIndex;
            }
        }
    }
}
