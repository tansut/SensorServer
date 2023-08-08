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

namespace Kalitte.Sensors.Web.UI.Pages.Processors
{
    public partial class Editor : EditorViewControl<ProcessorBusiness>
    {
        protected void closeWindow(object sender, DirectEventArgs e)
        {
            ctlAnalyseItem.Reset();
            ctlModuleEditor.HideAllWindows();
            ctlLogicalSensorEditor.HideAllWindows();
            ctlLogView.DisableAutoRefresh();
            ctlLastEvents.DisableAutoRefresh();
        }

        protected override void OnInit(EventArgs e)
        {
            ctlModuleEditor.ProcessorEditor = this;
            ctlLogicalSensorEditor.ProcessorEditor = this;
            base.OnInit(e);
        }

        public void initForm()
        {
            ctlGenForm.ClearFields();
            ctlLocForm.ClearFields();
            ctlLocBindingsForm.ClearFields();

            dsLogicalSensors.DataSource = new LogicalSensorBusiness().GetItems();
            dsLogicalSensors.DataBind();
            dsModules.DataSource = new EventModuleBusiness().GetItems();
            dsModules.DataBind();



            if (!string.IsNullOrEmpty(CurrentID))
            {
                CurrentLogicalSensorBindings = new List<Logical2ProcessorBindingEntity>(BusinessObject.GetProcessor2LogicalBindings(CurrentID));
                dsLogicalBindings.DataSource = CurrentLogicalSensorBindings;
                dsLogicalBindings.DataBind();

                CurrentModuleBindings = new List<Processor2ModuleBindingEntity>(BusinessObject.GetProcessor2ModuleBindings(CurrentID));
                dsProcessorModules.DataSource = CurrentModuleBindings;
                dsProcessorModules.DataBind();
            }
        }








        #region ModuleBindings


        private List<Processor2ModuleBindingEntity> CurrentModuleBindings
        {
            get
            {
                if (ViewState["CurrentModuleBindings"] == null)
                    ViewState["CurrentModuleBindings"] = new List<Processor2ModuleBindingEntity> { };
                return (List<Processor2ModuleBindingEntity>)ViewState["CurrentModuleBindings"];
            }
            set
            {
                ViewState["CurrentModuleBindings"] = value;
            }
        }

        [CommandHandler(CommandName = "DecrementModuleBindingPosition", ControllerType = typeof(ProcessorBusiness))]
        public void DecrementModuleBindingPositionHandler(object sender, CommandInfo command)
        {
            var bindings = CurrentModuleBindings;
            var currentIndex = CurrentModuleBindings.FindIndex(p => p.Name == command.RecordID);
            if (currentIndex != 0 && bindings.Count > 1)
            {
                var save = bindings[currentIndex - 1];
                bindings[currentIndex - 1] = bindings[currentIndex];
                bindings[currentIndex] = save;
                dsProcessorModules.DataSource = CurrentModuleBindings;
                dsProcessorModules.DataBind();
            }
        }

        [CommandHandler(CommandName = "IncrementModuleBindingPosition", ControllerType = typeof(ProcessorBusiness))]
        public void IncrementModuleBindingPositionHandler(object sender, CommandInfo command)
        {
            var bindings = CurrentModuleBindings;
            var currentIndex = CurrentModuleBindings.FindIndex(p => p.Name == command.RecordID);
            if (currentIndex != bindings.Count - 1 && bindings.Count > 1)
            {
                var save = bindings[currentIndex + 1];
                bindings[currentIndex + 1] = bindings[currentIndex];
                bindings[currentIndex] = save;
                dsProcessorModules.DataSource = CurrentModuleBindings;
                dsProcessorModules.DataBind();
            }
        }


        [CommandHandler(CommandName = "CreateModuleProcessorInEditor", ControllerType = typeof(ProcessorBusiness))]
        public void CreateModuleProcessorInEditorHandler(object sender, CommandInfo command)
        {
            //if (CurrentModuleBindings.Any(p => p.Processor == CurrentID && p.Module == ctlModules.SelectedAsString))
            //    throw new BusinessException("Module already exists. You can only create one instance of module for processor");
            command.Parameters.Add("processorName", CurrentID);
            command.Parameters.Add("moduleName", ctlModules.SelectedAsString);
            command.Parameters.Add("execOrder", CurrentModuleBindings.Count);
            ctlModuleEditor.CreateInEditorHandler(sender, command);
        }

        [CommandHandler(CommandName = "RemoveModuleBinding", ControllerType = typeof(ProcessorBusiness))]
        public void RemoveModuleBindingHandler(object sender, CommandInfo command)
        {
            var bindings = CurrentModuleBindings;
            bindings.RemoveAll(p => p.Name == command.RecordID);
            CurrentModuleBindings = bindings;
            dsProcessorModules.DataSource = CurrentModuleBindings;
            dsProcessorModules.DataBind();
        }

