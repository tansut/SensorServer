using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Web.Business;
using Kalitte.Sensors.Processing;
using Ext.Net;
using Kalitte.Sensors.Web.Core;

namespace Kalitte.Sensors.Web.UI.Pages.Server
{
    public partial class ServerManagement : ViewPageBase
    {
        protected void ctlLogSource_Select(object sender, DirectEventArgs e)
        {
            Dictionary<ProcessingItem, IEnumerable<string>> logSources = ViewState["logSources"] as Dictionary<ProcessingItem, IEnumerable<string>>;
            var selectedSource = (ProcessingItem)Enum.Parse(typeof(ProcessingItem), ctlLogSource.SelectedAsString);
            if (ctlLogInstance.GetStore() != null)
                ctlLogInstance.GetStore().RemoveAll();
            string firstItem = null;
            foreach (var item in logSources[selectedSource])
            {
                ctlLogInstance.AddItem(item, item);
                if (firstItem == null)
                    firstItem = item;
            }
            ctlLogInstance.SetValueAndFireSelect(firstItem);
        }

        [CommandHandler(CommandName = "QueryLog", ControllerType = typeof(ProcessorBusiness))]
        public void QueryLogHandler(object sender, CommandInfo command)
        {
            var selectedSource = (ProcessingItem)Enum.Parse(typeof(ProcessingItem), ctlLogSource.SelectedAsString);
            ctlLogView.InitProperties(selectedSource, ctlLogInstance.SelectedAsString, true);
        }

        protected void ctlBindLogClick(object sender, DirectEventArgs e)
        {

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                var bll = new LogBusiness();
                var logSources = bll.GetLogSources();

                foreach (var source in logSources)
                {
                    ctlLogSource.Items.Add(new Ext.Net.ListItem(source.Key.ToString(), source.Key.ToString()));
                }
                ctlLogSource.SetValueAndFireSelect(ProcessingItem.Server.ToString());
                ViewState["logSources"] = logSources;
            }
        }
    }
}