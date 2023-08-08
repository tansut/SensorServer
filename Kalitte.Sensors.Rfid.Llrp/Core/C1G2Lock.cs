namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class C1G2Lock : C1G2OPSpec
    {
        private Collection<C1G2LockPayload> m_lockPayloads;
        private uint m_password;

        internal C1G2Lock(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2Lock, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            index += OPSpec.BaseLength;
            uint password = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            Collection<C1G2LockPayload> lockPayloads = new Collection<C1G2LockPayload>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.C1G2LockPayload, bitArray, index, parameterEndLimit))
            {
                lockPayloads.Add(new C1G2LockPayload(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(password, lockPayloads);
        }

        public C1G2Lock(uint password, Collection<C1G2LockPayload> lockPayloads) : base(LlrpParameterType.C1G2Lock)
        {
            this.Init(password, lockPayloads);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.Password, 0x20, true);
            Util.Encode<C1G2LockPayload>(this.LockPayloads, stream);
        }

        private void Init(uint password, Collection<C1G2LockPayload> lockPayloads)
        {
            if ((lockPayloads == null) || (lockPayloads.Count == 0))
            {
                throw new ArgumentNullException("lockPayloads");
            }
            Util.CheckCollectionForNonNullElement<C1G2LockPayload>(lockPayloads);
            this.m_password = password;
            this.m_lockPayloads = lockPayloads;
            this.ParameterLength = 0x20 + Util.GetTotalBitLengthOfParam<C1G2LockPayload>(this.m_lockPayloads);
        }

        public Collection<C1G2LockPayload> LockPayloads
        {
            get
            {
                return this.m_lockPayloads;
            }
        }

        public uint Password
        {
            get
            {
                return this.m_password;
            }
        }
    }
}
