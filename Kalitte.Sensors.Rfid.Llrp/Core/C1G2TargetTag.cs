namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Rfid.Llrp.Exceptions;
    using Kalitte.Sensors.Rfid.Core;

    [Serializable]
    public sealed class C1G2TargetTag : LlrpTlvParameterBase
    {
        private ushort m_bitCount;
        private byte[] m_mask;
        private bool m_matchPattern;
        private C1G2MemoryBank m_memoryBank;
        private ushort m_pointer;
        private byte[] m_tagData;

        internal C1G2TargetTag(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2TargetTag, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            C1G2MemoryBank enumInstance = BitHelper.GetEnumInstance<C1G2MemoryBank>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 2));
            bool matchPattern = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 5;
            ushort pointer = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort bits = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort num4 = Util.BitsToPad(bits);
            byte[] mask = null;
            if (bits > 0)
            {
                mask = BitHelper.ConvertBitArrayToByteArray(bitArray, ref index, bits, false);
            }
            index += num4;
            ushort num5 = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            if (bits != num5)
            {
                throw new DecodingException("Mask and data count mismatch", LlrpResources.TagMaskAndDataLengthDoesNotMatch);
            }
            byte[] tagData = null;
            if (bits > 0)
            {
                tagData = BitHelper.ConvertBitArrayToByteArray(bitArray, ref index, bits, false);
            }
            index += num4;
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, matchPattern, pointer, bits, mask, tagData);
        }

        public C1G2TargetTag(C1G2MemoryBank bank, bool matchPattern, ushort pointer, ushort bitCount, byte[] mask, byte[] tagData) : base(LlrpParameterType.C1G2TargetTag)
        {
            this.Init(bank, matchPattern, pointer, bitCount, mask, tagData);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.MemoryBank), 2, true);
            stream.Append(this.MatchPattern, 1, true);
            stream.Append((ulong) 0L, 5, true);
            stream.Append((long) this.Pointer, 0x10, true);
            stream.Append((long) this.BitCount, 0x10, true);
            Util.AppendBytesAndPadToByteBoundary(stream, this.m_mask, this.BitCount, false);
            stream.Append((long) this.BitCount, 0x10, true);
            Util.AppendBytesAndPadToByteBoundary(stream, this.m_tagData, this.BitCount, false);
        }

        public byte[] GetMask()
        {
            return Util.GetByteArrayClone(this.m_mask);
        }

        public byte[] GetTagData()
        {
            return Util.GetByteArrayClone(this.m_tagData);
        }

        private void Init(C1G2MemoryBank bank, bool matchPattern, ushort pointer, ushort bitCount, byte[] mask, byte[] tagData)
        {
            if ((mask == null) || (tagData == null))
            {
                if ((mask != null) || (tagData != null))
                {
                    throw new ArgumentException(LlrpResources.TagMaskAndDataLengthDoesNotMatch);
                }
                if (bitCount > 0)
                {
                    throw new ArgumentOutOfRangeException("bitCount");
                }
            }
            else if ((bitCount > (mask.Length * 8)) || (bitCount > (tagData.Length * 8)))
            {
                throw new ArgumentOutOfRangeException("bitCount");
            }
            this.m_memoryBank = bank;
            this.m_matchPattern = matchPattern;
            this.m_pointer = pointer;
            this.m_bitCount = bitCount;
            this.m_mask = mask;
            this.m_tagData = tagData;
            ushort num = (ushort) (Util.BitsToPad(this.m_bitCount) + this.m_bitCount);
            this.ParameterLength = (uint) (0x38 + (num * 2));
        }

        public ushort BitCount
        {
            get
            {
                return this.m_bitCount;
            }
        }

        public bool MatchPattern
        {
            get
            {
                return this.m_matchPattern;
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
