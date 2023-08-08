using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Web.Utility
{
    public static class ConvertHelper
    {
        public static int? GetNullable(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            else return int.Parse(text);
        }

        public static object ConvertType(Type t, object data, object originalValue)
        {
            object returnData = null;
            try
            {
                returnData = Convert.ChangeType(data, t);
            }
            catch
            {

            }

            if (returnData != null)
                return returnData;
            else return originalValue;
        }
    }
}
