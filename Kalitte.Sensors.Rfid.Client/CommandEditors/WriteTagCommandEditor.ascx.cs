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
    public partial class WriteTagCommandEditor : System.Web.UI.UserControl, ISensorCommandEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var defaultPosition = new C1G2MemoryBankPosition(Core.C1G2MemoryBank.User, 0, 0);
                ctlMemVis.SetPosition(defaultPosition);
            }
        }

        

        #region ISensorCommandEditor Members

        public SensorCommand CreateCommand(string sensorName, string source)
        {
            var position = ctlMemVis.GetPosition();
            return new WriteTagCommand(ctlMemVis.GetPasscode(),
                ctlMemVis.GetTagId(), (int)position.MemoryBank, HexHelper.HexDecode(ctlTagData.Text), System.IO.SeekOrigin.Begin, position.Start);
        }

        public void ShowResponse(ResponseEventArgs e)
        {

        }

        #endregion


    }
}