using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;


namespace Kalitte.Sensors.Web.Controls
{
    public class TTNumberField: NumberField
    {

        public long? ValueAsNullableLong
        {
            get
            {                
                if (IsEmpty)
                    return null;
                else return ValueAsLong;
            }
            set
            {
                if (!value.HasValue)
                {
                    Clear();
                }
                else ValueAsLong = value.Value;
            }
        }

        public int? ValueAsNullableInt
        {
            get
            {
                if (IsEmpty)
                    return null;
                else return ValueAsInt;
            }
            set
            {
                if (!value.HasValue)
                {
                    Clear();
                }
                else ValueAsInt = value.Value;
            }
        }

        public long ValueAsLong
        {
            get
            {
                return Convert.ToInt64(Text);
            }
            set
            {
                Text = value.ToString();
            }
        }

        public int ValueAsInt
        {
            get
            {
                return Convert.ToInt32(Text);
            }
            set
            {
                Text = value.ToString();
            }
        }

        public decimal ValueAsDecimal
        {
            get
            {
                return System.Convert.ToDecimal(Value);
            }
            set
            {                
                Text = value.ToString();
            }
        }

        public decimal? ValueAsNullableDecimal
        {
            get
            {
                if (IsEmpty) return null;
                else return System.Convert.ToDecimal(Value);
            }
            set
            {
                if (!value.HasValue)
                {
                    Clear();
                }
                else ValueAsDecimal = value.Value;
            }
        }

        public double ValueAsDouble
        {
            get
            {
                return System.Convert.ToDouble(Value);
            }
            set
            {
                Text = value.ToString();
            }
        }

        public double? ValueAsNullableDouble
        {
            get
            {
                if (IsEmpty) return null;
                return System.Convert.ToDouble(Value);
            }
            set
            {
                if (!value.HasValue)
                {
                    Clear();
                }
                else ValueAsDouble = value.Value ;
               
            }
        }
    }
}
