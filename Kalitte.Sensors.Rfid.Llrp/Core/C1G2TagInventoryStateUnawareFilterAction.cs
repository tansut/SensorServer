namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class C1G2TagInventoryStateUnawareFilterAction : LlrpTlvParameterBase
    {
        private TagInventoryUnawareAction m_action;

        public C1G2TagInventoryStateUnawareFilterAction(TagInventoryUnawareAction action) : base(LlrpParameterType.C1G2TagInventoryStateUnawareFilterAction)
        {
            this.Init(action);
        }

        internal C1G2TagInventoryStateUnawareFilterAction(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2TagInventoryStateUnawareFilterAction, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            TagInventoryUnawareAction enumInstance = BitHelper.GetEnumInstance<TagInventoryUnawareAction>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.Action), 8, true);
        }

        private void Init(TagInventoryUnawareAction action)
        {
            this.m_action = action;
            this.ParameterLength = 8;
        }

        public TagInventoryUnawareAction Action
        {
            get
            {
                return this.m_action;
            }
        }
    }
}
