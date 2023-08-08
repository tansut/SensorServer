using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;

namespace Kalitte.Dashboard.Management
{
    public partial class WidgetMaximize : System.Web.UI.Page
    {

        private WidgetInstance instance = null;

        private WidgetInstance Instance
        {
            get
            {
                if (instance == null)
                {
                    string id = Request["ID"];
                    instance = Kalitte.Dashboard.Framework.DashboardFramework.GetWidgetInstance(id);
                }
                return instance;
            }
        }

        private int Width
        {
            get
            {
                return int.Parse(Request["w"]);
            }
        }

        private int Height
        {
            get
            {
                return int.Parse(Request["h"]);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                var args = new Dictionary<string, object>();
                UpdateMode temp = UpdateMode.None;
                var command = new Kalitte.Dashboard.Framework.WidgetCommandInfo() { CommandName = CommandConstant.Maximized };
                command.Arguments.Add("width", Width);
                command.Arguments.Add("height", Height);
                command.Arguments.Add("mode", MaximizeMode.External.ToString());
                WidgetContainer1.CurrentInstance = Instance;

                WidgetContainer1.DataBind(false);

                var result = WidgetContainer1.Widget.Command(Instance, command, ref temp);
                if (result == null || result.Length == 0)
                {
                    WidgetContainer1.Widget.Bind(Instance);
                }

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
