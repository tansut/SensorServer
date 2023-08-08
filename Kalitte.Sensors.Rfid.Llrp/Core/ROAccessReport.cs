namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class ROAccessReport : LlrpMessageBase
    {
        private Collection<CustomParameterBase> m_customParameters;
        private Collection<RFSurveyReportData> m_surveyReportDatas;
        private Collection<TagReportData> m_tagReportDatas;

        internal ROAccessReport(BitArray bitArray) : base(LlrpMessageType.ROAccessReport, bitArray)
        {
            int index = 80;
            Collection<TagReportData> tagReports = new Collection<TagReportData>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.TagReportData, bitArray, index))
            {
                tagReports.Add(new TagReportData(bitArray, ref index));
            }
            Collection<RFSurveyReportData> surveyReports = new Collection<RFSurveyReportData>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.RFSurveyReportData, bitArray, index))
            {
                surveyReports.Add(new RFSurveyReportData(bitArray, ref index));
            }
            Collection<CustomParameterBase> customParameters = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index))
            {
                customParameters.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, (uint) bitArray.Count, base.GetType().FullName);
            this.Init(tagReports, surveyReports, customParameters);
        }

        public ROAccessReport(Collection<TagReportData> tagReports, Collection<RFSurveyReportData> surveyReports, Collection<CustomParameterBase> customParameters) : base(LlrpMessageType.ROAccessReport, IdGenerator.GenerateLlrpMessageId())
        {
            this.Init(tagReports, surveyReports, customParameters);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = this.CreateHeaderStream();
            Util.Encode<TagReportData>(this.m_tagReportDatas, stream);
            Util.Encode<RFSurveyReportData>(this.m_surveyReportDatas, stream);
            Util.Encode<CustomParameterBase>(this.m_customParameters, stream);
            return stream.Merge();
        }

        private void Init(Collection<TagReportData> tagReports, Collection<RFSurveyReportData> surveyReports, Collection<CustomParameterBase> customParameters)
        {
            Util.CheckCollectionForNonNullElement<TagReportData>(tagReports);
            Util.CheckCollectionForNonNullElement<RFSurveyReportData>(surveyReports);
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customParameters);
            this.m_tagReportDatas = tagReports;
            this.m_surveyReportDatas = surveyReports;
            this.m_customParameters = customParameters;
            this.MessageLength = (Util.GetTotalBitLengthOfParam<TagReportData>(this.m_tagReportDatas) + Util.GetTotalBitLengthOfParam<RFSurveyReportData>(this.m_surveyReportDatas)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.m_customParameters);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<RO Access Report Message>");
            strBuilder.Append(base.ToString());
            Util.ToString<TagReportData>(this.TagReports, strBuilder);
            Util.ToString<RFSurveyReportData>(this.RFSurveyReports, strBuilder);
            Util.ToString<CustomParameterBase>(this.CustomParameters, strBuilder);
            strBuilder.Append("</RO Access Report Message>");
            return strBuilder.ToString();
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customParameters;
            }
        }

        public Collection<RFSurveyReportData> RFSurveyReports
        {
            get
            {
                return this.m_surveyReportDatas;
            }
        }

        public Collection<TagReportData> TagReports
        {
            get
            {
                return this.m_tagReportDatas;
            }
        }
    }
}
