namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Core;

    [Serializable]
    public sealed class C1G2BlockWrite : C1G2OPSpec
    {
        private C1G2MemoryBank m_bank;
        private short[] m_data;
        private uint m_password;
        private ushort m_pointer;

        internal C1G2BlockWrite(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2BlockWrite, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            index += OPSpec.BaseLength;
            uint password = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            C1G2MemoryBank enumInstance = BitHelper.GetEnumInstance<C1G2MemoryBank>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 2));
            index += 6;
            ushort pointer = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort num4 = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            short[] data = null;
            if (num4 > 0)
            {
                data = new short[num4];
                for (int i = 0; i < num4; i++)
                {
                    data[i] = (short) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
                }
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(password, enumInstance, pointer, data);
        }

        public C1G2BlockWrite(uint password, C1G2MemoryBank bank, ushort pointer, short[] data) : base(LlrpParameterType.C1G2BlockWrite)
        {
            this.Init(password, bank, pointer, data);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.Password, 0x20, true);
            stream.Append((long) ((byte) this.Bank), 2, true);
            stream.Append((ulong) 0L, 6, true);
            stream.Append((long) this.Pointer, 0x10, true);
            stream.Append((long) this.m_data.Length, 0x10, true);
            foreach (short num in this.m_data)
            {
                stream.Append((long) num, 0x10, true);
            }
        }

        public short[] GetData()
        {
            return this.m_data;
        }

        private void Init(uint password, C1G2MemoryBank bank, ushort pointer, short[] data)
        {
            if ((data == null) || (data.Length == 0))
            {
                throw new ArgumentNullException("data");
            }
            this.m_password = password;
            this.m_bank = bank;
            this.m_pointer = pointer;
            this.m_data = data;
            this.ParameterLength = (uint) (0x48 + (this.m_data.Length * 0x10));
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
    }
}
