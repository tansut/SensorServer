using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTDateColumn : Ext.Net.DateColumn
    {
        public TTDateColumn()
        {
            Format = "dd.MM.yyyy";
        }
    }
}
