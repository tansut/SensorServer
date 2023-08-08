using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Dashboard.Framework.Types;

namespace Kalitte.Sensors.Web.UI.Controls.Widgets.ServerAnalyse
{
    public partial class Editor : System.Web.UI.UserControl, IWidgetEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Edit(object instanceKey)
        {
            ViewState["Key"] = instanceKey;
            WidgetInstance instance = Kalitte.Dashboard.Framework.DashboardFramework.GetWidgetInstance(instanceKey);
            watchParams.Bind();
            if (instance.WidgetSettings.ContainsKey("providerName"))
            {
                watchParams.ProviderName = instance.WidgetSettings["providerName"].ToString();
                watchParams.CategoryName = instance.WidgetSettings["categoryName"].ToString();
                watchParams.InstanceName = instance.WidgetSettings["instanceName"].ToString();
                watchParams.MeasureName = instance.WidgetSettings["measureName"].ToString();
                ctlAutoStart.Checked = instance.WidgetSettings.ContainsKey("autoStart") ?
                    (bool)instance.WidgetSettings["autoStart"] : false;

            }
        }



        public bool EndEdit(Dictionary<string, object> arguments)
        {
            if (Page.IsValid)
            {
                WidgetInstance instance = Kalitte.Dashboard.Framework.DashboardFramework.GetWidgetInstance(ViewState["Key"]);
                instance.WidgetSettings["providerName"] = watchParams.ProviderName;
                instance.WidgetSettings["categoryName"] = watchParams.CategoryName;
                instance.WidgetSettings["instanceName"] = watchParams.InstanceName;
                instance.WidgetSettings["measureName"] = watchParams.MeasureName;
                instance.WidgetSettings["autoStart"] = ctlAutoStart.Checked;
                Kalitte.Dashboard.Framework.DashboardFramework.UpdateWidget(instance);
                return true;

            }
            else return false;

        }
    }
}