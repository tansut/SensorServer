using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Sensors.Web.Core;
using Kalitte.Sensors.Web.Security;
using Kalitte.Dashboard.Framework;

namespace Kalitte.Sensors.Web.UI.Controls.Widgets.ServerAnalyse
{
    public partial class View : System.Web.UI.UserControl, IWidgetControl, ICommandHandler
    {
        ControlCommandHandler handler = new ControlCommandHandler();
        DashboardSurface surface;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [CommandHandler(CommandName = "OpenWatchInNewWindow")]
        public void OpenWatchInNewWindowHandler(object sender, CommandInfo command)
        {
            //ctlNewWindow.ShowWindow(watchParams.ProviderName, watchParams.CategoryName, watchParams.InstanceName, new string[] { watchParams.MeasureName });
        }



        [CommandHandler(CommandName = "StartWatch")]
        public void StartWatchHandler(object sender, CommandInfo command)
        {
            //if (string.IsNullOrEmpty(watchParams.ProviderName) ||
            //    string.IsNullOrEmpty(watchParams.CategoryName) ||
            //    string.IsNullOrEmpty(watchParams.InstanceName) ||
            //    string.IsNullOrEmpty(watchParams.MeasureName))
            //    throw new BusinessException("Please select parameters");

            ctlWatch.Start();
        }

        [CommandHandler(CommandName = "StopWatch")]
        public void StopWatchHandler(object sender, CommandInfo command)
        {
            ctlWatch.Stop();
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

        #region IWidgetControl Members

        public void Bind(WidgetInstance instance)
        {
            if (instance.WidgetSettings.ContainsKey("providerName"))
            {
                bool restart = false;
                if (ctlWatch.IsRunning)
                {
                    ctlWatch.Reset();
                    restart = true;
                }
                ctlWatch.WatchName = instance.WidgetSettings["providerName"].ToString();
                ctlWatch.CategoryName = instance.WidgetSettings["categoryName"].ToString();
                ctlWatch.InstanceName = instance.WidgetSettings["instanceName"].ToString();
                ctlWatch.MeasureNames =  new string[] {  instance.WidgetSettings["measureName"].ToString() };
                bool autoStart = instance.WidgetSettings.ContainsKey("autoStart") ?
                    (bool)instance.WidgetSettings["autoStart"] : false;
                if (instance.Height.HasValue)
                    ctlWatch.Height = instance.Height.Value;
                if (restart)
                    ctlWatch.Start();
            }
        }

        public UpdatePanel[] Command(WidgetInstance instance, Dashboard.Framework.WidgetCommandInfo commandData, ref UpdateMode updateMode)
        {
            switch (commandData.CommandType)
            {
                case WidgetCommandType.Refresh:
                    Bind(instance);
                    break;

                case WidgetCommandType.SettingsChanged:
                    Bind(instance);
                    break;

                default:
                    break;
            }
            return null;
        }

        public void InitControl(Dashboard.Framework.WidgetInitParameters parameters)
        {
            surface = parameters.Surface;
            surface.WidgetAdded += new WidgetHandler(surface_WidgetAdded);
        }

        void surface_WidgetAdded(object sender, WidgetEventArgs e)
        {
            surface.ReloadPage();
        }

        #endregion
    }
}