using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTDateField : DateField
    {

        public TTDateField()
        {
            Format = "dd.MM.yyyy";
        }
            

        public DateTime ValueAsDate
        {
            get
            {                
                return ((DateTime)this.Value).Date;
            }
            set
            {
                SetValue(value);
            }
        }

        public DateTime? ValueAsNullableDate
        {
            get
            {
                if (this.IsEmpty) return null;
                else return ((DateTime)this.Value).Date;
            }
            set
            {
                SetValue(value);
            }
        }
    }
}
