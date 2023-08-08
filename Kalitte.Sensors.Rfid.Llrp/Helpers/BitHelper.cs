using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Llrp.Core;
using System.Collections;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using System.Collections.ObjectModel;
using System.Globalization;
using Kalitte.Sensors.Rfid.Llrp.Core;
using Kalitte.Sensors.Rfid.Llrp;
using Kalitte.Sensors.Rfid.Llrp.Exceptions;

namespace Kalitte.Sensors.Rfid.Llrp.Helpers
{
    internal class BitHelper
    {
        // Fields
        private static readonly byte[] Bits = new byte[] { 1, 2, 4, 8, 0x10, 0x20, 0x40, 0x80 };
        private static readonly byte[] BitsNegated = new byte[] { 0xfe, 0xfc, 0xfb, 0xf7, 0xef, 0xdf, 0xbf, 0x7f };
        private static readonly Dictionary<LlrpParameterType, ushort> m_tvParameterLength = new Dictionary<LlrpParameterType, ushort>();

        // Methods
        static BitHelper()
        {

            m_tvParameterLength.Add(LlrpParameterType.EPC96, 13);
            m_tvParameterLength.Add(LlrpParameterType.ROSpecId, 5);
            m_tvParameterLength.Add(LlrpParameterType.SpecIndex, 3);
            m_tvParameterLength.Add(LlrpParameterType.InventoryParameterSpecId, 3);
            m_tvParameterLength.Add(LlrpParameterType.AntennaId, 3);
            m_tvParameterLength.Add(LlrpParameterType.PeakRssi, 2);
            m_tvParameterLength.Add(LlrpParameterType.ChannelIndex, 3);
            m_tvParameterLength.Add(LlrpParameterType.FirstSeenTimestampUtc, 9);
            m_tvParameterLength.Add(LlrpParameterType.FirstSeenTimestampUptime, 9);
            m_tvParameterLength.Add(LlrpParameterType.LastSeenTimestampUtc, 9);
            m_tvParameterLength.Add(LlrpParameterType.LastSeenTimestampUptime, 9);
            m_tvParameterLength.Add(LlrpParameterType.TagSeenCount, 3);
            m_tvParameterLength.Add(LlrpParameterType.ClientOperationOPSpecResult, 3);
            m_tvParameterLength.Add(LlrpParameterType.AccessSpecId, 5);
            m_tvParameterLength.Add(LlrpParameterType.OPSpecId, 3);
            m_tvParameterLength.Add(LlrpParameterType.C1G2PC, 3);
            m_tvParameterLength.Add(LlrpParameterType.C1G2Crc, 3);
            m_tvParameterLength.Add(LlrpParameterType.C1G2SingulationDetails, 5);
        }

        private BitHelper()
        {
        }

        internal static byte[] ConvertBitArrayToByteArray(BitArray bitArray, ref int startingIndex, int length, bool bigEndian)
        {
            int num3;
            if (bitArray.Length < (startingIndex + length))
            {
                throw new DecodingException("Incomplete Message", LlrpResources.InCompleteMessage);
            }
            int num = ((length - 1) / 8) + 1;
            byte[] buffer = new byte[num];
            int num2 = length;
            if (!bigEndian)
            {
                for (int j = 0; j < num; j++)
                {
                    num3 = (num2 > 7) ? 8 : num2;
                    buffer[j] = GetByte(bitArray, startingIndex, num3);
                    startingIndex += num3;
                    num2 -= num3;
                }
                return buffer;
            }
            for (int i = num - 1; i >= 0; i--)
            {
                num3 = ((num2 % 8) == 0) ? 8 : (num2 % 8);
                buffer[i] = GetByte(bitArray, startingIndex, num3);
                startingIndex += num3;
                num2 -= num3;
            }
            return buffer;
        }

        internal static ulong ConvertBitArrayToNumber(BitArray bitArray, ref int startingIndex, int length)
        {
            if (bitArray.Length < (startingIndex + length))
            {
                throw new DecodingException("Incomplete Message", LlrpResources.InCompleteMessage);
            }
            int num = ((length - 1) / 8) + 1;
            ulong num3 = 0L;
            int num4 = length;
            for (int i = num - 1; i >= 0; i--)
            {
                int num6 = ((num4 % 8) == 0) ? 8 : (num4 % 8);
                byte num2 = GetByte(bitArray, startingIndex, num6);
                num3 = num3 << 8;
                num3 += num2;
                startingIndex += num6;
                num4 -= num6;
            }
            return num3;
        }

