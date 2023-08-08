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

namespace Kalitte.Sensors.Web.UI.Pages.EventModules
{
    public partial class Management : ViewPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.DeleteEntity, ControllerType = typeof(EventModuleBusiness))]
        public void DeleteItem(object sender, CommandInfo command)
        {
            EventModuleBusiness bll = GetBusinessObject<EventModuleBusiness>();
            bll.DeleteItem(command.RecordID);
            WebHelper.ShowMessage("Event module deleted successfully.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.StartItem, ControllerType = typeof(EventModuleBusiness))]
        public void StartItem(object sender, CommandInfo command)
        {
            EventModuleBusiness bll = GetBusinessObject<EventModuleBusiness>();
            bll.ChangeState(command.RecordID, ItemState.Running);
            WebHelper.ShowMessage("Event module enabled.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.StopItem, ControllerType = typeof(EventModuleBusiness))]
        public void StopItem(object sender, CommandInfo command)
        {
            EventModuleBusiness bll = GetBusinessObject<EventModuleBusiness>();
            bll.ChangeState(command.RecordID, ItemState.Stopped);
            WebHelper.ShowMessage("Event module disabled.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }
    }
}