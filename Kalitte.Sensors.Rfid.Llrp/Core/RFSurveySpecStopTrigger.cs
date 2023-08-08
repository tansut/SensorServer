namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class RFSurveySpecStopTrigger : LlrpTlvParameterBase
    {
        private uint m_duration;
        private uint m_iteration;
        private RFSurveySpecStopTriggerType m_stopTrigger;

        internal RFSurveySpecStopTrigger(BitArray bitArray, ref int index) : base(LlrpParameterType.RFSurveySpecStopTrigger, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            RFSurveySpecStopTriggerType enumInstance = BitHelper.GetEnumInstance<RFSurveySpecStopTriggerType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            uint duration = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint iteration = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, duration, iteration);
        }

        public RFSurveySpecStopTrigger(RFSurveySpecStopTriggerType stopTrigger, uint duration, uint iteration) : base(LlrpParameterType.RFSurveySpecStopTrigger)
        {
            this.Init(stopTrigger, duration, iteration);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.TriggerType), 8, true);
            stream.Append((long) this.Duration, 0x20, true);
            stream.Append((long) this.Iteration, 0x20, true);
        }

        private void Init(RFSurveySpecStopTriggerType stopTrigger, uint duration, uint iteration)
        {
            this.m_stopTrigger = stopTrigger;
            this.m_duration = duration;
            this.m_iteration = iteration;
            this.ParameterLength = 0x48;
        }

        public uint Duration
        {
            get
            {
                return this.m_duration;
            }
        }

        public uint Iteration
        {
            get
            {
                return this.m_iteration;
            }
        }

        public RFSurveySpecStopTriggerType TriggerType
        {
            get
            {
                return this.m_stopTrigger;
            }
        }
    }
}
