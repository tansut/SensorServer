using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Rfid.Client
{
    internal static class RfidHelper
    {
        public static byte[] GetBytes(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;
            else return ASCIIEncoding.ASCII.GetBytes(data);
        }
    }
}
