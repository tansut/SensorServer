using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTColumn : Column
    {
        public bool FormatAsDate { get; set; }

        public override Renderer Renderer
        {
            get
            {
                if (FormatAsDate)
                {
                    Renderer r = new Renderer();
                    r.Fn = "Ext.util.Format.dateRenderer('d.m.Y H:i:s')";
                    return r;
                }

                else
                    return base.Renderer;
            }
            set
            {
                base.Renderer = value;
            }
        }


    }
}
