using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.UI;
using Kalitte.Sensors.Rfid.Commands;
using System.Text;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Rfid.Utilities;


namespace Kalitte.Sensors.Rfid.Client.CommandEditors
{
    public partial class QueryTagsCommandEditor : System.Web.UI.UserControl, ISensorCommandEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        #region ISensorCommandEditor Members

        public SensorCommand CreateCommand(string sensorName, string source)
        {
            return new QueryTagsCommand(RfidHelper.GetBytes(ctlPasscode.Text), ctlDataSelect.GetSelector());
        }

        public void ShowResponse(ResponseEventArgs e)
        {
            if (e.CommandError == null)
            {
                tagRepeater.Visible = true;
                var response = ((Kalitte.Sensors.Rfid.Commands.QueryTagsCommand)(e.Command)).Response;

                UnifyTagsHandler handler = new UnifyTagsHandler(UnifiyTagSelectStrategy.SelectLastTag, response.Tags);
                tagRepeater.DataSource = handler.FilterDuplicates();
                tagRepeater.DataBind();
            }
            else tagRepeater.Visible = false;
        }

        #endregion

        protected void tagRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                var tagIdLabel = e.Item.FindControl("ctlTagId") as Label;
                var tagDataLabel = e.Item.FindControl("ctlTagData") as Label;
                var tagLastSeen = e.Item.FindControl("ctlLastSeen") as Label;
                var tagRssi = e.Item.FindControl("ctlRssi") as Label;
                var tagReadCount = e.Item.FindControl("ctlReadCount") as Label;
                var tagVendor = e.Item.FindControl("ctlVendorData") as Label;

                TagReadEvent data = e.Item.DataItem as TagReadEvent;
                tagIdLabel.Text = HexHelper.HexEncode(data.GetId());
                tagDataLabel.Text = HexHelper.HexEncode(data.GetData());
                if (data.VendorSpecificData.ContainsKey(TagReadEvent.LastSeen))
                    tagLastSeen.Text = data.VendorSpecificData[TagReadEvent.LastSeen].ToString();
                if (data.VendorSpecificData.ContainsKey(TagReadEvent.Rssi))
                    tagRssi.Text = data.VendorSpecificData[TagReadEvent.Rssi].ToString();
                if (data.VendorSpecificData.ContainsKey(TagReadEvent.ReadCount))
                    tagReadCount.Text = data.VendorSpecificData[TagReadEvent.ReadCount].ToString();
                tagVendor.Text = data.HasVendorData().ToString();
                if (data.HasVendorData())
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in data.VendorSpecificData)
                    {
                        if (item.Key != TagReadEvent.LastSeen && item.Key != TagReadEvent.ReadCount && item.Key != TagReadEvent.Rssi)
                            sb.AppendFormat("{0}: {1}\n", item.Key, item.Value == null ? "null" : item.Value.ToString());
                    }
                    tagVendor.ToolTip = sb.ToString();
                }
            }
        }
    }
}