namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Xml;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Exceptions;
    using Kalitte.Sensors.Rfid.Llrp.Exceptions;
    using Kalitte.Sensors.Rfid.Llrp.Communication;

    public abstract class LlrpMessageBase : Message
    {
        internal const uint HeaderLength = 80;
        private uint m_messageId;
        private ulong m_messageLength;
        private LlrpMessageType m_messageType;
        private byte m_messageVersion;

        protected LlrpMessageBase(LlrpMessageType type) : this(type, IdGenerator.GenerateLlrpMessageId())
        {
        }

        protected internal LlrpMessageBase(LlrpMessageType type, BitArray bitArray)
        {
            if (bitArray.Count < 80L)
            {
                throw new DecodingException("Incomplete Message", string.Format(CultureInfo.CurrentCulture, LlrpResources.InCompleteMessageWithName, new object[] { base.GetType().FullName }));
            }
            int startingIndex = 0;
            startingIndex += 3;
            byte version = (byte) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 3);
            if (version != 1)
            {
                throw new DecodingException("Invalid Version", string.Format(CultureInfo.CurrentCulture, LlrpResources.InvalidMessageVersion, new object[] { version }));
            }
            LlrpMessageType enumInstance = BitHelper.GetEnumInstance<LlrpMessageType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 10));
            if (!enumInstance.Equals(type))
            {
                throw new DecodingException("Invalid Message", string.Format(CultureInfo.CurrentCulture, LlrpResources.UnMatchedMessageType, new object[] { this.MessageType, enumInstance }));
            }
            this.m_messageType = enumInstance;
            uint num3 = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 0x20);
            if (bitArray.Count < (num3 * 8))
            {
                throw new DecodingException("Incomplete Message", string.Format(CultureInfo.CurrentCulture, LlrpResources.InCompleteMessageWithName, new object[] { base.GetType().FullName }));
            }
            uint id = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 0x20);
            this.Initialize(enumInstance, id, num3 * 8, version);
        }

        protected LlrpMessageBase(LlrpMessageType messageType, uint messageId)
        {
            this.m_messageType = messageType;
            this.m_messageId = messageId;
            this.m_messageVersion = 1;
        }

        internal virtual LLRPMessageStream CreateHeaderStream()
        {
            LLRPMessageStream stream = new LLRPMessageStream();
            stream.Append((ulong) 0L, 3, true);
            stream.Append((long) 1L, 3, true);
            stream.Append((long) ((ushort) this.MessageType), 10, true);
            stream.Append((ulong) (this.MessageLength / ((ulong) 8L)), 0x20, true);
            stream.Append((long) this.Id, 0x20, true);
            return stream;
        }


        internal abstract byte[] Encode();
        internal static LlrpMessageType GetMessageType(BitArray bitArray)
        {
            if (bitArray.Count < 80L)
            {
                throw new DecodingException("Incomplete Message", LlrpResources.InCompleteMessage);
            }
            int startingIndex = 6;
            return BitHelper.GetEnumInstance<LlrpMessageType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 10));
        }

        private static LlrpMessageBase GetResponseMessage(byte[] message, string messageName)
        {
            LlrpMessageBase base3;
            try
            {
                LlrpMessageBase base2 = LlrpBinaryEncoder.GetLlrpMessage(message, (uint) message.Length, null);
                if (base2 == null)
                {
                    throw new SensorProviderException(string.Format(CultureInfo.CurrentCulture, LlrpResources.UnknownLlrpMessage, new object[] { messageName }));
                }
                base3 = base2;
            }
            catch (SensorProviderException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new SensorProviderException(string.Format(CultureInfo.CurrentCulture, LlrpResources.ErrorDuringCreatingLlrpMessage, new object[] { exception.Message }));
            }
            return base3;
        }

        private void Initialize(LlrpMessageType type, uint id, uint length, byte version)
        {
            this.m_messageType = type;
            this.m_messageId = id;
            this.m_messageLength = length;
            this.m_messageVersion = version;
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<MessageVersion>");
            builder.Append(this.MessageVersion);
            builder.Append("</MessageVersion>");
            builder.Append("<MessageLength>");
            builder.Append(this.MessageLength);
            builder.Append("</MessageLength>");
            builder.Append("<MessageType>");
            builder.Append(this.MessageType);
            builder.Append("</MessageType>");
            builder.Append("<MessageId>");
            builder.Append(this.Id);
            builder.Append("</MessageId>");
            return builder.ToString();
        }

        public override MessageHeaders Headers
        {
            get
            {
                return null;
            }
        }

        public uint Id
        {
            get
            {
                return this.m_messageId;
            }
        }

        internal virtual ulong MessageLength
        {
            get
            {
                return this.m_messageLength;
            }
            set
            {
                if ((value / ((ulong) 8L)) > 0xffffffffL)
                {
                    throw new ArgumentOutOfRangeException("MessageLength", string.Format(CultureInfo.CurrentCulture, LlrpResources.MessageLengthExceeded, new object[] { uint.MaxValue }));
                }
                this.m_messageLength = value + ((ulong) 80L);
            }
        }

        internal LlrpMessageType MessageType
        {
            get
            {
                return this.m_messageType;
            }
        }

        public byte MessageVersion
        {
            get
            {
                return this.m_messageVersion;
            }
        }

        public override MessageProperties Properties
        {
            get
            {
                return null;
            }
        }

        public override System.ServiceModel.Channels.MessageVersion Version
        {
            get
            {
                return null;
            }
        }
    }
}
