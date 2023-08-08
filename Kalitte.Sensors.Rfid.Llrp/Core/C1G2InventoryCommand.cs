namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class C1G2InventoryCommand : AirProtocolInventoryCommandSettings
    {
        private Collection<CustomParameterBase> m_customParameters;
        private Collection<C1G2Filter> m_filters;
        private C1G2RFControl m_rfControl;
        private C1G2SingulationControl m_singulationControl;
        private bool m_stateAware;

        internal C1G2InventoryCommand(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2InventoryCommand, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            bool stateAware = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 7;
            Collection<C1G2Filter> filters = new Collection<C1G2Filter>();
            C1G2RFControl rfControl = null;
            C1G2SingulationControl singulationControl = null;
            Collection<CustomParameterBase> customParameters = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.C1G2Filter, bitArray, index, parameterEndLimit))
            {
                filters.Add(new C1G2Filter(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.C1G2RFControl, bitArray, index, parameterEndLimit))
            {
                rfControl = new C1G2RFControl(bitArray, ref index);
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.C1G2SingulationControl, bitArray, index, parameterEndLimit))
            {
                singulationControl = new C1G2SingulationControl(bitArray, ref index);
            }
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index, parameterEndLimit))
            {
                customParameters.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(stateAware, filters, rfControl, singulationControl, customParameters);
        }

        public C1G2InventoryCommand(bool stateAware, Collection<C1G2Filter> filters, C1G2RFControl rfControl, C1G2SingulationControl singulationControl, Collection<CustomParameterBase> customParameters) : base(LlrpParameterType.C1G2InventoryCommand)
        {
            this.Init(stateAware, filters, rfControl, singulationControl, customParameters);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append(this.StateAware, 1, true);
            stream.Append((ulong) 0L, 7, true);
            Util.Encode<C1G2Filter>(this.Filters, stream);
            Util.Encode(this.RFControl, stream);
            Util.Encode(this.SingulationControl, stream);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
        }

        private void Init(bool stateAware, Collection<C1G2Filter> filters, C1G2RFControl rfControl, C1G2SingulationControl singulationControl, Collection<CustomParameterBase> customParameters)
        {
            Util.CheckCollectionForNonNullElement<C1G2Filter>(filters);
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customParameters);
            this.m_stateAware = stateAware;
            this.m_filters = filters;
            this.m_rfControl = rfControl;
            this.m_singulationControl = singulationControl;
            this.m_customParameters = customParameters;
            this.ParameterLength = (((8 + Util.GetTotalBitLengthOfParam<C1G2Filter>(this.Filters)) + Util.GetBitLengthOfParam(this.RFControl)) + Util.GetBitLengthOfParam(this.SingulationControl)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.CustomParameters);
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customParameters;
            }
        }

        public Collection<C1G2Filter> Filters
        {
            get
            {
                return this.m_filters;
            }
        }

        public C1G2RFControl RFControl
        {
            get
            {
                return this.m_rfControl;
            }
        }

        public C1G2SingulationControl SingulationControl
        {
            get
            {
                return this.m_singulationControl;
            }
        }

        public bool StateAware
        {
            get
            {
                return this.m_stateAware;
            }
        }
    }
}
