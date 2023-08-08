namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class C1G2SingulationControl : LlrpTlvParameterBase
    {
        private C1G2TagInventoryStateAwareSingulationAction m_action;
        private Kalitte.Sensors.Rfid.Llrp.Core.TagSession m_session;
        private ushort m_tagPopulation;
        private uint m_tagTransitTime;

        internal C1G2SingulationControl(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2SingulationControl, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            C1G2TagInventoryStateAwareSingulationAction action = null;
            Kalitte.Sensors.Rfid.Llrp.Core.TagSession enumInstance = BitHelper.GetEnumInstance<Kalitte.Sensors.Rfid.Llrp.Core.TagSession>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 2));
            index += 6;
            ushort tagPopulation = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            uint tagTransitTime = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.C1G2TagInventoryStateAwareSingulationAction, bitArray, index, parameterEndLimit))
            {
                action = new C1G2TagInventoryStateAwareSingulationAction(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, tagPopulation, tagTransitTime, action);
        }

        public C1G2SingulationControl(Kalitte.Sensors.Rfid.Llrp.Core.TagSession session, ushort tagPopulation, uint tagTransitTime, C1G2TagInventoryStateAwareSingulationAction action) : base(LlrpParameterType.C1G2SingulationControl)
        {
            this.Init(session, tagPopulation, tagTransitTime, action);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.TagSession), 2, true);
            stream.Append((ulong) 0L, 6, true);
            stream.Append((long) this.TagPopulation, 0x10, true);
            stream.Append((long) this.TagTransitTime, 0x20, true);
            Util.Encode(this.Action, stream);
        }

        private void Init(Kalitte.Sensors.Rfid.Llrp.Core.TagSession session, ushort tagPopulation, uint tagTransitTime, C1G2TagInventoryStateAwareSingulationAction action)
        {
            this.m_session = session;
            this.m_tagPopulation = tagPopulation;
            this.m_tagTransitTime = tagTransitTime;
            this.m_action = action;
            this.ParameterLength = 0x38 + Util.GetBitLengthOfParam(this.Action);
        }

        public C1G2TagInventoryStateAwareSingulationAction Action
        {
            get
            {
                return this.m_action;
            }
        }

        public ushort TagPopulation
        {
            get
            {
                return this.m_tagPopulation;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.TagSession TagSession
        {
            get
            {
                return this.m_session;
            }
        }

        public uint TagTransitTime
        {
            get
            {
                return this.m_tagTransitTime;
            }
        }
    }
}
