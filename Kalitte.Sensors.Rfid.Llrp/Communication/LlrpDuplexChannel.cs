namespace Kalitte.Sensors.Rfid.Llrp.Communication
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Core;

    internal sealed class LlrpDuplexChannel : IDuplexChannel, IInputChannel, IOutputChannel, IChannel, ICommunicationObject, IDisposable
    {
        private bool disposed;
        private EndpointAddress m_address;
        private BufferPool m_bufferPool;
        private CommunicationState m_currentState;
        private object m_currentStateLock;
        private MessageEncoder m_encoder;
        private ILogger m_logger;
        private TcpClient m_tcpClient;

        public event EventHandler Closed;

        public event EventHandler Closing;

        public event EventHandler Faulted;

        public event EventHandler Opened;

        public event EventHandler Opening;

        private LlrpDuplexChannel(BindingContext context, ILogger logger)
        {
            this.m_currentStateLock = new object();
            this.m_bufferPool = new BufferPool();
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            Collection<MessageEncodingBindingElement> collection = context.BindingParameters.FindAll<MessageEncodingBindingElement>();
            if ((collection == null) || (collection.Count == 0))
            {
                this.m_encoder = new LlrpBinaryEncoder(logger);
            }
            else
            {
                MessageEncoderFactory factory = collection[0].CreateMessageEncoderFactory();
                if (factory == null)
                {
                    throw new ArgumentNullException(LlrpResources.MessageEncoderFactoryIsNull);
                }
                this.m_encoder = factory.Encoder;
                if (this.m_encoder == null)
                {
                    throw new ArgumentNullException(LlrpResources.MessageEncoderIsNull);
                }
            }
            this.m_logger = logger;
        }

        public LlrpDuplexChannel(EndpointAddress address, BindingContext context, ILogger logger) : this(context, logger)
        {
            if (null == address)
            {
                throw new ArgumentNullException("address");
            }
            this.m_address = address;
            this.m_tcpClient = new TcpClient();
            this.Logger.Info("Duplex channel intialized with end point {0}", new object[] { this.m_address });
        }

        public LlrpDuplexChannel(TcpClient client, EndpointAddress address, BindingContext context, ILogger logger) : this(address, context, logger)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            if (!client.Connected)
            {
                throw new ArgumentException(LlrpResources.TcpClientNotConnected);
            }
            this.m_tcpClient = client;
            this.SetTcpKeepAlives(this.m_tcpClient);
            this.State = CommunicationState.Opened;
            this.Logger.Info("Duplex channel intialized for reader intiated connection with end point {0}", new object[] { this.m_address });
        }

        public void Abort()
        {
            this.State = CommunicationState.Closed;
        }

        public IAsyncResult BeginClose(AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public IAsyncResult BeginOpen(AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public IAsyncResult BeginReceive(AsyncCallback callback, object state)
        {
            this.ThrowIfDisposed();
            return new ReceiveMessageAsyncResult(callback, state, this);
        }

        public IAsyncResult BeginReceive(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public IAsyncResult BeginSend(Message message, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public IAsyncResult BeginSend(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public IAsyncResult BeginTryReceive(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public IAsyncResult BeginWaitForMessage(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public void Close()
        {
            this.ThrowIfDisposed();
            lock (this.m_currentStateLock)
            {
                if (this.State != CommunicationState.Closed)
                {
                    this.State = CommunicationState.Closing;
                    try
                    {
                        if (this.Stream != null)
                        {
                            this.Stream.Close();
                        }
                    }
                    catch (Exception exception)
                    {
                        this.Logger.Error("Encountered exception {0} while disposing LLRP duplex channel (Closing the stream)", new object[] { exception });
                    }
                    try
                    {
                        this.Client.Close();
                    }
                    catch (Exception exception2)
                    {
                        this.Logger.Error("Exception {0} encountered while closing the TCP connection for device {1}", new object[] { exception2, this.m_address });
                    }
                    this.State = CommunicationState.Closed;
                }
            }
        }

        public void Close(TimeSpan timeout)
        {
            this.Close();
        }

        public void Dispose()
        {
            lock (this.m_currentStateLock)
            {
                if (!this.disposed)
                {
                    this.Close();
                    if (this.m_tcpClient != null)
                    {
                        try
                        {
                            ((IDisposable) this.m_tcpClient).Dispose();
                        }
                        catch (Exception exception)
                        {
                            this.Logger.Error("Error {0} during disposing the tcp client in the duplex channel for the device {1}", new object[] { exception, this.m_address });
                        }
                    }
                    this.disposed = true;
                }
            }
        }

        public void EndClose(IAsyncResult result)
        {
            throw new NotSupportedException();
        }

        public void EndOpen(IAsyncResult result)
        {
            throw new NotSupportedException();
        }

        public Message EndReceive(IAsyncResult result)
        {
            this.ThrowIfDisposed();
            return TypedAsyncResult<Message>.End(result);
        }

        public void EndSend(IAsyncResult result)
        {
            throw new NotSupportedException();
        }

        public bool EndTryReceive(IAsyncResult result, out Message message)
        {
            throw new NotSupportedException();
        }

        public bool EndWaitForMessage(IAsyncResult result)
        {
            throw new NotSupportedException();
        }

        public T GetProperty<T>() where T: class
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private void HandleReceiveException(Exception ex)
        {
            this.Logger.Error("Encountered exception {0} in WCF channel while trying to read message from device {1}", new object[] { ex, this.m_address });
            this.State = CommunicationState.Faulted;
        }

        private void HandleSendException(Exception ex)
        {
            this.Logger.Error("Encountered exception {0} in WCF channel while trying to send message to device {1}", new object[] { ex, this.m_address });
            this.State = CommunicationState.Faulted;
        }

        public void Open()
        {
            lock (this.m_currentStateLock)
            {
                if (this.State == CommunicationState.Created)
                {
                    this.State = CommunicationState.Opening;
                    this.m_tcpClient = new TcpClient(this.m_address.Uri.Host, this.m_address.Uri.Port);
                    this.SetTcpKeepAlives(this.m_tcpClient);
                    this.State = CommunicationState.Opened;
                }
                else
                {
                    this.State = CommunicationState.Faulted;
                }
            }
        }

        public void Open(TimeSpan timeout)
        {
            throw new NotSupportedException();
        }

        public Message Receive()
        {
            Message message;
            this.ThrowIfDisposed();
            try
            {
                IAsyncResult result = this.BeginReceive(null, null);
                message = this.EndReceive(result);
            }
            catch (ObjectDisposedException exception)
            {
                this.HandleReceiveException(exception);
                throw;
            }
            catch (InvalidOperationException exception2)
            {
                this.HandleReceiveException(exception2);
                throw;
            }
            catch (IOException exception3)
            {
                this.HandleReceiveException(exception3);
                throw;
            }
            catch (NotSupportedException exception4)
            {
                this.HandleReceiveException(exception4);
                throw;
            }
            return message;
        }

        public Message Receive(TimeSpan timeout)
        {
            throw new NotSupportedException();
        }

        public void Send(Message message)
        {
            try
            {
                this.Encoder.WriteMessage(message, this.Stream);
            }
            catch (ObjectDisposedException exception)
            {
                this.HandleSendException(exception);
                throw;
            }
            catch (InvalidOperationException exception2)
            {
                this.HandleSendException(exception2);
                throw;
            }
            catch (IOException exception3)
            {
                this.HandleSendException(exception3);
                throw;
            }
            catch (NotSupportedException exception4)
            {
                this.HandleSendException(exception4);
                throw;
            }
        }

        public void Send(Message message, TimeSpan timeout)
        {
            throw new NotSupportedException();
        }

        private void SetTcpKeepAlives(TcpClient tcpClient)
        {
            try
            {
                int tcpKeepAliveTime = 5000;
                int num2 = 0x3e8;
                byte[] optionInValue = new byte[12];
                optionInValue[0] = 1;
                optionInValue[4] = (byte) (tcpKeepAliveTime & 0xff);
                optionInValue[5] = (byte) ((tcpKeepAliveTime >> 8) & 0xff);
                optionInValue[6] = (byte) ((tcpKeepAliveTime >> 0x10) & 0xff);
                optionInValue[7] = (byte) ((tcpKeepAliveTime >> 0x18) & 0xff);
                optionInValue[8] = (byte) (num2 & 0xff);
                optionInValue[9] = (byte) ((num2 >> 8) & 0xff);
                optionInValue[10] = (byte) ((num2 >> 0x10) & 0xff);
                optionInValue[11] = (byte) ((num2 >> 0x18) & 0xff);
                tcpClient.Client.IOControl(IOControlCode.KeepAliveValues, optionInValue, null);
            }
            catch (Exception exception)
            {
                this.Logger.Error("Setting keep alive failed {0}", new object[] { exception });
            }
        }

        private void ThrowIfDisposed()
        {
            lock (this.m_currentStateLock)
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("TcpClient");
                }
            }
        }

        public bool TryReceive(TimeSpan timeout, out Message message)
        {
            throw new NotSupportedException();
        }

        public bool WaitForMessage(TimeSpan timeout)
        {
            throw new NotSupportedException();
        }

        internal BufferPool BufferPool
        {
            get
            {
                return this.m_bufferPool;
            }
        }

        private TcpClient Client
        {
            get
            {
                lock (this.m_currentStateLock)
                {
                    this.ThrowIfDisposed();
                    return this.m_tcpClient;
                }
            }
        }

        internal MessageEncoder Encoder
        {
            get
            {
                return this.m_encoder;
            }
        }

        public EndpointAddress LocalAddress
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        internal ILogger Logger
        {
            get
            {
                return this.m_logger;
            }
        }

        public EndpointAddress RemoteAddress
        {
            get
            {
                return this.m_address;
            }
        }

        public CommunicationState State
        {
            get
            {
                lock (this.m_currentStateLock)
                {
                    return this.m_currentState;
                }
            }
            internal set
            {
                lock (this.m_currentStateLock)
                {
                    this.m_currentState = value;
                }
                switch (value)
                {
                    case CommunicationState.Opening:
                    {
                        EventHandler opening = this.Opening;
                        if (opening == null)
                        {
                            break;
                        }
                        opening(this, EventArgs.Empty);
                        return;
                    }
                    case CommunicationState.Opened:
                    {
                        EventHandler opened = this.Opened;
                        if (opened == null)
                        {
                            break;
                        }
                        opened(this, EventArgs.Empty);
                        return;
                    }
                    case CommunicationState.Closing:
                    {
                        EventHandler closing = this.Closing;
                        if (closing == null)
                        {
                            break;
                        }
                        closing(this, EventArgs.Empty);
                        return;
                    }
                    case CommunicationState.Closed:
                    {
                        EventHandler closed = this.Closed;
                        if (closed == null)
                        {
                            break;
                        }
                        closed(this, EventArgs.Empty);
                        return;
                    }
                    case CommunicationState.Faulted:
                    {
                        EventHandler faulted = this.Faulted;
                        if (faulted != null)
                        {
                            faulted(this, EventArgs.Empty);
                        }
                        break;
                    }
                    default:
                        return;
                }
            }
        }

        internal NetworkStream Stream
        {
            get
            {
                return this.Client.GetStream();
            }
        }

        public Uri Via
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }
}
