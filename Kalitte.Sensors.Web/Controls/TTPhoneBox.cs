using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTPhoneBox : Ext.Net.TextField
    {
        public TTPhoneBox()
            : base()
        {
            this.MaskRe = @"[0-9]";
            this.RegexText = "Lütfen geçerli bir numara giriniz."; 
        }
    }
}
