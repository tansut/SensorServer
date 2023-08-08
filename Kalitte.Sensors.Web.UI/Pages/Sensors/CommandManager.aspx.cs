using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Web.Business;
using Kalitte.Sensors.Web.Core;
using System.Drawing;
using Kalitte.Sensors.UI;
using Kalitte.Sensors.Utilities;
using System.Diagnostics;
using Kalitte.Sensors.Web.Security;

namespace Kalitte.Sensors.Web.UI.Pages.Sensors
{
    public partial class CommandManager : ViewPageBase
    {
        public string TypeToManage { get; set; }
        public string CommandName { get; set; }
        public string SensorName { get; set; }

        private ISensorCommandEditor cmdCtrl = null;
        private Type commandType = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlExecCommand.Text = string.Format("Execute <b>{0}</b>", CommandName);
                dsSensorSources.DataSource = new SensorBusiness().GetSensorSourcesAsList(SensorName);
                dsSensorSources.DataBind();
                ctlSourceList.SelectedAsString = string.Empty;
            }
        }


        private ISensorCommandEditor GetCommandControl()
        {
            Type commandType = TypesHelper.GetType(TypeToManage);
            string controlPath = (commandType.GetCustomAttributes(typeof(SensorCommandEditorAttribute), true)[0] as SensorCommandEditorAttribute).UserControlPath;
            string virtualPath = "~/Controls/CommandEditors/" + controlPath;
            string absolutePath = Server.MapPath(virtualPath);
            Control c = Page.LoadControl(virtualPath);
            return c as ISensorCommandEditor;
        }

        protected override void OnInit(EventArgs e)
        {
            TypeToManage = Request["type"];
            SensorName = Request["sensorName"];
            var parts = TypeToManage.Split('.');
            if (parts.Length > 0)
                CommandName = parts[parts.Length - 1];
            else CommandName = TypeToManage;
            cmdCtrl = GetCommandControl();
            ctlCmdEditorHolder.Controls.Add(cmdCtrl as Control);

            base.OnInit(e);
        }


        private void showError(string err)
        {
            ctlCmdError.ForeColor = Color.Red;
            ctlCmdError.Text = err;
        }

        [CommandHandler(CommandName = "ExecuteCommand")]
        public void ExecuteCommandHandler(object sender, CommandInfo command)
        {
            try
            {
                SensorBusiness bll = new SensorBusiness();
                string source = string.IsNullOrEmpty(ctlSourceList.SelectedAsString) ? null : ctlSourceList.SelectedAsString;
                SensorCommand cmd = cmdCtrl.CreateCommand(SensorName, source);
                Stopwatch watch = new Stopwatch();
                watch.Start();
                ResponseEventArgs args = bll.SensorProxy.ExecuteCommand(SensorName, source, cmd);
                watch.Stop();
                if (args.CommandError != null)
                {
                    showError(args.CommandError.Message);
                    cmdCtrl.ShowResponse(args);
                }
                else
                {
                    ctlCmdError.Text = string.Format("Successfully executed command on sensor in {0:0.00} seconds.", watch.Elapsed.TotalSeconds);
                    ctlCmdError.ForeColor = Color.Green;
                    cmdCtrl.ShowResponse(args);
                }
                ctlResultAsstringLabel.Text = args.ToString();
            }
            catch (Exception exc)
            {
                showError(ExceptionManager.Manage(exc).Message);
            }

        }
    }
}