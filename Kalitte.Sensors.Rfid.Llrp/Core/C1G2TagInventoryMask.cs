namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Core;

    [Serializable]
    public sealed class C1G2TagInventoryMask : LlrpTlvParameterBase
    {
        private byte[] m_mask;
        private ushort m_maskBitCount;
        private C1G2MemoryBank m_memoryBank;
        private ushort m_pointer;

        internal C1G2TagInventoryMask(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2TagInventoryMask, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            C1G2MemoryBank enumInstance = BitHelper.GetEnumInstance<C1G2MemoryBank>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 2));
            index += 6;
            ushort pointer = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort length = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            byte[] mask = null;
            if (length > 0)
            {
                mask = BitHelper.ConvertBitArrayToByteArray(bitArray, ref index, length, false);
            }
            ushort num4 = Util.BitsToPad(length);
            index += num4;
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, pointer, length, mask);
        }

        public C1G2TagInventoryMask(C1G2MemoryBank memoryBank, ushort pointer, ushort maskBitCount, byte[] mask) : base(LlrpParameterType.C1G2TagInventoryMask)
        {
            this.Init(memoryBank, pointer, maskBitCount, mask);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.MemoryBank), 2, true);
            stream.Append((ulong) 0L, 6, true);
            stream.Append((long) this.Pointer, 0x10, true);
            stream.Append((long) this.MaskBitCount, 0x10, true);
            Util.AppendBytesAndPadToByteBoundary(stream, this.m_mask, this.MaskBitCount, false);
        }

        public byte[] GetMask()
        {
            return Util.GetByteArrayClone(this.m_mask);
        }

        private void Init(C1G2MemoryBank memoryBank, ushort pointer, ushort maskBitCount, byte[] mask)
        {
            if (memoryBank == C1G2MemoryBank.Reserved)
            {
                throw new ArgumentOutOfRangeException("memoryBank");
            }
            if (mask == null)
            {
                if (maskBitCount > 0)
                {
                    throw new ArgumentOutOfRangeException("maskBitCount");
                }
            }
            else if (maskBitCount > (mask.Length * 8))
            {
                throw new ArgumentOutOfRangeException("maskBitCount");
            }
            this.m_memoryBank = memoryBank;
            this.m_pointer = pointer;
            this.m_maskBitCount = maskBitCount;
            this.m_mask = mask;
            ushort num = Util.BitsToPad(this.m_maskBitCount);
            this.ParameterLength = (uint) ((40 + this.m_maskBitCount) + num);
        }

        public ushort MaskBitCount
        {
            get
            {
                return this.m_maskBitCount;
            }
        }

        public C1G2MemoryBank MemoryBank
        {
            get
            {
                return this.m_memoryBank;
            }
        }

        public ushort Pointer
        {
            get
            {
                return this.m_pointer;
            }
        }
    }
}
