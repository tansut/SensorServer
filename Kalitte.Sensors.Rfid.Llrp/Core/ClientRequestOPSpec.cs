namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class ClientRequestOPSpec : OPSpec
    {
        public ClientRequestOPSpec() : base(LlrpParameterType.ClientRequestOPSpec)
        {
        }

        internal ClientRequestOPSpec(BitArray bitArray, ref int index) : base(LlrpParameterType.ClientRequestOPSpec, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            index += OPSpec.BaseLength;
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init();
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
        }

        private void Init()
        {
            this.ParameterLength = 0;
        }
    }
}
