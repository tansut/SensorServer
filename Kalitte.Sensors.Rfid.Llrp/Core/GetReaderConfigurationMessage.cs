namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class GetReaderConfigurationMessage : LlrpMessageRequestBase
    {
        private ushort m_antennaId;
        private Collection<CustomParameterBase> m_customs;
        private ushort m_gpiNum;
        private ushort m_gpoNum;
        private ReaderConfigurationRequestedData m_requestedData;

        internal GetReaderConfigurationMessage(BitArray bitArray) : base(LlrpMessageType.GetReaderConfig, bitArray)
        {
            int startingIndex = 80;
            ushort antennaId = 0;
            antennaId = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 0x10);
            ReaderConfigurationRequestedData enumInstance = BitHelper.GetEnumInstance<ReaderConfigurationRequestedData>(BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 8));
            ushort gpiNum = 0;
            gpiNum = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 0x10);
            ushort gpoNum = 0;
            gpoNum = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 0x10);
            Collection<CustomParameterBase> customs = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, startingIndex))
            {
                customs.Add(CustomParameterBase.GetInstance(bitArray, ref startingIndex));
            }
            BitHelper.ValidateEndOfParameterOrMessage(startingIndex, (uint) bitArray.Count, base.GetType().FullName);
            this.Init(enumInstance, antennaId, gpiNum, gpoNum, customs);
        }

        public GetReaderConfigurationMessage(ReaderConfigurationRequestedData requestedData, ushort antennaId, ushort gpiNumber, ushort gpoNumber, Collection<CustomParameterBase> customParameters) : base(LlrpMessageType.GetReaderConfig)
        {
            this.Init(requestedData, antennaId, gpiNumber, gpoNumber, customParameters);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = this.CreateHeaderStream();
            stream.Append((long) this.AntennaId, 0x10, true);
            stream.Append((long) ((byte) this.RequestedData), 8, true);
            stream.Append((long) this.InputPort, 0x10, true);
            stream.Append((long) this.OutputPort, 0x10, true);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
            return stream.Merge();
        }

        private void Init(ReaderConfigurationRequestedData requestedData, ushort antennaId, ushort gpiNum, ushort gpoNum, Collection<CustomParameterBase> customs)
        {
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customs);
            this.m_requestedData = requestedData;
            this.m_antennaId = antennaId;
            this.m_gpiNum = gpiNum;
            this.m_gpoNum = gpoNum;
            this.m_customs = customs;
            this.MessageLength = 0x38 + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.m_customs);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Get Reader Configuration Message>");
            strBuilder.Append(base.ToString());
            strBuilder.Append("<Requested Data>");
            strBuilder.Append(this.RequestedData);
            strBuilder.Append("</Requested Data>");
            strBuilder.Append("<Antenna Id>");
            strBuilder.Append(this.AntennaId);
            strBuilder.Append("</Antenna Id>");
            strBuilder.Append("<Input Port>");
            strBuilder.Append(this.InputPort);
            strBuilder.Append("</Input Port>");
            strBuilder.Append("<Output Port>");
            strBuilder.Append(this.OutputPort);
            strBuilder.Append("</Output Port>");
            Util.ToString<CustomParameterBase>(this.CustomParameters, strBuilder);
            strBuilder.Append("</Get Reader Configuration Message>");
            return strBuilder.ToString();
        }

        public ushort AntennaId
        {
            get
            {
                return this.m_antennaId;
            }
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customs;
            }
        }

        public ushort InputPort
        {
            get
            {
                return this.m_gpiNum;
            }
        }

        public ushort OutputPort
        {
            get
            {
                return this.m_gpoNum;
            }
        }

        public ReaderConfigurationRequestedData RequestedData
        {
            get
            {
                return this.m_requestedData;
            }
        }
    }
}
