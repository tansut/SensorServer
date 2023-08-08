using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Commands;
using System.Collections.ObjectModel;
using Kalitte.Sensors.UI;

namespace Kalitte.Sensors.Web.UI.Pages.Sensors
{
    public partial class CommandWindow : System.Web.UI.UserControl
    {
        class TypeInfo
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        internal void Execute(string sensorName)
        {
            ViewState["sensorName"] = sensorName;
            BindTypes();
            entityWindow.Show();
        }

        private void BindTypes()
        {
            var types = TypesHelper.GetTypes(typeof(SensorCommand));
            var list = new Collection<TypeInfo>();
            foreach (var item in types)
            {
                if (!item.IsAbstract && item.GetCustomAttributes(typeof(SensorCommandEditorAttribute), true).Length > 0)
                    list.Add(new TypeInfo() { Name = item.FullName, DisplayName = item.Name });
            }
            dsCommands.DataSource = list;
            dsCommands.DataBind();
        }

        protected void SelectionChange_Event(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = ctlCommandsGrid.SelectionModel.Primary as RowSelectionModel;
            var url = string.Format("{0}?sensorName={1}&type={2}",
                Page.ResolveClientUrl("~/pages/sensors/CommandManager.aspx"),
                Server.UrlEncode((string)ViewState["sensorName"]),
                sm.SelectedRecordID);
            ctlCommandPanel.AutoLoad.Url = url;
            
            ctlCommandPanel.LoadContent();
            
        }
    }
}