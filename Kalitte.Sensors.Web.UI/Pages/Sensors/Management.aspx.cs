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

namespace Kalitte.Sensors.Web.UI.Pages.Sensors
{
    public partial class Management : ViewPageBase
    {

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.DeleteEntity, ControllerType = typeof(SensorBusiness))]
        public void DeleteItem(object sender, CommandInfo command)
        {
            SensorBusiness bll = GetBusinessObject<SensorBusiness>();
            bll.DeleteItem(command.RecordID);
            WebHelper.ShowMessage("Sensor device deleted successfully.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.StartItem, ControllerType = typeof(SensorBusiness))]
        public void StartItem(object sender, CommandInfo command)
        {
            SensorBusiness bll = GetBusinessObject<SensorBusiness>();
            bll.ChangeState(command.RecordID, ItemState.Running);            
            WebHelper.ShowMessage("Sensor device started.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.StopItem, ControllerType = typeof(SensorBusiness))]
        public void StopItem(object sender, CommandInfo command)
        {
            SensorBusiness bll = GetBusinessObject<SensorBusiness>();
            bll.ChangeState(command.RecordID, ItemState.Stopped);    
            WebHelper.ShowMessage("Sensor device stopped.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }

        [CommandHandler(CommandName="OpenCommandWindow", ControllerType = typeof(SensorBusiness))]
        public void OpenCommandWindowHandler(object sender, CommandInfo command)
        {
            cmdWindow.Execute(command.RecordID);
        }
        
    }
}