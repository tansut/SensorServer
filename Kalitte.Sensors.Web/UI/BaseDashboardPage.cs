using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Dashboard.Framework;
using System.Threading;
using Kalitte.Dashboard.Framework.Types;


namespace Kalitte.Sensors.Web.UI
{
    public abstract class BaseDashboardPage : BasePage
    {

        private void CopyDashboard(string defaultDashboard)
        {
            
            DashboardInstance dashboard = DashboardFramework.GetDashboard(defaultDashboard);
            List<WidgetInstance> widgets = DashboardFramework.GetWidgetInstances(defaultDashboard);

            Guid newDashbaord = Guid.NewGuid();
            dashboard.InstanceKey = newDashbaord;

            foreach (var row in dashboard.Rows)
            {
                row.DashboardInstanceKey = dashboard.InstanceKey;
                Guid oldKey = (Guid)row.InstanceKey;
                row.InstanceKey = Guid.NewGuid();
                foreach (var widgetInstance in widgets)
                {
                    widgetInstance.DashboardKey = dashboard.InstanceKey;
                    if ((Guid)widgetInstance.RowKey == oldKey)
                        widgetInstance.RowKey = row.InstanceKey;
                }
            }

            DashboardFramework.CreateDashboard(dashboard);
            foreach (var widgetInstance in widgets)
                DashboardFramework.CreateWidget(widgetInstance);


        }

        public bool ShowUserDashboardsMenu
        {
            get
            {
                return true;
            }
        }

        public WidgetViewMode GetViewMode(DashboardSurface dashboard)
        {

            if (Thread.CurrentPrincipal.Identity.Name == dashboard.CurrentInstance.Username)
                return WidgetViewMode.Edit;
            else return WidgetViewMode.Browse;
        }

        protected virtual void BindDashboard()
        {
            Dashboard.DataBind();
            SetDashboardView(Dashboard);
            
        }


        protected override void OnLoad(EventArgs e)
        {
            if (Dashboard != null)
                try
                {
                    if (!Page.IsPostBack)
                        BindDashboard();
                }
                catch (ArgumentException) // Ivalid dashboardKey
                {
                    throw;
                }

            base.OnLoad(e);
        }


        protected virtual void SetDashboardView(DashboardSurface dashboard)
        {
            if (dashboard != null)
            {
                dashboard.ViewMode = GetViewMode(dashboard);
                dashboard.AddtoColumnText = "&nbsp;&nbsp;<b>Create</b>&nbsp;";
                dashboard.CreatatingWidgetMask = "Creating ...";
                dashboard.CreateWidgetsButtonText = "Create Widgets";
                dashboard.WidgetTypePanelTitle = "Check widgets to create in dashboard";


            }

        }

        protected virtual void SetDashboardProperties(DashboardSurface dashboard)
        {

            if (dashboard != null)
            {
                dashboard.DashboardToolbarPrepare += new DashboardToolbarPrepareHandler(dashboard_DashboardToolbarPrepare);
                dashboard.WidgetPropertiesSetting += new WidgetHandler(Dashboard_WidgetPropertiesSetting);
                dashboard.DashboardCommand += new DashboardCommandHandler(Dashboard_DashboardCommand);
                dashboard.DefaultDashboardViewUrl = "/pages/dynamic/dashboard.aspx?d={0}";
                dashboard.DashboardMenuPrepare += new DashboardMenuPrepareEventHandler(dashboard_DashboardMenuPrepare);
                dashboard.DashboardPropertiesSetting += new WidgetDashboardHandler(dashboard_DashboardPropertiesSetting);
                dashboard.ShowDashboardListPanel = false;
                dashboard.CreateInSectionText = "{0}. Section";

            }
        }





        void dashboard_DashboardPropertiesSetting(object sender, DashboardEventArgs e)
        {
            e.Instance.WidgetCreateAnimation = "fadeIn({ endOpacity: 1, easing: 'easeIn', duration: 2 })";
            e.Instance.WidgetCommandAnimation = "frame('ff0000', 1, { duration: 1 })";
            e.Instance.WidgetDropAnimation = "fadeIn({ endOpacity: 1, easing: 'easeIn', duration: 0.5 })";
            e.Instance.DashboardUpdateAnimation = "frame('C3DAF9', 1, { duration: 1 })";
            if (Dashboard.ViewMode == WidgetViewMode.Browse)
            {
                e.Instance.BodyBorder = false;
                e.Instance.Border = false;
            }

        }


