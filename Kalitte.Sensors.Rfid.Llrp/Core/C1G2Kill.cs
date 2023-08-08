namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class C1G2Kill : C1G2OPSpec
    {
        private uint m_password;

        public C1G2Kill(uint password) : base(LlrpParameterType.C1G2Kill)
        {
            this.Init(password);
        }

        internal C1G2Kill(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2Kill, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            index += OPSpec.BaseLength;
            uint password = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(password);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.KillPassword, 0x20, true);
        }

        private void Init(uint password)
        {
            this.m_password = password;
            this.ParameterLength = 0x20;
        }

        public uint KillPassword
        {
            get
            {
                return this.m_password;
            }
        }
    }
}
