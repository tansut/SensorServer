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

namespace Kalitte.Sensors.Web.UI.Pages.Dispatchers
{
    public partial class Management : ViewPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.DeleteEntity, ControllerType = typeof(DispatcherBusiness))]
        public void DeleteItem(object sender, CommandInfo command)
        {
            DispatcherBusiness bll = GetBusinessObject<DispatcherBusiness>();
            bll.DeleteItem(command.RecordID);
            WebHelper.ShowMessage("Dispatcher deleted successfully.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.StartItem, ControllerType = typeof(DispatcherBusiness))]
        public void StartItem(object sender, CommandInfo command)
        {
            DispatcherBusiness bll = GetBusinessObject<DispatcherBusiness>();
            bll.ChangeState(command.RecordID, ItemState.Running);
            WebHelper.ShowMessage("Dispatcher started.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.StopItem, ControllerType = typeof(DispatcherBusiness))]
        public void StopItem(object sender, CommandInfo command)
        {
            DispatcherBusiness bll = GetBusinessObject<DispatcherBusiness>();
            bll.ChangeState(command.RecordID, ItemState.Stopped);
            WebHelper.ShowMessage("Dispatcher stopped.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }
    }
}