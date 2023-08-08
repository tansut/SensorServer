namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    public sealed class LlrpStatus : LlrpTlvParameterBase
    {
        private StatusCode m_errorCode;
        private Kalitte.Sensors.Rfid.Llrp.Core.FieldError m_fieldError;
        private Kalitte.Sensors.Rfid.Llrp.Core.ParameterError m_parameterError;
        private string m_statusString;

        internal LlrpStatus(BitArray bitArray, ref int index) : base(LlrpParameterType.LlrpStatus, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            string statusString = null;
            Kalitte.Sensors.Rfid.Llrp.Core.FieldError fieldError = null;
            Kalitte.Sensors.Rfid.Llrp.Core.ParameterError parameterError = null;
            short num2 = (short) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            StatusCode enumInstance = BitHelper.GetEnumInstance<StatusCode>(num2);
            ushort byteCount = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            statusString = BitHelper.GetString(bitArray, ref index, byteCount);
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.FieldError, bitArray, index, parameterEndLimit))
            {
                fieldError = new Kalitte.Sensors.Rfid.Llrp.Core.FieldError(bitArray, ref index);
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ParameterError, bitArray, index, parameterEndLimit))
            {
                parameterError = new Kalitte.Sensors.Rfid.Llrp.Core.ParameterError(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, statusString, fieldError, parameterError);
        }

        public LlrpStatus(StatusCode errorCode, string status, Kalitte.Sensors.Rfid.Llrp.Core.FieldError fieldError, Kalitte.Sensors.Rfid.Llrp.Core.ParameterError parameterError) : base(LlrpParameterType.LlrpStatus)
        {
            this.Init(errorCode, status, fieldError, parameterError);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((short) this.ErrorCode), 0x10, true);
            if (this.StatusString != null)
            {
                byte[] element = Util.ConvertUnicodeToUTF8(this.StatusString);
                stream.Append((long) ((ushort) element.Length), 0x10, true);
                stream.Append(element, (uint) (element.Length * 8), false);
            }
            else
            {
                stream.Append((long) 0L, 0x10, true);
            }
            Util.Encode(this.FieldError, stream);
            Util.Encode(this.ParameterError, stream);
        }

        private void Init(StatusCode errorCode, string statusString, Kalitte.Sensors.Rfid.Llrp.Core.FieldError fieldError, Kalitte.Sensors.Rfid.Llrp.Core.ParameterError parameterError)
        {
            this.m_errorCode = errorCode;
            this.m_statusString = statusString;
            this.m_fieldError = fieldError;
            this.m_parameterError = parameterError;
            ushort num = 0;
            if (this.StatusString != null)
            {
                num = (ushort) (Util.ConvertUnicodeToUTF8(this.StatusString).Length * 8);
            }
            this.ParameterLength = (((uint) (0x20 + num)) + Util.GetBitLengthOfParam(this.FieldError)) + Util.GetBitLengthOfParam(this.ParameterError);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(LlrpResources.LlrpStatusToStringHeader);
            Util.ToString(this.ErrorCode, strBuilder);
            Util.ToString(this.FieldError, strBuilder);
            Util.ToString(this.ParameterError, strBuilder);
            if (this.StatusString != null)
            {
                strBuilder.Append(LlrpResources.ErrorDescriptionToStringHeader);
                strBuilder.Append(this.StatusString);
                strBuilder.Append(LlrpResources.ErrorDescriptionToStringFooter);
            }
            strBuilder.Append(LlrpResources.LlrpStatusToStringFooter);
            return strBuilder.ToString();
        }

        public StatusCode ErrorCode
        {
            get
            {
                return this.m_errorCode;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.FieldError FieldError
        {
            get
            {
                return this.m_fieldError;
            }
        }

        public bool IsSuccess
        {
            get
            {
                return (this.m_errorCode == StatusCode.MessageSuccess);
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.ParameterError ParameterError
        {
            get
            {
                return this.m_parameterError;
            }
        }

        public string StatusString
        {
            get
            {
                return this.m_statusString;
            }
        }
    }
}
