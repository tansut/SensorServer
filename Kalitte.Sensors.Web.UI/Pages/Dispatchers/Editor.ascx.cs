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

namespace Kalitte.Sensors.Web.UI.Pages.Dispatchers
{
    public partial class Editor : EditorViewControl<DispatcherBusiness>
    {
        protected override void OnInit(EventArgs e)
        {
            ctlProcessorEditor.DispatcherEditor = this;
            base.OnInit(e);
        }

        protected void closeWindow(object sender, DirectEventArgs e)
        {
            ctlAnalyseItem.Reset();
            ctlProcessorEditor.HideAllWindows();
            ctlLogView.DisableAutoRefresh();
            ctlLastEvents.DisableAutoRefresh();

        }

        public void initForm()
        {
            ctlGenForm.ClearFields();
            ctlLocForm.ClearFields();


            dsProcessors.DataSource = new ProcessorBusiness().GetItems();
            dsProcessors.DataBind();

            if (!string.IsNullOrEmpty(CurrentID))
            {
                CurrentProcessorBindings = new List<Dispatcher2ProcessorBindingEntity>(BusinessObject.GetDispatcher2ProcessorBindings(CurrentID));
                dsDispatcherProcessors.DataSource = CurrentProcessorBindings;
                dsDispatcherProcessors.DataBind();
            } 
        }



        #region ModuleBindings


        private List<Dispatcher2ProcessorBindingEntity> CurrentProcessorBindings
        {
            get
            {
                if (ViewState["CurrentProcessorBindings"] == null)
                    ViewState["CurrentProcessorBindings"] = new List<Dispatcher2ProcessorBindingEntity> { };
                return (List<Dispatcher2ProcessorBindingEntity>)ViewState["CurrentProcessorBindings"];
            }
            set
            {
                ViewState["CurrentProcessorBindings"] = value;
            }
        }



        [CommandHandler(CommandName = "CreateProcessorInEditor", ControllerType = typeof(DispatcherBusiness))]
        public void CreateModuleProcessorInEditorHandler(object sender, CommandInfo command)
        {
            if (CurrentProcessorBindings.Any(p => p.Dispatcher == CurrentID && p.Processor == ctlProcessors.SelectedAsString))
                throw new BusinessException("Processor already exists. You can only create one instance of processor for dispatcher");
            command.Parameters.Add("dispatcherName", CurrentID);
            command.Parameters.Add("processorName", ctlProcessors.SelectedAsString);
            ctlProcessorEditor.CreateInEditorHandler(sender, command);
        }

        [CommandHandler(CommandName = "RemoveProcessorBinding", ControllerType = typeof(DispatcherBusiness))]
        public void RemoveModuleBindingHandler(object sender, CommandInfo command)
        {
            var bindings = CurrentProcessorBindings;
            bindings.RemoveAll(p => p.Name == command.RecordID);
            CurrentProcessorBindings = bindings;
            dsDispatcherProcessors.DataSource = CurrentProcessorBindings;
            dsDispatcherProcessors.DataBind();

        }

        [CommandHandler(CommandName = "EditProcessorBinding", ControllerType = typeof(DispatcherBusiness))]
        public void EditModuleBindingHandler(object sender, CommandInfo command)
        {
            var binding = CurrentProcessorBindings.Single(p => p.Name == command.RecordID);
            command.Parameters.Add("binding", binding);
            ctlProcessorEditor.EditInEditorHandler(this, command);
        }


        protected void dsProcessorBindings_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            CurrentProcessorBindings = new List<Dispatcher2ProcessorBindingEntity>(BusinessObject.GetDispatcher2ProcessorBindings(CurrentID));
            dsDispatcherProcessors.DataSource = CurrentProcessorBindings;
            dsDispatcherProcessors.DataBind();
        }

        public void AddProcessorBinding(Dispatcher2ProcessorBindingEntity entity)
        {
            var bindings = CurrentProcessorBindings;
            bindings.Add(entity);
            CurrentProcessorBindings = bindings;
            dsDispatcherProcessors.DataSource = CurrentProcessorBindings;
            dsDispatcherProcessors.DataBind();
        }


        internal void UpdateProcessorBinding(Dispatcher2ProcessorBindingEntity entity)
        {
            var bindings = CurrentProcessorBindings;
            var currentIndex = CurrentProcessorBindings.FindIndex(p => p.Name == entity.Name);
            bindings[currentIndex] = entity;
            CurrentProcessorBindings = bindings;
            dsDispatcherProcessors.DataSource = CurrentProcessorBindings;
            dsDispatcherProcessors.DataBind();
        }

        #endregion



        

