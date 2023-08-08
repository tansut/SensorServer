using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Kalitte.Sensors.Web.UI.Pages
{
    public partial class _default : BaseDashboardPage
    {
        protected override bool UseQueryStringForDashboardKey
        {
            get
            {
                return false;
            }
        }

        protected override void BindDashboard()
        {
            Dashboard.DashboardKey = "0f2a1fd4-cb57-4e15-a9ec-7d88fb310a6a";
            base.BindDashboard();
        }



 

        protected override Dashboard.Framework.DashboardSurface Dashboard
        {
            get { return surface; }
        }

         
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}