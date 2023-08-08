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

namespace Kalitte.Sensors.SetupConfiguration.Controls
{
    public partial class ServerConfigureWizardControl : SetupConfigEditor
    {
        public ServerConfigureWizardControl()
        {
            InitializeComponent();
        }

        public override bool IsValid()
        {

            return true;
        }

        public override void Bind(SetupConfig curent)
        {
            Current = curent;
            ctlInstallasService.Checked = curent.InstallAsService;
            ctlManagementPort.Value = curent.ManagementPort;
        }



        public override void Retrieve(SetupConfig curent)
        {
            curent.InstallAsService = ctlInstallasService.Checked;
            curent.ManagementPort = (int)ctlManagementPort.Value;
        }

        public override string WizardMessage
        {
            get { return "Service Configuration"; }
        }
    }
}
