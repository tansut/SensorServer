namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    
    
    using System;
    using System.Collections;
    
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Events.Management;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Llrp.Events;
    using Kalitte.Sensors.Events;

    public sealed class RFSurveyEvent : LlrpEvent
    {
        private RFSurveyEventType m_eventType;
        private uint m_roSpecId;
        private ushort m_specIndex;

        internal override Notification ConvertToRfidNotification()
        {
            VendorData vendorData = new VendorData();
            vendorData.Add("RO Spec Id", this.ROSpecId);
            vendorData.Add("Spec Index", this.SpecIndex);
            vendorData.Add("Event Type", this.EventType.ToString());
            return new Notification(new VendorDefinedManagementEvent(EventLevel.Info, LlrpEventTypes.RFSurveyEvent, LlrpEventTypes.RFSurveyEvent.Description, typeof(RFSurveyEvent).Name, vendorData));
        }

 

 


        internal RFSurveyEvent(BitArray bitArray, ref int index) : base(bitArray, index, LlrpParameterType.RFSurveyEvent)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            RFSurveyEventType enumInstance = BitHelper.GetEnumInstance<RFSurveyEventType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            uint roSpecId = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            ushort specIndex = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, roSpecId, specIndex);
        }



        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.EventType), 8, true);
            stream.Append((long) this.ROSpecId, 0x20, true);
            stream.Append((long) this.SpecIndex, 0x10, true);
        }

        private void Init(RFSurveyEventType type, uint roSpecId, ushort specIndex)
        {
            this.m_eventType = type;
            this.m_roSpecId = roSpecId;
            this.m_specIndex = specIndex;
            this.ParameterLength = 0x38;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<RF Survey Event>");
            builder.Append(base.ToString());
            builder.Append("<RO Spec Id>");
            builder.Append(this.ROSpecId);
            builder.Append("</RO Spec Id>");
            builder.Append("<Spec Index>");
            builder.Append(this.SpecIndex);
            builder.Append("</Spec Index>");
            builder.Append("<Type>");
            builder.Append(this.EventType);
            builder.Append("</Type>");
            builder.Append("</RF Survey Event>");
            return builder.ToString();
        }

        public RFSurveyEventType EventType
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
    }
}
