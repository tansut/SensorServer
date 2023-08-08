using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kalitte.Sensors.SetupConfiguration.Core.SetupConfigEditor;
using Kalitte.Sensors.SetupConfiguration.Core.Data;
using Kalitte.Sensors.SetupConfiguration.Helpers;
using System.IO;

namespace Kalitte.Sensors.SetupConfiguration.Controls
{
    public partial class ConfigureWizardControl : SetupConfigEditor
    {
        public ConfigureWizardControl()
        {
            InitializeComponent();
        }

        public override bool IsValid()
        {
            return true;
        }

        public override event Core.ProcessingCompletedHandler ProcessingCompleted = null;

        public override void Bind(SetupConfig curent)
        {
            Current = curent;
        }

        private void DoConfigurations()
        {
            if (MessageBox.Show("do configs", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (Current.InstallAsService)
                {
                    UpdateMessage("Installing Kalitte Sensor Server as a Windows Service...");
                    WindowsServiceHelper.InstallAndStart("KaliteSensorServer", "Kalitte Sensor Server", Path.Combine(Current.ApplicationInstallPath, @"Server\Kalitte.Sensors.Server.exe"));
                }
                UpdateMessage("Configuring provider and management port...");
                SensorConfigurationHelper sch = new SensorConfigurationHelper(Current.ApplicationInstallPath);
                sch.Configure(Current.DataProvider.Path, Current.ManagementPort.ToString());
                UpdateMessage("Creating metadata database...");
                SqlHelper.ExecuteBatchSqlCommand(Current.DataProvider.Path, ServiceHelper.ReadFile(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "MetadataScript.sql")));
                if (Current.CreateIISApplication)
                {
                    UpdateMessage("Creating IIS application pool and site.");
                    IISHelper.CreateWebSiteOnIIS(Current.WebSiteName, Current.ApplicationPool, Current.ApplicationInstallPath);
                }
                UpdateMessage("Done.");
            }
            if (ProcessingCompleted != null) ProcessingCompleted(this, new EventArgs());
        }

        private void UpdateMessage(string p)
        {
            ctlFinish.Text = p;
            Application.DoEvents();
        }

        public override bool IsProcessControl
        {
            get
            {
                return true;
            }
        }


        public override void Retrieve(SetupConfig curent)
        {
            DoConfigurations();
        }

        public override string WizardMessage
        {
            get { return "Process Configuration"; }
        }
    }
}
