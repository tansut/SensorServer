using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Rfid.VirtualProvider.Communication;
using Kalitte.Sensors.Rfid.Commands;

namespace Kalitte.Sensors.Rfid.VirtualProvider.Commands
{
    internal abstract class CommandHandler
    {
        private SensorCommand m_command;
        private VirtualDevice m_device;
        private VirtualDeviceState m_deviceState;
        private ILogger m_logger;
        private string m_sourceName;


        protected CommandHandler(string source, SensorCommand command,  VirtualDevice device, VirtualDeviceState state, ILogger logger)
        {
            this.m_command = command;
            this.m_sourceName = source;
            this.m_device = device;
            this.m_logger = logger;
            this.m_deviceState = state;
        }

        internal static CommandHandler GetInstance(string source, SensorCommand command, VirtualDevice device, VirtualDeviceState state, ILogger logger)
        {
            if (command is QueryTagsCommand)
                return new QueryTagsCommandHandler(source, command, device, state, logger);
            if (command is GetActivePropertyListCommand)
                return new GetActivePropertyListCommandHandler(source, command, device, state, logger);
            if (command is ApplyPropertyListCommand)
                return new ApplyPropertyListCommandHandler(source, command, device, state, logger);
            else return null;
        }

        internal abstract ResponseEventArgs ExecuteCommand();

        protected SensorCommand Command
        {
            get
            {
                return this.m_command;
            }
        }

        protected VirtualDevice Device
        {
            get
            {
                return this.m_device;
            }
        }

        protected VirtualDeviceState DeviceState
        {
            get
            {
                return this.m_deviceState;
            }
        }

        protected ILogger Logger
        {
            get
            {
                return this.m_logger;
            }
        }

        protected string SourceName
        {
            get
            {
                return this.m_sourceName;
            }
        }
    }


}
