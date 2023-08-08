using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using System.ServiceModel.Channels;
using Kalitte.Sensors.Rfid.Llrp.Core;
using Kalitte.Sensors.Rfid.Llrp.Helpers;
using System.Collections;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Security;

namespace Kalitte.Sensors.Rfid.Llrp.Communication
{
    internal sealed class LlrpBinaryEncoder : LlrpBinaryEncoderBase
    {
        // Methods
        internal LlrpBinaryEncoder(ILogger logger)
            : base(logger)
        {
        }

        internal static LlrpMessageBase GetLlrpMessage(byte[] message, uint length, ILogger logger)
        {
            BitArray bitArray = BitHelper.CreateBitArrayInReverseBitsOrder(message, length);
            LlrpMessageType messageType = LlrpMessageBase.GetMessageType(bitArray);
            LogMessage(logger, LogLevel.Verbose, "Decoding message of the type {0}", new object[] { messageType });
            LlrpMessageType type2 = messageType;
            if (type2 <= LlrpMessageType.GetAccessSpecResponse)
            {
                switch (type2)
                {
                    case LlrpMessageType.GetReaderCapabilitiesResponse:
                        return new GetReaderCapabilitiesResponse(bitArray);

                    case LlrpMessageType.GetReaderConfigResponse:
                        return new GetReaderConfigurationResponse(bitArray);

                    case LlrpMessageType.SetReaderConfigResponse:
                        return new SetReaderConfigurationResponse(bitArray);

                    case LlrpMessageType.CloseConnectionResponse:
                        return new CloseConnectionResponse(bitArray);

                    case LlrpMessageType.AddROSpecResponse:
                        return new AddROSpecResponse(bitArray);

                    case LlrpMessageType.DeleteROSpecResponse:
                        return new DeleteROSpecResponse(bitArray);

                    case LlrpMessageType.StartROSpecResponse:
                        return new StartROSpecResponse(bitArray);

                    case LlrpMessageType.StopROSpecResponse:
                        return new StopROSpecResponse(bitArray);

                    case LlrpMessageType.EnableROSpecResponse:
                        return new EnableROSpecResponse(bitArray);

                    case LlrpMessageType.DisableROSpecResponse:
                        return new DisableROSpecResponse(bitArray);

                    case LlrpMessageType.GetROSpecsResponse:
                        return new GetROSpecResponse(bitArray);

                    case (LlrpMessageType.GetROSpecsResponse | LlrpMessageType.GetReaderCapabilities):
                    case (LlrpMessageType.GetROSpecsResponse | LlrpMessageType.GetReaderConfig):
                    case (LlrpMessageType.GetROSpecsResponse | LlrpMessageType.SetReaderConfig):
                    case LlrpMessageType.AddAccessSpec:
                    case LlrpMessageType.DeleteAccessSpec:
                    case LlrpMessageType.EnableAccessSpec:
                    case LlrpMessageType.DisableAccessSpec:
                    case LlrpMessageType.GetAccessSpec:
                    case (LlrpMessageType.GetAccessSpec | LlrpMessageType.GetReaderConfig):
                    case (LlrpMessageType.ClientRequestOP | LlrpMessageType.GetReaderConfig):
                    case ((LlrpMessageType)0x30):
                    case ((LlrpMessageType)0x31):
                        goto Label_018F;

                    case LlrpMessageType.ClientRequestOP:
                        return new ClientRequestOP(bitArray);

                    case LlrpMessageType.AddAccessSpecResponse:
                        return new AddAccessSpecResponse(bitArray);

                    case LlrpMessageType.DeleteAccessSpecResponse:
                        return new DeleteAccessSpecResponse(bitArray);

                    case LlrpMessageType.EnableAccessSpecResponse:
                        return new EnableAccessSpecResponse(bitArray);

                    case LlrpMessageType.DisableAccessSpecResponse:
                        return new DisableAccessSpecResponse(bitArray);

                    case LlrpMessageType.GetAccessSpecResponse:
                        return new GetAccessSpecResponse(bitArray);
                }
            }
            else
            {
                switch (type2)
                {
                    case LlrpMessageType.ROAccessReport:
                        return new ROAccessReport(bitArray);

                    case LlrpMessageType.KeepAlive:
                        return new KeepAliveMessage(bitArray);

                    case LlrpMessageType.ReaderEventNotification:
                        return new ReaderEventNotificationMessage(bitArray);

                    case LlrpMessageType.ErrorMessage:
                        return new ErrorMessage(bitArray);

                    case LlrpMessageType.CustomMessage:
                        return CustomMessageBase.GetInstance(bitArray);
                }
            }
        Label_018F: ;
            LogMessage(logger, LogLevel.Error, "Unsupported message type in the encoder class {0}", new object[] { messageType });
            return null;
        }

        protected override Message GetMessage(byte[] message, uint length)
        {
            return GetLlrpMessage(message, length, base.m_logger);
        }

        private static void LogMessage(ILogger logger, LogLevel logLevel, string formatMessage, params object[] obj)
        {
            if (logger != null)
            {
                logger.Log(logLevel, formatMessage, obj);
            }
        }
    }




}
