namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class AccessReportSpec : LlrpTlvParameterBase
    {
        private AccessReportTrigger m_trigger;

        public AccessReportSpec(AccessReportTrigger trigger) : base(LlrpParameterType.AccessReportSpec)
        {
            this.Init(trigger);
        }

        internal AccessReportSpec(BitArray bitArray, ref int index) : base(LlrpParameterType.AccessReportSpec, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            AccessReportTrigger enumInstance = BitHelper.GetEnumInstance<AccessReportTrigger>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.Trigger), 8, true);
        }

        private void Init(AccessReportTrigger trigger)
        {
            this.m_trigger = trigger;
            this.ParameterLength = 8;
        }

        public AccessReportTrigger Trigger
        {
            get
            {
                return this.m_trigger;
            }
        }
    }
}
