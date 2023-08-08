namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class PeakRssi : LlrpTVParameterBase, ICloneable
    {
        private sbyte m_peakRSSI;

        public PeakRssi(sbyte peakRSSI) : base(LlrpParameterType.PeakRssi)
        {
            this.m_peakRSSI = peakRSSI;
        }

        internal PeakRssi(BitArray bitArray, ref int index) : base(LlrpParameterType.PeakRssi, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            this.m_peakRSSI = (sbyte) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
        }

        public object Clone()
        {
            return new PeakRssi(this.m_peakRSSI);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.m_peakRSSI, 8, true);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Peak RSSI>");
            builder.Append(base.ToString());
            builder.Append(this.Rssi);
            builder.Append("</Peak RSSI>");
            return builder.ToString();
        }

        public sbyte Rssi
        {
            get
            {
                return this.m_peakRSSI;
            }
        }
    }
}
