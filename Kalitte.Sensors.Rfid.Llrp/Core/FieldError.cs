namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    public sealed class FieldError : LlrpTlvParameterBase
    {
        private StatusCode m_errorCode;
        private short m_fieldNumber;

        internal FieldError(BitArray bitArray, ref int index) : base(LlrpParameterType.FieldError, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            short fieldNumber = (short) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            short num3 = (short) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            StatusCode enumInstance = BitHelper.GetEnumInstance<StatusCode>(num3);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(fieldNumber, enumInstance);
        }

        public FieldError(short fieldNumber, StatusCode errorCode) : base(LlrpParameterType.FieldError)
        {
            this.Init(fieldNumber, errorCode);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.FieldNumber, 0x10, true);
            stream.Append((long) ((short) this.ErrorCode), 0x10, true);
        }

        private void Init(short fieldNumber, StatusCode errorCode)
        {
            this.m_fieldNumber = fieldNumber;
            this.m_errorCode = errorCode;
            this.ParameterLength = 0x20;
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(LlrpResources.FieldErrorToStringHeader);
            strBuilder.Append(LlrpResources.FieldNumberToStringHeader);
            strBuilder.Append(this.FieldNumber);
            strBuilder.Append(LlrpResources.FieldNumberToStringFooter);
            Util.ToString(this.ErrorCode, strBuilder);
            strBuilder.Append(LlrpResources.FieldErrorToStringFooter);
            return strBuilder.ToString();
        }

        public StatusCode ErrorCode
        {
            get
            {
                return this.m_errorCode;
            }
        }

        public short FieldNumber
        {
            get
            {
                return this.m_fieldNumber;
            }
        }
    }
}
