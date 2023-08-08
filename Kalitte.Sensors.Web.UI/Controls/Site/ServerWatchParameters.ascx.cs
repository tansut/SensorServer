using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Web.Business;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;
using Ext.Net;

namespace Kalitte.Sensors.Web.UI.Controls.Site
{
    public partial class ServerWatchParameters : System.Web.UI.UserControl
    {
        ServerAnalysisBusiness business = new ServerAnalysisBusiness();
        public ServerAnalyseItem? AnalyseItem { get; set; }


        public string ActiveInstance
        {
            get
            {
                return (string)ViewState["ActiveInstance"];
            }

            set
            {
                ViewState["ActiveInstance"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string ProviderName
        {
            get
            {
                return ctlProviders.SelectedAsString;
            }
            set
            {
                ctlProviders.Value = value;
            }
        }

        public string InstanceName
        {
            get
            {
                return ctlInstance.SelectedAsString;
            }
            set
            {
                ctlInstance.Value = value;
            }
        }
        public string CategoryName
        {
            get
            {
                return ctlCategory.SelectedAsString;
            }
            set
            {
                ctlCategory.Value = value;
            }
        }

        public string MeasureName
        {
            get
            {
                return ctlMeasure.SelectedAsString;
            }
            set
            {
                ctlMeasure.Value = value;
            }
        }

        public bool PanelDisabled { get { return ctlSourcePanel.Disabled; } set { ctlSourcePanel.Disabled = value; } }

        private NameDescriptionList BindCategories(string analyserName)
        {
            NameDescriptionList list;
            if (AnalyseItem.HasValue)
                list = business.GetCategoryNames(analyserName, AnalyseItem.Value);
            else
                list = business.GetCategories(analyserName);
            dsCategory.DataSource = list;
            dsCategory.DataBind();
            if (list.Count > 0)
                ctlCategory.SetValue(list[0].Name);
            return list;
        }


        private void BindInstanceAndMeasures(string providerName, string categoryName)
        {
            var list = business.GetInstanceNames(providerName, categoryName);
            //if (!string.IsNullOrEmpty(ActiveInstance))
            //    list.RemoveAll(p => p.Name.ToUpperInvariant() != ActiveInstance.ToUpperInvariant());
            dsInstance.DataSource = list;
            dsInstance.DataBind();
            if (list.Count > 0)
                ctlInstance.SetValue(ActiveInstance ?? list[0].Name);

            var measureList = business.GetMeasureNames(ctlProviders.SelectedAsString, ctlCategory.SelectedAsString);
            dsMeasure.DataSource = measureList;
            dsMeasure.DataBind();
            if (measureList.Count > 0)
                ctlMeasure.SetValue(measureList[0].Name);
        }

        public void Bind(string activeInstance = null)
        {
            this.ActiveInstance = activeInstance;
            var watchList = business.GetWatcherNames();
            dsWatch.DataSource = watchList;
            dsWatch.DataBind();
            if (watchList.Count > 0)
            {
                string analyserName = watchList[0].Name;
                ctlProviders.SetValue(analyserName);
                var categories = BindCategories(analyserName);
                if (categories.Count > 0)
                    BindInstanceAndMeasures(analyserName, categories[0].Name);
            }
        }



        protected void ctlProviders_Select(object sender, DirectEventArgs e)
        {
            BindCategories(ctlProviders.SelectedAsString);
        }

        protected void ctlCategories_Select(object sender, DirectEventArgs e)
        {
            BindInstanceAndMeasures(ctlProviders.SelectedAsString, ctlCategory.SelectedAsString);
        }
    }
}