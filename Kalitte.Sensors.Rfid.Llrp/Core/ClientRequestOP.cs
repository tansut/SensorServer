namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class ClientRequestOP : LlrpMessageBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.TagReportData m_tagReportData;

        public ClientRequestOP(Kalitte.Sensors.Rfid.Llrp.Core.TagReportData tagReportData) : base(LlrpMessageType.ClientRequestOP, IdGenerator.GenerateLlrpMessageId())
        {
            this.Init(tagReportData);
        }

        internal ClientRequestOP(BitArray bitArray) : base(LlrpMessageType.ClientRequestOP, bitArray)
        {
            int index = 80;
            Kalitte.Sensors.Rfid.Llrp.Core.TagReportData tagReportData = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.TagReportData, bitArray, index))
            {
                tagReportData = new Kalitte.Sensors.Rfid.Llrp.Core.TagReportData(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, (uint) bitArray.Count, base.GetType().FullName);
            this.Init(tagReportData);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = this.CreateHeaderStream();
            Util.Encode(this.TagReportData, stream);
            return stream.Merge();
        }

        private void Init(Kalitte.Sensors.Rfid.Llrp.Core.TagReportData tagReportData)
        {
            if (tagReportData == null)
            {
                throw new ArgumentNullException("tagReportData");
            }
            this.m_tagReportData = tagReportData;
            this.MessageLength = this.TagReportData.ParameterLength;
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.TagReportData TagReportData
        {
            get
            {
                return this.m_tagReportData;
            }
        }
    }
}
