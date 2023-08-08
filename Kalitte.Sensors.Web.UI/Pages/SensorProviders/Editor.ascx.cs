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

namespace Kalitte.Sensors.Web.UI.Pages.SensorProviders
{
    public partial class Editor : EditorViewControl<SensorProviderBusiness>
    {
        protected void closeWindow(object sender, DirectEventArgs e)
        {
            ctlAnalyseItem.Reset();
            ctlLogView.DisableAutoRefresh();
        }


        private void FillUI(CommandInfo cmd)
        {

        }

        [CommandHandler(CommandName = "QueryLog", ControllerType = typeof(SensorProviderBusiness))]
        public void QueryLogHandler(object sender, CommandInfo command)
        {
            ctlLogView.Bind();
        }



        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.EditInEditor, ControllerType = typeof(SensorProviderBusiness))]
        public void EditInEditorHandler(object sender, CommandInfo command)
        {
            SensorProviderEntity entity = BusinessObject.GetItem(command.RecordID);
            ctlGenForm.ClearFields();
            ctlLocForm.ClearFields();
            entityWindow.Title = string.Format("Edit Provider: {0}", entity.Name);
            ctlSave.CommandName = KnownCommand.UpdateEntity.ToString();
            CurrentID = entity.Name;
            ctlName.Text = entity.Name;
            ctlName.ReadOnly = true;
            ctlType.Text = entity.TypeQ;
            ctlLastException.Exception = entity.Properties.StateInfo.LastException;
            ctlInitialStartup.SelectedAsString = entity.Properties.Startup.ToString();
            ctlDescription.Text = entity.Description;
            dsSensors.DataSource = new SensorBusiness().GetSensorDevicesForProvider(entity.Name);
            dsSensors.DataBind();


            ctlDiscoveryMatch.SelectedAsString = entity.Properties.DiscoveryBehavior.SensorMatch.ToString();
            ctlAutoCreateSensor.Checked = entity.Properties.DiscoveryBehavior.CreateSensorIfNoMatch;

            ctlTabs.ActiveTabIndex = 0;
            ctlSensorBindingsPanel.Disabled = false;
            ctlProfilePanel.Disabled = false;
            ctlExtendedProfilePanel.Disabled = false;
            ctlLogViewPanel.Disabled = false;
            ctlGenProps.Disabled = false;
            ctlAnalysisPanel.Disabled = false;


            var metaData = BusinessObject.GetProviderMetadata(entity.Name);
            if (metaData != null)
            {
                var metaDataOfItem = SensorCommon.GetEntityMetadata<ProviderPropertyMetadata>(metaData.ProviderPropertyMetadata);
                ctlProfileEditor.Edit(entity.Properties.Profile, metaDataOfItem);
            }
            else ctlProfileEditor.Clear();

            var extendedMetadata = BusinessObject.GetItemExtendedMetadata(ProcessingItem.SensorProvider);

            if (extendedMetadata != null)
            {
                var extendedMetadataOfItem = SensorCommon.GetEntityMetadata<ExtendedPropertyMetadata>(extendedMetadata.PropertyMetadata);
                ctlExtendedProfileEditor.Edit(entity.Properties.ExtendedProfile, extendedMetadataOfItem);
            }
            else ctlExtendedProfileEditor.Clear();

            ctlLogLevel.AllowBlank = false;
            ctlDiscoveryMatch.AllowBlank = false;
            ctlLogLevel.SelectedAsString = entity.LogLevel.Level.ToString();
            ctlInheritLogLevel.Checked = entity.LogLevel.Inherit;
            ctlTabs.ActiveTabIndex = command.Parameters.ContainsKey("CommandArgument") ? int.Parse(command.Parameters["CommandArgument"].ToString()) : 0;

            if (ctlTabs.ActiveTab == ctlLogViewPanel)
            {
                ctlLogView.InitProperties(ProcessingItem.SensorProvider, CurrentID, true);
            }
            else ctlLogView.InitProperties(ProcessingItem.SensorProvider, CurrentID);
            ctlAnalyseItem.Bind(entity.Name);
            ctlMonitoringDataEditor.Bind(entity.Properties.MonitoringData);
            entityWindow.Show();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.CreateInEditor, ControllerType = typeof(SensorProviderBusiness))]
        public void CreateInEditorHandler(object sender, CommandInfo command)
        {
            entityWindow.Title = "Create New Provider";
            ctlSave.CommandName = KnownCommand.CreateEntity.ToString();
            ctlGenForm.ClearFields();
            ctlLocForm.ClearFields();
            ctlMonitoringDataEditor.Clear();
            ctlName.ReadOnly = false;
            ctlName.Focus();
            ctlTabs.ActiveTabIndex = 0;
            ctlSensorBindingsPanel.Disabled = true;
            ctlProfilePanel.Disabled = true;
            ctlExtendedProfilePanel.Disabled = true;
            ctlLogViewPanel.Disabled = false;
            ctlGenProps.Disabled = true;
            ctlAnalysisPanel.Disabled = true;
            ctlLogLevel.AllowBlank = true;
            ctlDiscoveryMatch.AllowBlank = true;

            entityWindow.Show();
        }



        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.CreateEntity, ControllerType = typeof(SensorProviderBusiness))]
        public void CreateEntityHandler(object sender, CommandInfo command)
        {
            BusinessObject.Create(ctlName.Text, ctlDescription.Text, ctlType.Text, ctlInitialStartup.GetSelectedAsType<ItemStartupType>());
            PageInstance.GetLister<SensorProviderBusiness>().LoadItems();
            entityWindow.Hide();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.UpdateEntity, ControllerType = typeof(SensorProviderBusiness))]
        public void UpdateEntityHandler(object sender, CommandInfo command)
        {
            SensorProviderEntity entity = BusinessObject.GetItem(CurrentID);
            entity.Description = ctlDescription.Text;
            entity.Properties.Startup = ctlInitialStartup.GetSelectedAsType<ItemStartupType>();
            entity.Properties.Profile = ctlProfileEditor.EndEdit();
            entity.Properties.ExtendedProfile = ctlExtendedProfileEditor.EndEdit();
            entity.Properties.LogLevel.Inherit = ctlInheritLogLevel.Checked;
            entity.Properties.LogLevel.Level = ctlLogLevel.GetSelectedAsType<LogLevel>();
            entity.Properties.DiscoveryBehavior.SensorMatch = ctlDiscoveryMatch.GetSelectedAsType<Kalitte.Sensors.Processing.Metadata.DiscoverySensorMatchType>();
            entity.Properties.DiscoveryBehavior.CreateSensorIfNoMatch = ctlAutoCreateSensor.Checked;
            ctlMonitoringDataEditor.Retrieve(entity.Properties.MonitoringData);
            BusinessObject.UpdateItem(entity);
            PageInstance.GetLister<SensorProviderBusiness>().LoadItems();
            entityWindow.Hide();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                dsStartup.DataBind();
                dsLogLevel.DataBind();
                dsDiscovery.DataBind();
            }
        }

        protected void dsSensors_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            dsSensors.DataSource = new SensorBusiness().GetSensorDevicesForProvider(CurrentID);
            dsSensors.DataBind();
        }

    }
}