        [CommandHandler(CommandName = "EditModuleBinding", ControllerType = typeof(ProcessorBusiness))]
        public void EditModuleBindingHandler(object sender, CommandInfo command)
        {
            var binding = CurrentModuleBindings.Single(p => p.Name == command.RecordID);
            command.Parameters.Add("binding", binding);
            ctlModuleEditor.EditInEditorHandler(this, command);
        }


        protected void dsModuleBindings_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            CurrentModuleBindings = new List<Processor2ModuleBindingEntity>(BusinessObject.GetProcessor2ModuleBindings(CurrentID));
            dsProcessorModules.DataSource = CurrentModuleBindings;
            dsProcessorModules.DataBind();
        }

        public void AddModuleBinding(Processor2ModuleBindingEntity newModuleBinding)
        {
            var bindings = CurrentModuleBindings;
            bindings.Add(newModuleBinding);
            CurrentModuleBindings = bindings;
            dsProcessorModules.DataSource = CurrentModuleBindings;
            dsProcessorModules.DataBind();
        }


        internal void UpdateModuleBinding(Processor2ModuleBindingEntity entity)
        {
            var bindings = CurrentModuleBindings;
            var currentIndex = CurrentModuleBindings.FindIndex(p => p.Name == entity.Name);
            bindings[currentIndex] = entity;
            CurrentModuleBindings = bindings;
            dsProcessorModules.DataSource = CurrentModuleBindings;
            dsProcessorModules.DataBind();
        }

        #endregion



        #region logicalSensorBindings

        private List<Logical2ProcessorBindingEntity> CurrentLogicalSensorBindings
        {
            get
            {
                if (ViewState["currentLogicalSensorBindings"] == null)
                    ViewState["currentLogicalSensorBindings"] = new List<Logical2ProcessorBindingEntity>();
                return (List<Logical2ProcessorBindingEntity>)ViewState["currentLogicalSensorBindings"];
            }
            set
            {
                ViewState["currentLogicalSensorBindings"] = value;
            }
        }

        public void AddLogicalSensorBinding(Logical2ProcessorBindingEntity newModuleBinding)
        {
            var bindings = CurrentLogicalSensorBindings;
            bindings.Add(newModuleBinding);
            CurrentLogicalSensorBindings = bindings;
            dsLogicalBindings.DataSource = CurrentLogicalSensorBindings;
            dsLogicalBindings.DataBind();
        }


        internal void UpdateLogicalSensorBinding(Logical2ProcessorBindingEntity entity)
        {
            var bindings = CurrentLogicalSensorBindings;
            var currentIndex = CurrentLogicalSensorBindings.FindIndex(p => p.Name == entity.Name);
            bindings[currentIndex] = entity;
            CurrentLogicalSensorBindings = bindings;
            dsLogicalBindings.DataSource = CurrentLogicalSensorBindings;
            dsLogicalBindings.DataBind();
        }

        [CommandHandler(CommandName = "DecrementLogicalSensorBindingPosition", ControllerType = typeof(ProcessorBusiness))]
        public void DecrementEditLogicalSensorBindingPositionHandler(object sender, CommandInfo command)
        {
            var bindings = CurrentLogicalSensorBindings;
            var currentIndex = CurrentLogicalSensorBindings.FindIndex(p => p.Name == command.RecordID);
            if (currentIndex != 0 && bindings.Count > 1)
            {
                var save = bindings[currentIndex - 1];
                bindings[currentIndex - 1] = bindings[currentIndex];
                bindings[currentIndex] = save;
                dsLogicalBindings.DataSource = CurrentLogicalSensorBindings;
                dsLogicalBindings.DataBind();
            }
        }

        [CommandHandler(CommandName = "IncrementLogicalSensorBindingPosition", ControllerType = typeof(ProcessorBusiness))]
        public void IncrementEditLogicalSensorBindingPositionHandler(object sender, CommandInfo command)
        {
            var bindings = CurrentLogicalSensorBindings;
            var currentIndex = CurrentLogicalSensorBindings.FindIndex(p => p.Name == command.RecordID);
            if (currentIndex != bindings.Count - 1 && bindings.Count > 1)
            {
                var save = bindings[currentIndex + 1];
                bindings[currentIndex + 1] = bindings[currentIndex];
                bindings[currentIndex] = save;
                dsLogicalBindings.DataSource = CurrentLogicalSensorBindings;
                dsLogicalBindings.DataBind();
            }
        }


