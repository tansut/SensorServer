namespace Kalitte.Sensors.Rfid.Llrp.PhysicalDevices
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading;
    using Kalitte.Sensors.Communication;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp;
    using Kalitte.Sensors.Exceptions;
    using Kalitte.Sensors.Rfid.Llrp.Exceptions;
    using Kalitte.Sensors.Rfid.Llrp.Events;
    using Kalitte.Sensors.Core;

    internal sealed class LlrpDevice
    {
        private IDuplexChannel m_channel;
        private object m_connectionAttemptLock;
        private ManualResetEvent m_connectionAttemptWaitEventHandle;
        private ConnectionInformation m_connectionInformation;
        private string m_internalDeviceName;
        private ConnectionAttemptEvent m_llrpConnectionAttemptEvent;
        private LlrpDeviceState m_llrpState;
        private ILogger m_logger;
        private Dictionary<uint, LlrpMessageEventArgs> m_messageResponseMapper;
        private object m_messageResponseMapperLock;
        private int m_messageTimeout;
        private object m_messageTimeoutLock;
        private Dictionary<uint, PendingMessageInformation> m_pendingInternalMessages;
        private object m_pendingInternalMsgLock;
        private static Collection<LlrpMessageType> s_deviceMessages = new Collection<LlrpMessageType>();

        internal event EventHandler<LlrpMessageEventArgs> MessageReceivedEvent;

        internal event EventHandler<LlrpMessageEventArgs> UnprocessedMessageReceivedEvent;

        internal event EventHandler<LlrpMessageEventArgs> UnprocessedMessageSendEvent;

        static LlrpDevice()
        {
            s_deviceMessages.Add(LlrpMessageType.KeepAlive);
            s_deviceMessages.Add(LlrpMessageType.ReaderEventNotification);
            s_deviceMessages.Add(LlrpMessageType.ROAccessReport);
        }

        private LlrpDevice(ConnectionInformation connectionInformation, ILogger logger)
        {
            this.m_connectionAttemptWaitEventHandle = new ManualResetEvent(false);
            this.m_connectionAttemptLock = new object();
            this.m_pendingInternalMessages = new Dictionary<uint, PendingMessageInformation>();
            this.m_messageResponseMapper = new Dictionary<uint, LlrpMessageEventArgs>();
            this.m_messageTimeout = LlrpProviderContext.LlrpMessageTimeout;
            this.m_pendingInternalMsgLock = new object();
            this.m_messageResponseMapperLock = new object();
            this.m_messageTimeoutLock = new object();
            this.m_llrpState = new Kalitte.Sensors.Rfid.Llrp.PhysicalDevices.LlrpDeviceState();
            if (connectionInformation == null)
            {
                throw new ArgumentNullException("connectionInformation");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            this.m_connectionInformation = connectionInformation;
            this.m_logger = logger;
        }

        internal LlrpDevice(ConnectionInformation connectionInformation, IDuplexChannel channel, ILogger logger) : this(connectionInformation, logger)
        {
            if (channel == null)
            {
                throw new ArgumentNullException("channel");
            }
            this.m_channel = channel;
            this.m_internalDeviceName = this.m_channel.RemoteAddress.Uri.ToString();
        }

        internal void ActuallyClose(bool closeOnlyTheChannel)
        {
            if (this.IsConnectionAlive())
            {
                if (!closeOnlyTheChannel)
                {
                    try
                    {
                        this.Logger.Info("Close called on device {0}", new object[] { this.m_connectionInformation });
                        this.SendCloseConnectionMessage();
                    }
                    catch (Exception exception)
                    {
                        this.Logger.Error("Exception {0} occured while closing connection to the device {1}", new object[] { exception, this.m_connectionInformation });
                    }
                }
                try
                {
                    this.m_channel.Close();
                }
                catch (Exception exception2)
                {
                    this.Logger.Error("Exception {0} occured while closing the WCF duplex channel for device {1}", new object[] { exception2, this.m_connectionInformation });
                }
            }
            else
            {
                this.Logger.Warning("Connection to the device {0} is not alive thus not sending close connection message", new object[] { this.m_connectionInformation });
            }
            try
            {
                IDisposable channel = this.m_channel as IDisposable;
                if (channel != null)
                {
                    channel.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }

        internal void Close()
        {
            this.ActuallyClose(false);
        }

        private void HandleConnectionAttemptEvent(LlrpMessageEventArgs args)
        {
            if (args.IsSuccess && (args.Response is ReaderEventNotificationMessage))
            {
                ReaderEventNotificationMessage response = (ReaderEventNotificationMessage) args.Response;
                if (((response != null) && (response.ReaderEventNotificationData != null)) && (response.ReaderEventNotificationData.LlrpEvents != null))
                {
                    foreach (LlrpEvent event2 in response.ReaderEventNotificationData.LlrpEvents)
                    {
                        if (event2 is ConnectionAttemptEvent)
                        {
                            this.Logger.Info("Received connection attempt on the device {0}", new object[] { this.DeviceName });
                            lock (this.m_connectionAttemptLock)
                            {
                                this.m_llrpConnectionAttemptEvent = (ConnectionAttemptEvent) event2;
                            }
                            this.m_connectionAttemptWaitEventHandle.Set();
                            break;
                        }
                    }
                }
            }
        }

        private void HandleOpenException(Exception ex)
        {
            this.Logger.Error("Error during open {0}", new object[] { ex });
            this.ActuallyClose(true);
            throw new ConnectionFailedException("Unable to connect sensor", ex);
        }

        private void HandleReadException(Exception ex)
        {
            if (ex is ObjectDisposedException)
            {
                this.Logger.Info("Failed to receive LLRP message due to object disposed exception. Probably the connection has been closed for device {0}", new object[] { this.m_internalDeviceName });
                this.Logger.Verbose(ex.ToString());
            }
            else
            {
                this.Logger.Error("Failed to receive LLRP message due to exception {0} for device {1}", new object[] { ex, this.m_internalDeviceName });
            }
        }

        private static void HandleSendException(Exception ex)
        {
            throw new SensorProviderException(ex.Message, ex);
        }

        internal bool IsConnectionAlive()
        {
            return ((this.Channel != null) && (this.Channel.State == CommunicationState.Opened));
        }

        private bool IsResponseMessage(LlrpMessageEventArgs args)
        {
            if (!this.m_pendingInternalMessages.ContainsKey(args.RequestId))
            {
                return false;
            }
            LlrpMessageBase requestMessage = this.m_pendingInternalMessages[args.RequestId].RequestMessage;
            LlrpMessageType messageType = args.MessageType;
            LlrpMessageType type2 = requestMessage.MessageType;
            if (((messageType != LlrpMessageType.ROAccessReport) || ((type2 != LlrpMessageType.GetReport) && (type2 != LlrpMessageType.EnableEventsAndReports))) && s_deviceMessages.Contains(args.MessageType))
            {
                return false;
            }
            return true;
        }

        internal void OnReceive(LlrpMessageEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            this.HandleConnectionAttemptEvent(args);
            this.PopulateROAccessReport(args);
            this.ProcessMessage(args);
        }

        internal void Open()
        {
            try
            {
                this.Logger.Info("Open called on device {0}", new object[] { this.m_connectionInformation });
                this.m_channel.Open();
                this.StartReading();
                this.WaitForConnectionAttemptEvent();
            }
            catch (FormatException exception)
            {
                this.HandleOpenException(exception);
            }
            catch (SocketException exception2)
            {
                this.HandleOpenException(exception2);
            }
            catch (SensorProviderException exception3)
            {
                this.HandleOpenException(exception3);
            }
            catch (Exception exception4)
            {
                this.m_logger.Error("Unexpected exception while opening the device {0}", new object[] { exception4 });
            }
        }

        private void PopulateROAccessReport(LlrpMessageEventArgs args)
        {
            if (args.IsSuccess && (args.Response is ROAccessReport))
            {
                ROAccessReport response = args.Response as ROAccessReport;
                if (response != null)
                {
                    lock (this.LlrpDeviceState)
                    {
                        this.LlrpDeviceState.LastTagInformation.PopulateTagReportData(response.TagReports);
                    }
                }
            }
        }

        private void ProcessMessage(LlrpMessageEventArgs args)
        {
            uint requestId = args.RequestId;
            bool flag = true;
            if (!args.IsSuccess)
            {
                this.Logger.Warning("Response for message {0} for device {1} is not in an expected format due to exception {2}", new object[] { args.RequestId, this.m_internalDeviceName, args.Exception });
            }
            LlrpMessageResponseBase response = args.Response as LlrpMessageResponseBase;
            if (((response != null) && (response.Status != null)) && !response.Status.IsSuccess)
            {
                this.Logger.Warning("Received a message of Id {0} with status {1} on device {2}", new object[] { args.RequestId, response.Status.ToString(), this.m_internalDeviceName });
            }
            lock (this.m_pendingInternalMsgLock)
            {
                if (this.IsResponseMessage(args))
                {
                    this.Logger.Info("Received a response for internal message {0} for device {1}", new object[] { requestId, this.m_internalDeviceName });
                    PendingMessageInformation information = this.m_pendingInternalMessages[requestId];
                    ManualResetEvent resetEvent = information.ResetEvent;
                    if (resetEvent != null)
                    {
                        flag = false;
                        lock (this.m_messageResponseMapperLock)
                        {
                            this.m_messageResponseMapper[requestId] = args;
                        }
                        resetEvent.Set();
                    }
                }
                //else
                //{
                //    if (this.m_pendingInternalMessages.ContainsKey(args.RequestId))
                //    {
                //        PendingMessageInformation information = this.m_pendingInternalMessages[requestId];
                //        ManualResetEvent resetEvent = information.ResetEvent;
                //        if (resetEvent != null)
                //        {
                //            flag = false;
                //            lock (this.m_messageResponseMapperLock)
                //            {
                //                this.m_messageResponseMapper[requestId] = args;
                //            }
                //            resetEvent.Set();
                //        }
                //    }

                //}
            }
            if (flag)
            {
                EventHandler<LlrpMessageEventArgs> messageReceivedEvent = this.MessageReceivedEvent;
                if (messageReceivedEvent != null)
                {
                    messageReceivedEvent(this, args);
                }
            }
        }

        private void RaiseUnprocessedMessageEvent(LlrpMessageEventArgs args)
        {
            EventHandler<LlrpMessageEventArgs> unprocessedMessageReceivedEvent = this.UnprocessedMessageReceivedEvent;
            if (unprocessedMessageReceivedEvent != null)
            {
                unprocessedMessageReceivedEvent(this, args);
            }
        }

        private void ReceiveCallBack(IAsyncResult result)
        {
            try
            {
                LlrpMessageBase messageResponse = this.Channel.EndReceive(result) as LlrpMessageBase;
                this.Logger.Verbose("Received message {0} from device {1}", new object[] { messageResponse, this.m_internalDeviceName });
                try
                {
                    if (messageResponse != null)
                    {
                        this.RaiseUnprocessedMessageEvent(new LlrpMessageEventArgs(messageResponse));
                    }
                }
                finally
                {
                    try
                    {
                        this.StartReading();
                    }
                    catch (Exception exception)
                    {
                        this.Logger.Error("Encountered exception {0} during read. Not reading any more from the device {1},as connection to the device is closed", new object[] { exception, this.m_internalDeviceName });
                    }
                }
            }
            catch (DecodingException exception2)
            {
                this.Logger.Error("Failed to receive LLRP message due to exception {0} for device {1}", new object[] { exception2, this.m_internalDeviceName });
                this.Logger.Error(exception2.ToString());
                SensorProviderException exception3 = new SensorProviderException(exception2.ToString());
                this.RaiseUnprocessedMessageEvent(new LlrpMessageEventArgs((uint) exception2.MessageId, exception2.MessageType, exception3));
                try
                {
                    this.StartReading();
                }
                catch (Exception exception4)
                {
                    this.Logger.Error("Encountered exception {0} during read. Not reading any more from the device {1},as connection to the device is closed", new object[] { exception4, this.m_internalDeviceName });
                }
            }
            catch (IOException exception5)
            {
                this.HandleReadException(exception5);
            }
            catch (ObjectDisposedException exception6)
            {
                this.HandleReadException(exception6);
            }
            catch (NotSupportedException exception7)
            {
                this.HandleReadException(exception7);
            }
            catch (InvalidOperationException exception8)
            {
                this.HandleReadException(exception8);
            }
            catch (Exception exception9)
            {
                this.m_logger.Error("Invalid exception while doing end receive {0}", new object[] { exception9 });
            }
        }

        internal LlrpMessageBase Request(LlrpMessageBase message)
        {
            this.Logger.Info("Request on device {0} for message {1}", new object[] { this.m_internalDeviceName, message });
            return this.Send(message, true);
        }

        internal void Send(LlrpMessageBase message)
        {
            EventHandler<LlrpMessageEventArgs> unprocessedMessageSendEvent = this.UnprocessedMessageSendEvent;
            if (unprocessedMessageSendEvent != null)
            {
                unprocessedMessageSendEvent(this, new LlrpMessageEventArgs(message));
            }
        }

        private LlrpMessageBase Send(LlrpMessageBase message, bool synchronous)
        {
            ManualResetEvent resetEvent = null;
            if (synchronous)
            {
                resetEvent = new ManualResetEvent(false);
            }
            lock (this.m_pendingInternalMsgLock)
            {
                this.m_pendingInternalMessages.Add(message.Id, new PendingMessageInformation(message, resetEvent));
            }
            this.Send(message);
            if (synchronous)
            {
                return this.WaitForResponseFromDevice(message.Id, resetEvent);
            }
            return null;
        }

        private void SendCloseConnectionMessage()
        {
            try
            {
                CloseConnectionResponse message = this.Request(new CloseConnectionMessage()) as CloseConnectionResponse;
                Util.ThrowIfNull(message, LlrpMessageType.CloseConnection);
                Util.ThrowIfFailed(message.Status);
            }
            catch (SensorProviderException exception)
            {
                this.Logger.Error("Error while sending close message {0}", new object[] { exception });
            }
            catch (TimeoutException exception2)
            {
                this.Logger.Error("Error while sending close message {0}", new object[] { exception2 });
            }
        }

        internal void SendToDevice(LlrpMessageBase message)
        {
            this.ThrowIfNotValidState();
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            this.Logger.Info("Sending message[Id : {0}, type : {1}] to device {2}", new object[] { message.Id, message.MessageType, this.m_internalDeviceName });
            this.Logger.Verbose("Message is {0}", new object[] { message });
            try
            {
                this.Channel.Send(message);
            }
            catch (ArgumentException exception)
            {
                HandleSendException(exception);
            }
            catch (ObjectDisposedException exception2)
            {
                HandleSendException(exception2);
            }
            catch (InvalidOperationException exception3)
            {
                HandleSendException(exception3);
            }
            catch (IOException exception4)
            {
                HandleSendException(exception4);
            }
            catch (NotSupportedException exception5)
            {
                HandleSendException(exception5);
            }
            catch (Exception exception6)
            {
                this.m_logger.Error("Unexpected expection while sending message to the device in the llrp device {0}", new object[] { exception6 });
            }
        }

        internal void StartReading()
        {
            this.ThrowIfNotValidState();
            this.Channel.BeginReceive(new AsyncCallback(this.ReceiveCallBack), null);
        }

        internal void ThrowIfNotValidState()
        {
            if ((this.Channel == null) || (this.Channel.State != CommunicationState.Opened))
            {
                throw new SensorProviderException(LlrpResources.DeviceStateIsNotOpened);
            }
        }

        internal void WaitForConnectionAttemptEvent()
        {
            try
            {
                this.m_connectionAttemptWaitEventHandle.WaitOne(this.MessageTimeout, false);
                lock (this.m_connectionAttemptLock)
                {
                    if (this.m_llrpConnectionAttemptEvent == null)
                    {
                        this.Logger.Error("No connection attempt event received with in stipulated time {0} on dveice {1}", new object[] { this.MessageTimeout, this.DeviceName });
                        throw new SensorProviderException(string.Format(CultureInfo.CurrentCulture, LlrpResources.ConnectionTimedout, new object[] { new TimeSpan(0, 0, 0, 0, this.MessageTimeout) }));
                    }
                    if (this.m_llrpConnectionAttemptEvent.Status != ConnectionAttemptEventType.Success)
                    {
                        this.Logger.Error("Connection to the device {0} failed with reason {1}", new object[] { this.DeviceName, this.m_llrpConnectionAttemptEvent.Status });
                        throw new SensorProviderException(this.m_llrpConnectionAttemptEvent.Status.ToString());
                    }
                    this.m_llrpConnectionAttemptEvent = null;
                    this.m_connectionAttemptWaitEventHandle.Reset();
                }
            }
            catch (SensorProviderException exception)
            {
                this.Logger.Error("Connection to device {0} failed for reason {1}", new object[] { this.DeviceName, exception });
                try
                {
                    this.ActuallyClose(true);
                }
                catch (Exception exception2)
                {
                    this.Logger.Error("Error {0} during closing connection to the device {1}. Ignoring the error.", new object[] { exception2, this.DeviceName });
                }
                throw;
            }
        }

        private LlrpMessageBase WaitForResponseFromDevice(uint messageId, ManualResetEvent manualEvent)
        {
            LlrpMessageBase base2;
            this.Logger.Verbose("Waiting for the response {0} from the device {1}", new object[] { messageId, this.m_internalDeviceName });
            bool flag = manualEvent.WaitOne(this.MessageTimeout, false);
            lock (this.m_pendingInternalMsgLock)
            {
                PendingMessageInformation information = this.m_pendingInternalMessages[messageId];
                this.m_pendingInternalMessages.Remove(messageId);
                information.Dispose();
                lock (this.m_messageResponseMapperLock)
                {
                    if (flag)
                    {
                        this.Logger.Verbose("Response {0} received", new object[] { messageId });
                        LlrpMessageEventArgs args = this.m_messageResponseMapper[messageId];
                        this.m_messageResponseMapper.Remove(messageId);
                        if (!args.IsSuccess)
                        {
                            throw args.Exception;
                        }
                        return args.Response;
                    }
                    this.Logger.Error("Timed out for message {0}", new object[] { messageId });
                    throw new TimeoutException(string.Format(CultureInfo.CurrentCulture, LlrpResources.LlrpResponseTimeout, new object[] { this.MessageTimeout }));
                }
            }
            return base2;
        }

 

 




        internal IDuplexChannel Channel
        {
            get
            {
                return this.m_channel;
            }
        }

        internal string DeviceName
        {
            get
            {
                return this.m_internalDeviceName;
            }
        }

        private LlrpDeviceState LlrpDeviceState
        {
            get
            {
                return this.m_llrpState;
            }
        }

        private ILogger Logger
        {
            get
            {
                return this.m_logger;
            }
        }

        internal int MessageTimeout
        {
            get
            {
                lock (this.m_messageTimeoutLock)
                {
                    return this.m_messageTimeout;
                }
            }
            set
            {
                lock (this.m_messageTimeoutLock)
                {
                    this.m_messageTimeout = value;
                }
            }
        }
    }
}
