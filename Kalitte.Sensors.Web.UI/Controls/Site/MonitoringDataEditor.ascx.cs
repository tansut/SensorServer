using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Web.UI.Controls.Site
{
    public partial class MonitoringDataEditor : BaseEditor<ItemMonitoringData>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void Bind(ItemMonitoringData entity)
        {
            Current = entity;
            ctlCheckInterval.ValueAsInt = entity.CheckInterval;
            ctlMaxRetryCount.ValueAsInt = entity.MaxRetryCount;
            ctlEnabled.Checked = entity.Enabled;
        }

        public override void Retrieve(ItemMonitoringData entity)
        {
            entity.CheckInterval = ctlCheckInterval.ValueAsInt;
            entity.MaxRetryCount = ctlMaxRetryCount.ValueAsInt;
            entity.Enabled = ctlEnabled.Checked;
        }

        public override void Clear()
        {
            ctlCheckInterval.ValueAsInt = 5000;
            ctlMaxRetryCount.ValueAsInt = 0;
            ctlEnabled.Checked = false;
        }
    }
}