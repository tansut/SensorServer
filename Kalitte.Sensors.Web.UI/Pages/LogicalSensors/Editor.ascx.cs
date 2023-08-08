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

namespace Kalitte.Sensors.Web.UI.Pages.LogicalSensors
{
    public partial class Editor : EditorViewControl<LogicalSensorBusiness>
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
            ctlLastEvents.DisableAutoRefresh();
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

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.CreateInEditor, ControllerType = typeof(LogicalSensorBusiness))]
        public void CreateInEditorHandler(object sender, CommandInfo command)
        {
            entityWindow.Title = "Create New Logical Sensor";
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
            ctlLastEventsPanel.Disabled = true;

            entityWindow.Show();
        }

        protected void dsLogicalBindings_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            currentLogicalSensorBindings = BusinessObject.GetSensorBindings(CurrentID);
            dsLogicalBindings.DataSource = currentLogicalSensorBindings;
            dsLogicalBindings.DataBind();
        }


        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.CreateEntity, ControllerType = typeof(LogicalSensorBusiness))]
        public void CreateEntityHandler(object sender, CommandInfo command)
        {
            ItemStartupType startup = ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();
            BusinessObject.CreateItem(ctlName.Text, ctlDescription.Text, startup);
            PageInstance.GetLister<LogicalSensorBusiness>().LoadItems();
            entityWindow.Hide();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.UpdateEntity, ControllerType = typeof(LogicalSensorBusiness))]
        public void UpdateEntityHandler(object sender, CommandInfo command)
        {
            var entity = BusinessObject.GetItem(CurrentID);

            ItemStartupType startup = ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();
            entity.Properties.Profile = ctlProfileEditor.EndEdit();
            entity.Properties.ExtendedProfile = ctlExtendedProfileEditor.EndEdit();
            entity.Properties.Startup = startup;
            entity.Description = ctlDescription.Text;
            BusinessObject.UpdateItem(entity);

            PageInstance.GetLister<LogicalSensorBusiness>().LoadItems();
            entityWindow.Hide();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.EditInEditor, ControllerType = typeof(LogicalSensorBusiness))]
        public void EditInEditorHandler(object sender, CommandInfo command)
        {
            LogicalSensorEntity entity = BusinessObject.GetItem(command.RecordID);
            ctlGenForm.ClearFields();
            ctlLocForm.ClearFields();
            entityWindow.Title = string.Format("Edit Logical Sensor: {0}", entity.Name);
            ctlSave.CommandName = KnownCommand.UpdateEntity.ToString();
            CurrentID = entity.Name;
            ctlName.Text = entity.Name;
            ctlName.ReadOnly = true;
            ctlDescription.Text = entity.Description;
            ctlLastException.Exception = entity.Properties.StateInfo.LastException;
            ctlLastEvents.ItemName = entity.Name;

            ctlInitialStartup.SelectedAsString = entity.Properties.Startup.ToString();

            currentLogicalSensorBindings = BusinessObject.GetSensorBindings(entity.Name);
            dsLogicalBindings.DataSource = currentLogicalSensorBindings;
            dsLogicalBindings.DataBind();
            ctlTabs.ActiveTabIndex = command.Parameters.ContainsKey("CommandArgument") ? int.Parse(command.Parameters["CommandArgument"].ToString()) : 0;

            ctlSensorBindingsPanel.Disabled = false;
            ctlProfilePanel.Disabled = false;
            ctlExtendedProfilePanel.Disabled = false;
            ctlAnalysisPanel.Disabled = false;
            ctlLastEventsPanel.Disabled = false;

            if (ctlTabs.ActiveTab == ctlLastEventsPanel)
                ctlLastEvents.LoadItems();
            else ctlLastEvents.ClearEvents();

            var metaDataOfItem = BusinessObject.GetItemDefaultMetadata(ProcessingItem.LogicalSensor);
            ctlProfileEditor.Edit(entity.Properties.Profile, metaDataOfItem);

            var extendedMetadata = BusinessObject.GetItemExtendedMetadata(ProcessingItem.LogicalSensor);
            var extendedMetadataOfItem = SensorCommon.GetEntityMetadata<ExtendedPropertyMetadata>(extendedMetadata.PropertyMetadata);
            ctlExtendedProfileEditor.Edit(entity.Properties.ExtendedProfile, extendedMetadataOfItem);
            ctlAnalyseItem.Bind(entity.Name);
            entityWindow.Show();
        }
    }
}