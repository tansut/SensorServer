using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Kalitte.Sensors.Extensions
{
    public static class Extensions
    {     
        public static T ToEnum<T>(this string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }

        public static object ToEnum(this string str,Type enumType)
        {
            return Enum.Parse(enumType, str);
        }  
    }
}
