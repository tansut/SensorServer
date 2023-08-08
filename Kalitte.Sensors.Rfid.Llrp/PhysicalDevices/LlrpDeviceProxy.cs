using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Utilities;
using System.ServiceModel.Channels;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Rfid;
using Kalitte.Sensors.Rfid.Llrp;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Core;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Rfid.Llrp.Helpers;
using System.Threading;
using Kalitte.Sensors.Security;
using System.Globalization;
using Kalitte.Sensors.Events.Management;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Rfid.Llrp.Commands;
using Kalitte.Sensors.Rfid.Llrp.Events;
using Kalitte.Sensors.Rfid.Llrp.Utilities;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Rfid.PhysicalDevices;

namespace Kalitte.Sensors.Rfid.Llrp.PhysicalDevices
{

    public sealed class LlrpDeviceProxy : RfidDeviceProxy
    {
        // Fields
        private DspiCommandProcessor m_commandProcessor;
        private object m_ConnectionLock = new object();
        private LlrpDevice m_device;
        private SensorDeviceInformation m_deviceInformation;
        private PDPState m_deviceState = new PDPState();
        private object m_disposeLock = new object();
        private bool m_fConnectionAttempted;
        private bool m_fDisposed;
        private bool m_fReaderIntiatedConnection;
        private ILogger m_logger;
        private EventHandler<ResponseEventArgs> cmdResponseEvent;
        private EventHandler<NotificationEventArgs> deviceNotificationEvent;

        internal event EventHandler<ConnectionCloseEventArgs> OnConnectionClosedEvent;


        public override Dictionary<PropertyKey, DevicePropertyMetadata> GetPropertyMetadata(string propertyGroupName)
        {
            this.Logger.Info("Get metadata called for device {0} for the group {1}", new object[] { this.PhysicalDevice.DeviceName, propertyGroupName });
            ResponseEventArgs args = null;
            GetMetadataCommand command = new GetMetadataCommand(propertyGroupName);
            try
            {
                args = this.CommandProcessor.ExecuteCommand(null, command, this.DeviceState);
            }
            catch (SensorProviderException exception)
            {
                this.Logger.Error("Exception {0} encountered during get metadata command on device {1}", new object[] { exception, this.PhysicalDevice.DeviceName });
                throw;
            }
            if (args.CommandError != null)
            {
                this.Logger.Error("Get metadata on device {0} failed with error {1}", new object[] { this.PhysicalDevice.DeviceName, args.CommandError });
                throw new SensorProviderException(args.CommandError.Message);
            }
            return command.Response.Metadata;
        }



        internal LlrpDeviceProxy(ConnectionInformation connectionInformation, IDuplexChannel duplexChannel, ILogger logger)
        {

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            if (connectionInformation == null)
            {
                throw new ArgumentNullException("connectionInformation");
            }
            if (duplexChannel == null)
            {
                throw new ArgumentNullException("duplexChannel");
            }
            if (!(connectionInformation.TransportSettings is TcpTransportSettings))
            {
                throw new ArgumentException(LlrpResources.NotSupportedConnectionType);
            }
            logger.Info("Creating a new device proxy for {0}", new object[] { connectionInformation });
            this.m_deviceInformation = new SensorDeviceInformation(string.Empty, connectionInformation, null);
            this.m_logger = logger;
            this.m_device = new LlrpDevice(connectionInformation, duplexChannel, logger);
            this.IsReaderIntiatedConnection = this.m_device.IsConnectionAlive();
            this.m_commandProcessor = DspiCommandProcessor.GetInstance(this.m_device, this.m_logger);
            if (this.IsReaderIntiatedConnection)
            {
                this.Init();
            }
        }


        internal void CleanUp()
        {
            lock (this.m_disposeLock)
            {
                if (this.m_fDisposed)
                {
                    return;
                }
                this.m_fDisposed = true;
            }
            try
            {
                this.m_commandProcessor = null;
                this.m_device = null;
                this.m_deviceInformation = null;
                this.m_deviceState = null;
            }
            catch
            {
            }
        }

