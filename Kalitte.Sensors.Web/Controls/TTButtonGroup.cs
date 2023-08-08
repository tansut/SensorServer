using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTButtonGroup: ButtonGroup
    {
        protected override void OnPreRender(EventArgs e)
        {
            Visible = Items.Any(p => p.Visible);
            base.OnPreRender(e);
        }
    }
}
