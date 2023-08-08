namespace Kalitte.Sensors.Rfid.Llrp.Communication
{

    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid.Llrp;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Rfid.Llrp.Exceptions;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Security;

    internal class ReceiveMessageAsyncResult : TypedAsyncResult<Message>
    {
        private const int HeaderLength = 10;
        private uint m_bodyLength;
        private LlrpDuplexChannel m_duplexChannel;
        private byte[] m_headerBytes;
        private int m_length;
        private byte[] m_messageBytes;
        private int m_offset;

        public ReceiveMessageAsyncResult(AsyncCallback callBack, object state, LlrpDuplexChannel channel) : base(callBack, state)
        {
            this.m_duplexChannel = channel;
            this.ReadHeader();
        }

        private void EncodeData()
        {
            Message data = null;
            if (this.Logger.CurrentLevel == LogLevel.Verbose)
            {
                try
                {
                    string str = Util.ConvertByteArrayToHexString(this.m_messageBytes, 0, this.m_bodyLength + 10);
                    this.Logger.Verbose("Attempting to decode received message: \r\n{0}", new object[] { str });
                }
                catch (Exception exception)
                {
                    this.Logger.Warning("Hit an exception while logging the complete message: {0}", new object[] { exception });
                }
            }
            try
            {
                data = this.Channel.Encoder.ReadMessage(new ArraySegment<byte>(this.m_messageBytes, 0, ((int) this.m_bodyLength) + 10), null, null);
            }
            catch (ArgumentException exception2)
            {
                DecodingException exception3 = new DecodingException("Argument error while decoding", exception2.Message, exception2);
                exception3.MessageId = this.GetMessageId(this.m_messageBytes);
                exception3.MessageType = this.GetMessageType(this.m_messageBytes);
                base.Complete(false, exception3);
                return;
            }
            catch (DecodingException exception4)
            {
                try
                {
                    exception4.MessageId = this.GetMessageId(this.m_messageBytes);
                    exception4.MessageType = this.GetMessageType(this.m_messageBytes);
                }
                catch (Exception exception5)
                {
                    this.Logger.Warning("Error {0} during setting the message id or type of the decoding exception for device {1}. Ignoring this error", new object[] { exception5, this.Channel.RemoteAddress });
                }
                base.Complete(false, exception4);
                return;
            }
            finally
            {
                this.ReleaseBuffers();
            }
            base.Complete(data, false);
        }

        private object ExecuteCode(ReadDelegate code)
        {
            try
            {
                return code();
            }
            catch (DecodingException exception)
            {
                this.HandleReadMessageException(this, exception);
            }
            catch (ArgumentException exception2)
            {
                this.HandleReadMessageException(this, exception2);
            }
            catch (IOException exception3)
            {
                this.HandleReadMessageException(this, exception3);
            }
            catch (ObjectDisposedException exception4)
            {
                this.HandleReadMessageException(this, exception4);
            }
            catch (NotSupportedException exception5)
            {
                this.HandleReadMessageException(this, exception5);
            }
            catch (InvalidOperationException exception6)
            {
                this.HandleReadMessageException(this, exception6);
            }
            catch (ThreadAbortException exception7)
            {
                this.Logger.Error("ThreadAbortException during read {0}", new object[] { exception7 });
            }
            catch (Exception exception8)
            {
                this.Logger.Error("Unexpected exception during read {0} on device {1}", new object[] { exception8, this.Channel.RemoteAddress });
                if (exception8 is OutOfMemoryException)
                {
                    this.HandleReadMessageException(this, exception8);
                }
                else
                {
                    this.ReleaseBuffers();
                    base.Complete(false, exception8);
                }
            }
            return null;
        }

        private uint GetMessageId(byte[] messageHeaders)
        {
            int startingIndex = 6;
            int num2 = 4 + startingIndex;
            return this.GetNumber(messageHeaders, startingIndex, num2 - startingIndex);
        }

        private uint GetMessageLength(byte[] messageHeaders)
        {
            int startingIndex = 2;
            int num2 = 4 + startingIndex;
            return this.GetNumber(messageHeaders, startingIndex, num2 - startingIndex);
        }

        private LlrpMessageType GetMessageType(byte[] messageHeaders)
        {
            uint num = this.GetNumber(messageHeaders, 0, 2) & 0x3ff;
            try
            {
                return BitHelper.GetEnumInstance<LlrpMessageType>(num);
            }
            catch (DecodingException)
            {
                return LlrpMessageType.None;
            }
        }

        private uint GetNumber(byte[] bytes, int startingIndex, int length)
        {
            int num = startingIndex + length;
            uint num2 = 0;
            for (int i = startingIndex; i < num; i++)
            {
                num2 = num2 << 8;
                num2 += bytes[i];
            }
            return num2;
        }

        private void HandleReadMessageException(ReceiveMessageAsyncResult result, Exception ex)
        {
            if (!(ex is DecodingException))
            {
                this.Channel.State = CommunicationState.Faulted;
            }
            this.ReleaseBuffers();
            if (ex is ObjectDisposedException)
            {
                this.Logger.Info("Failed to receive LLRP message due to object disposed exception at the channel. Probably the connection has been closed for device {0}", new object[] { this.Channel.RemoteAddress });
                this.Logger.Verbose(ex.ToString());
            }
            else
            {
                this.Logger.Error("Error during read {0} on device {1}", new object[] { ex, this.Channel.RemoteAddress });
            }
            result.Complete(false, ex);
        }

        private void ReadBodyCallBack(IAsyncResult result)
        {
            ReadDelegate code = delegate {
                Stream stream = this.Channel.Stream;
                int num = stream.EndRead(result);
                this.Logger.Verbose("body bytes read {0} for device {1}", new object[] { this.Channel.RemoteAddress, num });
                if (num == 0)
                {
                    throw new IOException(LlrpResources.ConnectionClosed);
                }
                if (num < this.m_length)
                {
                    this.m_offset += num;
                    this.m_length -= num;
                    this.Logger.Verbose("Reading the remaining body bytes {0} for the device {1}", new object[] { this.m_length, this.Channel.RemoteAddress });
                    stream.BeginRead(this.m_messageBytes, this.m_offset, this.m_length, new AsyncCallback(this.ReadBodyCallBack), null);
                }
                else
                {
                    this.EncodeData();
                }
                return null;
            };
            this.ExecuteCode(code);
        }

        private void ReadHeader()
        {
            ReadDelegate code = delegate {
                this.Logger.Verbose("Reading the header for device {0}", new object[] { this.Channel.RemoteAddress });
                Stream stream = this.Channel.Stream;
                this.m_headerBytes = this.m_duplexChannel.BufferPool.GetBuffer(10);
                this.m_offset = 0;
                this.m_length = 10;
                stream.BeginRead(this.m_headerBytes, this.m_offset, this.m_length, new AsyncCallback(this.ReadHeaderCallBack), null);
                return null;
            };
            this.ExecuteCode(code);
        }

        private void ReadHeaderCallBack(IAsyncResult result)
        {
            ReadDelegate code = delegate {
                Stream stream = this.Channel.Stream;
                int num = stream.EndRead(result);
                this.Logger.Verbose("header bytes read {0} for device {1}", new object[] { num, this.Channel.RemoteAddress });
                if (num == 0)
                {
                    throw new IOException(LlrpResources.ConnectionClosed);
                }
                if (num < this.m_length)
                {
                    this.m_offset += num;
                    this.m_length -= num;
                    this.Logger.Verbose("Reading the remaining header bytes {0} for the device {1}", new object[] { this.m_length, this.Channel.RemoteAddress });
                    stream.BeginRead(this.m_headerBytes, this.m_offset, this.m_length, new AsyncCallback(this.ReadHeaderCallBack), null);
                }
                else
                {
                    uint messageLength = this.GetMessageLength(this.m_headerBytes);
                    if (messageLength < 10)
                    {
                        throw new DecodingException("Invalid Message", LlrpResources.InvalidMessageHeaderReceived);
                    }
                    this.m_messageBytes = this.m_duplexChannel.BufferPool.GetBuffer((int) messageLength);
                    Array.Copy(this.m_headerBytes, this.m_messageBytes, 10);
                    this.m_bodyLength = messageLength - 10;
                    this.Logger.Verbose("body length {0} for device {1}", new object[] { this.m_bodyLength, this.Channel.RemoteAddress });
                    this.m_offset = 10;
                    this.m_length = (int) this.m_bodyLength;
                    if (this.m_bodyLength > 0)
                    {
                        stream.BeginRead(this.m_messageBytes, this.m_offset, this.m_length, new AsyncCallback(this.ReadBodyCallBack), null);
                    }
                    else if (this.m_bodyLength == 0)
                    {
                        this.EncodeData();
                    }
                }
                return null;
            };
            this.ExecuteCode(code);
        }

        private void ReleaseBuffers()
        {
            if (this.m_headerBytes != null)
            {
                this.m_duplexChannel.BufferPool.ReturnBuffer(this.m_headerBytes);
            }
            if (this.m_messageBytes != null)
            {
                this.m_duplexChannel.BufferPool.ReturnBuffer(this.m_messageBytes);
            }
        }

        internal LlrpDuplexChannel Channel
        {
            get
            {
                return this.m_duplexChannel;
            }
        }

        private ILogger Logger
        {
            get
            {
                return this.Channel.Logger;
            }
        }

        private delegate object ReadDelegate();
    }
}
