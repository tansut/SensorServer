namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class GetROSpecResponse : LlrpMessageResponseBase
    {
        private Collection<ROSpec> m_specs;

        internal GetROSpecResponse(BitArray bitArray) : base(LlrpMessageType.GetROSpecsResponse, bitArray)
        {
            int baseLength = base.BaseLength;
            Collection<ROSpec> roSpecs = new Collection<ROSpec>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ROSpec, bitArray, baseLength))
            {
                roSpecs.Add(new ROSpec(bitArray, ref baseLength));
            }
            this.Init(roSpecs);
            BitHelper.ValidateEndOfParameterOrMessage(baseLength, (uint) bitArray.Count, base.GetType().FullName);
        }

        public GetROSpecResponse(uint messageId, LlrpStatus status, Collection<ROSpec> roSpecs) : base(LlrpMessageType.GetROSpecsResponse, messageId, status)
        {
            this.Init(roSpecs);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = base.CreateResponseHeaderStream();
            Util.Encode<ROSpec>(this.m_specs, stream);
            return stream.Merge();
        }

        private void Init(Collection<ROSpec> roSpecs)
        {
            Util.CheckCollectionForNonNullElement<ROSpec>(roSpecs);
            this.m_specs = roSpecs;
            this.MessageLength = Util.GetTotalBitLengthOfParam<ROSpec>(this.m_specs);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Get RO Spec Response>");
            strBuilder.Append(base.ToString());
            Util.ToString<ROSpec>(this.Specs, strBuilder);
            strBuilder.Append("</Get RO Spec Response>");
            return strBuilder.ToString();
        }

        public Collection<ROSpec> Specs
        {
            get
            {
                return this.m_specs;
            }
        }
    }
}
