namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    [Serializable]
    public sealed class ROSpecStartTrigger : LlrpTlvParameterBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger m_gpiTrigger;
        private Kalitte.Sensors.Rfid.Llrp.Core.PeriodicTrigger m_periodicTrigger;
        private ROSpecStartTriggerType m_triggerType;

        internal ROSpecStartTrigger(BitArray bitArray, ref int index) : base(LlrpParameterType.ROSpecStartTrigger, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ROSpecStartTriggerType enumInstance = BitHelper.GetEnumInstance<ROSpecStartTriggerType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            Kalitte.Sensors.Rfid.Llrp.Core.PeriodicTrigger periodicTrigger = null;
            Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger gpiTrigger = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.PeriodicTriggerValue, bitArray, index, parameterEndLimit))
            {
                periodicTrigger = new Kalitte.Sensors.Rfid.Llrp.Core.PeriodicTrigger(bitArray, ref index);
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.GpiTriggerValue, bitArray, index, parameterEndLimit))
            {
                gpiTrigger = new Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, periodicTrigger, gpiTrigger);
        }

        public ROSpecStartTrigger(ROSpecStartTriggerType triggerType, Kalitte.Sensors.Rfid.Llrp.Core.PeriodicTrigger periodicTrigger, Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger gpiTrigger) : base(LlrpParameterType.ROSpecStartTrigger)
        {
            this.Init(triggerType, periodicTrigger, gpiTrigger);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.TriggerType), 8, true);
            Util.Encode(this.PeriodicTrigger, stream);
            Util.Encode(this.GpiTrigger, stream);
        }

        private void Init(ROSpecStartTriggerType triggerType, Kalitte.Sensors.Rfid.Llrp.Core.PeriodicTrigger periodicTrigger, Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger gpiTrigger)
        {
            if ((triggerType == ROSpecStartTriggerType.Gpi) && (gpiTrigger == null))
            {
                throw new ArgumentNullException("gpiTrigger");
            }
            if ((triggerType == ROSpecStartTriggerType.Periodic) && (periodicTrigger == null))
            {
                throw new ArgumentNullException("periodicTrigger");
            }
            if ((periodicTrigger != null) && (gpiTrigger != null))
            {
                throw new ArgumentException(LlrpResources.InvalidROSpecStartTriggerNonNullGpiAndPeriodic);
            }
            this.m_triggerType = triggerType;
            this.m_periodicTrigger = periodicTrigger;
            this.m_gpiTrigger = gpiTrigger;
            this.ParameterLength = (8 + Util.GetBitLengthOfParam(this.PeriodicTrigger)) + Util.GetBitLengthOfParam(this.GpiTrigger);
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.GpiTrigger GpiTrigger
        {
            get
            {
                return this.m_gpiTrigger;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.PeriodicTrigger PeriodicTrigger
        {
            get
            {
                return this.m_periodicTrigger;
            }
        }

        public ROSpecStartTriggerType TriggerType
        {
            get
            {
                return this.m_triggerType;
            }
        }
    }
}
