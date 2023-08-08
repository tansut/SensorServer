namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class GetReaderCapabilitiesResponse : LlrpMessageResponseBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolLlrpCapabilities m_air;
        private Collection<CustomParameterBase> m_custom;
        private Kalitte.Sensors.Rfid.Llrp.Core.GeneralDeviceCapabilities m_general;
        private Kalitte.Sensors.Rfid.Llrp.Core.LlrpCapabilities m_llrp;
        private Kalitte.Sensors.Rfid.Llrp.Core.RegulatoryCapabilities m_regulatory;

        internal GetReaderCapabilitiesResponse(BitArray bitArray) : base(LlrpMessageType.GetReaderCapabilitiesResponse, bitArray)
        {
            LlrpParameterType type;
            int baseLength = base.BaseLength;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.GeneralDeviceCapabilities, bitArray, baseLength))
            {
                this.m_general = new Kalitte.Sensors.Rfid.Llrp.Core.GeneralDeviceCapabilities(bitArray, ref baseLength);
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.LlrpCapabilities, bitArray, baseLength))
            {
                this.m_llrp = new Kalitte.Sensors.Rfid.Llrp.Core.LlrpCapabilities(bitArray, ref baseLength);
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.RegulatoryCapabilities, bitArray, baseLength))
            {
                this.m_regulatory = new Kalitte.Sensors.Rfid.Llrp.Core.RegulatoryCapabilities(bitArray, ref baseLength);
            }
            Collection<LlrpParameterType> expectedTypes = new Collection<LlrpParameterType>();
            expectedTypes.Add(LlrpParameterType.C1G2LlrpCapabilities);
            if (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, baseLength, out type))
            {
                switch (type)
                {
                    case LlrpParameterType.C1G2LlrpCapabilities:
                    {
                        this.m_air = new C1G2LlrpCapabilities(bitArray, ref baseLength);
                        break;
                    }
                }
            }
            this.m_custom = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, baseLength))
            {
                this.m_custom.Add(CustomParameterBase.GetInstance(bitArray, ref baseLength));
            }
            BitHelper.ValidateEndOfParameterOrMessage(baseLength, (uint) bitArray.Count, base.GetType().FullName);
        }

        public GetReaderCapabilitiesResponse(uint messageId, LlrpStatus status, Kalitte.Sensors.Rfid.Llrp.Core.GeneralDeviceCapabilities general, Kalitte.Sensors.Rfid.Llrp.Core.LlrpCapabilities llrp, Kalitte.Sensors.Rfid.Llrp.Core.RegulatoryCapabilities regulatory, C1G2LlrpCapabilities airProtocolCapabilities, Collection<CustomParameterBase> customs) : base(LlrpMessageType.GetReaderCapabilitiesResponse, messageId, status)
        {
            this.Init(general, llrp, regulatory, airProtocolCapabilities, customs);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = base.CreateResponseHeaderStream();
            Util.Encode(this.m_general, stream);
            Util.Encode(this.m_llrp, stream);
            Util.Encode(this.m_regulatory, stream);
            Util.Encode(this.m_air, stream);
            Util.Encode<CustomParameterBase>(this.m_custom, stream);
            return stream.Merge();
        }

        private void Init(Kalitte.Sensors.Rfid.Llrp.Core.GeneralDeviceCapabilities general, Kalitte.Sensors.Rfid.Llrp.Core.LlrpCapabilities llrp, Kalitte.Sensors.Rfid.Llrp.Core.RegulatoryCapabilities regulatory, C1G2LlrpCapabilities airProtocolCapabilities, Collection<CustomParameterBase> customs)
        {
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customs);
            this.m_general = general;
            this.m_llrp = llrp;
            this.m_regulatory = regulatory;
            this.m_air = airProtocolCapabilities;
            this.m_custom = customs;
            this.MessageLength = (((Util.GetBitLengthOfParam(this.m_general) + Util.GetBitLengthOfParam(this.m_llrp)) + Util.GetBitLengthOfParam(this.m_regulatory)) + Util.GetBitLengthOfParam(this.m_air)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(customs);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Get Reader Capabilities Response>");
            strBuilder.Append(base.ToString());
            Util.ToString(this.GeneralDeviceCapabilities, strBuilder);
            Util.ToString(this.LlrpCapabilities, strBuilder);
            Util.ToString(this.RegulatoryCapabilities, strBuilder);
            Util.ToString(this.AirProtocolLlrpCapabilities, strBuilder);
            Util.ToString<CustomParameterBase>(this.CustomParameters, strBuilder);
            strBuilder.Append("</Get Reader Capabilities Response>");
            return strBuilder.ToString();
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolLlrpCapabilities AirProtocolLlrpCapabilities
        {
            get
            {
                return this.m_air;
            }
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_custom;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.GeneralDeviceCapabilities GeneralDeviceCapabilities
        {
            get
            {
                return this.m_general;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.LlrpCapabilities LlrpCapabilities
        {
            get
            {
                return this.m_llrp;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.RegulatoryCapabilities RegulatoryCapabilities
        {
            get
            {
                return this.m_regulatory;
            }
        }
    }
}
