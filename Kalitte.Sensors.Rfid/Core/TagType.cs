using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Kalitte.Sensors.Rfid.Core
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct TagType : IEquatable<TagType>
    {
        private const int UninitializedValue = 0;
        private const int EpcClass0Value = 1;
        private const int EpcClass1Gen1Value = 2;
        private const int EpcClass1Gen2Value = 3;
        private const int IsoAValue = 4;
        private const int IsoBValue = 5;
        private const int Iso14443Value = 6;
        private const int Iso15693Value = 7;
        private const int BarcodeValue = 8;
        internal const int LastValue = 8;
        public static readonly TagType Uninitialized;
        public static readonly TagType EpcClass0;
        public static readonly TagType EpcClass1Gen1;
        public static readonly TagType EpcClass1Gen2;
        public static readonly TagType IsoA;
        public static readonly TagType IsoB;
        public static readonly TagType Iso14443;
        public static readonly TagType Iso15693;
        public static readonly TagType Barcode;
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
        internal TagType(int value)
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

        public TagType(int value, string description)
        {
            if (0 >= value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            if (value <= 8)
            {
                throw new InvalidOperationException("UseStandardCons");
            }
            this.enumValue = value;
            this.description = description;
        }

        private static void Init()
        {
            standardDescriptions = new Dictionary<int, string>();
            standardDescriptions[1] = "EpcClass0";
            standardDescriptions[2] = "EpcClass1Gen1";
            standardDescriptions[3] = "EpcClass1Gen2";
            standardDescriptions[4] = "ISOA";
            standardDescriptions[5] = "ISOB";
            standardDescriptions[6] = "ISO14443";
            standardDescriptions[7] = "ISO15693";
            standardDescriptions[8] = "Barcode";
            standardDescriptions[0] = "Unknown";
        }

        public static explicit operator TagType(int value)
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
                    return EpcClass0;

                case 2:
                    return EpcClass1Gen1;

                case 3:
                    return EpcClass1Gen2;

                case 4:
                    return IsoA;

                case 5:
                    return IsoB;

                case 6:
                    return Iso14443;

                case 7:
                    return Iso15693;

                case 8:
                    return Barcode;
            }
            throw new ArgumentException("NonstandardValue");
        }

        public bool Equals(TagType other)
        {
            return (this == other);
        }

        public override bool Equals(object obj)
        {
            return ((obj is TagType) && (this == ((TagType)obj)));
        }

        public override int GetHashCode()
        {
            return this.enumValue;
        }

        public static bool operator ==(TagType tagType1, TagType tagType2)
        {
            return (tagType1.enumValue == tagType2.enumValue);
        }

        public static bool operator !=(TagType tagType1, TagType tagType2)
        {
            return (tagType1.enumValue != tagType2.enumValue);
        }

        public override string ToString()
        {
            return this.Description;
        }

        static TagType()
        {
            Uninitialized = new TagType(0);
            EpcClass0 = new TagType(1);
            EpcClass1Gen1 = new TagType(2);
            EpcClass1Gen2 = new TagType(3);
            IsoA = new TagType(4);
            IsoB = new TagType(5);
            Iso14443 = new TagType(6);
            Iso15693 = new TagType(7);
            Barcode = new TagType(8);
        }
    }





}
