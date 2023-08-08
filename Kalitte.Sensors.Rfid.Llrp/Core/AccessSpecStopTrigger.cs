namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class AccessSpecStopTrigger : LlrpTlvParameterBase
    {
        private ushort m_operationCountValue;
        private AccessSpecStopTriggerType m_triggerType;

        public AccessSpecStopTrigger(AccessSpecStopTriggerType type, ushort operationCount) : base(LlrpParameterType.AccessSpecStopTrigger)
        {
            this.Init(type, operationCount);
        }

        internal AccessSpecStopTrigger(BitArray bitArray, ref int index) : base(LlrpParameterType.AccessSpecStopTrigger, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            AccessSpecStopTriggerType enumInstance = BitHelper.GetEnumInstance<AccessSpecStopTriggerType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            ushort operationCount = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, operationCount);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.TriggerType, 8, true);
            stream.Append((long) this.OperationCountValue, 0x10, true);
        }

        private void Init(AccessSpecStopTriggerType type, ushort operationCount)
        {
            this.m_triggerType = type;
            this.m_operationCountValue = operationCount;
            this.ParameterLength = 0x18;
        }

        public ushort OperationCountValue
        {
            get
            {
                return this.m_operationCountValue;
            }
        }

        public AccessSpecStopTriggerType TriggerType
        {
            get
            {
                return this.m_triggerType;
            }
        }
    }
}
