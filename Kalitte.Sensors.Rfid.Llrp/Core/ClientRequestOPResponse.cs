namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class ClientRequestOPResponse : LlrpMessageRequestBase
    {
        private ClientRequestResponseParameter m_clientResponse;

        internal ClientRequestOPResponse(BitArray bitArray) : base(LlrpMessageType.ClientRequestOPResponse, bitArray)
        {
            int index = 80;
            ClientRequestResponseParameter clientResponse = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ClientRequestResponse, bitArray, index))
            {
                clientResponse = new ClientRequestResponseParameter(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, (uint) bitArray.Count, base.GetType().FullName);
            this.Init(clientResponse);
        }

        public ClientRequestOPResponse(uint messageId, ClientRequestResponseParameter clientResponse) : base(LlrpMessageType.ClientRequestOPResponse, messageId)
        {
            this.Init(clientResponse);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = this.CreateHeaderStream();
            Util.Encode(this.ClientResponse, stream);
            return stream.Merge();
        }

        private void Init(ClientRequestResponseParameter clientResponse)
        {
            if (clientResponse == null)
            {
                throw new ArgumentNullException("clientResponse");
            }
            this.m_clientResponse = clientResponse;
            this.MessageLength = this.ClientResponse.ParameterLength;
        }

        public ClientRequestResponseParameter ClientResponse
        {
            get
            {
                return this.m_clientResponse;
            }
        }
    }
}
