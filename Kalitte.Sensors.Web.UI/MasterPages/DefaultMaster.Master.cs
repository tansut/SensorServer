using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;

namespace Kalitte.Sensors.Web.UI.MasterPages
{
    public partial class DefaultMaster : System.Web.UI.MasterPage
    {
        public Ext.Net.ResourceManager ExtResourceManager
        {
            get
            {
                return Ext.Net.ResourceManager.GetInstance(this.Page);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            ExtResourceManager.AjaxViewStateMode = Ext.Net.ViewStateMode.Enabled; 
            ExtResourceManager.DirectMethodNamespace = "TT";
            ExtResourceManager.CleanResourceUrl = false;
#if DEBUG
            ExtResourceManager.ScriptMode = Ext.Net.ScriptMode.Debug;
#endif
            if (Session["Ext.Net.Theme"] == null)
                Session["Ext.Net.Theme"] = Theme.Gray;
            base.OnInit(e);
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                ExtResourceManager.RegisterIcon(Icon.Information);
                ExtResourceManager.RegisterIcon(Icon.Error);
                Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "Global", ResolveClientUrl("~/Resource/Script/kalitte.js"));
                
            }
        }
    }
}