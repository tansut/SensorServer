namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Core;

    [Serializable]
    public sealed class C1G2Read : C1G2OPSpec
    {
        private C1G2MemoryBank m_bank;
        private uint m_password;
        private ushort m_pointer;
        private ushort m_wordCount;

        internal C1G2Read(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2Read, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            index += OPSpec.BaseLength;
            uint password = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            C1G2MemoryBank enumInstance = BitHelper.GetEnumInstance<C1G2MemoryBank>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 2));
            index += 6;
            ushort pointer = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort count = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(password, enumInstance, pointer, count);
        }

        public C1G2Read(uint password, C1G2MemoryBank bank, ushort pointer, ushort count) : base(LlrpParameterType.C1G2Read)
        {
            this.Init(password, bank, pointer, count);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.Password, 0x20, true);
            stream.Append((long) ((byte) this.Bank), 2, true);
            stream.Append((ulong) 0L, 6, true);
            stream.Append((long) this.Pointer, 0x10, true);
            stream.Append((long) this.WordCount, 0x10, true);
        }

        private void Init(uint password, C1G2MemoryBank bank, ushort pointer, ushort count)
        {
            this.m_password = password;
            this.m_bank = bank;
            this.m_pointer = pointer;
            this.m_wordCount = count;
            this.ParameterLength = 0x48;
        }

        public C1G2MemoryBank Bank
        {
            get
            {
                return this.m_bank;
            }
        }

        public uint Password
        {
            get
            {
                return this.m_password;
            }
        }

        public ushort Pointer
        {
            get
            {
                return this.m_pointer;
            }
        }

        public ushort WordCount
        {
            get
            {
                return this.m_wordCount;
            }
        }
    }
}
