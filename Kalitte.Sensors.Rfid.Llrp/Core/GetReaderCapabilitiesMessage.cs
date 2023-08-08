namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class GetReaderCapabilitiesMessage : LlrpMessageRequestBase
    {
        private Collection<CustomParameterBase> m_customParams;
        private ReaderCapabilitiesRequestedData m_requestedData;

        internal GetReaderCapabilitiesMessage(BitArray bitArray) : base(LlrpMessageType.GetReaderCapabilities, bitArray)
        {
            int startingIndex = 80;
            ReaderCapabilitiesRequestedData enumInstance = BitHelper.GetEnumInstance<ReaderCapabilitiesRequestedData>(BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 8));
            Collection<CustomParameterBase> customParams = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, startingIndex))
            {
                customParams.Add(CustomParameterBase.GetInstance(bitArray, ref startingIndex));
            }
            this.Init(enumInstance, customParams);
            BitHelper.ValidateEndOfParameterOrMessage(startingIndex, (uint) bitArray.Count, base.GetType().FullName);
        }

        public GetReaderCapabilitiesMessage(ReaderCapabilitiesRequestedData requestedData, Collection<CustomParameterBase> customParameters) : base(LlrpMessageType.GetReaderCapabilities)
        {
            this.Init(requestedData, customParameters);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = this.CreateHeaderStream();
            stream.Append((long) ((byte) this.RequestedData), 8, true);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
            return stream.Merge();
        }

        private void Init(ReaderCapabilitiesRequestedData requestedData, Collection<CustomParameterBase> customParams)
        {
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customParams);
            this.m_requestedData = requestedData;
            this.m_customParams = customParams;
            this.MessageLength = 8 + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.m_customParams);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Get Reader Capabilities Message>");
            strBuilder.Append(base.ToString());
            strBuilder.Append("<Requested Data>");
            strBuilder.Append(this.RequestedData);
            strBuilder.Append("</Requested Data>");
            Util.ToString<CustomParameterBase>(this.CustomParameters, strBuilder);
            strBuilder.Append("</Get Reader Capabilities Message>");
            return strBuilder.ToString();
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customParams;
            }
        }

        public ReaderCapabilitiesRequestedData RequestedData
        {
            get
            {
                return this.m_requestedData;
            }
        }
    }
}
