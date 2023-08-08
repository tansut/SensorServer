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
    public partial class ReadTagDataCommandEditor : System.Web.UI.UserControl, ISensorCommandEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        

        #region ISensorCommandEditor Members

        public SensorCommand CreateCommand(string sensorName, string source)
        {
            return new ReadTagDataCommand(RfidHelper.GetBytes(ctlPasscode.Text), HexHelper.HexDecode(ctlTagId.Text));
        }

        public void ShowResponse(ResponseEventArgs e)
        {
            if (e.CommandError == null)
            {
                var response = ((Kalitte.Sensors.Rfid.Commands.ReadTagDataCommand)(e.Command)).Response;
                ctlResult.Visible = true;
                ctlTagData.Text = HexHelper.HexEncode(response.GetTagData());
            }
            else ctlResult.Visible = false;
        }

        #endregion


    }
}