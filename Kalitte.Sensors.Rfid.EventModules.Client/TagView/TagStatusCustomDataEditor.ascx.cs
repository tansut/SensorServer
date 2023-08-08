using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.UI;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Rfid.EventModules.Client.TagView.TagStatusCustomDataEditor
{
    public partial class TagStatusCustomDataEditor : System.Web.UI.UserControl, ICustomPropertyEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region ICustomPropertyEditor Members

        public void Edit(string serializedValue)
        {
            if (string.IsNullOrWhiteSpace(serializedValue))
                ccc.Text = "default";
            else
            {
                TagStatusCustomData data = SerializationHelper.DeserializeFromXmlDataContract < TagStatusCustomData>(serializedValue);
                ccc.Text = data.Prop1;
            }
        }

        public string EndEdit()
        {
            TagStatusCustomData d = new TagStatusCustomData();
            d.Prop1 = ccc.Text;
            return SerializationHelper.SerializeToXmlDataContract(d);
        }

        #endregion
    }
}