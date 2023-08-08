using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kalitte.Sensors.SetupConfiguration.Core;
using Kalitte.Sensors.SetupConfiguration.Core.Data;
using System.IO;
using Kalitte.Sensors.SetupConfiguration.Core.Exception;
using Kalitte.Sensors.SetupConfiguration.Core.SetupConfigEditor;
using Kalitte.Sensors.SetupConfiguration.Helpers;
using System.Threading;

namespace Kalitte.Sensors.SetupConfiguration.Controls
{
    public partial class IntroductionWizardControl : SetupConfigEditor
    {
        public IntroductionWizardControl()
            : base()
        {
            InitializeComponent();            
        }



        public override bool IsValid()
        {
            bool bindPrerequisties = false;
            if (string.IsNullOrWhiteSpace(ctlApplicationPath.Text)) throw new UserException("Application install path cannot be blank!");
            if (!Directory.Exists(ctlApplicationPath.Text.Trim())) throw new UserException("Selected folder not exist!");
            switch (Current.MsmqState)
            {
                case MSMQState.Installed:
                    if (ShowInstallDialog(MsmqHelper.MsmqDisplayName) == DialogResult.Yes)
                    {
                        MsmqHelper.StartMsmq();
                        bindPrerequisties = true;
                    }
                    break;
                case MSMQState.NotInstalled:
                    var detail = "Please check the installation web page below.\r\n\r\nhttp://msdn.microsoft.com/en-us/library/aa967729.aspx";
                    throw new UserException(string.Format("{0} must be installed.", MsmqHelper.MsmqDisplayName), detail);
            }
            switch (Current.IisState)
            {
                case IISState.Installed:
                    if (ShowInstallDialog(IISHelper.IISServiceDisplayName) == DialogResult.Yes)
                    {
                        IISHelper.StartIIS();
                        bindPrerequisties = true;
                    }
                    break;
                case IISState.NotInstalled:
                    var detail = "Please check the installation web page below.\r\n\r\nhttp://msdn.microsoft.com/en-us/library/ms143748(v=sql.90).aspx\r\n\r\nOr please download and install IIS Express from\r\n\r\nhttp://www.microsoft.com/download/en/details.aspx?id=1038";
                    throw new UserException(string.Format("{0} must be installed.", IISHelper.IISDisplayName), detail);
                case IISState.WWWPublishNotInstalled:
                    detail = "Please check the installation web page below.\r\n\r\nhttp://msdn.microsoft.com/en-us/library/ms143748(v=sql.90).aspx";
                    throw new UserException(string.Format("{0} must be installed.", IISHelper.IISServiceDisplayName), detail);
                case IISState.AspNet4NotRegistered:
                    detail = "Please check the installation web page below.\r\n\r\nhttp://msdn.microsoft.com/en-us/library/k6h9cz8h.aspx";
                    throw new UserException(string.Format("{0} must be registered.", IISHelper.AspNet4DisplayName), detail);
            }
            if (bindPrerequisties)
            {
                Thread.Sleep(1000);
                BindPrerequisties();
            }
            return true;

        }


        public DialogResult ShowInstallDialog(string serviceDisplayName)
        {
            return MessageBox.Show(string.Format("Do you want to start {0} service ?", serviceDisplayName), "Start Service", MessageBoxButtons.YesNo);
        }
        public override void Bind(SetupConfig curent)
        {
            Current = curent;
            SetEnabled(!string.IsNullOrWhiteSpace(curent.ApplicationInstallPath));
            BindPrerequisties();
        }

        private void BindPrerequisties()
        {
            ctlPrerequisites.Items.Clear();
            var state = MsmqHelper.GetMsmqState();
            Current.MsmqState = state;
            ListViewItem lvi = new ListViewItem();
            lvi.SubItems.Add(MsmqHelper.MsmqDisplayName);
            switch (state)
            {
                case MSMQState.Installed:
                    lvi.SubItems.Add("Installed");
                    lvi.ImageIndex = 1;
                    break;
                case MSMQState.InstalledAndRunning:
                    lvi.SubItems.Add("Installed and Running");
                    lvi.ImageIndex = 0;
                    break;
                case MSMQState.NotInstalled:
                    lvi.SubItems.Add("Not installed");
                    lvi.ImageIndex = 2;
                    break;
                default:
                    break;
            }
            ctlPrerequisites.Items.Add(lvi);

            var iisState = IISHelper.GetIISState();
            Current.IisState = iisState;
            ListViewItem lviIIS = new ListViewItem();
            lviIIS.SubItems.Add(IISHelper.IISDisplayName);
            switch (iisState)
            {
                case IISState.Installed:
                    lviIIS.SubItems.Add("Installed but WWW Publish Service is not started");
                    lviIIS.ImageIndex = 1;
                    break;
                case IISState.InstalledAndRunning:
                    lviIIS.SubItems.Add("Installed and Running");
                    lviIIS.ImageIndex = 0;
                    break;
                case IISState.NotInstalled:
                    lviIIS.SubItems.Add("Not installed");
                    lviIIS.ImageIndex = 2;
                    break;
                case IISState.WWWPublishNotInstalled:
                    lviIIS.SubItems.Add("IIS Installed but WWW Publish Service is not installed");
                    lviIIS.ImageIndex = 1;
                    break;
                case IISState.AspNet4NotRegistered:
                    lviIIS.SubItems.Add("IIS Installed but ASP.NET 4.0 is not registered");
                    lviIIS.ImageIndex = 1;
                    break;
                default:
                    break;
            }
            ctlPrerequisites.Items.Add(lviIIS);
        }

        private void SetEnabled(bool p)
        {
            ctlApplicationPath.ReadOnly = p;
            ctlBrowse.Enabled = !p;
            if (p) ctlApplicationPath.Text = Current.ApplicationInstallPath;
        }

        public override void Retrieve(SetupConfig curent)
        {
            curent.ApplicationInstallPath = ctlApplicationPath.Text.Trim();
            curent.MsmqState = Current.MsmqState;
        }

        public override string WizardMessage
        {
            get { return "Introduction"; }
        }

        private void ctlBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select application install folder";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ctlApplicationPath.Text = fbd.SelectedPath;
            }
        }

        private void ctlRefresh_Click(object sender, EventArgs e)
        {
            BindPrerequisties();
        }


    }
}