        public override void Close()
        {
            lock (this.m_ConnectionLock)
            {
                if (!this.m_fConnectionAttempted)
                {
                    return;
                }
                this.m_fConnectionAttempted = false;
                this.m_logger.Info("Closing the connection to device {0}", new object[] { this.m_deviceInformation.ConnectionInformation });
                try
                {
                    this.CommandProcessor.ExecuteCommand(null, new ShutdownCommand(this.IsReaderIntiatedConnection), this.DeviceState);
                    m_device.Close();
                }
                catch (Exception exception)
                {
                    this.Logger.Error("shut down command failed {0}.", new object[] { exception });
                }
                finally
                {
                    EventHandler<ConnectionCloseEventArgs> onConnectionClosedEvent = this.OnConnectionClosedEvent;
                    if (onConnectionClosedEvent != null)
                    {
                        onConnectionClosedEvent(this, new ConnectionCloseEventArgs(this.m_deviceInformation.ConnectionInformation));
                    }
                    if (!this.IsReaderIntiatedConnection)
                    {
                        this.m_device.UnprocessedMessageSendEvent -= new EventHandler<LlrpMessageEventArgs>(this.m_device_UnprocessedMessageSendEvent);
                        this.m_device.UnprocessedMessageReceivedEvent -= new EventHandler<LlrpMessageEventArgs>(this.OnReceive);
                        this.m_device.MessageReceivedEvent -= new EventHandler<LlrpMessageEventArgs>(this.m_device_MessageReceivedEvent);
                        this.CleanUp();
                    }
                }
            }
            this.m_logger.Info("Connection closed");
        }

        private Notification ConvertToRfidNotification(RFSurveyReportData data)
        {
            string str = null;
            if (data.FrequencyRssiLevelEntries != null)
            {
                str = LlrpSerializationHelper.SerializeToXmlDataContract(data.FrequencyRssiLevelEntries, false);
            }
            VendorData vendorData = new VendorData();
            if (data.ROSpecId != null)
            {
                vendorData.Add("RO Spec Id", data.ROSpecId.Id);
            }
            if (data.SpecIndex != null)
            {
                vendorData.Add("Spec Index", data.SpecIndex.Index);
            }
            vendorData.Add("Frequency RSSI Informations", str);
            return new Notification(new VendorDefinedManagementEvent(EventLevel.Info, LlrpEventTypes.RFSurveyReportEvent, LlrpEventTypes.RFSurveyReportEvent.Description, typeof(RFSurveyReportData).Name, vendorData));
        }


        public override Collection<string> GetPropertyGroupNames()
        {
            throw new SensorProviderException(LlrpResources.GetPropertyGroupNameNotSupported);
        }

        public override Dictionary<string, PropertyList> GetSources()
        {
            this.Logger.Info("Get sources called for device {0}", new object[] { this.PhysicalDevice.DeviceName });
            Dictionary<string, PropertyList> sources = null;
            lock (this.DeviceState)
            {
                sources = this.DeviceState.Sources;
            }
            if (sources != null)
            {
                return sources;
            }
            ResponseEventArgs args = null;
            GetSourcesCommand command = new GetSourcesCommand();
            try
            {
                args = this.CommandProcessor.ExecuteCommand(null, command, this.DeviceState);
            }
            catch (SensorProviderException exception)
            {
                this.Logger.Error("Exception {0} encountered during get sources command on device {1}", new object[] { exception, this.PhysicalDevice.DeviceName });
                throw;
            }
            if (args.CommandError != null)
            {
                this.Logger.Error("Get sources on device {0} failed with error {1}", new object[] { this.PhysicalDevice.DeviceName, args.CommandError });
                throw new SensorProviderException(args.CommandError.Message);
            }
            return command.Response.Sources;
        }

