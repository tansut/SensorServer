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
using Kalitte.Sensors.SetupConfiguration.Core.Exception;
using Kalitte.Sensors.SetupConfiguration.Helpers;

namespace Kalitte.Sensors.SetupConfiguration.Controls
{
    public partial class IISConfigureWizardControl : SetupConfigEditor
    {
        public IISConfigureWizardControl()
        {
            InitializeComponent();
        }

        public override bool IsValid()
        {
            if (ctlInstallasService.Checked)
            {
                if (string.IsNullOrWhiteSpace(ctlWebSiteName.Text)) throw new UserException("Web Site Name cannot be blank");
                if (string.IsNullOrWhiteSpace(ctlApplicationPoolName.Text)) throw new UserException("Application Pool Name cannot be blank");
                if (IISHelper.CheckWebSiteNameInUse(IISHelper.MetaBasePath, ctlWebSiteName.Text.Trim())) throw new UserException("Web Site Name is already in use");

                if (IISHelper.CheckApplicationPoolNameInUse(IISHelper.MetaBasePath, ctlApplicationPoolName.Text.Trim()))
                {
                    if (ShowInstallDialog(ctlApplicationPoolName.Text.Trim()) == DialogResult.Yes)
                    {
                        return true;
                    }
                    else throw new UserException("Application Pool Name is already in use");
                }

            }
            return true;
        }


        public DialogResult ShowInstallDialog(string appPoolName)
        {
            return MessageBox.Show(string.Format("Application Pool Name '{0}' is already in use!\n\nDo you want to use this Application Pool?", appPoolName), "Check Application Pool Name", MessageBoxButtons.YesNo);
        }
        public override void Bind(SetupConfig curent)
        {
            Current = curent;
            ctlInstallasService.Checked = curent.CreateIISApplication;
            ctlWebSiteName.Text = curent.WebSiteName;
            ctlApplicationPoolName.Text = curent.ApplicationPool;
        }



        public override void Retrieve(SetupConfig curent)
        {
            curent.WebSiteName = ctlWebSiteName.Text;
            curent.ApplicationPool = ctlApplicationPoolName.Text;
            curent.CreateIISApplication = ctlInstallasService.Checked;
        }

        public override string WizardMessage
        {
            get { return "Internet Information Services Configuration"; }
        }
    }
}
