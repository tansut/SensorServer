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
    public partial class FinishWizardControl : SetupConfigEditor
    {
        public FinishWizardControl()
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
        }



        public override void Retrieve(SetupConfig curent)
        {

        }

        public override string WizardMessage
        {
            get { return "End of Configuration"; }
        }
    }
}
