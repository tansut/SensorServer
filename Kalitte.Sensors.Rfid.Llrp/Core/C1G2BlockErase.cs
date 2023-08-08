namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Core;

    [Serializable]
    public sealed class C1G2BlockErase : C1G2OPSpec
    {
        private uint m_accessPassword;
        private C1G2MemoryBank m_memoryBank;
        private ushort m_wordCount;
        private ushort m_wordPointer;

        internal C1G2BlockErase(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2BlockErase, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            index += OPSpec.BaseLength;
            uint accessPassword = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            C1G2MemoryBank enumInstance = BitHelper.GetEnumInstance<C1G2MemoryBank>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 2));
            index += 6;
            ushort wordPointer = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort wordCount = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(accessPassword, enumInstance, wordPointer, wordCount);
        }

        public C1G2BlockErase(uint accessPassword, C1G2MemoryBank memoryBank, ushort wordPointer, ushort wordCount) : base(LlrpParameterType.C1G2BlockErase)
        {
            this.Init(accessPassword, memoryBank, wordPointer, wordCount);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.AccessPassword, 0x20, true);
            stream.Append((long) ((byte) this.MemoryBank), 2, true);
            stream.Append((ulong) 0L, 6, true);
            stream.Append((long) this.WordPointer, 0x10, true);
            stream.Append((long) this.WordCount, 0x10, true);
        }

        private void Init(uint accessPassword, C1G2MemoryBank memoryBank, ushort wordPointer, ushort wordCount)
        {
            this.m_accessPassword = accessPassword;
            this.m_memoryBank = memoryBank;
            this.m_wordPointer = wordPointer;
            this.m_wordCount = wordCount;
            this.ParameterLength = 0x48;
        }

        public uint AccessPassword
        {
            get
            {
                return this.m_accessPassword;
            }
        }

        public C1G2MemoryBank MemoryBank
        {
            get
            {
                return this.m_memoryBank;
            }
        }

        public ushort WordCount
        {
            get
            {
                return this.m_wordCount;
            }
        }

        public ushort WordPointer
        {
            get
            {
                return this.m_wordPointer;
            }
        }
    }
}
