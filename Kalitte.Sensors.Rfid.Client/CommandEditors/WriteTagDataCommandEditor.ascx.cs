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


namespace Kalitte.Sensors.Rfid.Client.CommandEditors
{
    public partial class WriteTagDataCommandEditor : System.Web.UI.UserControl, ISensorCommandEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        

        #region ISensorCommandEditor Members

        public SensorCommand CreateCommand(string sensorName, string source)
        {
            return new WriteTagDataCommand(RfidHelper.GetBytes(ctlPasscode.Text), HexHelper.HexDecode(ctlTagId.Text), HexHelper.HexDecode(ctlTagData.Text));
        }

        public void ShowResponse(ResponseEventArgs e)
        {

        }

        #endregion


    }
}