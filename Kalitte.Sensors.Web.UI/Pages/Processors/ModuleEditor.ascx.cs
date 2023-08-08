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
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Web.Utility;

namespace Kalitte.Sensors.Web.UI.Pages.Processors
{
    public partial class ModuleEditor : EditorViewControl<ProcessorBusiness>
    {

        protected void closeWindow(object sender, DirectEventArgs e)
        {
            ctlAnalyseItem2.Reset();
        }

        private void FillUI(CommandInfo cmd)
        {

        }

        protected int Order
        {
            get
            {
                if (ViewState["order"] == null)
                    return 0;
                else return (int)ViewState["order"];
            }
            set
            {
                ViewState["order"] = value;
            }
        }

        public void CreateInEditorHandler(object sender, CommandInfo command)
        {
            CurrentID= command.Parameters["processorName"].ToString();
            CurrentDetailID = command.Parameters["moduleName"].ToString();

            Order = (int)command.Parameters["execOrder"];

            ctlProcessorName.Text = CurrentID;
            ctlModuleName.Text = CurrentDetailID;

            entityWindow.Title = string.Format("Add new module to {0}", CurrentID); 
            ctlSave.CommandName = "CreateModuleProcessor";

            ctlGenForm.ClearFields();


            ctlTabs.ActiveTabIndex = 0;
            ctlProfilePanel.Disabled = true;
            ctlExtendedProfilePanel.Disabled = true;
            ctlInheritNonExistBehaviour.Checked = true;
            ctlInheritNullEvent.Checked = true;
            ctlAnalysisPanel.Disabled = true;

            ctlInitialStartup.SelectedAsString = ItemStartupType.Automatic.ToString();
            ctlNonExistEventHandler.SelectedAsString = NonExistEventHandlerBehavior.PassOriginalEventToNextModule.ToString();
            ctlNullSensorEvent.SelectedAsString = PipeNullEventBehavior.StopPipe.ToString();
            entityWindow.Show();
        }

        public void EditInEditorHandler(object sender, CommandInfo command)
        {
            ctlGenForm.ClearFields();
            Processor2ModuleBindingEntity entity = command.Parameters["binding"] as Processor2ModuleBindingEntity;
            CurrentID = entity.Processor;
            CurrentDetailID = entity.Module;
            ViewState["binding"] = entity;
            ctlProcessorName.Text = entity.Processor;
            ctlModuleName.Text = entity.Module;

            entityWindow.Title = string.Format("Edit module: {0}", entity.Module);
            ctlSave.CommandName = "UpdateModuleProcessor";

            ctlLastException.Exception = entity.Properties.StateInfo.LastException;
            ctlInitialStartup.SelectedAsString = entity.Properties.Startup.ToString();
            ctlDescription.Text = entity.Description;

            ctlTabs.ActiveTabIndex = 0;
            ctlProfilePanel.Disabled = false;
            ctlExtendedProfilePanel.Disabled = false;
            ctlAnalysisPanel.Disabled = false;

            ctlNonExistEventHandler.SelectedAsString = entity.Properties.ModuleNonExistEventHandlerBehavior.ToString();
            ctlInheritNonExistBehaviour.Checked = entity.Properties.InheritNonExistEventHandlerBehavior;

            ctlNullSensorEvent.SelectedAsString = entity.Properties.ModuleNullEventBehavior.ToString();
            ctlInheritNullEvent.Checked = entity.Properties.InheritNullEventBehaviorBehavior;

            var metaDataOfItem = new EventModuleBusiness().GetMetadata(entity.Module);


            var metaData = metaDataOfItem == null ? null: SensorCommon.GetEntityMetadata<EventModulePropertyMetadata>(metaDataOfItem.ModulePropertyMetadata);
            ctlProfileEditor.Edit(entity.Properties.Profile, metaData);

            var extendedMetadata = BusinessObject.GetItemExtendedMetadata(ProcessingItem.EventModule);
            var extendedMetadataOfItem = SensorCommon.GetEntityMetadata<ExtendedPropertyMetadata>(extendedMetadata.PropertyMetadata);
            ctlExtendedProfileEditor.Edit(entity.Properties.ExtendedProfile, extendedMetadataOfItem);
            ctlAnalyseItem2.Bind(entity.Module);
            if (entity.State == ItemState.Running)
                ctlProfileEditor.ShowSetPropertiesBtn();
            else ctlProfileEditor.HideSetPropertiesBtn();
            entityWindow.Show();
        }


        protected void setPropertiesHandler(object sender, SetPropertyProfileEventArgs e)
        {
            var entity = (Processor2ModuleBindingEntity)ViewState["binding"];
            BusinessObject.SetEventModuleProfile(CurrentID, entity.Name, e.UpdatedProfile);
            WebHelper.ShowMessage("Successfully changed properties on running module.\n\n" + e.UpdatedProfile.ToString(), MessageType.InfoAsFloating);
            //entityWindow.Hide();
        }


        [CommandHandler(CommandName = "CreateModuleProcessor", ControllerType = typeof(ProcessorBusiness))]
        public void CreateEntityHandler(object sender, CommandInfo command)
        {
            ItemStartupType startup = ItemStartupType.Automatic; //ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();

            Processor2ModuleBindingProperty properties = new Processor2ModuleBindingProperty(startup);
            properties.InheritNonExistEventHandlerBehavior = ctlInheritNonExistBehaviour.Checked;
            properties.ModuleNonExistEventHandlerBehavior = ctlNonExistEventHandler.GetSelectedAsType<NonExistEventHandlerBehavior>();
            properties.InheritNullEventBehaviorBehavior = ctlInheritNullEvent.Checked;
            properties.ModuleNullEventBehavior = ctlNullSensorEvent.GetSelectedAsType<PipeNullEventBehavior>();

            Processor2ModuleBindingEntity newEntity = new Processor2ModuleBindingEntity(CurrentID, CurrentDetailID, properties);
            newEntity.Description = ctlDescription.Text;
            newEntity.ExecOrder = this.Order;
            ProcessorEditor.AddModuleBinding(newEntity);
            entityWindow.Hide();
        }

        [CommandHandler(CommandName = "UpdateModuleProcessor", ControllerType = typeof(ProcessorBusiness))]
        public void UpdateEntityHandler(object sender, CommandInfo command)
        {
            Processor2ModuleBindingEntity entity = ViewState["binding"] as Processor2ModuleBindingEntity;
            var startup = ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();
            entity.Properties.Profile = ctlProfileEditor.EndEdit();
            entity.Properties.ExtendedProfile = ctlExtendedProfileEditor.EndEdit();
            entity.Properties.Startup = startup;
            entity.Description = ctlDescription.Text;
            entity.Properties.InheritNonExistEventHandlerBehavior = ctlInheritNonExistBehaviour.Checked;
            entity.Properties.ModuleNonExistEventHandlerBehavior = ctlNonExistEventHandler.GetSelectedAsType<NonExistEventHandlerBehavior>();
            entity.Properties.InheritNullEventBehaviorBehavior = ctlInheritNullEvent.Checked;
            entity.Properties.ModuleNullEventBehavior = ctlNullSensorEvent.GetSelectedAsType<PipeNullEventBehavior>();

            ProcessorEditor.UpdateModuleBinding(entity);
            entityWindow.Hide();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                dsStartup.DataBind();
                dsNonExistEventHandler.DataBind();
                dsNullEventBehavior.DataBind();
            }
        }




        public Editor ProcessorEditor { get; set; }


    }
}