namespace Kalitte.Sensors.Rfid.Llrp.Communication
{

    using System;
    using System.IO;
    using System.ServiceModel.Channels;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Security;

    internal abstract class LlrpBinaryEncoderBase : MessageEncoder
    {
        protected ILogger m_logger;
        private object m_writeLock = new object();

        internal LlrpBinaryEncoderBase(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            this.m_logger = logger;
        }

        protected abstract Message GetMessage(byte[] message, uint length);

        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            return this.GetMessage(buffer.Array, (uint) buffer.Count);
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            throw new NotSupportedException();
        }

        public override void WriteMessage(Message message, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            byte[] input = (message as LlrpMessageBase).Encode();
            if (this.m_logger.CurrentLevel == LogLevel.Verbose)
            {
                try
                {
                    this.m_logger.Verbose("Sending message: \r\n{0}", new object[] { Util.ConvertByteArrayToHexString(input, 0, (uint) input.Length) });
                }
                catch (Exception exception)
                {
                    this.m_logger.Warning("Hit an exception while logging the complete message: {0}", new object[] { exception });
                }
            }
            lock (this.m_writeLock)
            {
                stream.Write(input, 0, input.Length);
            }
        }

        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            throw new NotSupportedException();
        }

        public override string ContentType
        {
            get
            {
                return "llrp.bin";
            }
        }

        public override string MediaType
        {
            get
            {
                return this.ContentType;
            }
        }

        public override System.ServiceModel.Channels.MessageVersion MessageVersion
        {
            get
            {
                return System.ServiceModel.Channels.MessageVersion.None;
            }
        }
    }
}
