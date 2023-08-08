namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class C1G2TagInventoryStateAwareSingulationAction : LlrpTlvParameterBase
    {
        private TagInventoryState m_inventory;
        private TagSLState m_state;

        public C1G2TagInventoryStateAwareSingulationAction(TagInventoryState inventory, TagSLState state) : base(LlrpParameterType.C1G2TagInventoryStateAwareSingulationAction)
        {
            this.Init(inventory, state);
        }

        internal C1G2TagInventoryStateAwareSingulationAction(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2TagInventoryStateAwareSingulationAction, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            TagInventoryState enumInstance = BitHelper.GetEnumInstance<TagInventoryState>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1));
            TagSLState state = BitHelper.GetEnumInstance<TagSLState>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1));
            index += 6;
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, state);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.Inventory), 1, true);
            stream.Append((long) ((byte) this.State), 1, true);
            stream.Append((ulong) 0L, 6, true);
        }

        private void Init(TagInventoryState inventory, TagSLState state)
        {
            this.m_inventory = inventory;
            this.m_state = state;
            this.ParameterLength = 8;
        }

        public TagInventoryState Inventory
        {
            get
            {
                return this.m_inventory;
            }
        }

        public TagSLState State
        {
            get
            {
                return this.m_state;
            }
        }
    }
}