        //private void HandleDspiMappingOfNotification(ref Notification notification)
        //{
        //    RfidEventBase base2 = notification.Event;
        //    if (base2 != null)
        //    {
        //        string newName = null;
        //        if (base2 is Observation)
        //        {
        //            this.HandleDspiMappingOfObservation(ref notification);
        //        }
        //        else if (base2 is SourceDownEvent)
        //        {
        //            SourceDownEvent wrapme = (SourceDownEvent) base2;
        //            if (this.TryGetSourceMapping(wrapme.SourceName, LLRPSourceType.Antenna, out newName))
        //            {
        //                SourceDownEventWrapper wrapper = new SourceDownEventWrapper(wrapme);
        //                wrapper.m_sourceName = newName;
        //                notification = new Notification(wrapper.WrappedObject);
        //            }
        //        }
        //        else if (base2 is SourceUpEvent)
        //        {
        //            SourceUpEvent event3 = (SourceUpEvent) base2;
        //            if (this.TryGetSourceMapping(event3.SourceName, LLRPSourceType.Antenna, out newName))
        //            {
        //                SourceUpEventWrapper wrapper2 = new SourceUpEventWrapper(event3);
        //                wrapper2.m_sourceName = newName;
        //                notification = new Notification(wrapper2.WrappedObject);
        //            }
        //        }
        //        else if (base2 is SourceNoiseLevelHighEvent)
        //        {
        //            SourceNoiseLevelHighEvent event4 = (SourceNoiseLevelHighEvent) base2;
        //            if (this.TryGetSourceMapping(event4.SourceName, LLRPSourceType.Antenna, out newName))
        //            {
        //                SourceNoiseLevelHighEventWrapper wrapper3 = new SourceNoiseLevelHighEventWrapper(event4);
        //                wrapper3.m_sourceName = newName;
        //                notification = new Notification(wrapper3.WrappedObject);
        //            }
        //        }
        //        else if (base2 is SourceNoiseLevelNormalEvent)
        //        {
        //            SourceNoiseLevelNormalEvent event5 = (SourceNoiseLevelNormalEvent) base2;
        //            if (this.TryGetSourceMapping(event5.SourceName, LLRPSourceType.Antenna, out newName))
        //            {
        //                SourceNoiseLevelNormalEventWrapper wrapper4 = new SourceNoiseLevelNormalEventWrapper(event5);
        //                wrapper4.m_sourceName = newName;
        //                notification = new Notification(wrapper4.WrappedObject);
        //            }
        //        }
        //    }
        //}

        //private void HandleDspiMappingOfObservation(ref Notification notification)
        //{
        //    Observation observation = notification.Event as Observation;
        //    string newName = null;
        //    IOPortValueChangedEvent wrapme = observation as IOPortValueChangedEvent;
        //    if (wrapme != null)
        //    {
        //        if (this.TryGetSourceMapping(wrapme.PortName, LLRPSourceType.GPI, out newName))
        //        {
        //            IOPortValueChangedEventWrapper wrapper = new IOPortValueChangedEventWrapper(wrapme);
        //            wrapper.m_portName = newName;
        //            notification = new Notification(wrapper.WrappedObject);
        //        }
        //    }
        //    else if (this.TryGetSourceMapping(observation.Source, LLRPSourceType.Antenna, out newName))
        //    {
        //        observation.Source = newName;
        //        TagListEvent event3 = observation as TagListEvent;
        //        if ((event3 != null) && (event3.Tags != null))
        //        {
        //            foreach (TagReadEvent event4 in event3.Tags)
        //            {
        //                if ((event4 != null) && (event4.Source != null))
        //                {
        //                    event4.Source = newName;
        //                }
        //            }
        //        }
        //    }
        //}

        private void HandleKeepAliveMessage(KeepAliveMessage keepAliveMessage)
        {
            this.Logger.Verbose("Received keep alive message with Id {0} on device {1}, sending a ack for the same", new object[] { keepAliveMessage.Id, this.PhysicalDevice.DeviceName });
            try
            {
                this.Send(new KeepAliveAcknowledgementResponse(keepAliveMessage.Id));
            }
            catch (Exception exception)
            {
                this.Logger.Error("Error {0} while sending the Keep Alive acknowledgement response to the device {1} for message {2}", new object[] { exception, this.PhysicalDevice.DeviceName, keepAliveMessage.Id });
            }
        }

