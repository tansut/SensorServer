using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using Kalitte.Sensors.Web.Business;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Security;

namespace Kalitte.Sensors.Web.UI.Controls.Site
{
    public partial class LogViewer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                dsLogLevel.DataBind();
                ctlLogLevel.SelectedAsString = LogLevel.Off.ToString();
            }
        }

        protected void setAutoRefresh(object sender, DirectEventArgs e)
        {
            if (!ctlAutoRefresh.Checked)
                DisableAutoRefresh();
            else EnableAutoRefresh();
        }

        public void EnableAutoRefresh()
        {
            taskManager.Interval = ctlRefreshInterval.ValueAsInt;
            taskManager.StartAll();
        }

        public void DisableAutoRefresh()
        {
            taskManager.StopAll();
        }

        protected void UpdateLog(object sender, DirectEventArgs e)
        {
            Bind();
        }

        public ProcessingItem ItemType
        {
            get
            {
                return (ProcessingItem)ViewState["it"];
            }

            set
            {
                ViewState["it"] = value;
            }
        }

        public string ItemName
        {
            get
            {
                return (string)ViewState["in"];
            }

            set
            {
                ViewState["in"] = value;
            }
        }

        public void InitProperties(ProcessingItem item, string name, bool bind = false)
        {
            this.ItemType = item;
            this.ItemName = name;
            CollapseMessageWindow();
            if (bind)
                Bind();
            else Clear();
        }

        public void CollapseMessageWindow()
        {
            //ctlLocForm.DoLayout();
            //ctlMessagePanel.Collapse();
        }

        public void Bind()
        {
            LogQuery query = new LogQuery();
            query.Level = ctlLogLevel.GetSelectedAsType<LogLevel>();
            query.MaxTopItems = ctlMaxItems.ValueAsInt;
            query.MessageSearch = ctlLogSearch.Text;
            query.NameSearch = ctlNameFilter.Text;
            query.LogSet = ctlLogSet.SelectedAsString;

            var result = new LogBusiness().GetItemLog(ItemType, ItemName, query);
            List<object> logSetSource = new List<object>();
            foreach (var item in result.LogSets)
            {
                logSetSource.Add(new object[] { item });
            }
            ctlLogSet.GetStore().DataSource = logSetSource.ToArray();
            ctlLogSet.DataBind();


            List<object> nameSetSource = new List<object>();
            foreach (var item in result.NameSet)
            {
                nameSetSource.Add(new object[] { item });
            }
            ctlNameGrid.GetStore().DataSource = nameSetSource.ToArray();
            ctlNameFilter.DataBind();

            ctlNameFilter.Text = query.NameSearch;

            dsLog.DataSource = result.Log;
            dsLog.DataBind();
        }

        protected void dsLog_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bind();
        }

        internal void Clear()
        {
            dsLog.DataSource = new List<LogItemInfo>();
            dsLog.DataBind();
        }
    }
}