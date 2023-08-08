namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class AccessCommand : LlrpTlvParameterBase
    {
        private Collection<CustomParameterBase> m_customs;
        private Collection<OPSpec> m_opSpecs;
        private Kalitte.Sensors.Rfid.Llrp.Core.TagSpec m_tagSpec;

        internal AccessCommand(BitArray bitArray, ref int index) : base(LlrpParameterType.AccessSpecCommand, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Collection<LlrpParameterType> expectedTypes = new Collection<LlrpParameterType>();
            LlrpParameterType custom = LlrpParameterType.Custom;
            Kalitte.Sensors.Rfid.Llrp.Core.TagSpec tagSpec = null;
            expectedTypes.Add(LlrpParameterType.C1G2TagSpec);
            if (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, parameterEndLimit, out custom))
            {
                switch (custom)
                {
                    case LlrpParameterType.C1G2TagSpec:
                    {
                        tagSpec = new C1G2TagSpec(bitArray, ref index);
                        break;
                    }
                }
            }
            Collection<OPSpec> opSpecs = new Collection<OPSpec>();
            expectedTypes.Clear();
            expectedTypes.Add(LlrpParameterType.ClientRequestOPSpec);
            expectedTypes.Add(LlrpParameterType.C1G2Read);
            expectedTypes.Add(LlrpParameterType.C1G2Write);
            expectedTypes.Add(LlrpParameterType.C1G2Lock);
            expectedTypes.Add(LlrpParameterType.C1G2Kill);
            expectedTypes.Add(LlrpParameterType.C1G2BlockErase);
            expectedTypes.Add(LlrpParameterType.C1G2BlockWrite);
            while (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, parameterEndLimit, out custom))
            {
                switch (custom)
                {
                    case LlrpParameterType.C1G2Read:
                        opSpecs.Add(new C1G2Read(bitArray, ref index));
                        break;

                    case LlrpParameterType.C1G2Write:
                        opSpecs.Add(new C1G2Write(bitArray, ref index));
                        break;

                    case LlrpParameterType.C1G2Kill:
                        opSpecs.Add(new C1G2Kill(bitArray, ref index));
                        break;

                    case LlrpParameterType.C1G2Lock:
                        opSpecs.Add(new C1G2Lock(bitArray, ref index));
                        break;

                    case LlrpParameterType.C1G2BlockErase:
                        opSpecs.Add(new C1G2BlockErase(bitArray, ref index));
                        break;

                    case LlrpParameterType.C1G2BlockWrite:
                        opSpecs.Add(new C1G2BlockWrite(bitArray, ref index));
                        break;

                    case LlrpParameterType.ClientRequestOPSpec:
                        opSpecs.Add(new ClientRequestOPSpec(bitArray, ref index));
                        break;
                }
            }
            Collection<CustomParameterBase> customParameters = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index))
            {
                customParameters.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(tagSpec, opSpecs, customParameters);
        }

        public AccessCommand(Kalitte.Sensors.Rfid.Llrp.Core.TagSpec tagSpec, Collection<OPSpec> opSpecs, Collection<CustomParameterBase> customParameters) : base(LlrpParameterType.AccessSpecCommand)
        {
            this.Init(tagSpec, opSpecs, customParameters);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            Util.Encode(this.TagSpec, stream);
            Util.Encode<OPSpec>(this.OPSpecs, stream);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
        }

        private void Init(Kalitte.Sensors.Rfid.Llrp.Core.TagSpec tagSpec, Collection<OPSpec> opSpecs, Collection<CustomParameterBase> customParameters)
        {
            if (tagSpec == null)
            {
                throw new ArgumentNullException("tagSpec");
            }
            if ((opSpecs == null) || (opSpecs.Count == 0))
            {
                throw new ArgumentNullException("opSpecs");
            }
            Util.CheckCollectionForNonNullElement<OPSpec>(opSpecs);
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customParameters);
            this.m_tagSpec = tagSpec;
            this.m_opSpecs = opSpecs;
            this.m_customs = customParameters;
            this.ParameterLength = (this.TagSpec.ParameterLength + Util.GetTotalBitLengthOfParam<OPSpec>(this.OPSpecs)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.CustomParameters);
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customs;
            }
        }

        public Collection<OPSpec> OPSpecs
        {
            get
            {
                return this.m_opSpecs;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.TagSpec TagSpec
        {
            get
            {
                return this.m_tagSpec;
            }
        }
    }
}