        private void HandleReaderEventNotificationMessage(ReaderEventNotificationMessage readerEventNotificationMessage)
        {
            this.Logger.Info("Received reader event notification message on the device {0}", new object[] { this.PhysicalDevice.DeviceName });
            this.UpdateDeviceTime(readerEventNotificationMessage);
            try
            {
                if (readerEventNotificationMessage.ReaderEventNotificationData != null)
                {
                    ReaderEventNotificationData readerEventNotificationData = readerEventNotificationMessage.ReaderEventNotificationData;
                    if ((readerEventNotificationData.LlrpEvents != null) && (readerEventNotificationData.LlrpEvents.Count > 0))
                    {
                        foreach (LlrpEvent event2 in readerEventNotificationData.LlrpEvents)
                        {
                            Notification notification = event2.ConvertToRfidNotification();
                            if (notification != null)
                            {
                                this.OnNotification(this, new NotificationEventArgs(notification));
                            }
                            else
                            {
                                this.Logger.Info("Dropping event {0} from device {1}", new object[] { event2, this.PhysicalDevice.DeviceName });
                            }
                        }
                    }

                    this.SendAcknowledgementIfRequired(readerEventNotificationMessage, readerEventNotificationData.CustomParameters);
                }
            }
            catch (SensorException exception)
            {
                this.Logger.Error("Error {0} while raising notification from device {1}. No acknowledgement will be sent to the device, if the device required for this reader event notification message.", new object[] { exception, this.PhysicalDevice.DeviceName });
            }
        }

        private void HandleRFSurveyReports(ROAccessReport rOAccessReport)
        {
            if (rOAccessReport.RFSurveyReports != null)
            {
                Notification notification = null;
                foreach (RFSurveyReportData data in rOAccessReport.RFSurveyReports)
                {
                    notification = this.ConvertToRfidNotification(data);
                    this.OnNotification(this, new NotificationEventArgs(notification));
                }
            }
        }

        private void HandleROAccessReport(ROAccessReport rOAccessReport)
        {
            this.Logger.Verbose("Received tag report on device {0}", new object[] { this.PhysicalDevice.DeviceName });
            try
            {
                this.HandleTagReportData(rOAccessReport);
                this.HandleRFSurveyReports(rOAccessReport);
                this.SendAcknowledgementIfRequired(rOAccessReport, rOAccessReport.CustomParameters);
            }
            catch (SensorException exception)
            {
                this.Logger.Error("Error {0} while raising notification from device {1}. No acknowledgement will be sent to the device, if the device required for this RO Access report message.", new object[] { exception, this.PhysicalDevice.DeviceName });
            }
        }

        private void HandleTagReportData(ROAccessReport rOAccessReport)
        {
            Collection<TagReadEvent> collection = new Collection<TagReadEvent>();
            lock (this.DeviceState)
            {
                if (!this.DeviceState.IsInventoryOn)
                {
                    this.Logger.Info("Ignoring the tag report as device {0} is not in event mode", new object[] { this.PhysicalDevice.DeviceName });
                    return;
                }
                collection = Util.GetAsyncTags(this.DeviceState, rOAccessReport, this.Logger);
            }
            foreach (TagReadEvent event2 in collection)
            {
                this.OnNotification(this, new NotificationEventArgs(new Notification(event2)));
            }
        }

        internal void Init()
        {
            this.m_device.UnprocessedMessageSendEvent += new EventHandler<LlrpMessageEventArgs>(this.m_device_UnprocessedMessageSendEvent);
            this.m_device.UnprocessedMessageReceivedEvent += new EventHandler<LlrpMessageEventArgs>(this.OnReceive);
            this.m_device.MessageReceivedEvent += new EventHandler<LlrpMessageEventArgs>(this.m_device_MessageReceivedEvent);
        }

        public override bool IsConnectionAlive()
        {
            return this.PhysicalDevice.IsConnectionAlive();
        }

        private void m_device_MessageReceivedEvent(object sender, LlrpMessageEventArgs args)
        {
            //if (MessageReceived != null)
            //    MessageReceived(this, args);

            if (args.IsSuccess)
            {
                if (args.Response is ROAccessReport)
                {
                    this.HandleROAccessReport((ROAccessReport)args.Response);
                }
                else if (args.Response is ReaderEventNotificationMessage)
                {
                    this.HandleReaderEventNotificationMessage((ReaderEventNotificationMessage)args.Response);
                }
                else if (args.Response is KeepAliveMessage)
                {
                    this.HandleKeepAliveMessage((KeepAliveMessage)args.Response);
                }
                else
                {
                    this.Logger.Warning("Unknown message {0} on device {1}", new object[] { args.RequestId, this.PhysicalDevice.DeviceName });
                }
            }

        }

