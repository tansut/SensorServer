using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Web.Controls;
using Kalitte.Sensors.Web.Core;
using Kalitte.Sensors.Web.Business;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Web.Security;
using Kalitte.Sensors.Communication;
using Ext.Net;
using System.Security;
using Kalitte.Sensors.Security;
using System.Runtime.InteropServices;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Web.UI.Pages.Processors
{
    public partial class ModuleState : EditorViewControl<ProcessorBusiness>
    {

        private List<Processor2ModuleBindingEntity> CurrentBindings
        {
            get
            {
                if (ViewState["CurrentBindings"] == null)
                    ViewState["CurrentBindings"] = new List<Processor2ModuleBindingEntity> { };
                return (List<Processor2ModuleBindingEntity>)ViewState["CurrentBindings"];
            }
            set
            {
                ViewState["CurrentBindings"] = value;
            }
        }

        private void changeItemState(string bindingName, ItemState newState)
        {
            var binding = CurrentBindings.Single(p => p.Name == bindingName);
            BusinessObject.ChangeProcessorModuleState(CurrentID, binding.Module, newState);
            loadBindings();
        }

        private void loadBindings()
        {
            CurrentBindings = new List<Processor2ModuleBindingEntity>(BusinessObject.GetProcessor2ModuleBindings(CurrentID));
            dsBindings.DataSource = CurrentBindings;
            dsBindings.DataBind();
        }

        [CommandHandler(CommandName = "ViewModuleStates", ControllerType = typeof(ProcessorBusiness))]
        public void ShowStates(object sender, CommandInfo command)
        {
            CurrentID = command.RecordID;
            loadBindings();
            entityWindow.Show();

        }

        [CommandHandler(CommandName = "StartModule", ControllerType = typeof(ProcessorBusiness))]
        public void StartModuleHandler(object sender, CommandInfo command)
        {
            changeItemState(command.RecordID, ItemState.Running);
        }

        [CommandHandler(CommandName = "StopModule", ControllerType = typeof(ProcessorBusiness))]
        public void StopModuleHandler(object sender, CommandInfo command)
        {
            changeItemState(command.RecordID, ItemState.Stopped);

        }

        protected void dsBindings_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            loadBindings();
        }


    }
}