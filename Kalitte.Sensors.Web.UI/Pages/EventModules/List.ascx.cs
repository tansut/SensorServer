using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Web.Business;

namespace Kalitte.Sensors.Web.UI.Pages.EventModules
{

    public partial class List : ListerViewControl<EventModuleBusiness>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override Web.Controls.TTStore Store
        {
            get { return dsMain; }
        }

        protected override Web.Controls.TTGrid Grid
        {
            get { return grid; }
        }
    }
}