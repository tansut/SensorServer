namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;

    internal class ErrorCodes
    {
        public const string ArgumentError = "Argument error while decoding";
        public const string C1G2TargetTagBitCountDoesnotMatch = "Mask and data count mismatch";
        public const string IncompleteMessage = "Incomplete Message";
        public const string IncompleteParameter = "Incomplete parameter";
        public const string InvalidEnumValue = "Invalid Enum value";
        public const string InvalidMessage = "Invalid Message";
        public const string InvalidMessageVersion = "Invalid Version";
        public const string MissingParameterInMessage = "Missing parameter in message";
        public const string UnknownTVParameter = "Unknown TV Parameter";
    }
}
