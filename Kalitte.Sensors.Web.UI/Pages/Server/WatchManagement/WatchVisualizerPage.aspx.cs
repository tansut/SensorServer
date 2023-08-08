using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Web.Core;
using Kalitte.Sensors.Web.Business;

namespace Kalitte.Sensors.Web.UI.Pages.Server.WatchManagement
{
    public partial class WatchVisualizerPage : ViewPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //    StartWatchHandler(sender, null);
        }

        public string WatchName
        {
            get
            {
                return Request["WatchName"];
            }
        }

        public string CategoryName
        {
            get
            {
                return Request["CategoryName"];
            }
        }

        public string InstanceName
        {
            get
            {
                return Request["InstanceName"];
            }
        }

        public string [] MeasureNames
        {
            get
            {
                return Request["MeasureNames"].Split(',');
            }
        }

        [CommandHandler(CommandName = "StartWatch", ControllerType = typeof(ServerAnalysisBusiness))]
        public void StartWatchHandler(object sender, CommandInfo command)
        {
            ctlWatch.WatchName = WatchName;
            ctlWatch.CategoryName = CategoryName;
            ctlWatch.InstanceName = InstanceName;
            ctlWatch.MeasureNames = MeasureNames;
            ctlWatch.Start();
        }

        [CommandHandler(CommandName = "StopWatch", ControllerType = typeof(ServerAnalysisBusiness))]
        public void StopWatchHandler(object sender, CommandInfo command)
        {
            ctlWatch.Stop();
        }

        [CommandHandler(CommandName = "ClearWatch", ControllerType = typeof(ServerAnalysisBusiness))]
        public void ClearWatchHandler(object sender, CommandInfo command)
        {
            ctlWatch.Clear();
        }
    }
}