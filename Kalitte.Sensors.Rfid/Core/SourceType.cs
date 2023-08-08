using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Kalitte.Sensors.Rfid.Core
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct RfidSourceType : IEquatable<RfidSourceType>
    {
        private const int UninitializedValue = 0;
        private const int AntennaValue = 1;
        private const int IOPortValue = 2;
        private const int LastValue = 2;
        public static readonly RfidSourceType Uninitialized;
        public static readonly RfidSourceType Antenna;
        public static readonly RfidSourceType IOPort;
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
        private RfidSourceType(int value)
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

        public RfidSourceType(int value, string description)
        {
            if (0 >= value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            if (value <= 2)
            {
                throw new InvalidOperationException("UseStandardCons");
            }
            this.enumValue = value;
            this.description = description;
        }

        private static void Init()
        {
            standardDescriptions = new Dictionary<int, string>();
            standardDescriptions[1] = "Antenna";
            standardDescriptions[2] = "IOPort";
            standardDescriptions[0] = "Uninitialized";
        }

        public static explicit operator RfidSourceType(int value)
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
                    return Antenna;

                case 2:
                    return IOPort;
            }
            throw new ArgumentException("NonstandardValue");
        }

        public bool IsRfid
        {
            get
            {
                return (this.enumValue == 1);
            }
        }
        public bool IsContinuousIO
        {
            get
            {
                return (this.enumValue == 2);
            }
        }
        public bool IsVendorDefined
        {
            get
            {
                return (this.enumValue > 0x3fffffff);
            }
        }
        public bool Equals(RfidSourceType other)
        {
            return (this == other);
        }

        public override bool Equals(object obj)
        {
            return ((obj is RfidSourceType) && this.Equals((RfidSourceType)obj));
        }

        public override int GetHashCode()
        {
            return this.enumValue;
        }

        public static bool operator ==(RfidSourceType providerCapability1, RfidSourceType providerCapability2)
        {
            return (providerCapability1.enumValue == providerCapability2.enumValue);
        }

        public static bool operator !=(RfidSourceType providerCapability1, RfidSourceType providerCapability2)
        {
            return (providerCapability1.enumValue != providerCapability2.enumValue);
        }

        public override string ToString()
        {
            return this.Description;
        }

        static RfidSourceType()
        {
            Uninitialized = new RfidSourceType(0);
            Antenna = new RfidSourceType(1);
            IOPort = new RfidSourceType(2);
        }
    }









}
