using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Web.Core;
using Kalitte.Sensors.Web.Business;
using Kalitte.Sensors.Web.Security;
using Kalitte.Sensors.Processing.Metadata;
using Ext.Net;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Web.UI.Pages.EventModules
{
    public partial class Editor : EditorViewControl<EventModuleBusiness>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                dsStartup.DataBind();
            }
        }

        protected void closeWindow(object sender, DirectEventArgs e)
        {
            ctlAnalyseItem.Reset();
        }

        private Logical2SensorBindingEntity[] currentLogicalSensorBindings
        {
            get
            {
                if (ViewState["currentLogicalSensorBindings"] == null)
                    ViewState["currentLogicalSensorBindings"] = new Logical2SensorBindingEntity[] { };
                return (Logical2SensorBindingEntity[])ViewState["currentLogicalSensorBindings"];
            }
            set
            {
                ViewState["currentLogicalSensorBindings"] = value;
            }
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.CreateInEditor, ControllerType = typeof(EventModuleBusiness))]
        public void CreateInEditorHandler(object sender, CommandInfo command)
        {
            entityWindow.Title = "Create New Module";
            ctlSave.CommandName = KnownCommand.CreateEntity.ToString();
            ctlGenForm.ClearFields();
            ctlLocForm.ClearFields();
            ctlName.ReadOnly = false;
            ctlName.Focus();
            ctlLastException.Hide();

            currentLogicalSensorBindings = new Logical2SensorBindingEntity[] { };
            ctlTabs.ActiveTabIndex = 0;
            ctlSensorBindingsPanel.Disabled = true;
            ctlProfilePanel.Disabled = true;
            ctlExtendedProfilePanel.Disabled = true;
            ctlAnalysisPanel.Disabled = true;

            entityWindow.Show();
        }

        protected void dsLogicalBindings_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //currentLogicalSensorBindings = BusinessObject.GetLogical2SensorBindings(CurrentID);
            dsLogicalBindings.DataSource = currentLogicalSensorBindings;
            dsLogicalBindings.DataBind();
        }


        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.CreateEntity, ControllerType = typeof(EventModuleBusiness))]
        public void CreateEntityHandler(object sender, CommandInfo command)
        {
            ItemStartupType startup = ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();
            BusinessObject.CreateItem(ctlName.Text, ctlDescription.Text, ctlType.Text, startup);
            PageInstance.GetLister<EventModuleBusiness>().LoadItems();
            entityWindow.Hide();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.UpdateEntity, ControllerType = typeof(EventModuleBusiness))]
        public void UpdateEntityHandler(object sender, CommandInfo command)
        {
            var entity = BusinessObject.GetItem(CurrentID);

            ItemStartupType startup = ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();
            entity.Properties.Startup = startup;
            entity.Description = ctlDescription.Text;
            entity.TypeQ = ctlType.Text;
            entity.Properties.Profile = ctlProfileEditor.EndEdit();
            entity.Properties.ExtendedProfile = ctlExtendedProfileEditor.EndEdit();
            BusinessObject.UpdateItem(entity);

            PageInstance.GetLister<EventModuleBusiness>().LoadItems();
            entityWindow.Hide();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.EditInEditor, ControllerType = typeof(EventModuleBusiness))]
        public void EditInEditorHandler(object sender, CommandInfo command)
        {
            var entity = BusinessObject.GetItem(command.RecordID);
            ctlGenForm.ClearFields();
            ctlLocForm.ClearFields();
            entityWindow.Title = string.Format("Edit Module: {0}", entity.Name);
            ctlSave.CommandName = KnownCommand.UpdateEntity.ToString();
            CurrentID = entity.Name;
            ctlName.Text = entity.Name;
            ctlName.ReadOnly = true;
            ctlDescription.Text = entity.Description;
            ctlLastException.Exception = entity.Properties.StateInfo.LastException;
            ctlType.Text = entity.TypeQ;

            ctlInitialStartup.SelectedAsString = entity.Properties.Startup.ToString();
            ctlProfilePanel.Disabled = false;
            ctlExtendedProfilePanel.Disabled = false;
            ctlTabs.ActiveTabIndex = command.Parameters.ContainsKey("CommandArgument") ? int.Parse(command.Parameters["CommandArgument"].ToString()) : 0;

            dsLogicalBindings.DataSource = currentLogicalSensorBindings;
            dsLogicalBindings.DataBind();
            ctlSensorBindingsPanel.Disabled = false;
            ctlAnalysisPanel.Disabled = false;

            var metaDataOfItem = BusinessObject.GetItemDefaultMetadata(ProcessingItem.EventModule, CurrentID);
            ctlProfileEditor.Edit(entity.Properties.Profile, metaDataOfItem);

            var extendedMetadata = BusinessObject.GetItemExtendedMetadata(ProcessingItem.LogicalSensor);
            var extendedMetadataOfItem = SensorCommon.GetEntityMetadata<ExtendedPropertyMetadata>(extendedMetadata.PropertyMetadata);
            ctlExtendedProfileEditor.Edit(entity.Properties.ExtendedProfile, extendedMetadataOfItem);
            ctlAnalyseItem.Bind(entity.Name);
            entityWindow.Show();
        }
    }
}