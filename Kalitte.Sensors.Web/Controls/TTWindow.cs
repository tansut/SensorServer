using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTWindow: Window
    {
        public TTWindow()
            : base()
        {
            AutoScroll = true;
            //Modal = true;
            Collapsible = false;
            Hidden = true;
            Maximizable = true;
        }
    }
}
