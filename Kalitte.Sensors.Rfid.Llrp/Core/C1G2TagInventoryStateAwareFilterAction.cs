namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class C1G2TagInventoryStateAwareFilterAction : LlrpTlvParameterBase
    {
        private TagInventoryAwareAction m_action;
        private TagInventoryAwareTarget m_target;

        public C1G2TagInventoryStateAwareFilterAction(TagInventoryAwareTarget target, TagInventoryAwareAction action) : base(LlrpParameterType.C1G2TagInventoryStateAwareFilterAction)
        {
            this.Init(target, action);
        }

        internal C1G2TagInventoryStateAwareFilterAction(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2TagInventoryStateAwareFilterAction, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            TagInventoryAwareTarget enumInstance = BitHelper.GetEnumInstance<TagInventoryAwareTarget>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            TagInventoryAwareAction action = BitHelper.GetEnumInstance<TagInventoryAwareAction>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, action);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.Target), 8, true);
            stream.Append((long) ((byte) this.Action), 8, true);
        }

        private void Init(TagInventoryAwareTarget target, TagInventoryAwareAction action)
        {
            this.m_target = target;
            this.m_action = action;
            this.ParameterLength = 0x10;
        }

        public TagInventoryAwareAction Action
        {
            get
            {
                return this.m_action;
            }
        }

        public TagInventoryAwareTarget Target
        {
            get
            {
                return this.m_target;
            }
        }
    }
}
