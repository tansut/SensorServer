using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;

namespace Kalitte.Sensors.Web.Controls
{
    public class MenuData
    {
        public string ID { get; set; }
    }

    public class PropertyProfileMenu : GridPanel
    {
        public delegate void OnRowSelected(object sender, string id);
        public event OnRowSelected RowSelected;

        public PropertyProfileMenu()
        {
            Width = 200;
            Region = Ext.Net.Region.West;
            AutoScroll = true;
            Split = true;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Ext.Net.Store store = new Store();
            store.ID = this.ID + "store";
            var reader = new JsonReader();

            reader.IDProperty = "ID";
            reader.Fields.Add("ID", RecordFieldType.String);
            store.Reader.Add(reader);

            Store.Add(store);

            if (!Ext.Net.X.IsAjaxRequest)
            {
                TTColumn column = new TTColumn();
                column.DataIndex = "ID";
                column.ColumnID = "idCol";
                this.ColumnModel.Columns.Add(column);
                this.AutoExpandColumn = "idCol";
                column.Header = "Property Groups";
            }

            RowSelectionModel rowModel = new RowSelectionModel();
            rowModel.ID = this.ID + "rowModel";
            rowModel.SingleSelect = true;
            

            rowModel.DirectEvents.RowSelect.Event += new ComponentDirectEvent.DirectEventHandler(SelectionChange_Event);
            EventMask mask = rowModel.DirectEvents.RowDeselect.EventMask;
            mask.ShowMask = true;
            mask.Target = MaskTarget.CustomTarget;
            mask.CustomTarget = Parent.ClientID;
            mask.Msg = "Loading ...";
            SelectionModel.Add(rowModel);


        }

        void SelectionChange_Event(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = SelectionModel.Primary as RowSelectionModel;
            if (RowSelected != null)
                RowSelected(this, sm.SelectedRecordID);
        }


        internal void Edit(Configuration.PropertyList profile)
        {
            List<MenuData> data = new List<MenuData>();

            foreach (var item in profile)
            {
                if (!data.Any(p => p.ID == item.Key.GroupName))
                    data.Add(new MenuData() { ID = item.Key.GroupName });
            }
            
            var store = this.GetStore();

            store.DataSource = data;
            store.DataBind();



        }



        internal void SelectFirst()
        {
            (SelectionModel.Primary as RowSelectionModel).SelectFirstRow();
        }

        internal void SelectById(string group)
        {
            (SelectionModel.Primary as RowSelectionModel).SelectById(group);
            
        }

        internal void Clear()
        {
            List<MenuData> data = new List<MenuData>();
            var store = this.GetStore();
            store.DataSource = data;
            store.DataBind();
        }
    }
}