        private void m_device_UnprocessedMessageSendEvent(object sender, LlrpMessageEventArgs e)
        {
            this.Send(e.Response);
        }

        private ResponseEventArgs Request(CommandArgs args)
        {
            SensorCommand command = args.Command;
            this.Logger.Info("Starting command execution {0}:{1} for device {2}", new object[] { command.GetType().Name, command.Id, this.PhysicalDevice.DeviceName });
            CommandError error = null;
            ResponseEventArgs args2;
            try
            {
                args2 = this.CommandProcessor.ExecuteCommand(args.SourceName, command, this.DeviceState);
            }
            catch (SensorProviderException exception)
            {
                this.Logger.Error("Exception {0} encountered during command {1}:{2} on device {3}", new object[] { exception, command.GetType().Name, command.Id, this.PhysicalDevice.DeviceName });
                error = new CommandError(ErrorCode.UnknownError, exception, exception.Message, ErrorCode.UnknownError.Description, null);
                args2 = new ResponseEventArgs(command, error);
            }
            this.Logger.Info("Command execution {0}:{1} completed for device {2}", new object[] { command.GetType().Name, command.Id, this.PhysicalDevice.DeviceName });
            this.Logger.Verbose("Command response is {0}", new object[] { args2 });
            return args2;
        }

        private void MyCommandHandler(object command)
        {
            try
            {
                CommandArgs args = command as CommandArgs;
                ResponseEventArgs args2 = Request(args);
                this.OnCmdResponse(this, args2);
            }
            catch (Exception exception2)
            {
                this.Logger.Error("Error {0} during command execution {1} on device {2}", new object[] { exception2, command, this.PhysicalDevice.DeviceName });
            }
        }

        public override event EventHandler<ResponseEventArgs> CmdResponseEvent
        {
            add
            {
                this.cmdResponseEvent = (EventHandler<ResponseEventArgs>)Delegate.Combine(this.cmdResponseEvent, value);
            }
            remove
            {
                this.cmdResponseEvent = (EventHandler<ResponseEventArgs>)Delegate.Remove(this.cmdResponseEvent, value);
            }
        }

        public override event EventHandler<NotificationEventArgs> DeviceNotificationEvent
        {
            add
            {
                this.deviceNotificationEvent = (EventHandler<NotificationEventArgs>)Delegate.Combine(this.deviceNotificationEvent, value);
            }
            remove
            {
                this.deviceNotificationEvent = (EventHandler<NotificationEventArgs>)Delegate.Remove(this.deviceNotificationEvent, value);
            }
        }


        private void OnCmdResponse(object sender, ResponseEventArgs args)
        {
            EventHandler<ResponseEventArgs> cmdResponseEvent = this.cmdResponseEvent;
            if (cmdResponseEvent != null)
            {
                cmdResponseEvent(sender, args);
            }

        }

        private void OnNotification(object sender, NotificationEventArgs args)
        {
            this.Logger.Verbose("Raising notification {0} from device {1}", new object[] { args, this.PhysicalDevice.DeviceName });


            EventHandler<NotificationEventArgs> deviceNotificationEvent = this.deviceNotificationEvent;

            if (deviceNotificationEvent != null)
                deviceNotificationEvent(this, args);

            //DoNotification(args);

            //if (deviceNotificationEvent != null)
            //{
            //    NotificationContextHelper.SetupNotificationContext();
            //    try
            //    {
            //        deviceNotificationEvent(sender, args);
            //        if (!NotificationContextHelper.GetEventConsumedStatus())
            //        {
            //            throw new SensorException("Event not consumed successfully by subscriber");
            //        }
            //        return;
            //    }
            //    finally
            //    {
            //        NotificationContextHelper.CleanupNotificationContext();
            //    }
            //}
            //throw new SensorException("No subcriber for the notification");
        }

