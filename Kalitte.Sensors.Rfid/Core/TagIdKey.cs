using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Rfid.Core
{
    public class TagIdKey
    {
        // Fields
        private byte[] m_id;

        // Methods
        public TagIdKey(byte[] id)
        {
            if (!IsValidId(id))
            {
                throw new ArgumentNullException("id");
            }
            this.m_id = id;
        }

        private static bool AreByteArraysEqual(byte[] first, byte[] second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }
            for (int i = first.Length - 1; i >= 0; i--)
            {
                if (first[i] != second[i])
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object second)
        {
            TagIdKey key = second as TagIdKey;
            if (key == null)
            {
                return false;
            }
            return AreByteArraysEqual(this.m_id, key.m_id);
        }

        public override int GetHashCode()
        {
            int num = 0x1505;
            foreach (byte num2 in this.m_id)
            {
                num = ((num << 5) + num) + num2;
            }
            return num;
        }

        public static bool IsValidId(byte[] id)
        {
            return ((id != null) && (id.Length != 0));
        }

        public override string ToString()
        {
            return HexHelper.HexEncode(this.m_id);
        }
    }




}
