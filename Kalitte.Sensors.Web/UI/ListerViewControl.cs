using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Web.Controls;
using Ext.Net;
using System.Collections;
using Kalitte.Sensors.Web.Business;

namespace Kalitte.Sensors.Web.UI
{
    public abstract class ListerViewControl<T> : ViewControlBase<T> where T: BusinessBase
    {
        protected abstract TTStore Store { get; }
        protected abstract TTGrid Grid { get; }

        public override ViewControlType ControlType
        {
            get { return ViewControlType.Lister; }
        }

        public virtual IList LoadItems()
        {
            IList source = GetItems();
            if (source == null)
                return null;
            Store.DataSource = source;
            Store.DataBind();
            if (source.Count > 0)
                Grid.SelectIfNotSelected(0);
            return source;
        }

        protected virtual System.Collections.IList GetItems()
        {
            return BusinessObject.GetItems();
        }

        public ListerViewControl()
            : base()
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Store.RefreshData += new Ext.Net.Store.AjaxRefreshDataEventHandler(Store_RefreshData);
            Page.Load += new EventHandler(Page_Load);
        }

        void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadItems();
        }

        void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest && Store.AutoLoad)
            {
                LoadItems();
            } 
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }
    }
}
