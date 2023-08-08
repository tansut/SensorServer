namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;

    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    public sealed class ClientRequestResponseParameter : LlrpTlvParameterBase
    {
        private uint m_accessSpecId;
        private Collection<AirProtocolOPSpec> m_airProtocolOPSpecs;
        private Kalitte.Sensors.Rfid.Llrp.Core.EPC96 m_epc96;
        private Kalitte.Sensors.Rfid.Llrp.Core.EpcData m_epcData;

        internal ClientRequestResponseParameter(BitArray bitArray, ref int index) : base(LlrpParameterType.ClientRequestResponse, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            uint accessSpecId = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            Kalitte.Sensors.Rfid.Llrp.Core.EPC96 epc = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.EPC96, bitArray, index, parameterEndLimit))
            {
                epc = new Kalitte.Sensors.Rfid.Llrp.Core.EPC96(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.EpcData epcData = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.EpcData, bitArray, index, parameterEndLimit))
            {
                epcData = new Kalitte.Sensors.Rfid.Llrp.Core.EpcData(bitArray, ref index);
            }
            Collection<AirProtocolOPSpec> airProtocolOPSpecs = new Collection<AirProtocolOPSpec>();
            Collection<LlrpParameterType> expectedTypes = new Collection<LlrpParameterType>();
            expectedTypes.Add(LlrpParameterType.C1G2Read);
            expectedTypes.Add(LlrpParameterType.C1G2Write);
            expectedTypes.Add(LlrpParameterType.C1G2Lock);
            expectedTypes.Add(LlrpParameterType.C1G2Kill);
            expectedTypes.Add(LlrpParameterType.C1G2BlockErase);
            expectedTypes.Add(LlrpParameterType.C1G2BlockWrite);
            LlrpParameterType custom = LlrpParameterType.Custom;
            while (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, parameterEndLimit, out custom))
            {
                switch (custom)
                {
                    case LlrpParameterType.C1G2Read:
                        airProtocolOPSpecs.Add(new C1G2Read(bitArray, ref index));
                        break;

                    case LlrpParameterType.C1G2Write:
                        airProtocolOPSpecs.Add(new C1G2Write(bitArray, ref index));
                        break;

                    case LlrpParameterType.C1G2Kill:
                        airProtocolOPSpecs.Add(new C1G2Kill(bitArray, ref index));
                        break;

                    case LlrpParameterType.C1G2Lock:
                        airProtocolOPSpecs.Add(new C1G2Lock(bitArray, ref index));
                        break;

                    case LlrpParameterType.C1G2BlockErase:
                        airProtocolOPSpecs.Add(new C1G2BlockErase(bitArray, ref index));
                        break;

                    case LlrpParameterType.C1G2BlockWrite:
                        airProtocolOPSpecs.Add(new C1G2BlockWrite(bitArray, ref index));
                        break;
                }
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(accessSpecId, epc, epcData, airProtocolOPSpecs);
        }

        public ClientRequestResponseParameter(uint accessSpecId, Kalitte.Sensors.Rfid.Llrp.Core.EPC96 epc96, Kalitte.Sensors.Rfid.Llrp.Core.EpcData epcData, Collection<AirProtocolOPSpec> airProtocolOPSpecs) : base(LlrpParameterType.ClientRequestResponse)
        {
            this.Init(accessSpecId, epc96, epcData, airProtocolOPSpecs);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.AccessSpecId, 0x20, true);
            Util.Encode(this.EPC96, stream);
            Util.Encode(this.EpcData, stream);
            Util.Encode<AirProtocolOPSpec>(this.AirProtocolOPSpecs, stream);
        }

        private void Init(uint accessSpecId, Kalitte.Sensors.Rfid.Llrp.Core.EPC96 epc96, Kalitte.Sensors.Rfid.Llrp.Core.EpcData epcData, Collection<AirProtocolOPSpec> airProtocolOPSpecs)
        {
            if ((epc96 == null) && (epcData == null))
            {
                throw new ArgumentException(LlrpResources.NoEPCDataFound);
            }
            if ((epc96 != null) && (epcData != null))
            {
                throw new ArgumentException(LlrpResources.BothEPCDataPresent);
            }
            Util.CheckCollectionForNonNullElement<AirProtocolOPSpec>(airProtocolOPSpecs);
            this.m_accessSpecId = accessSpecId;
            this.m_epc96 = epc96;
            this.m_epcData = epcData;
            this.m_airProtocolOPSpecs = airProtocolOPSpecs;
            this.ParameterLength = ((0x20 + Util.GetBitLengthOfParam(this.EPC96)) + Util.GetBitLengthOfParam(this.EpcData)) + Util.GetTotalBitLengthOfParam<AirProtocolOPSpec>(this.AirProtocolOPSpecs);
        }

        public uint AccessSpecId
        {
            get
            {
                return this.m_accessSpecId;
            }
        }

        public Collection<AirProtocolOPSpec> AirProtocolOPSpecs
        {
            get
            {
                return this.m_airProtocolOPSpecs;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.EPC96 EPC96
        {
            get
            {
                return this.m_epc96;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.EpcData EpcData
        {
            get
            {
                return this.m_epcData;
            }
        }
    }
}
