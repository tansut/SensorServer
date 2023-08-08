namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class ROBoundarySpec : LlrpTlvParameterBase
    {
        private ROSpecStartTrigger m_startTrigger;
        private ROSpecStopTrigger m_stopTrigger;

        public ROBoundarySpec(ROSpecStartTrigger startTrigger, ROSpecStopTrigger stopTrigger) : base(LlrpParameterType.ROBoundarySpec)
        {
            this.Init(startTrigger, stopTrigger);
        }

        internal ROBoundarySpec(BitArray bitArray, ref int index) : base(LlrpParameterType.ROBoundarySpec, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ROSpecStartTrigger startTrigger = null;
            ROSpecStopTrigger stopTrigger = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ROSpecStartTrigger, bitArray, index, parameterEndLimit))
            {
                startTrigger = new ROSpecStartTrigger(bitArray, ref index);
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ROSpecStopTrigger, bitArray, index, parameterEndLimit))
            {
                stopTrigger = new ROSpecStopTrigger(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(startTrigger, stopTrigger);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            Util.Encode(this.StartTrigger, stream);
            Util.Encode(this.StopTrigger, stream);
        }

        private void Init(ROSpecStartTrigger startTrigger, ROSpecStopTrigger stopTrigger)
        {
            if (startTrigger == null)
            {
                throw new ArgumentNullException("startTrigger");
            }
            if (stopTrigger == null)
            {
                throw new ArgumentNullException("stopTrigger");
            }
            this.m_startTrigger = startTrigger;
            this.m_stopTrigger = stopTrigger;
            this.ParameterLength = this.m_startTrigger.ParameterLength + this.m_stopTrigger.ParameterLength;
        }

        public ROSpecStartTrigger StartTrigger
        {
            get
            {
                return this.m_startTrigger;
            }
        }

        public ROSpecStopTrigger StopTrigger
        {
            get
            {
                return this.m_stopTrigger;
            }
        }
    }
}
