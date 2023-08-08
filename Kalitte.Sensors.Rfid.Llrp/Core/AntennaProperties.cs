namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class AntennaProperties : LlrpTlvParameterBase
    {
        private short m_antennaGain;
        private ushort m_id;
        private bool m_isConnected;

        internal AntennaProperties(BitArray bitArray, ref int index) : base(LlrpParameterType.AntennaProperties, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            bool isConnected = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 7;
            ushort antennaId = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            short antennaGain = (short) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(antennaId, isConnected, antennaGain);
        }

        public AntennaProperties(ushort antennaId, bool isConnected, short antennaGain) : base(LlrpParameterType.AntennaProperties)
        {
            this.Init(antennaId, isConnected, antennaGain);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append(this.IsConnected, 1, true);
            stream.Append((ulong) 0L, 7, true);
            stream.Append((long) this.AntennaId, 0x10, true);
            stream.Append((long) this.AntennaGain, 0x10, true);
        }

        private void Init(ushort antennaId, bool isConnected, short antennaGain)
        {
            this.m_id = antennaId;
            this.m_isConnected = isConnected;
            this.m_antennaGain = antennaGain;
            this.ParameterLength = 40;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Antenna Properties>");
            builder.Append(base.ToString());
            builder.Append("<Antenna Id>");
            builder.Append(this.AntennaId);
            builder.Append("</Antenna Id>");
            builder.Append("<Connected>");
            builder.Append(this.IsConnected);
            builder.Append("</Connected>");
            builder.Append("<Gain>");
            builder.Append(this.AntennaGain);
            builder.Append("</Gain>");
            builder.Append("</Antenna Properties>");
            return builder.ToString();
        }

        public short AntennaGain
        {
            get
            {
                return this.m_antennaGain;
            }
        }

        public ushort AntennaId
        {
            get
            {
                return this.m_id;
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.m_isConnected;
            }
        }
    }
}
