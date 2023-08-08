using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Web.Core;
using Kalitte.Sensors.Web.Business;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Utilities;
using Ext.Net;
using System.Collections;
using Kalitte.Sensors.Web.Security;

namespace Kalitte.Sensors.Web.UI.Controls.Site
{
    public partial class LastEventsViewer : ListerViewControl<ManagementBusiness>
    {

        public ProcessingItem ItemType { get; set; }
        public string ItemName { get { return (string)ViewState["it"]; } set { ViewState["it"] = value; } }

        IList<LastEvent> CurrentData
        {
            get
            {
                if (ViewState["cd"] == null)
                    return new List<LastEvent>();
                else return (IList<LastEvent>)SerializationHelper.BinaryDeSerialize((string)ViewState["cd"]);
            }

            set
            {
                ViewState["cd"] = SerializationHelper.BinarySerialize(value);
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

        protected void UpdateData(object sender, DirectEventArgs e)
        {
            LoadItems();
        }

        [CommandHandler(CommandName = "QueryLastEvents", ControllerType = typeof(ManagementBusiness))]
        public void QueryLastEventsHandler(object sender, CommandInfo command)
        {
            LoadItems();
        }

        [CommandHandler(CommandName = "ClearLastEventFilter", ControllerType = typeof(ManagementBusiness))]
        public void ClearLastEventFilterHandler(object sender, CommandInfo command)
        {
            ctlFilterType.Text = "";
            ctlFilterSource.Text = "";
            SetLastEventFilterHandler(sender, command);
            ctlFilterWindow.Hide();
        }


        [CommandHandler(CommandName = "SetLastEventFilter", ControllerType = typeof(ManagementBusiness))]
        public void SetLastEventFilterHandler(object sender, CommandInfo command)
        {
            LastEventFilter filter = LastEventFilter.Empty;
            if (!string.IsNullOrWhiteSpace(ctlFilterSource.Text) ||
                !string.IsNullOrWhiteSpace(ctlFilterType.Text))
            {   
                filter = new LastEventFilter(Sources, Types);
            }
            BusinessObject.SetLastEventFilter(ItemType, ItemName, filter);
            LoadItems();
            ctlFilterWindow.Hide();
        }



        HashSet<string> Sources
        {
            get
            {
                if (ViewState["Sources"] == null) ViewState["Sources"] = new HashSet<string>();
                return (HashSet<string>)ViewState["Sources"];
            }
            set
            {
                ViewState["Sources"] = value;
            }
        }



        HashSet<Type> Types
        {
            get
            {
                if (ViewState["Types"] == null) ViewState["Types"] = new HashSet<Type>();
                return (HashSet<Type>)ViewState["Types"];
            }
            set
            {
                ViewState["Types"] = value;
            }
        }


        protected void BindSources()
        {
            dsFilterSource.DataSource = Sources.Select(p => new { Source = p }).ToList();
            dsFilterSource.DataBind();
        }

        protected void BindTypes()
        {
            dsFilterType.DataSource = Types.Select(p => new { Type = p.ToString() }).ToList();
            dsFilterType.DataBind();
        }

        protected void ctlFilterSourceAdd_Click(object sender, DirectEventArgs e)
        {
            var src = ctlFilterSource.Text.Trim();
            if (!Sources.Where(p => p == src).Any()) Sources.Add(src);
            BindSources();

        }

        protected void ctlFilterTypeAdd_Click(object sender, DirectEventArgs e)
        {
            var type = TypesHelper.GetType(ctlFilterType.Text.Trim());
            if (type == null) throw new BusinessException("Unable to load type");
            if (!Types.Where(p => p == type).Any()) Types.Add(type);
            BindTypes();
        }

        [CommandHandler(CommandName = "filterSourceDelete", ControllerType = typeof(ManagementBusiness))]
        public void ctlFilterSourceGrid_Command(object sender, CommandInfo command)
        {
            Sources.RemoveWhere(p => p == command.Parameters["id"].ToString());
            BindSources();
        }

        [CommandHandler(CommandName = "filterTypeDelete", ControllerType = typeof(ManagementBusiness))]
        public void ctlFilterTypeGrid_Command(object sender, CommandInfo command)
        {
            var type = TypesHelper.GetType(command.Parameters["id"].ToString());
            if (type == null) throw new BusinessException("Unable to load type");
            Types.RemoveWhere(p => p == type);
            BindTypes();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                var sensorEvents = TypesHelper.GetTypes(typeof(SensorEventBase));
                foreach (var item in sensorEvents)
                {
                    ctlFilterType.Items.Add(new Ext.Net.ListItem(item.FullName));
                }
            }
        }

        protected void selectLastEvent(object sender, DirectEventArgs e)
        {
            var selected = CurrentData[RowSelectionModel2.SelectedIndex];
            showEvent(selected);
        }

        private void showEvent(LastEvent selected)
        {
            ctlEventType.Text = selected.Event.GetType().FullName;
            ctlEventData.Text = selected.Event.ToString();
        }


        internal void ClearEvents()
        {
            CurrentData = new List<LastEvent>();
            dsMain.DataSource = CurrentData;
            dsMain.DataBind();
            ctlEventType.Text = "";
            ctlEventData.Text = "";
        }



        DateTime LastLoad
        {
            get
            {
                if (ViewState["LastLoad"] == null) ViewState["LastLoad"] = DateTime.MinValue;
                return (DateTime)ViewState["LastLoad"];
            }
            set
            {
                ViewState["LastLoad"] = value;
            }
        }


        public override IList LoadItems()
        {
            var dt = LastLoad;
            this.PageInstance.ResMan.AddScript(string.Format("{0}.lastRefreshDate = new Date({1}, {2}, {3}, {4}, {5}, {6}, {7});", dsMain.ClientID, dt.Year, dt.Month - 1, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond));
            IList<LastEvent> source = (IList<LastEvent>)base.LoadItems();
            CurrentData = source;
            if (source.Count > 0)
            {
                showEvent(source[0]);
            }
            else
            {
                ctlEventType.Text = "";
                ctlEventData.Text = "";
            }
            var lastFilter = BusinessObject.GetLastEventFilter(ItemType, ItemName);
            if (lastFilter.IsEmpty)
            {
                ctlFilterSource.Text = "";
                ctlFilterType.Text = "";
            }
            else
            {
                Sources = lastFilter.ValidSources;
                Types = lastFilter.ValidEventTypes;
                BindSources();
                BindTypes();
            }
            if (source.Any())
                LastLoad = source.Max(p => p.EventTime);
            else LastLoad = DateTime.Now;

            return (IList)source;

        }

        protected override System.Collections.IList GetItems()
        {
            if (string.IsNullOrWhiteSpace(ItemName))
                return null;
            else
                return (IList)BusinessObject.GetLastEvents(this.ItemType, this.ItemName);
        }

        protected override Web.Controls.TTStore Store
        {
            get { return dsMain; }
        }

        protected override Web.Controls.TTGrid Grid
        {
            get { return ctlLastEventsGrid; }
        }
    }
}