        void dashboard_DashboardMenuPrepare(object sender, DashboardMenuPrepareEventArgs e)
        {
            var items = (sender as DashboardSurface).GetDefaultDashboardMenuItems();
            var userItems = items.Where(p => p.Instance.Username == (sender as DashboardSurface).GetUsername()).ToList();
            items = items.Where(p => p.Instance.Username != (sender as DashboardSurface).GetUsername()).ToList();
            var userMenu = new List<DashboardMenuItemData>(userItems.Count + 5);
            string lastGroup = "";
            int userDisplayOrder = 0;
            foreach (var userDashboard in userItems)
            {
                if (!string.IsNullOrEmpty(userDashboard.Group) && lastGroup != userDashboard.Group)
                {
                    var groupItem = new DashboardMenuItemData(userDashboard.Instance, userDashboard.Instance.ViewMode);
                    groupItem.RenderMode = DashboardMenuItemRenderMode.TextMenuItem;
                    groupItem.DisplayTitle = string.Format("<b class='menu-title'>{0}</b>", userDashboard.Instance.Group);
                    groupItem.Group = "Your Dashboards";
                    groupItem.DisplayOrder = userDisplayOrder++;
                    groupItem.GroupDisplayOrder = int.MaxValue;
                    userMenu.Add(groupItem);
                }
                lastGroup = userDashboard.Group;
                userDashboard.Group = "Your Dashboards";
                userDashboard.GroupDisplayOrder = int.MaxValue;
                userDashboard.DisplayOrder = userDisplayOrder++;
                userMenu.Add(userDashboard);
            }

            e.List.AddRange(items);
            e.List.AddRange(userMenu);

        }

        void Dashboard_DashboardCommand(object sender, DashboardCommandArgs e)
        {
            if (e.Command.CommandName == "EditDashboards")
            {
                Response.Redirect(string.Format("/Pages/Dynamic/EditDashboards.aspx?d={0}", e.Command.Arguments["key"]));
            }
        }

        void dashboard_DashboardToolbarPrepare(object sender, DashboardToolbarPrepareEventArgs e)
        {

            if ((sender as DashboardSurface).ViewMode == WidgetViewMode.Edit)
            {
                e.Toolbar.AddItem(new DashboardToolbarSeperator("ctlSeperator1"));
                DashboardToolbarButton btnEdit = new DashboardToolbarButton("ctlEditDashboard")
                {
                    Text = "Edit Dashboard",
                    Icon = WidgetIcon.PageEdit,
                    CommandName = "EditDashboards",
                    Hint = "<b>Edit</b><br/>Edit dashboard.",
                    MaskMessage = "Düzenleniyor..."
                };
                btnEdit.Arguments.Add("key", e.Instance.InstanceKey.ToString());

                e.Toolbar.AddItem(btnEdit);
            }
        }

        protected virtual bool UseQueryStringForDashboardKey
        {
            get
            {
                return false;
            }
        }


        protected override void OnInit(EventArgs e)
        {

            try
            {
                //Kalitte.Dashboard.Framework.ScriptManager.GetInstance(Page).Theme = (DashboardTheme)Enum.Parse(typeof(DashboardTheme), Theme);
                //Kalitte.Dashboard.Framework.ScriptManager.GetInstance(Page).Theme = DashboardTheme.Kalitte;
            }
            catch
            {
            }

            if (Dashboard == null)
                base.OnInit(e);

            else if (UseQueryStringForDashboardKey)
            {
                string key = Request["d"];
                bool keyValid = false;
                if (!string.IsNullOrEmpty(key))
                {
                    try
                    {
                        DashboardInstance d = DashboardFramework.GetDashboard(key);
                        keyValid = d != null;
                    }
                    catch
                    {
                        keyValid = false;
                    }
                }
                else keyValid = false;
                if (keyValid)
                    Dashboard.DashboardKey = key;
                else
                {
                    Response.Redirect("~/", true);
                }
            }

            SetDashboardProperties(Dashboard);
            base.OnInit(e);
        }




        void Dashboard_WidgetPropertiesSetting(object sender, WidgetEventArgs e)
        {
            if (((DashboardSurface)sender).ViewMode == WidgetViewMode.Browse)
            {
                if (e.Instance.HeaderDisplayMode == WidgetHeaderDisplayMode.MouseEnter)
                {
                    e.Instance.HeaderDisplayMode = WidgetHeaderDisplayMode.Always;
                    e.Instance.Header = false;
                }
            }
            else
            {
                //if (e.Instance.WidgetSettings.ContainsKey("__set0"))
                //    e.Instance.PanelSettings.Header = false;
                //if (e.Instance.WidgetSettings.ContainsKey("__set1"))
                //{
                //    e.Instance.PanelSettings.HeaderDisplayMode = WidgetHeaderDisplayMode.Always;
                //}
            }
        }

        protected abstract DashboardSurface Dashboard
        {
            get;
        }
    }

}
