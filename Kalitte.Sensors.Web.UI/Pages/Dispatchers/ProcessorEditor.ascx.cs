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

namespace Kalitte.Sensors.Web.UI.Pages.Dispatchers
{
    public partial class ProcessorEditor : EditorViewControl<DispatcherBusiness>
    {



        private void FillUI(CommandInfo cmd)
        {

        }


        public void CreateInEditorHandler(object sender, CommandInfo command)
        {
            CurrentID= command.Parameters["dispatcherName"].ToString();
            CurrentDetailID = command.Parameters["processorName"].ToString();

            ctlDispatcherName.Text = CurrentID;
            ctlProcessorName.Text = CurrentDetailID;

            entityWindow.Title = string.Format("Add new processor to {0}", CurrentID); 
            ctlSave.CommandName = "CreateProcessorBinding";

            ctlGenForm.ClearFields();


            ctlTabs.ActiveTabIndex = 0;
            ctlProfilePanel.Disabled = true;
            entityWindow.Show();
        }


        public void EditInEditorHandler(object sender, CommandInfo command)
        {
            ctlGenForm.ClearFields();
            var entity = command.Parameters["binding"] as Dispatcher2ProcessorBindingEntity;
            ViewState["binding"] = entity;
            ctlProcessorName.Text = entity.Processor;
            ctlDispatcherName.Text = entity.Dispatcher;

            entityWindow.Title = string.Format("Edit processor binding: {0}", entity.Processor);
            ctlSave.CommandName = "UpdateProcessorBinding";

            ctlLastException.Exception = entity.Properties.StateInfo.LastException;
            ctlInitialStartup.SelectedAsString = entity.Properties.Startup.ToString();
            ctlDescription.Text = entity.Description;

            ctlTabs.ActiveTabIndex = 0;
            ctlProfilePanel.Disabled = false;

            var metaDataOfItem = BusinessObject.GetMetadata(entity.Name);
            Dictionary<PropertyKey, EntityMetadata> metaData = new Dictionary<PropertyKey, EntityMetadata>();
            foreach (var item in metaDataOfItem.DispatcherPropertyMetadata)
                metaData.Add(item.Key, item.Value);

            profileEditorCtrl.Edit(entity.Properties.Profile, metaData);
            entityWindow.Show();
        }




        [CommandHandler(CommandName = "CreateProcessorBinding", ControllerType = typeof(DispatcherBusiness))]
        public void CreateEntityHandler(object sender, CommandInfo command)
        {
            ItemStartupType startup = ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();

            var properties = new Dispatcher2ProcessorBindingProperty(startup);
            var newEntity = new Dispatcher2ProcessorBindingEntity(CurrentID, CurrentDetailID, properties);
            newEntity.Description = ctlDescription.Text;
            DispatcherEditor.AddProcessorBinding(newEntity);
            entityWindow.Hide();
        }

        [CommandHandler(CommandName = "UpdateProcessorBinding", ControllerType = typeof(DispatcherBusiness))]
        public void UpdateEntityHandler(object sender, CommandInfo command)
        {
            var entity = ViewState["binding"] as Dispatcher2ProcessorBindingEntity;
            ItemStartupType startup = ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();
            entity.Properties.Profile = profileEditorCtrl.EndEdit();
            entity.Properties.Startup = startup;
            entity.Description = ctlDescription.Text;
            DispatcherEditor.UpdateProcessorBinding(entity);
            entityWindow.Hide();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                dsStartup.DataBind();
                ctlInitialStartup.SelectedAsString = ItemStartupType.Automatic.ToString();
            }
        }




        public Editor DispatcherEditor { get; set; }
    }
}