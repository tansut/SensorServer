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
using Kalitte.Sensors.Web.Utility;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Events.Management;

namespace Kalitte.Sensors.Web.UI.Pages.Sensors
{
    public partial class Editor : EditorViewControl<SensorBusiness>
    {

        protected void closeWindow(object sender, DirectEventArgs e)
        {
            ctlAnalyseItem.Reset();
            ctlLastEvents.DisableAutoRefresh();

        }

        private void FillUI(CommandInfo cmd)
        {

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


        [CommandHandler(CommandName = "RemoveLogicalBinding", ControllerType = typeof(SensorBusiness))]
        public void RemoveLogicalBindingHandler(object sender, CommandInfo command)
        {
            List<Logical2SensorBindingEntity> currentBindings = new List<Logical2SensorBindingEntity>(currentLogicalSensorBindings);
            currentBindings.RemoveAll(p => p.Name == command.RecordID);
            currentLogicalSensorBindings = currentBindings.ToArray();
            dsLogicalBindings.DataSource = currentLogicalSensorBindings;
            dsLogicalBindings.DataBind();
        }



        [CommandHandler(CommandName = "AddLogicalSensorBinding", ControllerType = typeof(SensorBusiness))]
        public void AddLogicalSensorBindingHandler(object sender, CommandInfo command)
        {
            List<Logical2SensorBindingEntity> currentBindings = new List<Logical2SensorBindingEntity>(currentLogicalSensorBindings);
            LogicalSensorEntity logicalEntity = new LogicalSensorBusiness().GetItem(ctlLogicalSensors.SelectedAsString);

            var bindingProps = new Logical2SensorBindingProperty(ItemStartupType.Automatic);

            Logical2SensorBindingEntity newEntity = new Logical2SensorBindingEntity(logicalEntity.Name, CurrentID, ctlSourceList.SelectedAsString, bindingProps);

            if (currentBindings.Any(p => p.Name == newEntity.Name))
                throw new BusinessException("Binding already exists.");

            if (!string.IsNullOrEmpty(newEntity.SensorSource))
            {
                var existingAllSource = currentBindings.Where(p => p.SensorName == newEntity.SensorName && p.LogicalSensorName == newEntity.LogicalSensorName && string.IsNullOrEmpty(p.SensorSource)).SingleOrDefault();
                if (existingAllSource != null)
                    throw new BusinessException("There is already a binding with all sources for this sensor.");
            }
            else
            {
                currentBindings.RemoveAll(p => (p.SensorName == newEntity.SensorName && p.LogicalSensorName == newEntity.LogicalSensorName && !string.IsNullOrEmpty(p.SensorSource)));
            }

            currentBindings.Add(newEntity);
            currentLogicalSensorBindings = currentBindings.ToArray();
            dsLogicalBindings.DataSource = currentLogicalSensorBindings;
            dsLogicalBindings.DataBind();
        }


        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.EditInEditor, ControllerType = typeof(SensorBusiness))]
        public void EditInEditorHandler(object sender, CommandInfo command)
        {
            SensorDeviceEntity entity = BusinessObject.GetItem(command.RecordID);
            dsProviders.DataSource = new SensorProviderBusiness().GetItems();
            dsProviders.DataBind();
            ctlGenForm.ClearFields();
            ctlLocForm.ClearFields();
            entityWindow.Title = string.Format("Edit Sensor: {0}", entity.Name);
            ctlSave.CommandName = KnownCommand.UpdateEntity.ToString();
            CurrentID = entity.Name;
            ctlName.Text = entity.Name;
            ctlSensorId.Text = entity.SensorId;
            ctlName.ReadOnly = true;
            ctlProviders.SelectedAsString = entity.ProviderName;
            ctlProviders.ReadOnly = true;
            ctlLastEvents.ItemName = entity.Name;

            ctlLastException.Exception = entity.Properties.StateInfo.LastException;

            if (entity.Properties.Connection.TransportSettings is TcpTransportSettings)
            {
                TcpTransportSettings transport = (TcpTransportSettings)entity.Properties.Connection.TransportSettings;
                ctlHost.Text = transport.Host;
                ctlPort.ValueAsInt = transport.Port;
            }
            if (entity.Properties.Authentication is UserNameAuthenticationInformation)
            {
                UserNameAuthenticationInformation userInfo = (UserNameAuthenticationInformation)entity.Properties.Authentication;
                ctlUsername.Text = userInfo.UserName;

                IntPtr passwordBSTR = default(IntPtr);
                string pwd = string.Empty;

                try
                {
                    passwordBSTR = Marshal.SecureStringToBSTR(userInfo.Password);
                    pwd = Marshal.PtrToStringBSTR(passwordBSTR);
                }
                catch
                {
                    pwd = "";
                }

                ctlPassword.Text = pwd;
            }

            ctlInitialStartup.SelectedAsString = entity.Properties.Startup.ToString();
            ctlDescription.Text = entity.Description;

            var sources = BusinessObject.GetSensorSourcesAsList(entity.Name);
            dsSensorSources.DataSource = sources;
            dsSensorSources.DataBind();
            ctlSourceList.SelectedAsString = string.Empty;

            var propertySources = sources.ToList();
            propertySources.RemoveAll(p => p.Value == string.Empty);
            propertySources.Insert(0, new Kalitte.Sensors.Web.Business.SensorBusiness.SensorSourceInfo(entity.Name, entity.Name));
            dsSensorPropertySources.DataSource = propertySources;
            dsSensorPropertySources.DataBind();
            ctlSource.SetValue(entity.Name);
            LastSelectedItem = entity.Name;
            var logicalBll = new LogicalSensorBusiness();
            dsLogicalSensors.DataSource = logicalBll.GetItems();
            dsLogicalSensors.DataBind();
            ctlMonitoringDataEditor.Bind(entity.Properties.MonitoringData);

            currentLogicalSensorBindings = BusinessObject.GetLogical2SensorBindings(entity.Name);
            dsLogicalBindings.DataSource = currentLogicalSensorBindings;
            dsLogicalBindings.DataBind();
            ctlTabs.ActiveTabIndex = command.Parameters.ContainsKey("CommandArgument") ? int.Parse(command.Parameters["CommandArgument"].ToString()) : 0;

            ctlSensorBindingsPanel.Disabled = false;
            ctlProfilePanel.Disabled = false;
            ctlExtendedProfilePanel.Disabled = false;
            ctlAnalysisPanel.Disabled = false;
            ctlLastEventsPanel.Disabled = false;

            var sensorMetaData = BusinessObject.GetMetadata(entity.ProviderName);
            var metaDataOfItem = SensorCommon.GetEntityMetadata<DevicePropertyMetadata>(sensorMetaData);
            ctlProfileEditor.Edit(entity.Properties.Profile, metaDataOfItem);

            //if (entity.State == ItemState.Running)
            //    ctlProfileEditor.ShowSetPropertiesBtn();
            //else ctlProfileEditor.HideSetPropertiesBtn();

            if (ctlTabs.ActiveTab == ctlLastEventsPanel)
                ctlLastEvents.LoadItems();
            else ctlLastEvents.ClearEvents();

            var extendedMetadata = BusinessObject.GetItemExtendedMetadata(ProcessingItem.SensorDevice);
            var extendedMetadataOfItem = SensorCommon.GetEntityMetadata<ExtendedPropertyMetadata>(extendedMetadata.PropertyMetadata);
            ctlExtendedProfileEditor.Edit(entity.Properties.ExtendedProfile, extendedMetadataOfItem);
            ctlAnalyseItem.Bind(entity.Name);
            entityWindow.Show();
        }



