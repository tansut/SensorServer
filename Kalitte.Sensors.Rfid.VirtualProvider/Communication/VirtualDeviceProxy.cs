using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.PhysicalDevices;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Rfid.VirtualProvider.Commands;
using Kalitte.Sensors.Rfid.VirtualProvider.Events;
using Kalitte.Sensors.Events;
using System.Threading;

namespace Kalitte.Sensors.Rfid.VirtualProvider.Communication
{
    public class VirtualDeviceProxy : RfidDeviceProxy
    {
        public override event EventHandler<Sensors.Commands.ResponseEventArgs> CmdResponseEvent;
        public override event EventHandler<Sensors.Events.NotificationEventArgs> DeviceNotificationEvent;

        private ILogger logger;
        private volatile bool isConnectionAlive;
        private SensorDeviceInformation deviceInformation;
        private VirtualDevice physicalDevice;
        private CommandProcessor cmdprocessor;
        private VirtualDeviceState state;

        internal VirtualDeviceProxy(ConnectionInformation connectionInformation, ILogger logger)
        {
            this.isConnectionAlive = false;
            this.logger = logger;
            this.deviceInformation = new SensorDeviceInformation(string.Empty, connectionInformation, new VendorData());
            this.physicalDevice = new VirtualDevice(connectionInformation, this.logger);
            this.state = new VirtualDeviceState();
            this.cmdprocessor = new CommandProcessor(this.physicalDevice, state, this.logger);

        }

        public override void Close()
        {
            isConnectionAlive = false;
            this.physicalDevice.MessageReceivedEvent -= new EventHandler<MessageEventArgs>(this.device_MessageReceivedEvent);

        }

        private void device_MessageReceivedEvent(object sender, MessageEventArgs args)
        {

            //if (args.IsSuccess)
            //{
            //    if (args.Response is ROAccessReport)
            //    {
            //        this.HandleROAccessReport((ROAccessReport)args.Response);
            //    }
            //    else if (args.Response is ReaderEventNotificationMessage)
            //    {
            //        this.HandleReaderEventNotificationMessage((ReaderEventNotificationMessage)args.Response);
            //    }
            //    else if (args.Response is KeepAliveMessage)
            //    {
            //        this.HandleKeepAliveMessage((KeepAliveMessage)args.Response);
            //    }
            //    else
            //    {
            //        this.Logger.Warning("Unknown message {0} on device {1}", new object[] { args.RequestId, this.PhysicalDevice.DeviceName });
            //    }
            //}

        }

        public override System.Collections.ObjectModel.Collection<string> GetPropertyGroupNames()
        {
            throw new NotSupportedException();
        }

        public override Dictionary<Sensors.Configuration.PropertyKey, Sensors.Configuration.DevicePropertyMetadata> GetPropertyMetadata(string propertyGroupName)
        {
            throw new NotSupportedException();
        }

        public override Dictionary<string, Sensors.Configuration.PropertyList> GetSources()
        {
            throw new NotImplementedException();
        }

        public override bool IsConnectionAlive()
        {
            return isConnectionAlive;
        }

        public override void Reboot()
        {
            throw new NotSupportedException();
        }

        public override void SendCommand(Sensors.Commands.SensorCommand command)
        {
            SendCommand(null, command);
        }

        public override void SendCommand(string sourceName, Sensors.Commands.SensorCommand command)
        {
            this.Logger.Info("Command received on device {0} for source {1}, command {2}", new object[] { this.PhysicalDevice.DeviceName, sourceName, command });
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.MyCommandHandler), new CommandArgs(sourceName, command));
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

        private void OnCmdResponse(object sender, ResponseEventArgs args)
        {
            EventHandler<ResponseEventArgs> cmdResponseEvent = this.CmdResponseEvent;
            if (cmdResponseEvent != null)
            {
                cmdResponseEvent(sender, args);
            }

        }


        private void OnNotification(object sender, NotificationEventArgs args)
        {
            this.Logger.Verbose("Raising notification {0} from device {1}", new object[] { args, this.PhysicalDevice.DeviceName });

            EventHandler<NotificationEventArgs> deviceNotificationEvent = this.DeviceNotificationEvent;

            if (deviceNotificationEvent != null)
                deviceNotificationEvent(this, args);
        }

        private ResponseEventArgs Request(CommandArgs args)
        {
            SensorCommand command = args.Command;
            this.Logger.Info("Starting command execution {0}:{1} for device ", new object[] { command.GetType().Name, command.Id, this.PhysicalDevice.DeviceName });
            CommandError error = null;
            ResponseEventArgs args2;
            try
            {
                args2 = this.CommandProcessor.ExecuteCommand(args.SourceName, command);
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

        public override Sensors.Commands.ResponseEventArgs ExecuteCommand(Sensors.Commands.SensorCommand command)
        {
            return ExecuteCommand(null, command);
        }

        public override Sensors.Commands.ResponseEventArgs ExecuteCommand(string sourceName, Sensors.Commands.SensorCommand command)
        {
            ResponseEventArgs args2 = this.Request(new CommandArgs(sourceName, command));
            return args2;
        }

        public override void SetupConnection(Security.AuthenticationInformation authenticationInfo)
        {
            this.physicalDevice.MessageReceivedEvent += new EventHandler<MessageEventArgs>(this.device_MessageReceivedEvent);
            isConnectionAlive = true;
        }

        public override SensorDevices.SensorDeviceInformation DeviceInformation
        {
            get { return deviceInformation; }
        }

        private ILogger Logger
        {
            get
            {
                return this.logger;
            }
        }

        internal VirtualDevice PhysicalDevice
        {
            get
            {
                return this.physicalDevice;
            }
        }

        private CommandProcessor CommandProcessor
        {
            get
            {
                return this.cmdprocessor;
            }
        }

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
