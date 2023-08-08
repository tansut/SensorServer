using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;
using Ext.Net;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Web.Controls
{
    public class PropertyProfileEditor : TTPanel
    {
        PropertyProfileGrid grid = new PropertyProfileGrid();
        PropertyProfileMenu menu = new PropertyProfileMenu();


        Ext.Net.Hidden hiddenProfile = new Ext.Net.Hidden();
        Ext.Net.Hidden hiddenLastSelected = new Ext.Net.Hidden();
        Ext.Net.Hidden hiddenMetaData = new Ext.Net.Hidden();
        Ext.Net.Hidden btnStatus = new Hidden();
        Ext.Net.Toolbar toolBar = new Toolbar();
        Ext.Net.Button setPropertyValuesBtn = new Button();
        Ext.Net.BorderLayout gridLayout = new BorderLayout();
        Ext.Net.TextArea descriptionArea = new TextArea();
        Ext.Net.Toolbar gridToolBar = new Toolbar();
        Ext.Net.FieldSet fieldSet = new FieldSet();
        FitLayout fitLayout = new FitLayout();
        //TTWindow customEditorWindow = new TTWindow();

        public event EventHandler<SetPropertyProfileEventArgs> SetPropertyProfile;

        

        public PropertyProfileEditor()
            : base()
        {
            Layout = "BorderLayout";
            Border = false;
            menu.RowSelected += new PropertyProfileMenu.OnRowSelected(menu_RowSelected);

        }

        public void ShowSetPropertiesBtn()
        {
            setPropertyValuesBtn.Show();
        }

        public void HideSetPropertiesBtn()
        {
            setPropertyValuesBtn.Hide();
        }


        protected override void OnInit(EventArgs e)
        {
            Items.Clear();
            grid.ID = this.ID + "grid";
            menu.ID = this.ID + "menu";
            hiddenProfile.ID = this.ID + "hiddenProfile";
            hiddenLastSelected.ID = this.ID + "lastSelected";
            hiddenMetaData.ID = this.ID + "hiddenMetadata";
            btnStatus.ID = this.ID + "btnStatus";
            descriptionArea.ID = this.ID+ "descriptionArea";                 
            gridToolBar.ID = this.ID + "gridToolBar";
            gridToolBar.AutoHeight = false;
            gridToolBar.Height = 80;
            fieldSet.ID = this.ID + "fieldSet";
            fieldSet.Title = "Description";
            fieldSet.Layout = "FitLayout";            
            fitLayout.ID = this.ID + "fitLayout";
            
            //descriptionArea.IDMode = Ext.Net.IDMode.Static;
            descriptionArea.Flex = 1;
            descriptionArea.HideLabel = true;
            descriptionArea.ReadOnly = true;            
            
            Items.Add(menu);
            //gridLayout.West.Items.Add(grid);
            //gridLayout.South.Items.Add(descriptionArea);
            grid.BottomBar.Add(gridToolBar);
            Items.Add(grid);
            gridToolBar.Add(fitLayout);
            fitLayout.Items.Add( fieldSet);
            fieldSet.Add(descriptionArea);                
            Items.Add(hiddenProfile);
            Items.Add(hiddenLastSelected);
            Items.Add(hiddenMetaData);
            Items.Add(btnStatus);
            //Items.Add(customEditorWindow);

            toolBar.ID = this.ID + "toolbar";
            toolBar.Layout = "HBoxLayout";            
            setPropertyValuesBtn.ID = this.ID + "setBtn";
            TopBar.Add(toolBar);
            toolBar.Add(setPropertyValuesBtn);
            if (!Ext.Net.X.IsAjaxRequest)
            {
                setPropertyValuesBtn.Hidden = true;
                setPropertyValuesBtn.Text = "Set Properties";
                setPropertyValuesBtn.Icon = Ext.Net.Icon.Accept;
            }
            grid.Listeners.CellClick.Handler = string.Format("Ext.getCmp('{0}').setValue(Ext.getCmp('{1}').descriptions[this.getStore().getAt(rowIndex).data.name])", descriptionArea.ClientID, grid.ClientID);
            setPropertyValuesBtn.DirectEvents.Click.Event += new ComponentDirectEvent.DirectEventHandler(setPropertyValuesBtn_DirectClick);
            base.OnInit(e);
        }

 


        void setPropertyValuesBtn_DirectClick(object sender, DirectEventArgs e)
        {
            Dictionary<PropertyKey, EntityMetadata> metaData = (Dictionary<PropertyKey, EntityMetadata>)SerializationHelper.BinaryDeSerialize(hiddenMetaData.Text);
            PropertyList profile = new PropertyList();
            grid.SaveProfile(profile, metaData, string.Empty, true);
            if (SetPropertyProfile != null)
                SetPropertyProfile(this, new SetPropertyProfileEventArgs(profile));
        }

        void menu_RowSelected(object sender, string group)
        {
            PropertyList profile = (PropertyList)SerializationHelper.BinaryDeSerialize(hiddenProfile.Text);
            Dictionary<PropertyKey, EntityMetadata> metaData = (Dictionary<PropertyKey, EntityMetadata>)SerializationHelper.BinaryDeSerialize(hiddenMetaData.Text);
            grid.SaveProfile(profile, metaData, hiddenLastSelected.Text);
            hiddenProfile.Text = SerializationHelper.BinarySerialize(profile);
            grid.Edit(profile, metaData, group);
            hiddenLastSelected.Text = group;
        }

        public PropertyList EndEdit()
        {
            if (string.IsNullOrEmpty(hiddenProfile.Text) || string.IsNullOrEmpty(hiddenMetaData.Text))
                return new PropertyList();
            PropertyList profile = (PropertyList)SerializationHelper.BinaryDeSerialize(hiddenProfile.Text);
            Dictionary<PropertyKey, EntityMetadata> metaData = (Dictionary<PropertyKey, EntityMetadata>)SerializationHelper.BinaryDeSerialize(hiddenMetaData.Text);
            grid.SaveProfile(profile, metaData, hiddenLastSelected.Text);
            return profile;
        }

        public void Edit(PropertyList profile, Dictionary<PropertyKey, EntityMetadata> metaData)
        {
            if (profile == null || metaData == null || profile.Count == 0)
            {
                hiddenProfile.Text = string.Empty;
                hiddenMetaData.Text = string.Empty;
                this.Clear();
                return;
            }
            hiddenProfile.Text = SerializationHelper.BinarySerialize(profile);
            hiddenMetaData.Text = SerializationHelper.BinarySerialize(metaData);
            menu.Edit(profile);
            var firstKey = profile.Keys.FirstOrDefault();
            if (firstKey != null)
            {
                menu.SelectById(firstKey.GroupName);
                grid.Edit(profile, metaData, firstKey.GroupName);
            }
            menu.Show();
            grid.Show();
        }

        public void Clear()
        {
            menu.Clear();
            grid.Clear();
            menu.Hide();
            grid.Hide();
            descriptionArea.Clear();
        }
    }
}