        #region dispatchers

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.EditInEditor, ControllerType = typeof(DispatcherBusiness))]
        public void EditInEditorHandler(object sender, CommandInfo command)
        {
            var entity = BusinessObject.GetItem(command.RecordID);
            CurrentID = entity.Name;
            initForm();

            ctlLastEvents.ItemName = entity.Name;
            

            entityWindow.Title = string.Format("Edit Dispatcher: {0}", entity.Name);
            ctlSave.CommandName = KnownCommand.UpdateEntity.ToString();

            ctlName.Text = entity.Name;
            ctlName.ReadOnly = true;

            ctlType.Text = entity.TypeQ;

            ctlAddModule.CommandArgument = entity.Name;

            ctlLastException.Exception = entity.Properties.StateInfo.LastException;
            ctlInitialStartup.SelectedAsString = entity.Properties.Startup.ToString();
            ctlDescription.Text = entity.Description;


            ctlTabs.ActiveTabIndex = command.Parameters.ContainsKey("CommandArgument") ? int.Parse(command.Parameters["CommandArgument"].ToString()) : 0;
            ctlSave.AddScript("{0}.confirm={1};", ctlSave.ClientID, (entity.State == ItemState.Running).ToString().ToLowerInvariant());

            ctlProfilePanel.Disabled = false;
            ctlAnalysisPanel.Disabled = false;
            ctlLogViewPanel.Disabled = false;
            ctlProcessorBindingsPanel.Disabled = false;
            ctlExtendedProfilePanel.Disabled = false;
            ctlLastEventsPanel.Disabled = false;

            ctlProfileEditor.Clear();
            var extendedMetadata = BusinessObject.GetItemExtendedMetadata(ProcessingItem.Dispatcher);
            var extendedMetadataOfItem = SensorCommon.GetEntityMetadata<ExtendedPropertyMetadata>(extendedMetadata.PropertyMetadata);
            ctlExtendedProfileEditor.Edit(entity.Properties.ExtendedProfile, extendedMetadataOfItem);
            ctlMonitoringDataEditor.Bind(entity.Properties.MonitoringData);


            if (ctlTabs.ActiveTab == ctlLogViewPanel)
            {
                ctlLogView.InitProperties(ProcessingItem.Processor, CurrentID, true);
            }
            else ctlLogView.InitProperties(ProcessingItem.Processor, CurrentID);

            if (ctlTabs.ActiveTab == ctlLastEventsPanel)
                ctlLastEvents.LoadItems();
            else ctlLastEvents.ClearEvents();

            var metaDataOfItem = BusinessObject.GetMetadata(entity.Name);
            var metaData = metaDataOfItem == null ? null: SensorCommon.GetEntityMetadata<DispatcherPropertyMetadata>(metaDataOfItem.DispatcherPropertyMetadata);
            ctlProfileEditor.Edit(entity.Properties.Profile, metaData);

            if (entity.State == ItemState.Running)
                ctlProfileEditor.ShowSetPropertiesBtn();
            else ctlProfileEditor.HideSetPropertiesBtn();

            ctlAnalyseItem.Bind(entity.Name);
            entityWindow.Show();
        }



        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.CreateInEditor, ControllerType = typeof(DispatcherBusiness))]
        public void CreateInEditorHandler(object sender, CommandInfo command)
        {
            CurrentID = string.Empty;
            initForm();
            entityWindow.Title = "Create New Dispatcher";
            ctlSave.CommandName = KnownCommand.CreateEntity.ToString();
            ctlGenForm.ClearFields();
            ctlLocForm.ClearFields();
            ctlMonitoringDataEditor.Clear();
            ctlName.ReadOnly = false;
            ctlName.Focus();



            ctlTabs.ActiveTabIndex = 0;
            ctlProfilePanel.Disabled = true;
            ctlAnalysisPanel.Disabled = true;
            ctlLogViewPanel.Disabled = true;
            ctlProcessorBindingsPanel.Disabled = true;
            ctlExtendedProfilePanel.Disabled = true;
            ctlLastEventsPanel.Disabled = true;
            
            ctlSave.AddScript("{0}.confirm={1};", ctlSave.ClientID, "false");

            entityWindow.Show();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.CreateEntity, ControllerType = typeof(DispatcherBusiness))]
        public void CreateEntityHandler(object sender, CommandInfo command)
        {

            ItemStartupType startup = ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();
            BusinessObject.CreateItem(ctlName.Text, ctlDescription.Text, ctlType.Text, startup);
            //BusinessObject.UpdateDispatcher2ProcessorBindings(ctlName.Text, CurrentProcessorBindings.ToArray());
            PageInstance.GetLister<DispatcherBusiness>().LoadItems();
            entityWindow.Hide();
        }

        [CommandHandler(CommandName = "QueryLog", ControllerType = typeof(DispatcherBusiness))]
        public void QueryLogHandler(object sender, CommandInfo command)
        {
            ctlLogView.Bind();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.UpdateEntity, ControllerType = typeof(DispatcherBusiness))]
        public void UpdateEntityHandler(object sender, CommandInfo command)
        {
            var entity = BusinessObject.GetItem(CurrentID);
            bool start = false;
            if (entity.State == ItemState.Running)
            {
                BusinessObject.ChangeState(entity.Name, ItemState.Stopped);
                start = true;
            }

            ItemStartupType startup = ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();

            entity.Properties.Profile = ctlProfileEditor.EndEdit();
            entity.Properties.Startup = startup;
            entity.Description = ctlDescription.Text;
            entity.TypeQ = ctlType.Text;
            ctlMonitoringDataEditor.Retrieve(entity.Properties.MonitoringData);
            BusinessObject.UpdateWithBindings(entity, CurrentProcessorBindings.ToArray());
            if (start)
                BusinessObject.ChangeState(entity.Name, ItemState.Running);
            PageInstance.GetLister<DispatcherBusiness>().LoadItems();
            entityWindow.Hide();
        }

        protected void setPropertiesHandler(object sender, SetPropertyProfileEventArgs e)
        {
            BusinessObject.SetProfile(CurrentID, e.UpdatedProfile);
            WebHelper.ShowMessage("Successfully changed properties on running dispatcher.\n\n" + e.UpdatedProfile.ToString());
            //entityWindow.Hide();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                dsStartup.DataBind();
            }
        }





    }
}