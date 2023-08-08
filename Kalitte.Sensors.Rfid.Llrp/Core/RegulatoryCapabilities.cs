namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;


    public sealed class RegulatoryCapabilities : LlrpTlvParameterBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.CommunicationStandard m_communicationStandard;
        private ushort m_countryCode;
        private Collection<CustomParameterBase> m_customParameters;
        private Kalitte.Sensors.Rfid.Llrp.Core.UhfBandCapabilities m_uhfBandCapabilities;

        internal RegulatoryCapabilities(BitArray bitArray, ref int index) : base(LlrpParameterType.RegulatoryCapabilities, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Kalitte.Sensors.Rfid.Llrp.Core.UhfBandCapabilities uhfBandCapabilities = null;
            Collection<CustomParameterBase> customParameters = new Collection<CustomParameterBase>();
            ushort countryCode = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            Kalitte.Sensors.Rfid.Llrp.Core.CommunicationStandard enumInstance = BitHelper.GetEnumInstance<Kalitte.Sensors.Rfid.Llrp.Core.CommunicationStandard>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10));
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.UhfBandCapabilities, bitArray, index, parameterEndLimit))
            {
                uhfBandCapabilities = new Kalitte.Sensors.Rfid.Llrp.Core.UhfBandCapabilities(bitArray, ref index);
            }
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index, parameterEndLimit))
            {
                customParameters.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(countryCode, enumInstance, uhfBandCapabilities, customParameters);
        }

        public RegulatoryCapabilities(ushort countryCode, Kalitte.Sensors.Rfid.Llrp.Core.CommunicationStandard communicationStandard, Kalitte.Sensors.Rfid.Llrp.Core.UhfBandCapabilities uhfBandCapabilities, Collection<CustomParameterBase> customParameters) : base(LlrpParameterType.RegulatoryCapabilities)
        {
            this.Init(countryCode, communicationStandard, uhfBandCapabilities, customParameters);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.CountryCode, 0x10, true);
            stream.Append((long) ((ushort) this.CommunicationStandard), 0x10, true);
            Util.Encode(this.UhfBandCapabilities, stream);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
        }

        private void Init(ushort countryCode, Kalitte.Sensors.Rfid.Llrp.Core.CommunicationStandard communicationStandard, Kalitte.Sensors.Rfid.Llrp.Core.UhfBandCapabilities uhfBandCapabilities, Collection<CustomParameterBase> customParameters)
        {
            this.m_countryCode = countryCode;
            this.m_communicationStandard = communicationStandard;
            this.m_uhfBandCapabilities = uhfBandCapabilities;
            this.m_customParameters = customParameters;
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customParameters);
            this.ParameterLength = (0x20 + Util.GetBitLengthOfParam(this.m_uhfBandCapabilities)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.m_customParameters);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Regulatory Capabilities>");
            strBuilder.Append(base.ToString());
            strBuilder.Append("<Country Code>");
            strBuilder.Append(this.CountryCode);
            strBuilder.Append("</Country Code>");
            strBuilder.Append("<Communication Standard>");
            strBuilder.Append(this.CommunicationStandard);
            strBuilder.Append("</Communication Standard>");
            //Util.ToStringSerialize(this.UhfBandCapabilities, strBuilder);
            Util.ToString<CustomParameterBase>(this.CustomParameters, strBuilder);
            return strBuilder.ToString();
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.CommunicationStandard CommunicationStandard
        {
            get
            {
                return this.m_communicationStandard;
            }
        }

        public ushort CountryCode
        {
            get
            {
                return this.m_countryCode;
            }
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customParameters;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.UhfBandCapabilities UhfBandCapabilities
        {
            get
            {
                return this.m_uhfBandCapabilities;
            }
        }
    }
}
