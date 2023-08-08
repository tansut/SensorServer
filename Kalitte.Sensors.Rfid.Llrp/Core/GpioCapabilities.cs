namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class GpioCapabilities : LlrpTlvParameterBase
    {
        private ushort m_numberOfGPI;
        private ushort m_numberOfGPO;

        internal GpioCapabilities(BitArray bitArray, ref int index) : base(LlrpParameterType.GpioCapabilities, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort numberOfGPI = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort numberOfGPO = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(numberOfGPI, numberOfGPO);
        }

        public GpioCapabilities(ushort numberOfGPI, ushort numberOfGPO) : base(LlrpParameterType.GpioCapabilities)
        {
            this.Init(numberOfGPI, numberOfGPO);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.NumberOfGpi, 0x10, true);
            stream.Append((long) this.NumberOfGpo, 0x10, true);
        }

        private void Init(ushort numberOfGPI, ushort numberOfGPO)
        {
            this.m_numberOfGPI = numberOfGPI;
            this.m_numberOfGPO = numberOfGPO;
            this.ParameterLength = 0x20;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<GPIO Capabilities>");
            builder.Append(base.ToString());
            builder.Append("<Number of GPI>");
            builder.Append(this.NumberOfGpi);
            builder.Append("</Number of GPI>");
            builder.Append("<Number of GPO>");
            builder.Append(this.NumberOfGpo);
            builder.Append("</Number of GPO>");
            builder.Append("</GPIO Capabilities>");
            return builder.ToString();
        }

        public ushort NumberOfGpi
        {
            get
            {
                return this.m_numberOfGPI;
            }
        }

        public ushort NumberOfGpo
        {
            get
            {
                return this.m_numberOfGPO;
            }
        }
    }
}
