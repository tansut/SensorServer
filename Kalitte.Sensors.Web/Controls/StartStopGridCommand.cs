using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Web.Security;

namespace Kalitte.Sensors.Web.Controls
{
    public class StartStopGridCommand: TTGridCommand
    {
        private string StartItemCommandName;

        public StartStopGridCommand(string StartItemCommandName)
            : base()
        {
            CommandName = StartItemCommandName;
            Icon = Ext.Net.Icon.PlayBlue;
            ToolTip.Text = "Start";
        }


    }
}
