namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    [Serializable]
    public sealed class AISpecStopTrigger : LlrpTlvParameterBase
    {
        private uint m_duration;
        private Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger m_gpiTrigger;
        private Kalitte.Sensors.Rfid.Llrp.Core.TagObservationTrigger m_tagObservationTrigger;
        private AISpecStopTriggerType m_triggerType;

        internal AISpecStopTrigger(BitArray bitArray, ref int index) : base(LlrpParameterType.AISpecStopTrigger, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            AISpecStopTriggerType enumInstance = BitHelper.GetEnumInstance<AISpecStopTriggerType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            uint duration = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger gpiTrigger = null;
            Kalitte.Sensors.Rfid.Llrp.Core.TagObservationTrigger tagObservationTrigger = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.GpiTriggerValue, bitArray, index, parameterEndLimit))
            {
                gpiTrigger = new Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger(bitArray, ref index);
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.TagObservationTrigger, bitArray, index, parameterEndLimit))
            {
                tagObservationTrigger = new Kalitte.Sensors.Rfid.Llrp.Core.TagObservationTrigger(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, duration, gpiTrigger, tagObservationTrigger);
        }

        public AISpecStopTrigger(AISpecStopTriggerType triggerType, uint duration, Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger gpiTrigger, Kalitte.Sensors.Rfid.Llrp.Core.TagObservationTrigger tagObservationTrigger) : base(LlrpParameterType.AISpecStopTrigger)
        {
            this.Init(triggerType, duration, gpiTrigger, tagObservationTrigger);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.TriggerType), 8, true);
            stream.Append((long) this.Duration, 0x20, true);
            Util.Encode(this.GpiTrigger, stream);
            Util.Encode(this.TagObservationTrigger, stream);
        }

        private void Init(AISpecStopTriggerType triggerType, uint duration, Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger gpiTrigger, Kalitte.Sensors.Rfid.Llrp.Core.TagObservationTrigger tagObservationTrigger)
        {
            if ((triggerType == AISpecStopTriggerType.Gpi) && (gpiTrigger == null))
            {
                throw new ArgumentNullException("gpiTrigger");
            }
            if ((triggerType == AISpecStopTriggerType.TagObservation) && (tagObservationTrigger == null))
            {
                throw new ArgumentNullException("tagObservationTrigger");
            }
            if ((gpiTrigger != null) && (tagObservationTrigger != null))
            {
                throw new ArgumentException(LlrpResources.InvalidAISpecStopTriggerNonNullGpiAndTag);
            }
            this.m_triggerType = triggerType;
            this.m_duration = duration;
            this.m_gpiTrigger = gpiTrigger;
            this.m_tagObservationTrigger = tagObservationTrigger;
            this.ParameterLength = (40 + Util.GetBitLengthOfParam(this.GpiTrigger)) + Util.GetBitLengthOfParam(this.TagObservationTrigger);
        }

        public uint Duration
        {
            get
            {
                return this.m_duration;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger GpiTrigger
        {
            get
            {
                return this.m_gpiTrigger;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.TagObservationTrigger TagObservationTrigger
        {
            get
            {
                return this.m_tagObservationTrigger;
            }
        }

        public AISpecStopTriggerType TriggerType
        {
            get
            {
                return this.m_triggerType;
            }
        }
    }
}
