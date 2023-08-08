namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;

    public sealed class EventsAndReport : LlrpTlvParameterBase
    {
        private bool m_holdEventsAndReportsUponReconnect;

        public EventsAndReport(bool holdOnReconnect) : base(LlrpParameterType.EventsAndReports)
        {
            this.Init(holdOnReconnect);
        }

        internal EventsAndReport(BitArray bitArray, ref int index) : base(LlrpParameterType.EventsAndReports, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            bool holdOnReconnect = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 7;
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(holdOnReconnect);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append(this.HoldEventAndReportUponReconnect, 1, true);
            stream.Append((ulong) 0L, 7, true);
        }

        private void Init(bool holdOnReconnect)
        {
            this.m_holdEventsAndReportsUponReconnect = holdOnReconnect;
            this.ParameterLength = 8;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Events and Report>");
            builder.Append(base.ToString());
            builder.Append(this.HoldEventAndReportUponReconnect);
            builder.Append("</Events and Report>");
            return builder.ToString();
        }

        public bool HoldEventAndReportUponReconnect
        {
            get
            {
                return this.m_holdEventsAndReportsUponReconnect;
            }
        }
    }
}
