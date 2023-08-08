namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    
    
    
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Exceptions;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Commands;

    internal sealed class LlrpMessageVendorDefinedCommandHandler : CommandHandler
    {
        private VendorCommand m_vendorDefinedCommand;
        private static Collection<LlrpMessageType> s_messageWithNoResponse = new Collection<LlrpMessageType>();

        static LlrpMessageVendorDefinedCommandHandler()
        {
            s_messageWithNoResponse.Add(LlrpMessageType.ClientRequestOPResponse);
        }

        internal LlrpMessageVendorDefinedCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
            this.m_vendorDefinedCommand = (VendorCommand) command;
        }

        private VendorResponse CreateVendorDefinedResponse(byte[] response)
        {
            VendorData vendorDefinedReplies = new VendorData();
            vendorDefinedReplies.Add(LlrpResources.LlrpMessageResponseKey, response);
            return new VendorResponse(this.m_vendorDefinedCommand.Name, vendorDefinedReplies);
        }

        private bool DoesMessageHaveResponse(LlrpMessageBase message)
        {
            if (s_messageWithNoResponse.Contains(message.MessageType))
            {
                base.Logger.Info("Message {0} does not expect any response. Ignoring the time out expection", new object[] { message.MessageType });
                return false;
            }
            return true;
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            LlrpMessageBase message = null;
            try
            {
                if (((this.m_vendorDefinedCommand.VendorDefinedParameters == null) || (this.m_vendorDefinedCommand.VendorDefinedParameters.Count == 0)) || !this.m_vendorDefinedCommand.VendorDefinedParameters.ContainsKey(LlrpResources.LlrpMessageCommandKey))
                {
                    throw new SensorProviderException(LlrpResources.VendorDefinedCommandDoesNotContainMessage);
                }
                object obj2 = this.m_vendorDefinedCommand.VendorDefinedParameters[LlrpResources.LlrpMessageCommandKey];
                if (obj2 == null)
                {
                    throw new SensorProviderException(LlrpResources.VendorDefinedCommandDoesNotContainMessage);
                }
                if (!(obj2 is byte[]))
                {
                    throw new SensorProviderException(string.Format(CultureInfo.CurrentCulture, LlrpResources.VendorDefinedMessageNotInValidFormat, new object[] { typeof(byte[]).Name, obj2.GetType().Name }));
                }
                message = this.GetMessage((byte[]) obj2);
            }
            catch (SensorProviderException exception)
            {
                base.Logger.Error("Error {0} during decoding the vendor defined command on device {1} and source {2}", new object[] { exception, base.Device.DeviceName, base.SourceName });
                throw;
            }
            catch (Exception exception2)
            {
                throw new SensorProviderException(string.Format(CultureInfo.CurrentCulture, LlrpResources.ErrorDuringCreatingLlrpMessage, new object[] { exception2.Message }));
            }
            base.Logger.Info("Executing message {0} on device {1} and source {2}", new object[] { message, base.Device.DeviceName, base.SourceName });
            CommandError error = null;
            try
            {
                LlrpMessageBase base3 = base.Device.Request(message);
                if (base3 is LlrpMessageResponseBase)
                {
                    Util.ThrowIfFailed(((LlrpMessageResponseBase) base3).Status);
                }
                this.m_vendorDefinedCommand.Response = this.CreateVendorDefinedResponse(base3.Encode());
            }
            catch (Exception exception3)
            {
                if ((exception3 is TimeoutException) && !this.DoesMessageHaveResponse(message))
                {
                    this.m_vendorDefinedCommand.Response = this.CreateVendorDefinedResponse(null);
                }
                else
                {
                    base.Logger.Error("Llrp message execution failed. Due to {0}", new object[] { exception3 });
                    error = new CommandError(LlrpErrorCode.CommandExecutionFailed, exception3.Message, LlrpErrorCode.CommandExecutionFailed.Description, null);
                }
            }
            if (error == null)
            {
                return new ResponseEventArgs(base.Command);
            }
            return new ResponseEventArgs(base.Command, error);
        }

        private LlrpMessageBase GetMessage(byte[] message)
        {
            uint length = (uint) message.Length;
            BitArray bitArray = BitHelper.CreateBitArrayInReverseBitsOrder(message, length);
            LlrpMessageType messageType = LlrpMessageBase.GetMessageType(bitArray);
            LlrpMessageType type2 = messageType;
            if (type2 <= LlrpMessageType.ClientRequestOPResponse)
            {
                if (type2 <= LlrpMessageType.GetROSpecs)
                {
                    switch (type2)
                    {
                        case LlrpMessageType.GetReaderCapabilities:
                            return new GetReaderCapabilitiesMessage(bitArray);

                        case LlrpMessageType.GetReaderConfig:
                            return new GetReaderConfigurationMessage(bitArray);

                        case LlrpMessageType.SetReaderConfig:
                            return new SetReaderConfigurationMessage(bitArray);

                        case LlrpMessageType.CloseConnection:
                            return new CloseConnectionMessage(bitArray);

                        case (LlrpMessageType.CloseConnection | LlrpMessageType.GetReaderCapabilities):
                        case ((LlrpMessageType) 0x10):
                        case ((LlrpMessageType) 0x11):
                        case ((LlrpMessageType) 0x12):
                        case ((LlrpMessageType) 0x13):
                            goto Label_0176;

                        case LlrpMessageType.AddROSpec:
                            return new AddROSpecMessage(bitArray);

                        case LlrpMessageType.DeleteROSpec:
                            return new DeleteROSpecMessage(bitArray);

                        case LlrpMessageType.StartROSpec:
                            return new StartROSpecMessage(bitArray);

                        case LlrpMessageType.StopROSpec:
                            return new StopROSpecMessage(bitArray);

                        case LlrpMessageType.EnableROSpec:
                            return new EnableROSpecMessage(bitArray);

                        case LlrpMessageType.DisableROSpec:
                            return new DisableROSpecMessage(bitArray);

                        case LlrpMessageType.GetROSpecs:
                            return new GetROSpecMessage(bitArray);
                    }
                }
                else
                {
                    switch (type2)
                    {
                        case LlrpMessageType.AddAccessSpec:
                            return new AddAccessSpecMessage(bitArray);

                        case LlrpMessageType.DeleteAccessSpec:
                            return new DeleteAccessSpecMessage(bitArray);

                        case LlrpMessageType.EnableAccessSpec:
                            return new EnableAccessSpecMessage(bitArray);

                        case LlrpMessageType.DisableAccessSpec:
                            return new DisableAccessSpecMessage(bitArray);

                        case LlrpMessageType.GetAccessSpec:
                            return new GetAccessSpecMessage(bitArray);

                        case LlrpMessageType.ClientRequestOPResponse:
                            return new ClientRequestOPResponse(bitArray);
                    }
                }
            }
            else
            {
                switch (type2)
                {
                    case LlrpMessageType.GetReport:
                        return new GetReportMessage(bitArray);

                    case LlrpMessageType.EnableEventsAndReports:
                        return new EnableEventsAndReportsMessage(bitArray);

                    case LlrpMessageType.KeepAliveAcknowledgement:
                        return new KeepAliveMessage(bitArray);

                    case LlrpMessageType.ErrorMessage:
                        return new ErrorMessage(bitArray);

                    case LlrpMessageType.CustomMessage:
                        return CustomMessageBase.GetInstance(bitArray);
                }
            }
        Label_0176:;
        throw new SensorProviderException(string.Format(CultureInfo.CurrentCulture, LlrpResources.UnknownLlrpMessage, new object[] { messageType }));
        }

        internal override bool IsConcurrentToInventoryOperation
        {
            get
            {
                return false;
            }
        }
    }
}
