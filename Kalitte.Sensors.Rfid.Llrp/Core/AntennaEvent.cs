namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    
    
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Events.Management;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Events;


    public sealed class AntennaEvent : LlrpEvent
    {
        private ushort m_antennaId;
        private AntennaEventType m_eventType;

        public AntennaEvent(AntennaEventType type, ushort antennaId) : base(LlrpParameterType.AntennaEvent)
        {
            this.Init(type, antennaId);
        }

        internal AntennaEvent(BitArray bitArray, ref int index) : base(bitArray, index, LlrpParameterType.AntennaEvent)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            AntennaEventType enumInstance = BitHelper.GetEnumInstance<AntennaEventType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            ushort antennaId = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, antennaId);
        }


        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.EventType), 8, true);
            stream.Append((long) this.AntennaId, 0x10, true);
        }

        private void Init(AntennaEventType type, ushort antennaId)
        {
            this.m_eventType = type;
            this.m_antennaId = antennaId;
            this.ParameterLength = 0x18;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Antenna Event>");
            builder.Append(base.ToString());
            builder.Append("<Antenna Id>");
            builder.Append(this.AntennaId);
            builder.Append("</RO Antenna Id>");
            builder.Append("<Type>");
            builder.Append(this.EventType);
            builder.Append("</Type>");
            builder.Append("</Antenna Event>");
            return builder.ToString();
        }

        public ushort AntennaId
        {
            get
            {
                return this.m_antennaId;
            }
        }

        public AntennaEventType EventType
        {
            get
            {
                return this.m_eventType;
            }
        }

        internal override Notification ConvertToRfidNotification()
        {
            string antennaName = Util.GetAntennaName(this.AntennaId);
            ManagementEvent managementEvent = null;
            if (this.EventType == AntennaEventType.AntennaConnected)
            {
                managementEvent = new SourceUpEvent(Kalitte.Sensors.Events.Management.EventType.SourceUp.Description, antennaName);
            }
            else if (this.EventType == AntennaEventType.AntennaDisconnected)
            {
                managementEvent = new SourceDownEvent(Kalitte.Sensors.Events.Management.EventType.SourceDown.Description, antennaName);
            }
            if (managementEvent != null)
            {
                return new Notification(managementEvent);
            }
            return null;
        }

 

 

    }
}