        internal static void CopyByteToArray(byte source, uint bitToCopy, byte[] destination, ref uint bitsRemainingInByte, ref uint byteIndex)
        {
            for (int i = ((int)bitToCopy) - 1; i >= 0; i--)
            {
                if ((source & Bits[i]) == Bits[i])
                {
                    destination[byteIndex] = (byte)(destination[byteIndex] | Bits[bitsRemainingInByte - 1]);
                }
                else
                {
                    destination[byteIndex] = (byte)(destination[byteIndex] & BitsNegated[bitsRemainingInByte - 1]);
                }
                bitsRemainingInByte--;
                if (bitsRemainingInByte == 0)
                {
                    bitsRemainingInByte = 8;
                    byteIndex++;
                }
            }
        }

        internal static BitArray CreateBitArrayInReverseBitsOrder(byte[] bytes, uint bytesToConsider)
        {
            BitArray array = new BitArray((int)(bytesToConsider * 8));
            int num = 0;
            for (int i = 0; i < bytesToConsider; i++)
            {
                int index = 0;
                int num4 = 7;
                while (num4 >= 0)
                {
                    array[num + num4] = (bytes[i] & Bits[index]) == Bits[index];
                    num4--;
                    index++;
                }
                num += 8;
            }
            return array;
        }

        internal static byte[] GetByte(short[] array)
        {
            if (array == null)
            {
                return null;
            }
            byte[] buffer = new byte[array.Length * 2];
            short num = 0;
            int index = 0;
            for (int i = 0; index < array.Length; i += 2)
            {
                num = array[index];
                buffer[i + 1] = (byte)(num & 0xff);
                buffer[i] = (byte)((num & 0xff00) >> 8);
                index++;
            }
            return buffer;
        }



        private static byte GetByte(BitArray bitArray, int startingIndex, int length)
        {
            byte num = 0;
            byte num2 = 1;
            for (int i = (startingIndex + length) - 1; i >= startingIndex; i--)
            {
                if (bitArray[i])
                {
                    num = (byte)(num + num2);
                }
                num2 = (byte)(num2 << 1);
            }
            return num;
        }

        internal static T GetEnumInstance<T>(object value)
        {
            object obj2 = Enum.ToObject(typeof(T), value);
            if (!Enum.IsDefined(typeof(T), obj2))
            {
                throw new DecodingException("Invalid Enum value", string.Format(CultureInfo.CurrentCulture, LlrpResources.InvalidEnum, new object[] { value, typeof(T).ToString() }));
            }
            return (T)obj2;
        }

        internal static short[] GetInt16Array(byte[] array)
        {
            if (array == null)
            {
                return null;
            }
            int num = ((array.Length % 2) == 0) ? (array.Length / 2) : ((array.Length / 2) + 1);
            short[] numArray = new short[num];
            for (int i = 0; i < num; i++)
            {
                int startIndex = i * 2;
                int numberOfBytesToConsider = ((startIndex + 2) <= array.Length) ? 2 : 1;
                numArray[i] = (short)GetNumber(array, startIndex, numberOfBytesToConsider);
            }
            return numArray;
        }

        internal static long GetNumber(byte[] array, int startIndex, int numberOfBytesToConsider)
        {
            long num = 0L;
            for (int i = startIndex; i < (startIndex + numberOfBytesToConsider); i++)
            {
                num = num << 8;
                num |= array[i];
            }
            return num;
        }

