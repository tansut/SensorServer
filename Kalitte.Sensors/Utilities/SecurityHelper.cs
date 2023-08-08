using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Kalitte.Sensors.Utilities
{
    public static class SecurityHelper
    {
        public static char[] CharArrayFromSecureString(SecureString secureString)
        {
            if (secureString == null)
            {
                return null;
            }
            char[] destination = new char[secureString.Length];
            IntPtr source = Marshal.SecureStringToCoTaskMemUnicode(secureString);
            try
            {
                Marshal.Copy(source, destination, 0, destination.Length);
            }
            finally
            {
                Marshal.ZeroFreeCoTaskMemUnicode(source);
            }
            return destination;
        }

        internal static byte[] Protect(SecureString password)
        {
            byte[] buffer2;
            if (password == null)
            {
                return null;
            }
            IntPtr zero = IntPtr.Zero;
            byte[] destination = null;
            try
            {
                destination = new byte[password.Length * 2];
                zero = Marshal.SecureStringToCoTaskMemUnicode(password);
                Marshal.Copy(zero, destination, 0, destination.Length);
                buffer2 = ProtectedData.Protect(destination, null, DataProtectionScope.CurrentUser);
            }
            finally
            {
                if (destination != null)
                {
                    Array.Clear(destination, 0, destination.Length);
                }
                if (zero != IntPtr.Zero)
                {
                    Marshal.ZeroFreeCoTaskMemUnicode(zero);
                }
            }
            return buffer2;
        }

        internal static SecureString SecureStringFromCharArray(char[] value)
        {
            if (value == null)
            {
                return null;
            }
            SecureString str = new SecureString();
            foreach (char ch in value)
            {
                str.AppendChar(ch);
            }
            str.MakeReadOnly();
            Array.Clear(value, 0, value.Length);
            return str;
        }

        internal static SecureString Unprotect(byte[] data)
        {
            SecureString str2;
            if (data == null)
            {
                return null;
            }
            byte[] bytes = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
            char[] array = null;
            try
            {
                array = new UnicodeEncoding().GetChars(bytes);
                SecureString str = new SecureString();
                foreach (char ch in array)
                {
                    str.AppendChar(ch);
                }
                str.MakeReadOnly();
                str2 = str;
            }
            finally
            {
                if (bytes != null)
                {
                    Array.Clear(bytes, 0, bytes.Length);
                }
                if (array != null)
                {
                    Array.Clear(array, 0, array.Length);
                }
            }
            return str2;
        }
    }
}