        Dictionary<string, PropertyList> Properties
        {
            get
            {
                if (ViewState["Properties"] == null) ViewState["Properties"] = new Dictionary<string, PropertyList>();
                return (Dictionary<string, PropertyList>)ViewState["Properties"];
            }
            set
            {
                ViewState["Properties"] = value;
            }
        }




        string LastSelectedItem
        {
            get
            {
                if (ViewState["LastSelectedItem"] == null) ViewState["LastSelectedItem"] = string.Empty;
                return (string)ViewState["LastSelectedItem"];
            }
            set
            {
                ViewState["LastSelectedItem"] = value;
            }
        }

        public void SetProperties(string name, PropertyList list)
        {
            if (Properties.Keys.Contains(name))
            {
                Properties[name] = list;
            }
            else Properties.Add(name, list);
        }

        private void SetEntityProfile(SensorDeviceEntity entity, Dictionary<string, PropertyList> properties)
        {
            foreach (var item in properties)
            {
                if (item.Key == entity.Name)
                    entity.Properties.Profile = item.Value;
                else
                {
                    if (entity.Properties.SourceProfiles.Keys.Contains(item.Key))
                        entity.Properties.SourceProfiles[item.Key] = item.Value;
                    else entity.Properties.SourceProfiles.Add(item.Key, item.Value);
                }
            }
        }