        [CommandHandler(CommandName = "RemoveLogicalSensorBinding", ControllerType = typeof(ProcessorBusiness))]
        public void RemoveLogicalSensorBindingHandler(object sender, CommandInfo command)
        {
            var bindings = CurrentLogicalSensorBindings;
            bindings.RemoveAll(p => p.Name == command.RecordID);
            CurrentLogicalSensorBindings = bindings;
            dsLogicalBindings.DataSource = CurrentLogicalSensorBindings;
            dsLogicalBindings.DataBind();

        }

        [CommandHandler(CommandName = "StartLogicalSensor", ControllerType = typeof(ProcessorBusiness))]
        public void StartLogicalSensorBindingHandler(object sender, CommandInfo command)
        {
            var bindings = CurrentLogicalSensorBindings;
            var bindingIndex = bindings.FindIndex(p => p.Name == command.RecordID);
            BusinessObject.ChangeProcessorLogicalSensorBindingState(CurrentID, bindings[bindingIndex].LogicalSensorName, ItemState.Running);
            bindings[bindingIndex].Properties.StateInfo.State = ItemState.Running;
            CurrentLogicalSensorBindings = bindings;
        }

        [CommandHandler(CommandName = "EditLogicalSensorBinding", ControllerType = typeof(ProcessorBusiness))]
        public void EditLogicalSensorBindingHandler(object sender, CommandInfo command)
        {
            var binding = CurrentLogicalSensorBindings.Single(p => p.Name == command.RecordID);
            command.Parameters.Add("binding", binding);
            ctlLogicalSensorEditor.EditInEditorHandler(this, command);
        }

        [CommandHandler(CommandName = "CreateLogicalSensorBindingInEditor", ControllerType = typeof(ProcessorBusiness))]
        public void CreateLogicalSensorBindingHandler(object sender, CommandInfo command)
        {
            if (CurrentLogicalSensorBindings.Any(p => p.ProcessorName == CurrentID && p.LogicalSensorName == ctlLogicalSensors.SelectedAsString))
                throw new BusinessException("Binding already exists. You can only create one instance of logical sensor binding for processor");
            command.Parameters.Add("processorName", CurrentID);
            command.Parameters.Add("logicalSensorName", ctlLogicalSensors.SelectedAsString);
            ctlLogicalSensorEditor.CreateInEditorHandler(sender, command);
        }

