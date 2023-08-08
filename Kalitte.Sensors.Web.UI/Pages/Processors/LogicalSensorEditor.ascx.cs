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
    public partial class LogicalSensorEditor : EditorViewControl<ProcessorBusiness>
    {



        private void FillUI(CommandInfo cmd)
        {

        }





        public void CreateInEditorHandler(object sender, CommandInfo command)
        {
            CurrentID= command.Parameters["processorName"].ToString();
            CurrentDetailID = command.Parameters["logicalSensorName"].ToString();

            ctlProcessorName.Text = CurrentID;
            ctlLogicalSensorName.Text = CurrentDetailID;

            entityWindow.Title = string.Format("Add new logical sensor to {0}", CurrentID); 
            ctlSave.CommandName = "CreateLogicalSensorBinding";

            ctlGenForm.ClearFields();


            ctlTabs.ActiveTabIndex = 0;
            //ctlProfilePanel.Disabled = true;
            //ctlExtendedProfileEditor.Disabled = true;
            entityWindow.Show();
        }


        public void EditInEditorHandler(object sender, CommandInfo command)
        {
            ctlGenForm.ClearFields();
            Logical2ProcessorBindingEntity entity = command.Parameters["binding"] as Logical2ProcessorBindingEntity;
            ViewState["binding"] = entity;
            ctlProcessorName.Text = entity.ProcessorName;
            ctlLogicalSensorName.Text = entity.LogicalSensorName;

            entityWindow.Title = string.Format("Edit logical sensor binding: {0}", entity.LogicalSensorName);
            ctlSave.CommandName = "UpdateLogicalSensorBinding";

            ctlLastException.Exception = entity.Properties.StateInfo.LastException;
            //ctlInitialStartup.SelectedAsString = entity.Properties.Startup.ToString();
            ctlDescription.Text = entity.Description;

            ctlTabs.ActiveTabIndex = 0;
            //ctlProfilePanel.Disabled = false;

            //var metaDataOfItem = BusinessObject.GetProcessorMetadata(entity.Name);
            //Dictionary<PropertyKey, EntityMetadata> metaData = new Dictionary<PropertyKey, EntityMetadata>();
            //foreach (var item in metaDataOfItem.ProcessorPropertyMetadata)
            //    metaData.Add(item.Key, item.Value);

            //ctlProfileEditor.Edit(entity.Properties.Profile, metaData);
            entityWindow.Show();
        }


        [CommandHandler(CommandName = "CreateLogicalSensorBinding", ControllerType = typeof(ProcessorBusiness))]
        public void CreateEntityHandler(object sender, CommandInfo command)
        {
            ItemStartupType startup = ItemStartupType.Automatic; //ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();

            var properties = new Logical2ProcessorBindingProperty(startup);
            var newEntity = new Logical2ProcessorBindingEntity(CurrentDetailID, CurrentID, properties);
            newEntity.Description = ctlDescription.Text;
            ProcessorEditor.AddLogicalSensorBinding(newEntity);
            entityWindow.Hide();
        }

        [CommandHandler(CommandName = "UpdateLogicalSensorBinding", ControllerType = typeof(ProcessorBusiness))]
        public void UpdateEntityHandler(object sender, CommandInfo command)
        {
            var entity = ViewState["binding"] as Logical2ProcessorBindingEntity;
            ItemStartupType startup = entity.Startup;// ctlInitialStartup.GetSelectedAsType<Kalitte.Sensors.Processing.ItemStartupType>();
            //entity.Properties.Profile = profileEditorCtrl.EndEdit();
            entity.Properties.Startup = startup;
            entity.Description = ctlDescription.Text;
            ProcessorEditor.UpdateLogicalSensorBinding(entity);
            entityWindow.Hide();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                dsStartup.DataBind();
                //ctlInitialStartup.SelectedAsString = ItemStartupType.Automatic.ToString();
            }
        }




        public Editor ProcessorEditor { get; set; }


    }
}