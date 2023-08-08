namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class ROReportSpec : LlrpTlvParameterBase
    {
        private Collection<CustomParameterBase> m_customParameters;
        private ushort m_numberOfTagReportData;
        private TagReportContentSelector m_reportContentSelector;
        private ROReportTrigger m_reportTrigger;

        internal ROReportSpec(BitArray bitArray, ref int index) : base(LlrpParameterType.ROReportSpec, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ROReportTrigger enumInstance = BitHelper.GetEnumInstance<ROReportTrigger>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            ushort numberOfTagReportData = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            TagReportContentSelector contentSelector = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.TagReportContentSelector, bitArray, index, parameterEndLimit))
            {
                contentSelector = new TagReportContentSelector(bitArray, ref index);
            }
            Collection<CustomParameterBase> customParameter = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index, parameterEndLimit))
            {
                customParameter.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, numberOfTagReportData, contentSelector, customParameter);
        }

        public ROReportSpec(ROReportTrigger reportTrigger, ushort numberOfTagReportData, TagReportContentSelector contentSelector, Collection<CustomParameterBase> customParameters) : base(LlrpParameterType.ROReportSpec)
        {
            this.Init(reportTrigger, numberOfTagReportData, contentSelector, customParameters);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.ReportTrigger), 8, true);
            stream.Append((long) this.NumberOfTagReportData, 0x10, true);
            Util.Encode(this.ContentSelector, stream);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
        }

        private void Init(ROReportTrigger reportTrigger, ushort numberOfTagReportData, TagReportContentSelector contentSelector, Collection<CustomParameterBase> customParameter)
        {
            if (contentSelector == null)
            {
                throw new ArgumentNullException("contentSelector");
            }
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customParameter);
            this.m_reportTrigger = reportTrigger;
            this.m_numberOfTagReportData = numberOfTagReportData;
            this.m_reportContentSelector = contentSelector;
            this.m_customParameters = customParameter;
            this.ParameterLength = (0x18 + Util.GetBitLengthOfParam(this.ContentSelector)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.CustomParameters);
        }

        public TagReportContentSelector ContentSelector
        {
            get
            {
                return this.m_reportContentSelector;
            }
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customParameters;
            }
        }

        public ushort NumberOfTagReportData
        {
            get
            {
                return this.m_numberOfTagReportData;
            }
        }

        public ROReportTrigger ReportTrigger
        {
            get
            {
                return this.m_reportTrigger;
            }
        }
    }
}
