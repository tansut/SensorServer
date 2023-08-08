using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Web.Business;
using Ext.Net;
using System.Configuration;
using Kalitte.Sensors.Configuration;
using System.Net;

namespace Kalitte.Sensors.Web.UI.Pages.Shared
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                ctlLoginHelp.Visible = ConfigurationManager.AppSettings["showInitialLoginHelp"] == "true";
                ctlPort.ValueAsInt = ServiceConfiguration.DefaultManagementServicePort;
                ctlHost.Text = Dns.GetHostName();
            }
        }

        protected void ctlLogin_Click(object sender, DirectEventArgs e)
        {
            AuthenticationBusiness.Login(ctlUsername.Text, ctlPassword.Text, ctlHost.Text, ctlPort.ValueAsInt, ctlRemember.Checked);
        }
    }
}