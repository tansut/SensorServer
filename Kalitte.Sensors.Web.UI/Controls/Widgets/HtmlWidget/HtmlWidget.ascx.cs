/*  ----------------------------------------------------------------------------
 *  Kalitte Professional Information Technologies
 *  ----------------------------------------------------------------------------
 *  Dynamic Dashboards
 *  ----------------------------------------------------------------------------
 *  File:       HtmlWidget.ascx.cs
 *  ----------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;

namespace Kalitte.WidgetLibrary.HtmlWidget
{


    public partial class HtmlWidget : System.Web.UI.UserControl, IWidgetControl
    {
        private DashboardSurface surface;
        private WidgetInstance instance;

        protected virtual void ExecuteJs()
        {
            if (instance.WidgetSettings.ContainsKey("Js"))
            {
                Kalitte.Dashboard.Framework.ScriptManager.GetInstance(this.Page).AddScript(instance.WidgetSettings["Js"].ToString());
            }
        }

        public void Bind(WidgetInstance instance)
        {
            if (instance.SerializedData != null)
                Literal1.Text = instance.SerializedData as string;
            ExecuteJs();
            
        }



        public UpdatePanel[] Command(WidgetInstance instance, WidgetCommandInfo cmd, ref UpdateMode updateMode)
        {
            switch (cmd.CommandType)
            {
                case WidgetCommandType.Refresh:
                    {
                        Bind(instance);
                        return new UpdatePanel [] { UpdatePanel1 };
                    }
                case WidgetCommandType.SettingsChanged:
                    {
                        Bind(instance);
                        return new UpdatePanel [] { UpdatePanel1 };
                    }
                default: return null;
            }
            
        }


        public void InitControl(WidgetInitParameters parameters)
        {
            surface = parameters.Surface;
            instance = parameters.Instance;
            surface.DashboardContentsUpdated += new EventHandler(surface_DashboardContentsUpdated);
           
            
        }

        void surface_DashboardContentsUpdated(object sender, EventArgs e)
        {
            ExecuteJs();
        }


    }
}
