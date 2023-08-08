namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Rfid.Llrp.Exceptions;

    public abstract class LlrpMessageResponseBase : LlrpMessageBase
    {
        private readonly int m_baseLength;
        private LlrpStatus m_status;

        protected internal LlrpMessageResponseBase(LlrpMessageType messageType, BitArray bitArray) : base(messageType, bitArray)
        {
            LlrpStatus status = null;
            int index = 80;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.LlrpStatus, bitArray, index))
            {
                status = new LlrpStatus(bitArray, ref index);
            }
            this.Init(status);
            this.m_baseLength = 80 + ((int) this.Status.ParameterLength);
        }

        protected LlrpMessageResponseBase(LlrpMessageType messageType, uint messageId, LlrpStatus status) : base(messageType, messageId)
        {
            this.Init(status);
            this.m_baseLength = 80 + ((int) this.Status.ParameterLength);
        }

        internal LLRPMessageStream CreateResponseHeaderStream()
        {
            LLRPMessageStream stream = this.CreateHeaderStream();
            Util.Encode(this.m_status, stream);
            return stream;
        }

        private void Init(LlrpStatus status)
        {
            if (status == null)
            {
                throw new DecodingException("Missing parameter in message", string.Format(CultureInfo.CurrentCulture, LlrpResources.MissingParameterInMessage, new object[] { typeof(LlrpStatus).FullName, base.GetType().FullName }));
            }
            this.m_status = status;
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<LLRP Message Response>");
            strBuilder.Append(base.ToString());
            Util.ToString(this.Status, strBuilder);
            strBuilder.Append("</LLRP Message Response>");
            return strBuilder.ToString();
        }

        protected int BaseLength
        {
            get
            {
                return this.m_baseLength;
            }
        }

        internal override ulong MessageLength
        {
            get
            {
                return base.MessageLength;
            }
            set
            {
                base.MessageLength = value + Util.GetBitLengthOfParam(this.m_status);
            }
        }

        public LlrpStatus Status
        {
            get
            {
                return this.m_status;
            }
        }
    }
}