        protected void dsLogicalBindings_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            CurrentLogicalSensorBindings = new List<Logical2ProcessorBindingEntity>(BusinessObject.GetProcessor2LogicalBindings(CurrentID));
            dsLogicalBindings.DataSource = CurrentLogicalSensorBindings;
            dsLogicalBindings.DataBind();
        }

        #endregion

        #region processors

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.EditInEditor, ControllerType = typeof(ProcessorBusiness))]
        public void EditInEditorHandler(object sender, CommandInfo command)
        {
            var entity = BusinessObject.GetItem(command.RecordID);
            CurrentID = entity.Name;
            initForm();


            entityWindow.Title = string.Format("Edit Processor: {0}", entity.Name);
            ctlSave.CommandName = KnownCommand.UpdateEntity.ToString();

            ctlLastEvents.ItemName = entity.Name;
            ctlName.Text = entity.Name;
            ctlName.ReadOnly = true;

            ctlInheritLogLevel.Checked = entity.Properties.LogLevel.Inherit;
            ctlLogLevel.SelectedAsString = entity.Properties.LogLevel.Level.ToString();

            ctlAddModule.CommandArgument = entity.Name;

            ctlLastException.Exception = entity.Properties.StateInfo.LastException;
            ctlInitialStartup.SelectedAsString = entity.Properties.Startup.ToString();
            ctlDescription.Text = entity.Description;
            ctlTabs.ActiveTabIndex = command.Parameters.ContainsKey("CommandArgument") ? int.Parse(command.Parameters["CommandArgument"].ToString()) : 0;

            ctlSensorBindingsPanel.Disabled = false;
            ctlProfilePanel.Disabled = false;
            ctlExtendedProfilePanel.Disabled = false;
            ctlModuleBindingsPanel.Disabled = false;
            ctlLogViewPanel.Disabled = false;
            ctlGenProps.Disabled = false;
            ctlAnalysisPanel.Disabled = false;
            ctlLastEventsPanel.Disabled = false;
            ctlMonitoringDataEditor.Bind(entity.Properties.MonitoringData);


            ctlNonExistEventHandler.SelectedAsString = entity.Properties.ModuleNonExistEventHandlerBehavior.ToString();
            ctlNullEventBehavior.SelectedAsString = entity.Properties.PipeNullEventBehavior.ToString();
            ctlProfileEditor.Clear();

            var extendedMetadata = BusinessObject.GetItemExtendedMetadata(ProcessingItem.Processor);
            var extendedMetadataOfItem = SensorCommon.GetEntityMetadata<ExtendedPropertyMetadata>(extendedMetadata.PropertyMetadata);
            ctlExtendedProfileEditor.Edit(entity.Properties.ExtendedProfile, extendedMetadataOfItem);

            ctlNonExistEventHandler.AllowBlank = false;
            ctlLogLevel.AllowBlank = false;
            ctlNullEventBehavior.AllowBlank = false;

            ctlSave.AddScript("{0}.confirm={1};", ctlSave.ClientID, (entity.State == ItemState.Running).ToString().ToLowerInvariant());


            if (ctlTabs.ActiveTab == ctlLogViewPanel)
            {
                ctlLogView.InitProperties(ProcessingItem.Processor, CurrentID, true);
            }
            else ctlLogView.InitProperties(ProcessingItem.Processor, CurrentID);

            if (ctlTabs.ActiveTab == ctlLastEventsPanel)
                ctlLastEvents.LoadItems();
            else ctlLastEvents.ClearEvents();

            ctlAnalyseItem.Bind(entity.Name);

            entityWindow.Show();
        }

        [CommandHandler(CommandName = "QueryLog", ControllerType = typeof(ProcessorBusiness))]
        public void QueryLogHandler(object sender, CommandInfo command)
        {
            ctlLogView.Bind();
        }





        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.CreateInEditor, ControllerType = typeof(ProcessorBusiness))]
        public void CreateInEditorHandler(object sender, CommandInfo command)
        {
            CurrentID = string.Empty;
            initForm();
            entityWindow.Title = "Create New Processor";
            ctlSave.CommandName = KnownCommand.CreateEntity.ToString();
            ctlGenForm.ClearFields();
            ctlLocForm.ClearFields();
            ctlMonitoringDataEditor.Clear();
            ctlName.ReadOnly = false;
            ctlName.Focus();

            dsModules.DataSource = new EventModuleBusiness().GetItems();
            dsModules.DataBind();

            ctlTabs.ActiveTabIndex = 0;
            ctlSensorBindingsPanel.Disabled = true;
            ctlProfilePanel.Disabled = true;
            ctlExtendedProfilePanel.Disabled = true;
            ctlModuleBindingsPanel.Disabled = true;
            ctlLogViewPanel.Disabled = true;
            ctlGenProps.Disabled = true;
            ctlAnalysisPanel.Disabled = true;
            ctlLastEventsPanel.Disabled = true;

            ctlNonExistEventHandler.AllowBlank = true;
            ctlLogLevel.AllowBlank = true;
            ctlNullEventBehavior.AllowBlank = true;
            ctlSave.AddScript("{0}.confirm={1};", ctlSave.ClientID, "false");

            entityWindow.Show();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.CreateEntity, ControllerType = typeof(ProcessorBusiness))]
        public void CreateEntityHandler(object sender, CommandInfo command)
        {

            ItemStartupType startup = ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();
            BusinessObject.CreateItem(ctlName.Text, ctlDescription.Text, startup);
            //BusinessObject.UpdateProcessor2ModuleBindings(ctlName.Text, CurrentModuleBindings.ToArray());
            PageInstance.GetLister<ProcessorBusiness>().LoadItems();
            entityWindow.Hide();
        }

        [CommandHandler(KnownCommand = Kalitte.Sensors.Web.Security.KnownCommand.UpdateEntity, ControllerType = typeof(ProcessorBusiness))]
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
            entity.Properties.ExtendedProfile = ctlExtendedProfileEditor.EndEdit();
            entity.Properties.ModuleNonExistEventHandlerBehavior = ctlNonExistEventHandler.GetSelectedAsType<NonExistEventHandlerBehavior>();
            entity.Properties.PipeNullEventBehavior = ctlNullEventBehavior.GetSelectedAsType<PipeNullEventBehavior>();
            entity.Properties.LogLevel.Inherit = ctlInheritLogLevel.Checked;
            entity.Properties.LogLevel.Level = ctlLogLevel.GetSelectedAsType<LogLevel>();
            ctlMonitoringDataEditor.Retrieve(entity.Properties.MonitoringData);
            BusinessObject.UpdateProcessorWithBindings(entity, CurrentModuleBindings.ToArray(), CurrentLogicalSensorBindings.ToArray());
            if (start)
                BusinessObject.ChangeState(entity.Name, ItemState.Running);
            PageInstance.GetLister<ProcessorBusiness>().LoadItems();

            entityWindow.Hide();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                dsStartup.DataBind();
                dsLogLevel.DataBind();
                dsNonExistEventHandler.DataBind();
                dsNullEventBehavior.DataBind();
            }
        }





    }
}