namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;

    public enum StatusCode
    {
        FieldInvalid = 300,
        FieldOutOfRange = 0x12d,
        MessageDuplicateParameter = 0x68,
        MessageFieldError = 0x65,
        MessageMissingParameter = 0x67,
        MessageOverflowField = 0x6a,
        MessageOverflowParameter = 0x69,
        MessageParameterError = 100,
        MessageSuccess = 0,
        MessageUnexpectedParameter = 0x66,
        MessageUnknownField = 0x6c,
        MessageUnknownParameter = 0x6b,
        MessageUnsupportedMessage = 0x6d,
        MessageUnsupportedParameter = 0x6f,
        MessageUnsupportedVersion = 110,
        ParameterDuplicateParameter = 0xcc,
        ParameterFieldError = 0xc9,
        ParameterMissingParameter = 0xcb,
        ParameterOverflowField = 0xce,
        ParameterOverflowParameter = 0xcd,
        ParameterParameterError = 200,
        ParameterUnexpectedError = 0xca,
        ParameterUnknownField = 0xd0,
        ParameterUnknownParameter = 0xcf,
        ParameterUnsupportedParameter = 0xd1,
        ReaderDeviceError = 0x191
    }
}
