namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Events;

    public sealed class AISpecEvent : LlrpEvent
    {
        private AISpecEventType m_eventType;
        private uint m_roSpecId;
        private Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolSingulationDetails m_singulationDetails;
        private ushort m_specIndex;

        internal AISpecEvent(BitArray bitArray, ref int index) : base(bitArray, index, LlrpParameterType.AISpecEvent)
        {
            LlrpParameterType type2;
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            AISpecEventType enumInstance = BitHelper.GetEnumInstance<AISpecEventType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            uint roSpecId = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            ushort specIndex = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            Collection<LlrpParameterType> expectedTypes = new Collection<LlrpParameterType>();
            expectedTypes.Add(LlrpParameterType.C1G2SingulationDetails);
            Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolSingulationDetails singulationDetails = null;
            if (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, out type2))
            {
                switch (type2)
                {
                    case LlrpParameterType.C1G2SingulationDetails:
                    {
                        singulationDetails = new C1G2SingulationDetails(bitArray, ref index);
                        break;
                    }
                }
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, roSpecId, specIndex, singulationDetails);
        }


        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.EventType), 8, true);
            stream.Append((long) this.ROSpecId, 0x20, true);
            stream.Append((long) this.SpecIndex, 0x10, true);
            Util.Encode(this.AirProtocolSingulationDetails, stream);
        }

        private void Init(AISpecEventType type, uint roSpecId, ushort specIndex, Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolSingulationDetails singulationDetails)
        {
            this.m_eventType = type;
            this.m_roSpecId = roSpecId;
            this.m_specIndex = specIndex;
            this.m_singulationDetails = singulationDetails;
            this.ParameterLength = 0x38 + Util.GetBitLengthOfParam(this.AirProtocolSingulationDetails);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<AI Spec Event>");
            strBuilder.Append(base.ToString());
            strBuilder.Append("<RO Spec Id>");
            strBuilder.Append(this.ROSpecId);
            strBuilder.Append("</RO Spec Id>");
            strBuilder.Append("<Spec Index>");
            strBuilder.Append(this.SpecIndex);
            strBuilder.Append("</Spec Index>");
            strBuilder.Append("<Type>");
            strBuilder.Append(this.EventType);
            strBuilder.Append("</Type>");
            Util.ToString(this.AirProtocolSingulationDetails, strBuilder);
            strBuilder.Append("</AI Spec Event>");
            return strBuilder.ToString();
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolSingulationDetails AirProtocolSingulationDetails
        {
            get
            {
                return this.m_singulationDetails;
            }
        }

        public AISpecEventType EventType
        {
            get
            {
                return this.m_eventType;
            }
        }

        public uint ROSpecId
        {
            get
            {
                return this.m_roSpecId;
            }
        }

        public ushort SpecIndex
        {
            get
            {
                return this.m_specIndex;
            }
        }

        internal override Notification ConvertToRfidNotification()
        {
            return null;
        }

 

    }
}
