using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;


namespace Kalitte.Sensors.Web.Controls
{
    public class TTGridCommand : GridCommand
    {
        public bool Confirm { get; set; }
        public bool UseDblClick { get; set; }
        private bool? isVisible = null;

        public override ConfigOptionsCollection ConfigOptions
        {
            get
            {
                ConfigOptionsCollection coll = base.ConfigOptions;
                coll.Remove("confirm");
                coll.Remove("useDblClick");
                if (Confirm)
                {
                    coll.Add("confirm", false, true);
                }
                if (UseDblClick)
                    coll.Add("useDblClick", false, true);
                return coll;
            }
        }
    }
}
