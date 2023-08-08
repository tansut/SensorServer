namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class C1G2TagSpec : TagSpec
    {
        private C1G2TargetTag m_spec1;
        private C1G2TargetTag m_spec2;

        public C1G2TagSpec(C1G2TargetTag spec1, C1G2TargetTag spec2) : base(LlrpParameterType.C1G2TagSpec)
        {
            this.Init(spec1, spec2);
        }

        internal C1G2TagSpec(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2TagSpec, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            C1G2TargetTag tag = null;
            C1G2TargetTag tag2 = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.C1G2TargetTag, bitArray, index, parameterEndLimit))
            {
                tag = new C1G2TargetTag(bitArray, ref index);
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.C1G2TargetTag, bitArray, index, parameterEndLimit))
            {
                tag2 = new C1G2TargetTag(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(tag, tag2);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            Util.Encode(this.Spec1, stream);
            Util.Encode(this.Spec2, stream);
        }

        private void Init(C1G2TargetTag spec1, C1G2TargetTag spec2)
        {
            if (spec1 == null)
            {
                throw new ArgumentNullException("spec1");
            }
            this.m_spec1 = spec1;
            this.m_spec2 = spec2;
            this.ParameterLength = Util.GetBitLengthOfParam(this.m_spec1) + Util.GetBitLengthOfParam(this.m_spec2);
        }

        public C1G2TargetTag Spec1
        {
            get
            {
                return this.m_spec1;
            }
        }

        public C1G2TargetTag Spec2
        {
            get
            {
                return this.m_spec2;
            }
        }
    }
}
