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

namespace Kalitte.Sensors.Web.UI.Pages.SensorProviders
{
    public partial class Management : ViewPageBase
    {

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.DeleteEntity, ControllerType = typeof(SensorProviderBusiness))]
        public void DeleteItem(object sender, CommandInfo command)
        {
            SensorProviderBusiness bll = GetBusinessObject<SensorProviderBusiness>();
            bll.DeleteItem(command.RecordID);
            WebHelper.ShowMessage("Sensor provider deleted successfully.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.StartItem, ControllerType = typeof(SensorProviderBusiness))]
        public void StartItem(object sender, CommandInfo command)
        {
            SensorProviderBusiness bll = GetBusinessObject<SensorProviderBusiness>();
            bll.ChangeState(command.RecordID, ItemState.Running);            
            WebHelper.ShowMessage("Sensor provider started.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.StopItem, ControllerType = typeof(SensorProviderBusiness))]
        public void StopItem(object sender, CommandInfo command)
        {
            SensorProviderBusiness bll = GetBusinessObject<SensorProviderBusiness>();
            bll.ChangeState(command.RecordID, ItemState.Stopped);    
            WebHelper.ShowMessage("Sensor provider stopped.", MessageType.InfoAsFloating);
            lister.LoadItems();
        }
    }
}