using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Web.Core;
using Kalitte.Sensors.Web.Business;
using Kalitte.Sensors.Web.Utility;
using Kalitte.Sensors.Processing;

namespace Kalitte.Sensors.Web.UI.Pages.LogicalSensors
{
    public partial class Management : ViewPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.DeleteEntity, ControllerType = typeof(LogicalSensorBusiness))]
        public void DeleteItem(object sender, CommandInfo command)
        {
            LogicalSensorBusiness bll = GetBusinessObject<LogicalSensorBusiness>();
            bll.DeleteItem(command.RecordID);
            WebHelper.ShowMessage("Logical sensor deleted successfully.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.StartItem, ControllerType = typeof(LogicalSensorBusiness))]
        public void StartItem(object sender, CommandInfo command)
        {
            LogicalSensorBusiness bll = GetBusinessObject<LogicalSensorBusiness>();
            bll.ChangeState(command.RecordID, ItemState.Running);
            WebHelper.ShowMessage("Logical sensor enabled.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.StopItem, ControllerType = typeof(LogicalSensorBusiness))]
        public void StopItem(object sender, CommandInfo command)
        {
            LogicalSensorBusiness bll = GetBusinessObject<LogicalSensorBusiness>();
            bll.ChangeState(command.RecordID, ItemState.Stopped);
            WebHelper.ShowMessage("Logical sensor disabled.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }
    }
}