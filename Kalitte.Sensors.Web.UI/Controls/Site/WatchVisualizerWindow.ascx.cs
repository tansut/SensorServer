using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Ext.Net;

namespace Kalitte.Sensors.Web.UI.Controls.Site
{
    public partial class WatchVisualizerWindow : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ShowWindow(string watchName, string categoryName, string instanceName, string[] measures)
        {
            string url = string.Format("{0}?WatchName={1}&CategoryName={2}&InstanceName={3}&MeasureNames={4}&R={5}",
                ResolveUrl("~/Pages/Server/WatchManagement/WatchVisualizerPage.aspx"),
                HttpUtility.UrlEncode(watchName),
                HttpUtility.UrlEncode(categoryName),
                HttpUtility.UrlEncode(instanceName),
                HttpUtility.UrlEncode(getUrl(measures)), new Random().NextDouble());

            var win = new Window
            {
                Title = string.Format("{0}/{1}/{2}/[{3}]", watchName, categoryName, instanceName, getUrl(measures)),
                Width = Unit.Pixel(800),
                Height = Unit.Pixel(335),
                Modal = false,
                Collapsible = true,
                Maximizable = true,
                Icon= Ext.Net.Icon.ChartBar,
                Hidden = false,
                CloseAction = CloseAction.Close,
                AutoScroll=false,
                BodyBorder=false
            };

            win.AutoLoad.Url = url;
            win.AutoLoad.Mode = LoadMode.IFrame;

            win.Render(this.Page.Form);
        }



        private string getUrl(string[] measures)
        {
            StringBuilder sb = new StringBuilder(measures.Length * 25);
            foreach (var item in measures)
            {
                sb.AppendFormat("{0},", item);
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}