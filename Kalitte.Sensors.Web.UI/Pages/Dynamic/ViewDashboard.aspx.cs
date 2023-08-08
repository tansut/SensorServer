using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Kalitte.Sensors.Web.UI.Pages.Dynamic
{
    public partial class ViewDashboard : BaseDashboardPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override bool UseQueryStringForDashboardKey
        {
            get
            {
                return true;
            }
        }

        protected override Kalitte.Dashboard.Framework.DashboardSurface Dashboard
        {
            get { return surface; }
        }

        protected void surface_WidgetAdding(object sender, Dashboard.Framework.WidgetAddingEventArgs e)
        {
            
        }
    }
}
