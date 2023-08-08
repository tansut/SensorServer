namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class GpoWriteData : LlrpTlvParameterBase
    {
        private bool m_data;
        private ushort m_portNum;

        internal GpoWriteData(BitArray bitArray, ref int index) : base(LlrpParameterType.GpoWriteData, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort port = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            bool data = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 7;
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(port, data);
        }

        public GpoWriteData(ushort port, bool data) : base(LlrpParameterType.GpoWriteData)
        {
            this.Init(port, data);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.PortNumber, 0x10, true);
            stream.Append(this.Data, 1, true);
            stream.Append((ulong) 0L, 7, true);
        }

        private void Init(ushort port, bool data)
        {
            if (port == 0)
            {
                throw new ArgumentOutOfRangeException("port");
            }
            this.m_portNum = port;
            this.m_data = data;
            this.ParameterLength = 0x18;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<GPO Write Data>");
            builder.Append(base.ToString());
            builder.Append("<Port number>");
            builder.Append(this.PortNumber);
            builder.Append("</Port number>");
            builder.Append("<Data>");
            builder.Append(this.Data);
            builder.Append("</Data>");
            builder.Append("</GPO Write Data>");
            return builder.ToString();
        }

        public bool Data
        {
            get
            {
                return this.m_data;
            }
        }

        public ushort PortNumber
        {
            get
            {
                return this.m_portNum;
            }
        }
    }
}
