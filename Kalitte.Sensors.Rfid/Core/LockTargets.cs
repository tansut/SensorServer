using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Kalitte.Sensors.Rfid.Core
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct LockTargets : IEquatable<LockTargets>
    {
        private const int NoneValue = 0;
        private const int IdValue = 1;
        private const int DataValue = 2;
        private const int BothValue = 3;
        private const int LastValue = 3;
        public static readonly LockTargets None;
        public static readonly LockTargets Id;
        public static readonly LockTargets Data;
        public static readonly LockTargets Both;
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
        private LockTargets(int value)
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

        public LockTargets(int value, string description)
        {
            if (0 > value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            if (value <= 3)
            {
                throw new InvalidOperationException("UseStandardCons");
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
            standardDescriptions[1] = "TagId";
            standardDescriptions[2] = "TagData";
            standardDescriptions[3] = "TagIdAndData";
        }

        public static explicit operator LockTargets(int value)
        {
            if (0 > value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            switch (value)
            {
                case 0:
                    return None;

                case 1:
                    return Id;

                case 2:
                    return Data;

                case 3:
                    return Both;
            }
            throw new ArgumentException("NonstandardValue");
        }

        public bool Equals(LockTargets other)
        {
            return (other == this);
        }

        public override bool Equals(object obj)
        {
            return ((obj is LockTargets) && this.Equals((LockTargets)obj));
        }

        public override int GetHashCode()
        {
            return this.enumValue;
        }

        public static bool operator ==(LockTargets lockTargets1, LockTargets lockTargets2)
        {
            return (lockTargets1.enumValue == lockTargets2.enumValue);
        }

        public static bool operator !=(LockTargets lockTargets1, LockTargets lockTargets2)
        {
            return (lockTargets1.enumValue != lockTargets2.enumValue);
        }

        public override string ToString()
        {
            return this.description;
        }

        static LockTargets()
        {
            None = new LockTargets(0);
            Id = new LockTargets(1);
            Data = new LockTargets(2);
            Both = new LockTargets(3);
        }
    }




}
