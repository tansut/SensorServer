namespace Kalitte.Sensors.Utilities
{
    using System;
    using System.Globalization;
    using System.Text;

    public sealed class HexHelper
    {
        private HexHelper()
        {
        }

        public static byte[] HexDecode(string input)
        {
            if (input == null)
            {
                return null;
            }
            if ((input.Length % 2) != 0)
            {
                throw new ArgumentException("Invalid length");
            }
            if (input.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
            {
                input = input.Substring(2);
            }
            byte[] buffer = new byte[input.Length / 2];
            for (int i = 0; i < input.Length; i += 2)
            {
                string s = input.Substring(i, 2);
                buffer[i / 2] = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.CurrentCulture);
            }
            return buffer;
        }

        public static string HexEncode(byte[] input)
        {
            return HexEncode(input, false);
        }

        public static string HexEncode(byte[] input, bool prefixWithHex)
        {
            StringBuilder builder;
            if (input == null)
            {
                return null;
            }
            if (prefixWithHex)
            {
                builder = new StringBuilder("0x");
            }
            else
            {
                builder = new StringBuilder();
            }
            foreach (byte num in input)
            {
                builder.Append(num.ToString("X2", CultureInfo.CurrentCulture));
            }
            return builder.ToString();
        }
    }
}