        public void ctlSource_Change(object sender, DirectEventArgs e)
        {
            SensorDeviceEntity entity = BusinessObject.GetItem(CurrentID);
            SetProperties(LastSelectedItem, ctlProfileEditor.EndEdit());
            var sensorMetaData = BusinessObject.GetMetadata(entity.ProviderName);
            if (ctlSource.SelectedItem.Value == entity.Name)
            {
                var metaDataOfItem = SensorCommon.GetEntityMetadata<DevicePropertyMetadata>(sensorMetaData);
                var plist = Properties.Where(p => p.Key == entity.Name).Select(u => u.Value).SingleOrDefault();
                if (plist == null) plist = entity.Properties.Profile;
                ctlProfileEditor.Edit(plist, metaDataOfItem);
            }
            else
            {
                var sourceMetaData = sensorMetaData.Where(p => p.Value.PropertyTargets != SensorPropertyRelation.Device).ToDictionary(p => p.Key, p => p.Value);
                var metaDataOfItem = SensorCommon.GetEntityMetadata<DevicePropertyMetadata>(sourceMetaData);
                var plist = Properties.Where(p => p.Key == ctlSource.SelectedItem.Value).Select(u => u.Value).SingleOrDefault();
                if (plist == null) plist = entity.Properties.SourceProfiles.Where(p => p.Key == ctlSource.SelectedItem.Value).Select(u => u.Value).SingleOrDefault();
                if (plist == null) plist = BusinessObject.GetSensorSources(entity.Name).Where(p => p.Key == ctlSource.SelectedItem.Value).Select(u => u.Value).Single();
                ctlProfileEditor.Edit(plist, metaDataOfItem);
            }
            LastSelectedItem = ctlSource.SelectedItem.Value;
        }

        public void ctlProfileEditor_OnSetPropertyProfile(object sender, SetPropertyProfileEventArgs e)
        {
            SensorDeviceEntity entity = BusinessObject.GetItem(CurrentID);
            SetProperties(LastSelectedItem, e.UpdatedProfile);
            var dic = new Dictionary<string, PropertyList>();
            dic.Add(LastSelectedItem, e.UpdatedProfile);
            SetEntityProfile(entity, dic);
            BusinessObject.SetSensorProfile(entity.Name, Properties);
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.CreateInEditor, ControllerType = typeof(SensorBusiness))]
        public void CreateInEditorHandler(object sender, CommandInfo command)
        {
            entityWindow.Title = "Create New Sensor";
            ctlSave.CommandName = KnownCommand.CreateEntity.ToString();
            ctlGenForm.ClearFields();
            ctlLocForm.ClearFields();
            ctlMonitoringDataEditor.Clear();
            ctlName.ReadOnly = false;
            ctlName.Focus();
            ctlProviders.ReadOnly = false;
            ctlProviders.ClearValue();

            dsProviders.DataSource = new SensorProviderBusiness().GetItems();
            dsProviders.DataBind();

            ctlHost.Text = "fx74001d370a";
            ctlPort.ValueAsInt = 5084;
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



        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.CreateEntity, ControllerType = typeof(SensorBusiness))]
        public void CreateEntityHandler(object sender, CommandInfo command)
        {
            var provider = new SensorProviderBusiness().GetItem(ctlProviders.SelectedAsString);
            var connectionInformation = new ConnectionInformation(provider.Name, new TcpTransportSettings(ctlHost.Text, ctlPort.ValueAsInt));
            var owd = new SecureString();
            foreach (char c in ctlPassword.Text)
                owd.AppendChar(c);
            var authenticationInformation = new UserNameAuthenticationInformation(ctlUsername.Text, owd);

            ItemStartupType startup = ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();


            BusinessObject.CreateItem(ctlName.Text, ctlSensorId.Text, ctlDescription.Text, connectionInformation, authenticationInformation, startup);
            PageInstance.GetLister<SensorBusiness>().LoadItems();
            entityWindow.Hide();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.UpdateEntity, ControllerType = typeof(SensorBusiness))]
        public void UpdateEntityHandler(object sender, CommandInfo command)
        {
            SensorDeviceEntity entity = BusinessObject.GetItem(CurrentID);
            var connectionInformation = new ConnectionInformation(entity.ProviderName, new TcpTransportSettings(ctlHost.Text, ctlPort.ValueAsInt));
            var owd = new SecureString();
            foreach (char c in ctlPassword.Text)
                owd.AppendChar(c);
            var authenticationInformation = new UserNameAuthenticationInformation(ctlUsername.Text, owd);
            entity.Properties.Authentication = authenticationInformation;
            ItemStartupType startup = ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();
            entity.SensorId = ctlSensorId.Text;
            entity.Properties.Connection = connectionInformation;
            SetProperties(LastSelectedItem, ctlProfileEditor.EndEdit());
            SetEntityProfile(entity, Properties);
            entity.Properties.ExtendedProfile = ctlExtendedProfileEditor.EndEdit();
            entity.Properties.Startup = startup;
            entity.Description = ctlDescription.Text;
            ctlMonitoringDataEditor.Retrieve(entity.Properties.MonitoringData);
            BusinessObject.UpdateSensorWithBindings(entity, currentLogicalSensorBindings);
            PageInstance.GetLister<SensorBusiness>().LoadItems();
            entityWindow.Hide();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                dsStartup.DataBind();
                ctlProfileEditor.HideSetPropertiesBtn();
            }
        }

        protected void dsLogicalBindings_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            currentLogicalSensorBindings = BusinessObject.GetLogical2SensorBindings(CurrentID);
            dsLogicalBindings.DataSource = currentLogicalSensorBindings;
            dsLogicalBindings.DataBind();
        }

    }
}