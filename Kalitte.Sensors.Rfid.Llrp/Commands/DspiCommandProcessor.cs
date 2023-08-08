using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp;
using Kalitte.Sensors.Rfid.Llrp.Core;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
using Kalitte.Sensors.Rfid.Llrp.Configuration;
using Kalitte.Sensors.Rfid.Llrp.Utilities;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Rfid.Llrp.Commands
{

    internal sealed class DspiCommandProcessor
    {
        // Fields
        private object m_commandSyncLock = new object();
        private LlrpDevice m_device;
        private ILogger m_logger;

        // Methods
        private DspiCommandProcessor(LlrpDevice llrpDevice, ILogger logger)
        {
            if (llrpDevice == null)
            {
                throw new ArgumentNullException("llrpDevice");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            this.m_device = llrpDevice;
            this.m_logger = logger;
        }

        internal ResponseEventArgs ExecuteCommand(string sourceName, SensorCommand command, PDPState deviceState)
        {
            this.m_logger.Info("Executing command {0}:{1} on device {2} for source {3}", new object[] { command.GetType().Name, command.Id, this.m_device.DeviceName, sourceName });
            lock (this.m_commandSyncLock)
            {
                this.m_device.ThrowIfNotValidState();
                this.m_logger.Info("Command execution started");
                bool flag = false;
                bool flag2 = false;
                bool flag3 = false;
                CommandHandler handler = CommandHandler.GetInstance(sourceName, command, deviceState, this.m_device, this.m_logger);
                flag = handler.IsConcurrentToInventoryOperation;
                this.m_logger.Info("Command is concurrent to inventory {0}", new object[] { flag });
                lock (deviceState)
                {
                    flag2 = deviceState.IsInventoryOn;
                }
                this.m_logger.Info("Current inventory mode {0}", new object[] { flag2 });
                ResponseEventArgs args = null;
                ResponseEventArgs args2 = null;
                if (!flag && flag2)
                {
                    args = CommandHandler.GetInstance(sourceName, new StopInventoryCommand(), deviceState, this.m_device, this.m_logger).ExecuteCommand();
                    if (args.CommandError != null)
                    {
                        return new ResponseEventArgs(command, args.CommandError);
                    }
                }
                try
                {
                    args2 = handler.ExecuteCommand();
                }
                catch (SensorProviderException exception)
                {
                    this.m_logger.Error("Error {0} during command execution {1}:{2} on device {3}", new object[] { exception, command.GetType().Name, command.Id, this.m_device.DeviceName });
                    args2 = new ResponseEventArgs(command, new CommandError(LlrpErrorCode.CommandExecutionFailed, exception, exception.Message, LlrpErrorCode.CommandExecutionFailed.Description, null));
                }
                lock (deviceState)
                {
                    if (deviceState.ProviderMaintainedProperties.ContainsKey(NotificationGroup.EventModeKey))
                    {
                        flag3 = (bool)deviceState.ProviderMaintainedProperties[NotificationGroup.EventModeKey];
                    }
                }
                if (!flag && (flag3 || flag2))
                {
                    args = CommandHandler.GetInstance(sourceName, new StartInventoryCommand(), deviceState, this.m_device, this.m_logger).ExecuteCommand();
                    if (args2.CommandError != null)
                    {
                        return args2;
                    }
                    if (args.CommandError != null)
                    {
                        return new ResponseEventArgs(command, args.CommandError);
                    }
                }
                this.m_logger.Info("Returning from command handler for command {0}:{1} on device {2}", new object[] { command.GetType().Name, command.Id, this.m_device.DeviceName });
                return args2;
            }
        }

 

 


        internal static DspiCommandProcessor GetInstance(LlrpDevice llrpDevice, ILogger logger)
        {
            return new DspiCommandProcessor(llrpDevice, logger);
        }
    }




}
