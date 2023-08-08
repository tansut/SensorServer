namespace Kalitte.Sensors.Core
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ErrorCode : IEquatable<ErrorCode>
    {
        private const int UninitializedValue = 0;
        private const int TimedOutValue = 1;
        private const int RadioErrorValue = 2;
        private const int HardwareErrorValue = 3;
        private const int CommunicationErrorValue = 4;
        private const int FunctionUnsupportedValue = 5;
        private const int RebootFailedValue = 6;
        private const int UnknownErrorValue = 7;
        private const int PropertyNotFoundValue = 8;
        private const int PropertyReadOnlyValue = 9;
        private const int PropertyInvalidValue = 10;
        private const int SourceNotFoundValue = 11;
        private const int ApplyPropertyListFailedValue = 12;
        private const int LogOnFailedValue = 13;
        private const int ConnectionDownValue = 14;
        private const int NetworkDownValue = 15;
        private const int InvalidParameterValue = 16;
        private const int ParameterRequiredValue = 17;
        private const int IncompatibleFirmwareValue = 18;
        private const int TemplateNotFoundValue = 19;
        private const int NoCurrentTemplateValue = 20;
        private const int InvalidPassCodeValue = 21;
        private const int BufferOverflowValue = 22;
        private const int FlashErrorValue = 23;
        private const int TextFieldTooLongValue = 24;
        private const int LastValue = 24;

        public static readonly ErrorCode Uninitialized;
        public static readonly ErrorCode TimedOut;
        public static readonly ErrorCode RadioError;
        public static readonly ErrorCode HardwareError;
        public static readonly ErrorCode CommunicationError;
        public static readonly ErrorCode FunctionUnsupported;
        public static readonly ErrorCode RebootFailed;
        public static readonly ErrorCode UnknownError;
        public static readonly ErrorCode PropertyNotFound;
        public static readonly ErrorCode PropertyReadOnly;
        public static readonly ErrorCode PropertyInvalid;
        public static readonly ErrorCode SourceNotFound;
        public static readonly ErrorCode ApplyPropertyListFailed;
        public static readonly ErrorCode LogOnFailed;
        public static readonly ErrorCode ConnectionDown;
        public static readonly ErrorCode NetworkDown;
        public static readonly ErrorCode InvalidParameter;
        public static readonly ErrorCode ParameterRequired;
        public static readonly ErrorCode IncompatibleFirmware;
        public static readonly ErrorCode TemplateNotFound;
        public static readonly ErrorCode NoCurrentTemplate;
        public static readonly ErrorCode InvalidPassCode;
        public static readonly ErrorCode BufferOverflow;
        public static readonly ErrorCode FlashError;
        public static readonly ErrorCode TextFieldTooLong;

        private static Dictionary<int, string> standardDescriptions;
        private readonly int enumValue;
        private readonly string description;

        public int Value
        {
            get
            {
                return this.enumValue;
            }
        }
        public string Description
        {
            get
            {
                return this.description;
            }
        }
        //public bool IsBasicErrorCode
        //{
        //    get
        //    {
        //        return ((this.enumValue > 0) && (this.enumValue <= 14));
        //    }
        //}
        //public bool IsFilterErrorCode
        //{
        //    get
        //    {
        //        return ((this.enumValue > 14) & (this.enumValue <= 0x10));
        //    }
        //}
        //public bool IsPropertyErrorCode
        //{
        //    get
        //    {
        //        return ((this.enumValue > 0x10) && (this.enumValue <= 0x13));
        //    }
        //}
        //public bool IsTemplateErrorCode
        //{
        //    get
        //    {
        //        return (this.enumValue == 30);
        //    }
        //}
        //public bool IsPrinterErrorCode
        //{
        //    get
        //    {
        //        return (this.IsTemplateErrorCode || ((this.enumValue >= 0x23) && (this.enumValue <= 0x2b)));
        //    }
        //}
        //public bool IsVendorErrorCode
        //{
        //    get
        //    {
        //        return (this.enumValue > 0x3fffffff);
        //    }
        //}
        private ErrorCode(int value)
        {
            if (standardDescriptions == null)
            {
                Init();
            }
            if (0 > value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            this.enumValue = value;
            this.description = standardDescriptions[value];
        }

        public ErrorCode(int value, string description)
        {
            if (0 >= value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            if (value <= LastValue)
            {
                throw new InvalidOperationException("Invalid code");
            }
            this.enumValue = value;
            if ((description == null) || (description.Length == 0))
            {
                throw new ArgumentNullException("description");
            }
            this.description = description;
        }

        private static void Init()
        {
            standardDescriptions = new Dictionary<int, string>();
            standardDescriptions[0] = "Uninitialized";
            standardDescriptions[1] = "RequestTimeout";
            standardDescriptions[2] = "RadioError";

            standardDescriptions[3] = "HardwareError";
            standardDescriptions[4] = "CommunicationErrorCode";
            standardDescriptions[5] = "FunctionUnsupported";
            standardDescriptions[6] = "RebootFailed";
            standardDescriptions[7] = "UnknownError";
            standardDescriptions[8] = "PropertyNotFound";
            standardDescriptions[9] = "PropertyReadonly";
            standardDescriptions[10] = "InvalidPropertyValue";
            standardDescriptions[11] = "SourceNotFound";
            standardDescriptions[12] = "ApplyPropertyProfileFailed";
            standardDescriptions[13] = "LogOnFailed";
            standardDescriptions[14] = "ConnectionDown";
            standardDescriptions[15] = "NetworkDownCode";
            standardDescriptions[16] = "InvalidParameter";
            standardDescriptions[17] = "ParameterRequired";
            standardDescriptions[18] = "IncompatibleFirmware";
            standardDescriptions[19] = "TemplateNotFound";
            standardDescriptions[20] = "NoCurrentTemplate";
            standardDescriptions[21] = "InvalidPassCode";
            standardDescriptions[22] = "BufferOverflow";
            standardDescriptions[23] = "FlashError";
            standardDescriptions[24] = "TextFieldTooLong";
        }

        public static explicit operator ErrorCode(int value)
        {
            if (0 > value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            switch (value)
            {
                case 0:
                    return Uninitialized;

                case 1:
                    return TimedOut;

                case 2:
                    return RadioError;

                case 3:
                    return HardwareError;

                case 4:
                    return CommunicationError;

                case 5:
                    return FunctionUnsupported;

                case 6:
                    return RebootFailed;

                case 7:
                    return UnknownError;


                case 8:
                    return PropertyNotFound;

                case 9:
                    return PropertyReadOnly;

                case 10:
                    return PropertyInvalid;

                case 11:
                    return SourceNotFound;

                case 12:
                    return ApplyPropertyListFailed;

                case 13:
                    return LogOnFailed;

                case 14:
                    return ConnectionDown;

                case 15:
                    return NetworkDown;

                case 16:
                    return InvalidParameter;

                case 17:
                    return ParameterRequired;

                case 18:
                    return IncompatibleFirmware;

                case 19:
                    return TemplateNotFound;

                case 20:
                    return NoCurrentTemplate;

                case 21:
                    return InvalidPassCode;

                case 22:
                    return BufferOverflow;

                case 23:
                    return FlashError;

                case 24:
                    return TextFieldTooLong;
            }
            throw new ArgumentException("NonstandardValue");
        }

        public bool Equals(ErrorCode other)
        {
            return (other == this);
        }

        public override bool Equals(object obj)
        {
            return ((obj is ErrorCode) && (this == ((ErrorCode) obj)));
        }

        public override int GetHashCode()
        {
            return this.enumValue;
        }

        public static bool operator ==(ErrorCode errorCode1, ErrorCode errorCode2)
        {
            return (errorCode1.enumValue == errorCode2.enumValue);
        }

        public static bool operator !=(ErrorCode errorCode1, ErrorCode errorCode2)
        {
            return (errorCode1.enumValue != errorCode2.enumValue);
        }

        public override string ToString()
        {
            return this.description;
        }

        static ErrorCode()
        {
            Uninitialized = new ErrorCode(0);
            TimedOut = new ErrorCode(1);
            RadioError = new ErrorCode(2);
            HardwareError = new ErrorCode(3);
            CommunicationError = new ErrorCode(4);
            FunctionUnsupported = new ErrorCode(5);
            RebootFailed = new ErrorCode(6);
            UnknownError = new ErrorCode(7);
            PropertyNotFound = new ErrorCode(8);
            PropertyReadOnly = new ErrorCode(9);
            PropertyInvalid = new ErrorCode(10);
            SourceNotFound = new ErrorCode(11);
            ApplyPropertyListFailed = new ErrorCode(12);
            LogOnFailed = new ErrorCode(13);
            ConnectionDown = new ErrorCode(14);
            NetworkDown = new ErrorCode(15);
            InvalidParameter = new ErrorCode(16);
            ParameterRequired = new ErrorCode(17);
            IncompatibleFirmware = new ErrorCode(18);
            TemplateNotFound = new ErrorCode(19);
            NoCurrentTemplate = new ErrorCode(20);
            InvalidPassCode = new ErrorCode(21);
            BufferOverflow = new ErrorCode(22);
            FlashError = new ErrorCode(23);
            TextFieldTooLong = new ErrorCode(24);
        }
    }
}