        private void OnReceive(LlrpMessageEventArgs args)
        {
            this.m_device.OnReceive(args);
        }

        private void OnReceive(object sender, LlrpMessageEventArgs args)
        {
            this.OnReceive(args);
        }

        public override void Reboot()
        {
            throw new SensorProviderException(LlrpResources.NotsupportedOperation);
        }

        internal void Send(LlrpMessageBase message)
        {
            this.m_device.SendToDevice(message);
        }

        private void SendAcknowledgementIfRequired(LlrpMessageBase message, Collection<CustomParameterBase> parameters)
        {

        }

        public override void SendCommand(SensorCommand command)
        {
            this.SendCommand(null, command);
        }

        public override void SendCommand(string sourceName, SensorCommand command)
        {
            if (command == null)
            {
                throw new SensorProviderException(LlrpResources.CommandIsNull);
            }
            this.Logger.Info("Command received on device {0} for source {1}, command {2}", new object[] { this.PhysicalDevice.DeviceName, sourceName, command });
            Util.LogThreadPoolStatus(this.Logger);
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.MyCommandHandler), new CommandArgs(sourceName, command));
        }

        public override ResponseEventArgs ExecuteCommand(SensorCommand command)
        {
            return ExecuteCommand(null, command);
        }

        public override ResponseEventArgs ExecuteCommand(string sourceName, SensorCommand command)
        {
            ResponseEventArgs args2 = this.Request(new CommandArgs(sourceName, command));
            return args2;
        }

        public override void SetupConnection(AuthenticationInformation authenticationInfo)
        {
            object obj2;
            Monitor.Enter(obj2 = this.m_ConnectionLock);
            try
            {
                this.m_fConnectionAttempted = true;
                this.m_logger.Info("Setting up connection for device {0}", new object[] { this.DeviceInformation.ConnectionInformation });
                if (!this.IsReaderIntiatedConnection)
                {
                    this.Init();
                    this.PhysicalDevice.Open();
                }
                ResponseEventArgs args = null;

                string deviceId = PhysicalDevice.DeviceName;

                args = this.m_commandProcessor.ExecuteCommand(null, new GetDeviceIdCommand(), this.DeviceState);
                GetDeviceIdCommand command = args.Command as GetDeviceIdCommand;

                if (args.CommandError != null)
                {
                    this.Logger.Error("Getting deviceID failed {0} during setup connection for device {1}. Ignoring this error during setup connection.", new object[] { args.CommandError.ProviderException, this.m_deviceInformation.ConnectionInformation });

                    this.m_deviceInformation = new SensorDeviceInformation(deviceId, this.m_deviceInformation.ConnectionInformation, this.m_deviceInformation.ProviderData);
                }
                else
                {
                    deviceId = command.Response.DeviceId;
                    this.m_deviceInformation = new SensorDeviceInformation(deviceId, this.m_deviceInformation.ConnectionInformation, this.m_deviceInformation.ProviderData);
                    try
                    {
                        args = this.CommandProcessor.ExecuteCommand(null, new GetMetadataCommand(null), this.DeviceState);
                        if (args.CommandError != null)
                        {
                            throw new SensorProviderException(args.CommandError.Message);
                        }
                    }
                    catch (Exception exception)
                    {
                        this.Logger.Error("Getting metadata failed {0} during setup connection for device {1}. Ignoring this error during setup connection.", new object[] { exception, this.m_deviceInformation.ConnectionInformation });
                    }
                }
                this.m_logger.Info("device connected");
            }
            catch (Exception exception2)
            {
                this.m_logger.Error("Set up connection failed for the device {0} with the error {1}", new object[] { this.m_deviceInformation.ConnectionInformation, exception2 });
                EventHandler<ConnectionCloseEventArgs> onConnectionClosedEvent = this.OnConnectionClosedEvent;
                if (onConnectionClosedEvent != null)
                {
                    onConnectionClosedEvent(this, new ConnectionCloseEventArgs(this.m_deviceInformation.ConnectionInformation));
                }
                throw;
            }
            finally
            {
                Monitor.Exit(obj2);
            }
        }




        private bool TryGetSourceMapping(string sourceName, LLRPSourceType sourceType, out string newName)
        {
            ushort num;
            newName = null;
            if (sourceName == null)
            {
                return true;
            }
            if (ushort.TryParse(sourceName, NumberStyles.None, CultureInfo.InvariantCulture, out num))
            {
                switch (sourceType)
                {
                    case LLRPSourceType.Antenna:
                        newName = Util.GetAntennaName(num);
                        goto Label_004E;

                    case LLRPSourceType.GPI:
                        newName = Util.GetGpiName(num);
                        goto Label_004E;

                    case LLRPSourceType.GPO:
                        newName = Util.GetGpoName(num);
                        goto Label_004E;
                }
            }
            return false;
        Label_004E: ;
            this.Logger.Verbose("Converting the source name from {0} to {1} for device {2}", new object[] { sourceName, newName, this.PhysicalDevice.DeviceName });
            return true;
        }

        private void UpdateDeviceTime(ReaderEventNotificationMessage readerEventNotificationMessage)
        {
            if (((readerEventNotificationMessage != null) && (readerEventNotificationMessage.ReaderEventNotificationData != null)) && ((readerEventNotificationMessage.ReaderEventNotificationData.LlrpEvents != null) && (readerEventNotificationMessage.ReaderEventNotificationData.LlrpEvents.Count != 0)))
            {
                foreach (LlrpEvent event2 in readerEventNotificationMessage.ReaderEventNotificationData.LlrpEvents)
                {
                    ConnectionAttemptEvent event3 = event2 as ConnectionAttemptEvent;
                    if ((event3 != null) && (event3.Status == ConnectionAttemptEventType.Success))
                    {
                        if (readerEventNotificationMessage.ReaderEventNotificationData.Uptime == null)
                        {
                            this.Logger.Info("Device {0} does not have UpTime information in the connection attempt event. If the device reports UpTime later in reports, the time reported will be wrong", new object[] { this.PhysicalDevice.DeviceName });
                        }
                        else
                        {
                            lock (this.DeviceState)
                            {
                                TimeSpan span = new TimeSpan((long)(readerEventNotificationMessage.ReaderEventNotificationData.Uptime.TimeElapsed * ((ulong)10L)));
                                this.DeviceState.DeviceStartTime = DateTime.UtcNow - span;
                                this.m_logger.Info("Indicates the case that the connection has been successfully establised to the device {0}, and the device time based on the device uptme is {1}. The boot up time is {2}", new object[] { this.PhysicalDevice.DeviceName, this.DeviceState.DeviceStartTime, span });
                                continue;
                            }
                        }
                    }
                }
            }
        }

        // Properties
        private DspiCommandProcessor CommandProcessor
        {
            get
            {
                return this.m_commandProcessor;
            }
        }

        public override SensorDeviceInformation DeviceInformation
        {
            get
            {
                return this.m_deviceInformation;
            }
        }

        private PDPState DeviceState
        {
            get
            {
                return this.m_deviceState;
            }
        }

        private bool IsReaderIntiatedConnection
        {
            get
            {
                return this.m_fReaderIntiatedConnection;
            }
            set
            {
                this.m_fReaderIntiatedConnection = value;
            }
        }

        private ILogger Logger
        {
            get
            {
                return this.m_logger;
            }
        }

        internal LlrpDevice PhysicalDevice
        {
            get
            {
                return this.m_device;
            }
        }

        // Nested Types
        private class CommandArgs
        {
            // Fields
            private SensorCommand m_command;
            private string m_sourceName;

            // Methods
            public CommandArgs(string sourceName, SensorCommand cmd)
            {
                this.m_sourceName = sourceName;
                this.m_command = cmd;
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Source : ");
                builder.Append(this.SourceName);
                builder.Append(Environment.NewLine);
                builder.Append("Command : ");
                builder.Append(this.Command);
                return builder.ToString();
            }

            // Properties
            public SensorCommand Command
            {
                get
                {
                    return this.m_command;
                }
            }

            public string SourceName
            {
                get
                {
                    return this.m_sourceName;
                }
            }
        }




    }




}
