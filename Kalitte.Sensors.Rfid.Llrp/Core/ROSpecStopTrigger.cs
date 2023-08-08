namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    [Serializable]
    public sealed class ROSpecStopTrigger : LlrpTlvParameterBase
    {
        private uint m_duration;
        private Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger m_gpiTTrigger;
        private ROSpecStopTriggerType m_triggerType;

        public ROSpecStopTrigger() : base(LlrpParameterType.ROSpecStopTrigger)
        {
            this.Init(ROSpecStopTriggerType.Null, 0, null);
        }

        public ROSpecStopTrigger(Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger gpiTrigger) : base(LlrpParameterType.ROSpecStopTrigger)
        {
            this.Init(ROSpecStopTriggerType.Gpi, 0, gpiTrigger);
        }

        public ROSpecStopTrigger(uint duration) : base(LlrpParameterType.ROSpecStopTrigger)
        {
            this.Init(ROSpecStopTriggerType.Duration, duration, null);
        }

        internal ROSpecStopTrigger(BitArray bitArray, ref int index) : base(LlrpParameterType.ROSpecStopTrigger, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ROSpecStopTriggerType enumInstance = BitHelper.GetEnumInstance<ROSpecStopTriggerType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            uint duration = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger gpiTrigger = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.GpiTriggerValue, bitArray, index, parameterEndLimit))
            {
                gpiTrigger = new Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, duration, gpiTrigger);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.TriggerType), 8, true);
            stream.Append((long) this.Duration, 0x20, true);
            Util.Encode(this.GpiTrigger, stream);
        }

        private void Init(ROSpecStopTriggerType triggerType, uint duration, Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger gpiTrigger)
        {
            if ((triggerType == ROSpecStopTriggerType.Gpi) && (gpiTrigger == null))
            {
                throw new ArgumentNullException("gpiTrigger");
            }
            if ((triggerType != ROSpecStopTriggerType.Gpi) && (gpiTrigger != null))
            {
                throw new ArgumentException(LlrpResources.InvalidROSpecStopTriggerNonNullGpi);
            }
            this.m_triggerType = triggerType;
            this.m_duration = duration;
            this.m_gpiTTrigger = gpiTrigger;
            this.ParameterLength = 40 + Util.GetBitLengthOfParam(this.GpiTrigger);
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
                return this.m_gpiTTrigger;
            }
        }

        public ROSpecStopTriggerType TriggerType
        {
            get
            {
                return this.m_triggerType;
            }
        }
    }
}
