using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;
using System.Web.UI;
using Kalitte.Sensors.Web.Core;


namespace Kalitte.Sensors.Web.Controls
{
    public class TTMenuItem : MenuItem
    {
        private bool? isVisible = null;
        public bool IgnorePermisson { get; set; }
        public string ConnectedGrid { get; set; }
        public event CommandHandler Command = null;


        protected override void OnInit(EventArgs e)
        {



            this.DirectEvents.Click.Event += new ComponentDirectEvent.DirectEventHandler(Click_Event);

            base.OnInit(e);
        }

        void Click_Event(object sender, DirectEventArgs e)
        {

            CommandInfo cmd = new CommandInfo(this.CommandName, e);

            if (Command != null)
                Command(sender, cmd);

        }

        protected override void OnLoad(EventArgs e)
        {
            if (Visible && !string.IsNullOrEmpty(ConnectedGrid))
            {
                string jsFunc = "getGridSelectedId(#{" + ConnectedGrid + "})";
                Listeners.Click.Handler = "return " + jsFunc + " != null;";
                DirectEvents.Click.ExtraParams.Add(new Parameter("id", jsFunc, ParameterMode.Raw));
            }

            base.OnLoad(e);
        }



    }
}
