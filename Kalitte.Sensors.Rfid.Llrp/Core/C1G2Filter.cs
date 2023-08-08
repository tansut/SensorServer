namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    [Serializable]
    public sealed class C1G2Filter : LlrpTlvParameterBase
    {
        private C1G2TagInventoryStateAwareFilterAction m_awareAction;
        private C1G2TagInventoryMask m_mask;
        private C1G2TruncateAction m_truncateAction;
        private C1G2TagInventoryStateUnawareFilterAction m_unawareAction;

        internal C1G2Filter(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2Filter, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            C1G2TruncateAction enumInstance = BitHelper.GetEnumInstance<C1G2TruncateAction>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 2));
            index += 6;
            C1G2TagInventoryMask mask = null;
            C1G2TagInventoryStateAwareFilterAction awareAction = null;
            C1G2TagInventoryStateUnawareFilterAction unawareAction = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.C1G2TagInventoryMask, bitArray, index, parameterEndLimit))
            {
                mask = new C1G2TagInventoryMask(bitArray, ref index);
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.C1G2TagInventoryStateAwareFilterAction, bitArray, index, parameterEndLimit))
            {
                awareAction = new C1G2TagInventoryStateAwareFilterAction(bitArray, ref index);
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.C1G2TagInventoryStateUnawareFilterAction, bitArray, index, parameterEndLimit))
            {
                unawareAction = new C1G2TagInventoryStateUnawareFilterAction(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(mask, enumInstance, awareAction, unawareAction);
        }

        public C1G2Filter(C1G2TagInventoryMask mask, C1G2TruncateAction truncate, C1G2TagInventoryStateAwareFilterAction awareAction, C1G2TagInventoryStateUnawareFilterAction unawareAction) : base(LlrpParameterType.C1G2Filter)
        {
            this.Init(mask, truncate, awareAction, unawareAction);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.TruncateAction), 2, true);
            stream.Append((ulong) 0L, 6, true);
            Util.Encode(this.Mask, stream);
            Util.Encode(this.StateAwareAction, stream);
            Util.Encode(this.StateUnawareAction, stream);
        }

        private void Init(C1G2TagInventoryMask mask, C1G2TruncateAction truncate, C1G2TagInventoryStateAwareFilterAction awareAction, C1G2TagInventoryStateUnawareFilterAction unawareAction)
        {
            if (mask == null)
            {
                throw new ArgumentNullException("mask");
            }
            if ((awareAction == null) && (unawareAction == null))
            {
                throw new ArgumentException(LlrpResources.C1G2FilterNoAction);
            }
            this.m_mask = mask;
            this.m_truncateAction = truncate;
            this.m_awareAction = awareAction;
            this.m_unawareAction = unawareAction;
            this.ParameterLength = ((8 + Util.GetBitLengthOfParam(this.Mask)) + Util.GetBitLengthOfParam(this.StateAwareAction)) + Util.GetBitLengthOfParam(this.StateUnawareAction);
        }

        public C1G2TagInventoryMask Mask
        {
            get
            {
                return this.m_mask;
            }
        }

        public C1G2TagInventoryStateAwareFilterAction StateAwareAction
        {
            get
            {
                return this.m_awareAction;
            }
        }

        public C1G2TagInventoryStateUnawareFilterAction StateUnawareAction
        {
            get
            {
                return this.m_unawareAction;
            }
        }

        public C1G2TruncateAction TruncateAction
        {
            get
            {
                return this.m_truncateAction;
            }
        }
    }
}
