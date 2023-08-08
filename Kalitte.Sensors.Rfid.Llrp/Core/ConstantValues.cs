namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;

    internal class ConstantValues
    {
        internal const string AntennaPrefix = "Antenna_";
        internal const byte BufferPercentageMaximum = 100;
        internal const byte BufferPercentageMinimum = 0;
        internal const ushort C1G2AccessPasswordStartWordPosition = 2;
        internal const ushort C1G2CodeLengthBytes = 4;
        internal const ushort C1G2EpcStartWordPosition = 2;
        internal const ushort C1G2KillPasswordStartWordPosition = 0;
        internal const string GpiPrefix = "GPI_";
        internal const string GpoPrefix = "GPO_";
        internal const int LlrpInboundPort = 0x13dc;
        internal const int LlrpOutboundPort = 0x13dd;
        internal const string LlrpScheme = "llrp.bin";
        internal static readonly DateTime LlrpUtcStartTime = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        internal const uint MaximumMessageSubtype = 0xff;
        internal static readonly ushort MaximumUserByteDataInCustomParameter = ((ushort) (0xffff - (((LlrpTlvParameterBase.HeaderLength + 0x20) + 0x20) / 8)));
        internal const byte MessageVersion = 1;
        internal const uint MicrosoftIANA = 0x137;
        internal const uint MinimumMessageSubtype = 0;
        internal const byte PriorityMaximum = 7;
        internal const byte PriorityMinimum = 0;
        internal const byte ReceiveSensitivtyMax = 0x80;
        internal const byte ReceiveSensitivtyMin = 0;
        internal const ulong ReservedValue = 0L;
        internal const sbyte RssiMaximum = 0x7f;
        internal const sbyte RssiMinimum = -128;
        internal const byte TLVParameterReservedByteValue = 0;
        internal const byte TVParameterReservedByteValue = 1;
    }
}
