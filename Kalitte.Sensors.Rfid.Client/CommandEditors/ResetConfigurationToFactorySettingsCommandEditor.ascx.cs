using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.UI;
using Kalitte.Sensors.Rfid.Commands;
using System.Text;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Rfid.Core;


namespace Kalitte.Sensors.Rfid.Client.CommandEditors
{
    public partial class ResetConfigurationToFactorySettingsCommandEditor : System.Web.UI.UserControl, ISensorCommandEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {
     
        }



        #region ISensorCommandEditor Members

        public SensorCommand CreateCommand(string sensorName, string source)
        {
            return new ResetConfigurationToFactorySettingsCommand();
        }

        public void ShowResponse(ResponseEventArgs e)
        {
            if (e.CommandError != null)
            {

            }
        }

        #endregion

    }
}