/*  ----------------------------------------------------------------------------
 *  Kalitte Professional Information Technologies
 *  ----------------------------------------------------------------------------
 *  Dynamic Dashboards
 *  ----------------------------------------------------------------------------
 *  File:       Editor.ascx.cs
 *  ----------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Dashboard.Framework.Types;

namespace Kalitte.WidgetLibrary.HtmlWidget
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
            TextBox1.Text = instance.SerializedData == null ? "" :
                instance.SerializedData.ToString();
            if (instance.WidgetSettings.ContainsKey("Js"))
                TextBox2.Text = instance.WidgetSettings["Js"].ToString();

        }



        public bool EndEdit(Dictionary<string, object> arguments)
        {

            if (Page.IsValid)
            {


                WidgetInstance instance = Kalitte.Dashboard.Framework.DashboardFramework.GetWidgetInstance(ViewState["Key"]);
                instance.SerializedData = TextBox1.Text;
                instance.WidgetSettings["Js"] = TextBox2.Text;
                Kalitte.Dashboard.Framework.DashboardFramework.UpdateWidget(instance);
                return true;

            }
            else return false;

        }



    }
}
