using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTEmailBox:Ext.Net.TextField
    {
        public TTEmailBox():base()
        {
            this.MaskRe=@"[a-zA-Z0-9_\.\-@]" ;
            this.Regex = @"^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            this.RegexText = "Invalid e-mail address";            
        }
    }
}
