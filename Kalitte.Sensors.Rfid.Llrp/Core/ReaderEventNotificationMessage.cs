namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class ReaderEventNotificationMessage : LlrpMessageBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.ReaderEventNotificationData m_notificationData;

        public ReaderEventNotificationMessage(Kalitte.Sensors.Rfid.Llrp.Core.ReaderEventNotificationData notificationData) : base(LlrpMessageType.ReaderEventNotification, IdGenerator.GenerateLlrpMessageId())
        {
            this.Init(notificationData);
        }

        internal ReaderEventNotificationMessage(BitArray bitArray) : base(LlrpMessageType.ReaderEventNotification, bitArray)
        {
            int index = 80;
            Kalitte.Sensors.Rfid.Llrp.Core.ReaderEventNotificationData notificationData = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ReaderEventNotificationData, bitArray, index))
            {
                notificationData = new Kalitte.Sensors.Rfid.Llrp.Core.ReaderEventNotificationData(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, (uint) bitArray.Count, base.GetType().FullName);
            this.Init(notificationData);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = this.CreateHeaderStream();
            Util.Encode(this.m_notificationData, stream);
            return stream.Merge();
        }

        private void Init(Kalitte.Sensors.Rfid.Llrp.Core.ReaderEventNotificationData notificationData)
        {
            if (notificationData == null)
            {
                throw new ArgumentNullException("notificationData");
            }
            this.m_notificationData = notificationData;
            this.MessageLength = this.ReaderEventNotificationData.ParameterLength;
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Reader Event Notification Message>");
            strBuilder.Append(base.ToString());
            Util.ToString(this.ReaderEventNotificationData, strBuilder);
            strBuilder.Append("</Reader Event Notification Message>");
            return strBuilder.ToString();
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.ReaderEventNotificationData ReaderEventNotificationData
        {
            get
            {
                return this.m_notificationData;
            }
        }
    }
}
