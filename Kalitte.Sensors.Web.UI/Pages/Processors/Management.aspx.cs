using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Web.Business;
using Kalitte.Sensors.Web.Core;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Web.Utility;
using Kalitte.Sensors.Processing;

namespace Kalitte.Sensors.Web.UI.Pages.Processors
{
    public partial class Management : ViewPageBase
    {

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.DeleteEntity, ControllerType = typeof(ProcessorBusiness))]
        public void DeleteItem(object sender, CommandInfo command)
        {
            var bll = GetBusinessObject<ProcessorBusiness>();
            bll.DeleteItem(command.RecordID);
            WebHelper.ShowMessage("Processor deleted successfully.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.StartItem, ControllerType = typeof(ProcessorBusiness))]
        public void StartItem(object sender, CommandInfo command)
        {
            var bll = GetBusinessObject<ProcessorBusiness>();
            bll.ChangeState(command.RecordID, ItemState.Running);
            WebHelper.ShowMessage("Processor started.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.StopItem, ControllerType = typeof(ProcessorBusiness))]
        public void StopItem(object sender, CommandInfo command)
        {
            var bll = GetBusinessObject<ProcessorBusiness>();
            bll.ChangeState(command.RecordID, ItemState.Stopped);
            WebHelper.ShowMessage("Processor stopped.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }
    }
}