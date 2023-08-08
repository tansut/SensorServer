namespace Kalitte.Sensors.Events.Management
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct EventType : IEquatable<EventType>
    {
        private const int UninitializedValue = 0;
        private const int PowerSupplyUpValue = 1;
        private const int PowerSupplyDownValue = 2;
        private const int NotificationChannelUpValue = 3;
        private const int NotificationChannelDownValue = 4;
        private const int FreeMemoryNormalValue = 5;
        private const int FreeMemoryLowValue = 6;
        private const int IOPortUpValue = 7;
        private const int IOPortDownValue = 8;
        private const int SourceUpValue = 9;
        private const int SourceDownValue = 10;
        private const int NetworkUpValue = 11;
        private const int NetworkDownValue = 12;
        private const int CommunicationNormalValue = 13;
        private const int CommunicationErrorValue = 14;
        private const int DeviceNameChangedValue = 15;
        private const int FailedReadValue = 0x10;
        private const int SourceNoiseLevelNormalValue = 0x11;
        private const int SourceNoiseLevelHighValue = 0x12;
        private const int BatteryNormalValue = 0x13;
        private const int BatteryLowValue = 20;
        private const int DeviceConnectionClosedValue = 0x15;
        private const int ProviderDefunctValue = 0x16;
        private const int FirmwareUpgradeProgressValue = 0x17;
        private const int LastValue = 0x17;
        public static readonly EventType Uninitialized;
        public static readonly EventType PowerSupplyUp;
        public static readonly EventType PowerSupplyDown;
        public static readonly EventType NotificationChannelUp;
        public static readonly EventType NotificationChannelDown;
        public static readonly EventType FreeMemoryNormal;
        public static readonly EventType FreeMemoryLow;
        public static readonly EventType IOPortUp;
        public static readonly EventType IOPortDown;
        public static readonly EventType SourceUp;
        public static readonly EventType SourceDown;
        public static readonly EventType NetworkUp;
        public static readonly EventType NetworkDown;
        public static readonly EventType CommunicationNormal;
        public static readonly EventType CommunicationError;
        public static readonly EventType DeviceNameChanged;
        public static readonly EventType FailedRead;
        public static readonly EventType SourceNoiseLevelNormal;
        public static readonly EventType SourceNoiseLevelHigh;
        public static readonly EventType BatteryNormal;
        public static readonly EventType BatteryLow;
        public static readonly EventType DeviceConnectionClosed;
        public static readonly EventType ProviderDefunct;
        public static readonly EventType FirmwareUpgradeProgress;
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
        public bool IsTagRelated
        {
            get
            {
                return (this.enumValue == 0x10);
            }
        }
        public bool IsDeviceRelated
        {
            get
            {
                if (((((this.enumValue != 20) && (this.enumValue != 0x13)) && ((this.enumValue != 15) && (this.enumValue != 5))) && (((this.enumValue != 6) && (this.enumValue != 1)) && ((this.enumValue != 2) && (this.enumValue != 7)))) && ((this.enumValue != 8) && (this.enumValue != 0x17)))
                {
                    return (this.enumValue == 0x15);
                }
                return true;
            }
        }
        public bool IsCommunicationRelated
        {
            get
            {
                if (((this.enumValue != 11) && (this.enumValue != 12)) && ((this.enumValue != 13) && (this.enumValue != 14)))
                {
                    return (this.enumValue == 0x15);
                }
                return true;
            }
        }
        public bool IsSourceRelated
        {
            get
            {
                if (((this.enumValue != 9) && (this.enumValue != 10)) && (this.enumValue != 0x11))
                {
                    return (this.enumValue == 0x12);
                }
                return true;
            }
        }
        public bool IsNotificationRelated
        {
            get
            {
                if (this.enumValue != 3)
                {
                    return (this.enumValue == 4);
                }
                return true;
            }
        }
        public bool IsProviderRelated
        {
            get
            {
                return (this.enumValue == 0x16);
            }
        }
        private EventType(int value)
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

        public EventType(int value, string description)
        {
            if (0 >= value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            if (value <= 0x17)
            {
                throw new InvalidOperationException("UseStandardCons");
            }
            this.enumValue = value;
            this.description = description;
        }

        private static void Init()
        {
            standardDescriptions = new Dictionary<int, string>();
            standardDescriptions[0] = "Uninitialized";
            standardDescriptions[1] = "PowerReturned";
            standardDescriptions[2] = "PowerDown";
            standardDescriptions[3] = "NotificationChannelUp";
            standardDescriptions[4] = "NotificationChannelDown";
            standardDescriptions[5] = "FreeMemoryNormal";
            standardDescriptions[6] = "FreeMemoryLow";
            standardDescriptions[7] = "IOPortUp";
            standardDescriptions[8] = "IOPortDown";
            standardDescriptions[9] = "SourceUp";
            standardDescriptions[10] = "SourceDown";
            standardDescriptions[11] = "NetworkUp";
            standardDescriptions[12] = "NetworkDownEvent";
            standardDescriptions[13] = "CommunicationNormal";
            standardDescriptions[14] = "CommunicationErrorEvent";
            standardDescriptions[15] = "DeviceNameChanged";
            standardDescriptions[0x10] = "FailedRead";
            standardDescriptions[0x11] = "AntennaNoiseNormal";
            standardDescriptions[0x12] = "AntennaNoiseHigh";
            standardDescriptions[0x13] = "BatteryNormal";
            standardDescriptions[20] = "BatteryLow";
            standardDescriptions[0x15] = "DeviceClosed";
            standardDescriptions[0x16] = "ProviderDefunct";
            standardDescriptions[0x17] = "FirmwareUpgradeProgress";
        }

        public static explicit operator EventType(int value)
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
                    return PowerSupplyUp;

                case 2:
                    return PowerSupplyDown;

                case 3:
                    return NotificationChannelUp;

                case 4:
                    return NotificationChannelDown;

                case 5:
                    return FreeMemoryNormal;

                case 6:
                    return FreeMemoryLow;

                case 7:
                    return IOPortUp;

                case 8:
                    return IOPortDown;

                case 9:
                    return SourceUp;

                case 10:
                    return SourceDown;

                case 11:
                    return NetworkUp;

                case 12:
                    return NetworkDown;

                case 13:
                    return CommunicationNormal;

                case 14:
                    return CommunicationError;

                case 15:
                    return DeviceNameChanged;

                case 0x10:
                    return FailedRead;

                case 0x11:
                    return SourceNoiseLevelNormal;

                case 0x12:
                    return SourceNoiseLevelHigh;

                case 0x13:
                    return BatteryNormal;

                case 20:
                    return BatteryLow;

                case 0x15:
                    return DeviceConnectionClosed;

                case 0x16:
                    return ProviderDefunct;

                case 0x17:
                    return FirmwareUpgradeProgress;
            }
            throw new ArgumentException("NonstandardValue");
        }

        public bool Equals(EventType other)
        {
            return (this == other);
        }

        public override bool Equals(object obj)
        {
            return ((obj is EventType) && (this == ((EventType) obj)));
        }

        public override int GetHashCode()
        {
            return this.enumValue;
        }

        public static bool operator ==(EventType tagType1, EventType tagType2)
        {
            return (tagType1.enumValue == tagType2.enumValue);
        }

        public static bool operator !=(EventType tagType1, EventType tagType2)
        {
            return (tagType1.enumValue != tagType2.enumValue);
        }

        public override string ToString()
        {
            return this.description;
        }

        static EventType()
        {
            Uninitialized = new EventType(0);
            PowerSupplyUp = new EventType(1);
            PowerSupplyDown = new EventType(2);
            NotificationChannelUp = new EventType(3);
            NotificationChannelDown = new EventType(4);
            FreeMemoryNormal = new EventType(5);
            FreeMemoryLow = new EventType(6);
            IOPortUp = new EventType(7);
            IOPortDown = new EventType(8);
            SourceUp = new EventType(9);
            SourceDown = new EventType(10);
            NetworkUp = new EventType(11);
            NetworkDown = new EventType(12);
            CommunicationNormal = new EventType(13);
            CommunicationError = new EventType(14);
            DeviceNameChanged = new EventType(15);
            FailedRead = new EventType(0x10);
            SourceNoiseLevelNormal = new EventType(0x11);
            SourceNoiseLevelHigh = new EventType(0x12);
            BatteryNormal = new EventType(0x13);
            BatteryLow = new EventType(20);
            DeviceConnectionClosed = new EventType(0x15);
            ProviderDefunct = new EventType(0x16);
            FirmwareUpgradeProgress = new EventType(0x17);
        }
    }
}
