namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    public sealed class ParameterError : LlrpTlvParameterBase
    {
        private LlrpParameterType m_erroneousParameter;
        private StatusCode m_errorCode;
        private Kalitte.Sensors.Rfid.Llrp.Core.FieldError m_fieldError;
        private ParameterError m_parameterError;

        internal ParameterError(BitArray bitArray, ref int index) : base(LlrpParameterType.ParameterError, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Kalitte.Sensors.Rfid.Llrp.Core.FieldError fieldError = null;
            ParameterError paramError = null;
            short num2 = (short) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            short num3 = (short) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            StatusCode enumInstance = BitHelper.GetEnumInstance<StatusCode>(num3);
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.FieldError, bitArray, index, parameterEndLimit))
            {
                fieldError = new Kalitte.Sensors.Rfid.Llrp.Core.FieldError(bitArray, ref index);
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ParameterError, bitArray, index, parameterEndLimit))
            {
                paramError = new ParameterError(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(BitHelper.GetEnumInstance<LlrpParameterType>(num2), enumInstance, fieldError, paramError);
        }

        public ParameterError(LlrpParameterType parameterType, StatusCode errorCode, Kalitte.Sensors.Rfid.Llrp.Core.FieldError fieldError, ParameterError parameterError) : base(LlrpParameterType.ParameterError)
        {
            this.Init(parameterType, errorCode, fieldError, parameterError);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((ushort) this.ErroneousParameterType), 0x10, true);
            stream.Append((long) ((short) this.ErrorCode), 0x10, true);
            Util.Encode(this.FieldError, stream);
            Util.Encode(this.InnerParameterError, stream);
        }

        private void Init(LlrpParameterType paramType, StatusCode errorCode, Kalitte.Sensors.Rfid.Llrp.Core.FieldError fieldError, ParameterError paramError)
        {
            this.m_erroneousParameter = paramType;
            this.m_errorCode = errorCode;
            this.m_fieldError = fieldError;
            this.m_parameterError = paramError;
            this.ParameterLength = (0x20 + Util.GetBitLengthOfParam(this.FieldError)) + Util.GetBitLengthOfParam(this.InnerParameterError);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(LlrpResources.ParameterErrorToStringHeader);
            Util.ToString(this.ErroneousParameterType, strBuilder);
            Util.ToString(this.ErrorCode, strBuilder);
            Util.ToString(this.FieldError, strBuilder);
            Util.ToString(this.InnerParameterError, strBuilder);
            strBuilder.Append(LlrpResources.ParameterErrorToStringFooter);
            return strBuilder.ToString();
        }

        public LlrpParameterType ErroneousParameterType
        {
            get
            {
                return this.m_erroneousParameter;
            }
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

        public ParameterError InnerParameterError
        {
            get
            {
                return this.m_parameterError;
            }
        }
    }
}
