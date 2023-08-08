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
    public partial class ProviderWizardControl : SetupConfigEditor
    {
        public ProviderWizardControl()
        {
            InitializeComponent();
        }

        public override bool IsValid()
        {
            return ctlSqlProvider.IsValid();
        }

        public override void Bind(SetupConfig curent)
        {
            Current = curent;
            ctlSqlProvider.Bind(curent.DataProvider.Path);
        }



        public override void Retrieve(SetupConfig curent)
        {
            var sqlResult = ctlSqlProvider.Retrieve();
            curent.DataProvider = new Provider(sqlResult);
        }

     
        public override string WizardMessage
        {
            get { return "Provider Configuration"; }
        }
    }
}
