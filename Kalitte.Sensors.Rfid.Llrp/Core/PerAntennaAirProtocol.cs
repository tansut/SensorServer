namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Rfid.Llrp.Exceptions;

    public sealed class PerAntennaAirProtocol : LlrpTlvParameterBase
    {
        private Collection<AirProtocolId> m_airProtocolSupported;
        private ushort m_antennaId;

        internal PerAntennaAirProtocol(BitArray bitArray, ref int index) : base(LlrpParameterType.PerAntennaAirProtocol, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Collection<AirProtocolId> airProtocolSupported = new Collection<AirProtocolId>();
            ushort antennaId = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort num3 = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            if (((long) (index + (num3 * 8))) > parameterEndLimit)
            {
                throw new DecodingException("Invalid Message", string.Format(CultureInfo.CurrentCulture, LlrpResources.InvalidMessageOrParameter, new object[] { base.GetType().FullName }));
            }
            while (num3 > 0)
            {
                byte num4 = (byte) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8);
                airProtocolSupported.Add(BitHelper.GetEnumInstance<AirProtocolId>(num4));
                num3 = (ushort) (num3 - 1);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(antennaId, airProtocolSupported);
        }

        public PerAntennaAirProtocol(ushort antennaId, Collection<AirProtocolId> airProtocolSupported) : base(LlrpParameterType.PerAntennaAirProtocol)
        {
            this.Init(antennaId, airProtocolSupported);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            ushort count = 0;
            if (this.AirProtocolSupported != null)
            {
                count = (ushort) this.AirProtocolSupported.Count;
            }
            stream.Append((long) this.AntennaId, 0x10, true);
            stream.Append((long) count, 0x10, true);
            if (count > 0)
            {
                foreach (AirProtocolId id in this.AirProtocolSupported)
                {
                    stream.Append((long) ((byte) id), 8, true);
                }
            }
        }

        private void Init(ushort antennaId, Collection<AirProtocolId> airProtocolSupported)
        {
            this.m_antennaId = antennaId;
            this.m_airProtocolSupported = airProtocolSupported;
            ushort num = 0;
            if (this.AirProtocolSupported != null)
            {
                num = (ushort) (this.AirProtocolSupported.Count * 8);
            }
            this.ParameterLength = (uint) (0x20 + num);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Per Antenna Air protocol>");
            builder.Append(base.ToString());
            builder.Append("<Antenna Id>");
            builder.Append(this.AntennaId);
            builder.Append("</Antenna Id>");
            if (this.AirProtocolSupported != null)
            {
                foreach (AirProtocolId id in this.AirProtocolSupported)
                {
                    builder.Append("<Air Protocol>");
                    builder.Append(id.ToString());
                    builder.Append("</Air Protocol>");
                }
            }
            builder.Append("</Per Antenna Air protocol>");
            return builder.ToString();
        }

        public Collection<AirProtocolId> AirProtocolSupported
        {
            get
            {
                return this.m_airProtocolSupported;
            }
        }

        public ushort AntennaId
        {
            get
            {
                return this.m_antennaId;
            }
        }
    }
}
