namespace Kalitte.Sensors.Rfid.Llrp.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Core;

    internal sealed class LlrpChannelListener<TChannel> : ChannelListenerBase<IDuplexChannel>
    {
        private BindingContext m_bindingContext;
        private Queue<TypedAsyncResult<IDuplexChannel>> m_callBacksToCall;
        private Queue<object> m_clientsAndException;
        private bool m_fCurrentlyListeningOnIPv4;
        private bool m_fCurrentlyListeningOnIPv6;
        private bool m_isClosed;
        private int m_localPort;
        private object m_lock;
        private ILogger m_logger;
        private TcpListener m_tcpListenerIPv4;
        private TcpListener m_tcpListenerIPv6;

        internal LlrpChannelListener(BindingContext context, int portToListenOn, ILogger logger)
        {
            this.m_callBacksToCall = new Queue<TypedAsyncResult<IDuplexChannel>>();
            this.m_clientsAndException = new Queue<object>();
            this.m_lock = new object();
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            if (portToListenOn <= 0)
            {
                throw new ArgumentOutOfRangeException("portToListenOn");
            }
            this.m_bindingContext = context;
            this.m_logger = logger;
            this.m_logger.Info("Using port {0} to listen for incoming Llrp connection", new object[] { portToListenOn });
            if (Util.HostSupportsIPv4())
            {
                this.m_logger.Info("Supports IPv4 for listening connection");
                this.m_tcpListenerIPv4 = new TcpListener(IPAddress.Any, portToListenOn);
            }
            if (Util.HostSupportsIPv6())
            {
                this.m_logger.Info("Supports IPv6 for listening connection");
                this.m_tcpListenerIPv6 = new TcpListener(IPAddress.IPv6Any, portToListenOn);
            }
            this.m_localPort = portToListenOn;
        }

        private void AddObjectToClientsQueue(object obj)
        {
            TcpClient item = obj as TcpClient;
            Exception exception = obj as Exception;
            lock (this.m_lock)
            {
                if (item != null)
                {
                    this.m_clientsAndException.Enqueue(item);
                }
                else if (exception != null)
                {
                    this.m_clientsAndException.Enqueue(exception);
                }
                else
                {
                    this.m_logger.Error("Not expected object {0} in the client queue", new object[] { obj });
                }
            }
        }

        private void CallCallbackIfRequired(bool completedSynchronously)
        {
            TypedAsyncResult<IDuplexChannel> result = null;
            TcpClient client = null;
            Exception exception = null;
            object obj2 = null;
            lock (this.m_lock)
            {
                if ((this.m_clientsAndException.Count > 0) && (this.m_callBacksToCall.Count > 0))
                {
                    result = this.m_callBacksToCall.Dequeue();
                    obj2 = this.m_clientsAndException.Dequeue();
                    client = obj2 as TcpClient;
                    exception = obj2 as Exception;
                }
            }
            if (result != null)
            {
                if (client != null)
                {
                    result.Complete(this.GetLlrpDuplexChannel(client), completedSynchronously);
                }
                else if (exception != null)
                {
                    result.Complete(completedSynchronously, exception);
                }
                else
                {
                    this.m_logger.Error("Invalid object in the client queue {0}", new object[] { obj2 });
                }
            }
        }

        private void CloseListener()
        {
            lock (this.m_lock)
            {
                if (this.m_isClosed)
                {
                    return;
                }
                this.m_isClosed = true;
            }
            if (this.m_tcpListenerIPv4 != null)
            {
                this.m_tcpListenerIPv4.Stop();
            }
            if (this.m_tcpListenerIPv6 != null)
            {
                this.m_tcpListenerIPv6.Stop();
            }
        }

        private LlrpDuplexChannel GetLlrpDuplexChannel(TcpClient client)
        {
            return new LlrpDuplexChannel(client, new EndpointAddress(this.GetUri(client.Client.RemoteEndPoint)), this.m_bindingContext, this.m_logger);
        }

        private string GetUri(EndPoint endPoint)
        {
            IPEndPoint point = endPoint as IPEndPoint;
            return Util.GetLlrpUriAddress(point.Address.ToString(), point.Port);
        }

        private void IPv4CallBack(IAsyncResult result)
        {
            lock (this.m_lock)
            {
                if (this.m_isClosed)
                {
                    this.m_logger.Info("Listener is closed, return from IPv4 call back");
                    return;
                }
            }
            try
            {
                if (this.m_tcpListenerIPv4 != null)
                {
                    TcpClient client = this.m_tcpListenerIPv4.EndAcceptTcpClient(result);
                    this.m_logger.Info("Accepted connection from IPv4 socket");
                    this.AddObjectToClientsQueue(client);
                }
            }
            catch (Exception exception)
            {
                this.m_logger.Error("Error {0} in ipv4 call back in listener class", new object[] { exception });
                this.AddObjectToClientsQueue(exception);
            }
            finally
            {
                lock (this.m_lock)
                {
                    this.m_fCurrentlyListeningOnIPv4 = false;
                }
            }
            this.CallCallbackIfRequired(false);
        }

        private void IPv6CallBack(IAsyncResult result)
        {
            lock (this.m_lock)
            {
                if (this.m_isClosed)
                {
                    this.m_logger.Info("Listener is closed, return from IPv6 call back");
                    return;
                }
            }
            try
            {
                if (this.m_tcpListenerIPv6 != null)
                {
                    TcpClient client = this.m_tcpListenerIPv6.EndAcceptTcpClient(result);
                    this.m_logger.Error("Accepted connection from IPv6 socket");
                    this.AddObjectToClientsQueue(client);
                }
            }
            catch (Exception exception)
            {
                this.m_logger.Info("Error {0} in ipv6 call back in listener class", new object[] { exception });
                this.AddObjectToClientsQueue(exception);
            }
            finally
            {
                lock (this.m_lock)
                {
                    this.m_fCurrentlyListeningOnIPv6 = false;
                }
            }
            this.CallCallbackIfRequired(false);
        }

        protected override void OnAbort()
        {
            this.CloseListener();
        }

        protected override IDuplexChannel OnAcceptChannel(TimeSpan timeout)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override IAsyncResult OnBeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            TypedAsyncResult<IDuplexChannel> item = null;
            lock (this.m_lock)
            {
                item = new TypedAsyncResult<IDuplexChannel>(callback, state);
                this.m_callBacksToCall.Enqueue(item);
            }
            if ((this.m_tcpListenerIPv4 != null) && !this.m_fCurrentlyListeningOnIPv4)
            {
                this.m_fCurrentlyListeningOnIPv4 = true;
                this.m_tcpListenerIPv4.BeginAcceptTcpClient(new AsyncCallback(this.IPv4CallBack), state);
            }
            if ((this.m_tcpListenerIPv6 != null) && !this.m_fCurrentlyListeningOnIPv6)
            {
                this.m_fCurrentlyListeningOnIPv6 = true;
                this.m_tcpListenerIPv6.BeginAcceptTcpClient(new AsyncCallback(this.IPv6CallBack), state);
            }
            this.CallCallbackIfRequired(true);
            return item;
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override IAsyncResult OnBeginWaitForChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void OnClose(TimeSpan timeout)
        {
            this.CloseListener();
        }

        protected override IDuplexChannel OnEndAcceptChannel(IAsyncResult result)
        {
            return TypedAsyncResult<IDuplexChannel>.End(result);
        }

        protected override void OnEndClose(IAsyncResult result)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool OnEndWaitForChannel(IAsyncResult result)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            if (this.m_tcpListenerIPv4 != null)
            {
                this.m_tcpListenerIPv4.Start();
            }
            if (this.m_tcpListenerIPv6 != null)
            {
                this.m_tcpListenerIPv6.Start();
            }
        }

        protected override bool OnWaitForChannel(TimeSpan timeout)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override System.Uri Uri
        {
            get
            {
                return new System.Uri(Util.GetLlrpUriAddress(Environment.MachineName, this.m_localPort));
            }
        }
    }
}
