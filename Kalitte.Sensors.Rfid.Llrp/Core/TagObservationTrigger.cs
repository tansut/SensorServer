namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class TagObservationTrigger : LlrpTlvParameterBase
    {
        private ushort m_idleTimeBetweenTagResponse;
        private ushort m_numberOfAttempts;
        private ushort m_numberOftags;
        private uint m_timeout;
        private TagObservationTriggerType m_triggerType;

        internal TagObservationTrigger(BitArray bitArray, ref int index) : base(LlrpParameterType.TagObservationTrigger, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            TagObservationTriggerType enumInstance = BitHelper.GetEnumInstance<TagObservationTriggerType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            index += 8;
            ushort numberOfTags = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort numberOfAttempts = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort idleTimeBetweenTagResponse = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            uint timeout = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, numberOfTags, numberOfAttempts, idleTimeBetweenTagResponse, timeout);
        }

        public TagObservationTrigger(TagObservationTriggerType triggerType, ushort numberOfTags, ushort numberOfAttempts, ushort idleTimeBetweenTagResponse, uint timeout) : base(LlrpParameterType.TagObservationTrigger)
        {
            this.Init(triggerType, numberOfTags, numberOfAttempts, idleTimeBetweenTagResponse, timeout);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.TriggerType), 8, true);
            stream.Append((ulong) 0L, 8, true);
            stream.Append((long) this.NumberOfTags, 0x10, true);
            stream.Append((long) this.NumberOfAttempts, 0x10, true);
            stream.Append((long) this.IdleTimeBetweenTagResponse, 0x10, true);
            stream.Append((long) this.Timeout, 0x20, true);
        }

        private void Init(TagObservationTriggerType triggerType, ushort numberOfTags, ushort numberOfAttempts, ushort idleTimeBetweenTagResponse, uint timeout)
        {
            this.m_triggerType = triggerType;
            this.m_numberOftags = numberOfTags;
            this.m_numberOfAttempts = numberOfAttempts;
            this.m_idleTimeBetweenTagResponse = idleTimeBetweenTagResponse;
            this.m_timeout = timeout;
            this.ParameterLength = 0x60;
        }

        public ushort IdleTimeBetweenTagResponse
        {
            get
            {
                return this.m_idleTimeBetweenTagResponse;
            }
        }

        public ushort NumberOfAttempts
        {
            get
            {
                return this.m_numberOfAttempts;
            }
        }

        public ushort NumberOfTags
        {
            get
            {
                return this.m_numberOftags;
            }
        }

        public uint Timeout
        {
            get
            {
                return this.m_timeout;
            }
        }

        public TagObservationTriggerType TriggerType
        {
            get
            {
                return this.m_triggerType;
            }
        }
    }
}
