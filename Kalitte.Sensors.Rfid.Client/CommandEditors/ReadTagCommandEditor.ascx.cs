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
    public partial class ReadTagCommandEditor : System.Web.UI.UserControl, ISensorCommandEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        #region ISensorCommandEditor Members

        public SensorCommand CreateCommand(string sensorName, string source)
        {
            var position = ctlMemVis.GetPosition();
            return new ReadTagCommand(ctlMemVis.GetPasscode(),
                ctlMemVis.GetTagId(), (int)position.MemoryBank, System.IO.SeekOrigin.Begin, position.Start, position.Length);
        }

        public void ShowResponse(ResponseEventArgs e)
        {
            if (e.CommandError == null)
            {
                var response = ((Kalitte.Sensors.Rfid.Commands.ReadTagCommand)(e.Command)).Response;
                ctlResult.Visible = true;
                ctlData.Text = HexHelper.HexEncode(response.GetReadData());
                if (response.VendorReplies != null && response.VendorReplies.Count > 0)
                {
                    ctlVendorRow.Visible = true;
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in response.VendorReplies)
                    {
                        sb.AppendFormat("{0}: {1}\n", item.Key, item.Value == null ? "null" : item.Value.ToString());
                    }
                    ctlVendor.Text = sb.ToString();
                }
                else ctlVendorRow.Visible = false;
            }
            else ctlResult.Visible = false;
        }

        #endregion


    }
}