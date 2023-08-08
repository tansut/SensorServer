namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class GetAccessSpecResponse : LlrpMessageResponseBase
    {
        private Collection<AccessSpec> m_accessSpecs;

        internal GetAccessSpecResponse(BitArray bitArray) : base(LlrpMessageType.GetAccessSpecResponse, bitArray)
        {
            int baseLength = base.BaseLength;
            Collection<AccessSpec> accessSpecs = new Collection<AccessSpec>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AccessSpec, bitArray, baseLength))
            {
                accessSpecs.Add(new AccessSpec(bitArray, ref baseLength));
            }
            BitHelper.ValidateEndOfParameterOrMessage(baseLength, (uint) bitArray.Count, base.GetType().FullName);
            this.Init(accessSpecs);
        }

        public GetAccessSpecResponse(uint messageId, LlrpStatus status, Collection<AccessSpec> accessSpecs) : base(LlrpMessageType.GetAccessSpecResponse, messageId, status)
        {
            this.Init(accessSpecs);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = base.CreateResponseHeaderStream();
            Util.Encode<AccessSpec>(this.AccessSpecs, stream);
            return stream.Merge();
        }

        private void Init(Collection<AccessSpec> accessSpecs)
        {
            if (accessSpecs == null)
            {
                accessSpecs = new Collection<AccessSpec>();
            }
            Util.CheckCollectionForNonNullElement<AccessSpec>(accessSpecs);
            this.m_accessSpecs = accessSpecs;
        }

        public Collection<AccessSpec> AccessSpecs
        {
            get
            {
                return this.m_accessSpecs;
            }
        }
    }
}
