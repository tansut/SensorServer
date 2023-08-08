using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Configuration;
using Ext.Net;
using Kalitte.Sensors.Web.Business;
using Kalitte.Sensors.Web.Core;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Web.Security;

namespace Kalitte.Sensors.Web.UI.Controls.Site
{
    public partial class ItemWatchControl : System.Web.UI.UserControl, ICommandHandler
    {
        public ServerAnalyseItem ? AnalyseItem { get; set; }
        ServerAnalysisBusiness business = new ServerAnalysisBusiness();
        ControlCommandHandler handler = new ControlCommandHandler();

        public void Bind(string activeInstance = null)
        {
            watchParams.AnalyseItem = AnalyseItem;
            watchParams.Bind(activeInstance);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }




        public void Reset()
        {
            ctlWatch.Reset();
            watchParams.PanelDisabled = false;
            //ctlSourcePanel.Disabled = false;

        }

        [CommandHandler(CommandName = "OpenWatchInNewWindow")]
        public void OpenWatchInNewWindowHandler(object sender, CommandInfo command)
        {
            ctlNewWindow.ShowWindow(watchParams.ProviderName, watchParams.CategoryName, watchParams.InstanceName, new string[] { watchParams.MeasureName });
        }



        [CommandHandler(CommandName = "StartWatch")]
        public void StartWatchHandler(object sender, CommandInfo command)
        {
            if (string.IsNullOrEmpty(watchParams.ProviderName) ||
                string.IsNullOrEmpty(watchParams.CategoryName) ||
                string.IsNullOrEmpty(watchParams.InstanceName) ||
                string.IsNullOrEmpty(watchParams.MeasureName))
                throw new BusinessException("Please select parameters");
            ctlWatch.WatchName = watchParams.ProviderName;
            ctlWatch.CategoryName = watchParams.CategoryName;
            ctlWatch.InstanceName = watchParams.InstanceName;
            ctlWatch.MeasureNames = new string[] { watchParams.MeasureName };
            watchParams.PanelDisabled = true;
            ctlWatch.Start();
        }

        [CommandHandler(CommandName = "StopWatch")]
        public void StopWatchHandler(object sender, CommandInfo command)
        {
            ctlWatch.Stop();
            watchParams.PanelDisabled = false;
        }

        [CommandHandler(CommandName = "ClearWatch")]
        public void ClearWatchHandler(object sender, CommandInfo command)
        {
            ctlWatch.Clear();
        }

        #region ICommandHandler Members

        public bool ProcessCommand(object sender, CommandInfo cmd)
        {
            return handler.ProcessCommand(sender, cmd, this);
        }

        #endregion
    }
}