        internal static uint GetParameterEndLimit(BitArray bitArray, ref int index)
        {
            ushort tVParameterLength;
            int num = index;
            LlrpParameterType custom = LlrpParameterType.Custom;
            if (IsTVParameter(bitArray, index))
            {
                custom = GetParameterType(bitArray, index);
                tVParameterLength = GetTVParameterLength(custom);
                index += LlrpTVParameterBase.HeaderLength;
            }
            else
            {
                index = (index + 1) + 5;
                custom = GetEnumInstance<LlrpParameterType>(ConvertBitArrayToNumber(bitArray, ref index, 10));
                tVParameterLength = (ushort)ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            }
            uint num3 = (uint)(num + (tVParameterLength * 8));
            if (num3 > bitArray.Count)
            {
                throw new DecodingException("Incomplete parameter", string.Format(CultureInfo.CurrentCulture, LlrpResources.InCompleteParameter, new object[] { custom }));
            }
            return num3;
        }

        private static LlrpParameterType GetParameterType(BitArray bitArray, int startingIndex)
        {
            int num;
            int index = startingIndex;
            if (IsTVParameter(bitArray, index))
            {
                index++;
                num = (int)ConvertBitArrayToNumber(bitArray, ref index, 7);
            }
            else
            {
                index += 6;
                num = (int)ConvertBitArrayToNumber(bitArray, ref index, 10);
            }
            return GetEnumInstance<LlrpParameterType>(num);
        }

        internal static string GetString(BitArray bitArray, ref int index, ushort byteCount)
        {
            string str = null;
            if (byteCount > 0)
            {
                str = Util.ConvertUTF8ToUnicodeString(ConvertBitArrayToByteArray(bitArray, ref index, byteCount * 8, false));
            }
            return str;
        }

        internal static ushort GetTVParameterLength(LlrpParameterType type)
        {
            if (!m_tvParameterLength.ContainsKey(type))
            {
                throw new DecodingException("Unknown TV Parameter", string.Format(CultureInfo.CurrentCulture, LlrpResources.UnKnownTVParameter, new object[] { type }));
            }
            return m_tvParameterLength[type];
        }

        internal static bool IsLLRPParameterPresent(LlrpParameterType expectedType, BitArray bitArray, int index)
        {
            return IsLLRPParameterPresent(expectedType, bitArray, index, (bitArray == null) ? 0 : ((uint)bitArray.Count));
        }

        internal static bool IsLLRPParameterPresent(LlrpParameterType expectedType, BitArray bitArray, int index, uint endLimit)
        {
            LlrpParameterType type;
            Collection<LlrpParameterType> expectedTypes = new Collection<LlrpParameterType>();
            expectedTypes.Add(expectedType);
            return IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, endLimit, out type);
        }

        internal static bool IsOneOfLLRPParameterPresent(Collection<LlrpParameterType> expectedTypes, BitArray bitArray, int index, out LlrpParameterType matchType)
        {
            return IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, (bitArray == null) ? 0 : ((uint)bitArray.Count), out matchType);
        }

        internal static bool IsOneOfLLRPParameterPresent(Collection<LlrpParameterType> expectedTypes, BitArray bitArray, int index, uint endLimit, out LlrpParameterType matchType)
        {
            matchType = LlrpParameterType.Custom;
            if (bitArray != null)
            {
                if (index <= 0)
                {
                    return false;
                }
                if (index >= bitArray.Count)
                {
                    return false;
                }
                if (index >= endLimit)
                {
                    return false;
                }
                if (IsTVParameter(bitArray, index))
                {
                    if (bitArray.Count < (index + LlrpTVParameterBase.HeaderLength))
                    {
                        return false;
                    }
                }
                else if (bitArray.Count < (index + LlrpTlvParameterBase.HeaderLength))
                {
                    return false;
                }
                LlrpParameterType parameterType = GetParameterType(bitArray, index);
                foreach (LlrpParameterType type2 in expectedTypes)
                {
                    if (type2.Equals(parameterType))
                    {
                        matchType = type2;
                        return true;
                    }
                }
            }
            return false;
        }

        internal static bool IsTVParameter(BitArray bitArray, int index)
        {
            return (ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L);
        }

        internal static void ValidateEndOfParameterOrMessage(int index, uint parameterEndLimit, string parameterName)
        {
            if (index != parameterEndLimit)
            {
                throw new DecodingException("Invalid Message", string.Format(CultureInfo.CurrentCulture, LlrpResources.InvalidMessageOrParameter, new object[] { parameterName }));
            }
        }
    }





}
