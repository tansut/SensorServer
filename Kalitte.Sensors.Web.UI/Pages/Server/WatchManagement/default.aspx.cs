using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using Kalitte.Sensors.Web.Business;
using Kalitte.Sensors.Web.Core;

namespace Kalitte.Sensors.Web.UI.Pages.Server.WatchManagement
{
    public partial class _default : ViewPageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                ctlAnalyseItem.Bind();
        }


    }
}