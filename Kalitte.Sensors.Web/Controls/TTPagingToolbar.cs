using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTPagingToolbar : PagingToolbar
    {
        public TTPagingToolbar()
            : base()
        {
            PageSize = 25;
            DisplayInfo = true;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
        }


    }
}
