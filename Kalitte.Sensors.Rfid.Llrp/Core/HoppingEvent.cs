namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    
    
    using System;
    using System.Collections;
    
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Events.Management;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Llrp.Events;
    using Kalitte.Sensors.Events;

    public sealed class HoppingEvent : LlrpEvent
    {
        private ushort m_hopTableId;
        private ushort m_nextChannelIndex;

        internal override Notification ConvertToRfidNotification()
        {
            VendorData vendorData = new VendorData();
            vendorData.Add("Hop Table Id", this.HopTableId);
            vendorData.Add("Next Channel Index", this.NextChannelIndex);
            return new Notification(new VendorDefinedManagementEvent(EventLevel.Info, LlrpEventTypes.HoppingEvent, LlrpEventTypes.HoppingEvent.Description, typeof(HoppingEvent).Name, vendorData));
        }

 

 


        internal HoppingEvent(BitArray bitArray, ref int index) : base(bitArray, index, LlrpParameterType.HoppingEvent)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort tableId = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort nextChannelIndex = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(tableId, nextChannelIndex);
        }


        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.HopTableId, 0x10, true);
            stream.Append((long) this.NextChannelIndex, 0x10, true);
        }

        private void Init(ushort tableId, ushort nextChannelIndex)
        {
            this.m_hopTableId = tableId;
            this.m_nextChannelIndex = nextChannelIndex;
            this.ParameterLength = 0x20;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Hopping event>");
            builder.Append(base.ToString());
            builder.Append("<Table Id>");
            builder.Append(this.HopTableId);
            builder.Append("</Table Id>");
            builder.Append("<Next Channel Index>");
            builder.Append(this.NextChannelIndex);
            builder.Append("</Next Channel Index>");
            builder.Append("</Hopping event>");
            return builder.ToString();
        }

        public ushort HopTableId
        {
            get
            {
                return this.m_hopTableId;
            }
        }

        public ushort NextChannelIndex
        {
            get
            {
                return this.m_nextChannelIndex;
            }
        }
    }
}
