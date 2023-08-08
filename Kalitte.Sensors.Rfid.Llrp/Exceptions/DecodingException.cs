namespace Kalitte.Sensors.Rfid.Llrp.Exceptions
{
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using System;
    using System.Text;

    internal class DecodingException : Exception
    {
        private string m_errorCode;
        private long m_id;
        private LlrpMessageType m_messageType;

        internal DecodingException(string errorCode, string message) : base(message)
        {
            this.m_errorCode = errorCode;
        }

        internal DecodingException(string errorCode, string message, Exception innerException) : base(message, innerException)
        {
            this.m_errorCode = errorCode;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("<DecodingErrorCode>");
            builder.Append(this.ErrorCode);
            builder.Append("</DecodingErrorCode>");
            return builder.ToString();
        }

        public string ErrorCode
        {
            get
            {
                return this.m_errorCode;
            }
        }

        public long MessageId
        {
            get
            {
                return this.m_id;
            }
            internal set
            {
                this.m_id = value;
            }
        }

        public LlrpMessageType MessageType
        {
            get
            {
                return this.m_messageType;
            }
            set
            {
                this.m_messageType = value;
            }
        }
    }
}
