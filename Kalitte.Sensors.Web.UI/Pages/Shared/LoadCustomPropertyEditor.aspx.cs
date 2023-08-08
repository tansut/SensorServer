using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Web.Core;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.UI;
using Kalitte.Sensors.Utilities;
using System.Web.Script.Serialization;

namespace Kalitte.Sensors.Web.UI.Pages.Shared
{
    public partial class LoadCustomPropertyEditor : ViewPageBase
    {
        public string PropertyType { get; set; }
        public string MetaDataType { get; set; }
        public PropertyKey Key { get; set; }



        private ICustomPropertyEditor cmdCtrl = null;


        protected void Page_Load(object sender, EventArgs e)
        {

        }


        private ICustomPropertyEditor GetCommandControl()
        {
            Type propertyType = TypesHelper.GetType(PropertyType);
            string controlPath = (propertyType.GetCustomAttributes(typeof(PropertyEditorAttribute), true)[0] as PropertyEditorAttribute).UserControlPath;
            string virtualPath = "~/Controls/PropertyEditors/" + controlPath;
            string absolutePath = Server.MapPath(virtualPath);
            Control c = Page.LoadControl(virtualPath);
            return c as ICustomPropertyEditor;
        }

        protected override void OnInit(EventArgs e)
        {
            PropertyType = Request["type"];
            MetaDataType = Request["metaDataType"];
            Key = new PropertyKey(Request["propertyGroup"], Request["propertyName"]);
            cmdCtrl = GetCommandControl();
            ctlCmdEditorHolder.Controls.Add(cmdCtrl as Control);

            base.OnInit(e);
        }

        [CommandHandler(CommandName = "SaveSettings")]
        public void ExecuteCommandHandler(object sender, CommandInfo command)
        {
            string serialized = null;

            try
            {
                serialized = cmdCtrl.EndEdit();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Ext.Net.ResourceManager.GetInstance().AddScript(string.Format("javascript:done({0});", serializer.Serialize(serialized)));
            }
            catch (Exception exc)
            {
                ctlMessage.Text = exc.Message;
            }


        }


        public void startEditing(object sender, EventArgs e)
        {
            try
            {
                cmdCtrl.Edit(ctlArgument.Value);
            }
            catch (Exception exc)
            {
                ctlMessage.Text = exc.Message;
            }
            
        }

    }
}