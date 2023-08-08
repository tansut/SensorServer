namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class AddAccessSpecMessage : LlrpMessageRequestBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.AccessSpec m_accessSpec;

        public AddAccessSpecMessage(Kalitte.Sensors.Rfid.Llrp.Core.AccessSpec accessSpec) : base(LlrpMessageType.AddAccessSpec)
        {
            this.Init(accessSpec);
        }

        internal AddAccessSpecMessage(BitArray bitArray) : base(LlrpMessageType.AddAccessSpec, bitArray)
        {
            int index = 80;
            Kalitte.Sensors.Rfid.Llrp.Core.AccessSpec accessSpec = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AccessSpec, bitArray, index))
            {
                accessSpec = new Kalitte.Sensors.Rfid.Llrp.Core.AccessSpec(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, (uint) bitArray.Count, base.GetType().FullName);
            this.Init(accessSpec);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = this.CreateHeaderStream();
            Util.Encode(this.AccessSpec, stream);
            return stream.Merge();
        }

        private void Init(Kalitte.Sensors.Rfid.Llrp.Core.AccessSpec accessSpec)
        {
            if (accessSpec == null)
            {
                throw new ArgumentNullException("accessSpec");
            }
            this.m_accessSpec = accessSpec;
            this.MessageLength = this.AccessSpec.ParameterLength;
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.AccessSpec AccessSpec
        {
            get
            {
                return this.m_accessSpec;
            }
        }
    }
}
