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
    public partial class TagDataSelect : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }




        internal TagDataSelector GetSelector()
        {
            if (ctlAll.Checked)
                return TagDataSelector.All;
            TagDataSelector selector = new TagDataSelector();
            selector.IsType = ctlTagType.Checked;
            selector.IsTime = ctlTime.Checked;
            selector.IsNumberingSystemIdentifier = ctlNumber.Checked;
            selector.IsData = ctlTagData.Checked;
            selector.IsId = ctlTagId.Checked;
            if (!selector.IsInitialized)
                throw new Exception("Invalid selection");
            return selector;
        }




    }
}