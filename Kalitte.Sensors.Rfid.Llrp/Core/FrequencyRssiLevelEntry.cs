namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    [Serializable]
    public sealed class FrequencyRssiLevelEntry : LlrpTlvParameterBase
    {
        private sbyte m_averageRSSI;
        private uint m_bandwidth;
        private uint m_frequency;
        private sbyte m_peakRSSI;
        private Kalitte.Sensors.Rfid.Llrp.Core.Uptime m_upTIme;
        private Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp m_utcTimestamp;

        internal FrequencyRssiLevelEntry(BitArray bitArray, ref int index) : base(LlrpParameterType.FrequencyRssiLevelEntry, bitArray, index)
        {
            LlrpParameterType type;
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            uint frequency = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint bandwidth = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            sbyte averageRSSI = (sbyte) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8);
            sbyte peakRSSI = (sbyte) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8);
            Collection<LlrpParameterType> expectedTypes = new Collection<LlrpParameterType>();
            expectedTypes.Add(LlrpParameterType.UtcTimestamp);
            expectedTypes.Add(LlrpParameterType.Uptime);
            Kalitte.Sensors.Rfid.Llrp.Core.Uptime uptime = null;
            Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp utcTimestamp = null;
            if (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, parameterEndLimit, out type))
            {
                switch (type)
                {
                    case LlrpParameterType.UtcTimestamp:
                        utcTimestamp = new Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp(bitArray, ref index);
                        break;

                    case LlrpParameterType.Uptime:
                        uptime = new Kalitte.Sensors.Rfid.Llrp.Core.Uptime(bitArray, ref index);
                        break;
                }
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(utcTimestamp, uptime, frequency, bandwidth, averageRSSI, peakRSSI);
        }

        public FrequencyRssiLevelEntry(Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp utcTimestamp, Kalitte.Sensors.Rfid.Llrp.Core.Uptime uptime, uint frequency, uint bandwidth, sbyte averageRSSI, sbyte peakRSSI) : base(LlrpParameterType.FrequencyRssiLevelEntry)
        {
            this.Init(utcTimestamp, uptime, frequency, bandwidth, averageRSSI, peakRSSI);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.Frequency, 0x20, true);
            stream.Append((long) this.Bandwidth, 0x20, true);
            stream.Append((long) this.AverageRssi, 8, true);
            stream.Append((long) this.PeakRssi, 8, true);
            Util.Encode(this.UtcTimestamp, stream);
            Util.Encode(this.Uptime, stream);
        }

        private void Init(Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp utcTimestamp, Kalitte.Sensors.Rfid.Llrp.Core.Uptime uptime, uint frequency, uint bandwidth, sbyte averageRSSI, sbyte peakRSSI)
        {
            if ((utcTimestamp == null) && (uptime == null))
            {
                throw new ArgumentException(LlrpResources.NoneTimestampPresent);
            }
            if ((uptime != null) && (utcTimestamp != null))
            {
                throw new ArgumentException(LlrpResources.BothTimestampPresent);
            }
            if ((averageRSSI < -128) || (averageRSSI > 0x7f))
            {
                throw new ArgumentOutOfRangeException("averageRSSI", LlrpResources.RssiValidValue);
            }
            if ((peakRSSI < -128) || (peakRSSI > 0x7f))
            {
                throw new ArgumentOutOfRangeException("peakRSSI", LlrpResources.RssiValidValue);
            }
            this.m_utcTimestamp = utcTimestamp;
            this.m_upTIme = uptime;
            this.m_frequency = frequency;
            this.m_bandwidth = bandwidth;
            this.m_averageRSSI = averageRSSI;
            this.m_peakRSSI = peakRSSI;
            this.ParameterLength = (80 + Util.GetBitLengthOfParam(this.m_utcTimestamp)) + Util.GetBitLengthOfParam(this.m_upTIme);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Frequency RSSI Level Entry>");
            Util.ToString(this.UtcTimestamp, strBuilder);
            Util.ToString(this.Uptime, strBuilder);
            strBuilder.Append("<Frequency>");
            strBuilder.Append(this.Frequency);
            strBuilder.Append("</Frequency>");
            strBuilder.Append("<Bandwidth>");
            strBuilder.Append(this.Bandwidth);
            strBuilder.Append("</Bandwidth>");
            strBuilder.Append("<Average RSSI>");
            strBuilder.Append(this.AverageRssi);
            strBuilder.Append("</Average RSSi>");
            strBuilder.Append("<Peak RSSI>");
            strBuilder.Append(this.PeakRssi);
            strBuilder.Append("</Peak RSSI>");
            strBuilder.Append("</Frequency RSSI Level Entry>");
            return strBuilder.ToString();
        }

        public sbyte AverageRssi
        {
            get
            {
                return this.m_averageRSSI;
            }
        }

        public uint Bandwidth
        {
            get
            {
                return this.m_bandwidth;
            }
        }

        public uint Frequency
        {
            get
            {
                return this.m_frequency;
            }
        }

        public sbyte PeakRssi
        {
            get
            {
                return this.m_peakRSSI;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.Uptime Uptime
        {
            get
            {
                return this.m_upTIme;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp UtcTimestamp
        {
            get
            {
                return this.m_utcTimestamp;
            }
        }
    }
}
