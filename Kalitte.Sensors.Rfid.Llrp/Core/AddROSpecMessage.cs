namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class AddROSpecMessage : LlrpMessageRequestBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.ROSpec m_roSpec;

        public AddROSpecMessage(Kalitte.Sensors.Rfid.Llrp.Core.ROSpec spec) : base(LlrpMessageType.AddROSpec)
        {
            this.Init(spec);
        }

        internal AddROSpecMessage(BitArray bitArray) : base(LlrpMessageType.AddROSpec, bitArray)
        {
            int index = 80;
            Kalitte.Sensors.Rfid.Llrp.Core.ROSpec spec = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ROSpec, bitArray, index))
            {
                spec = new Kalitte.Sensors.Rfid.Llrp.Core.ROSpec(bitArray, ref index);
            }
            this.Init(spec);
            BitHelper.ValidateEndOfParameterOrMessage(index, (uint) bitArray.Count, base.GetType().FullName);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = this.CreateHeaderStream();
            Util.Encode(this.ROSpec, stream);
            return stream.Merge();
        }

        private void Init(Kalitte.Sensors.Rfid.Llrp.Core.ROSpec spec)
        {
            if (spec == null)
            {
                throw new ArgumentNullException("spec");
            }
            this.m_roSpec = spec;
            this.MessageLength = this.m_roSpec.ParameterLength;
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Add RO Spec Message>");
            strBuilder.Append(base.ToString());
            Util.ToString(this.ROSpec, strBuilder);
            strBuilder.Append("</Add RO Spec Message>");
            return strBuilder.ToString();
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.ROSpec ROSpec
        {
            get
            {
                return this.m_roSpec;
            }
        }
    }
}
