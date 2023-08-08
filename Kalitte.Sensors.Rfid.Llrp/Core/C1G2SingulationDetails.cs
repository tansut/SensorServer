namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class C1G2SingulationDetails : AirProtocolSingulationDetails
    {
        private ushort m_numberOfCollisonSlots;
        private ushort m_numberOfEmptySlots;

        internal C1G2SingulationDetails(BitArray bitArray, ref int index) : base(bitArray, index, LlrpParameterType.C1G2SingulationDetails)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort collisionSlots = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort emptySlots = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(collisionSlots, emptySlots);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.NumberOfCollisionSlots, 0x10, true);
            stream.Append((long) this.NumberOfEmptySlots, 0x10, true);
        }

        private void Init(ushort collisionSlots, ushort emptySlots)
        {
            this.m_numberOfCollisonSlots = collisionSlots;
            this.m_numberOfEmptySlots = emptySlots;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<C1G2 singulation details>");
            builder.Append("<Number of collision slots>");
            builder.Append(this.NumberOfCollisionSlots);
            builder.Append("</Number of collision slots>");
            builder.Append("<Number of empty slots>");
            builder.Append(this.NumberOfEmptySlots);
            builder.Append("</Number of empty slots>");
            builder.Append("</C1G2 singulation details>");
            return builder.ToString();
        }

        public ushort NumberOfCollisionSlots
        {
            get
            {
                return this.m_numberOfCollisonSlots;
            }
        }

        public ushort NumberOfEmptySlots
        {
            get
            {
                return this.m_numberOfEmptySlots;
            }
        }
    }
}
