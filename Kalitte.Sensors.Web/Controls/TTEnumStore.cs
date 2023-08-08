using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Ext.Net;
using Kalitte.Sensors.Web.Utility;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTEnumStore: TTStore
    {                
        public string EnumType { get; set; }

        protected override void OnInit(EventArgs e)
        {
            JsonReader reader = new JsonReader(new JsonReader.Config() { IDProperty = "Key" });
            reader.Fields.Add("Key", RecordFieldType.String);
            reader.Fields.Add("Value", RecordFieldType.String);
            this.Reader.Clear();
            this.Reader.Add(reader);
            base.OnInit(e);
        }


        public Dictionary<string, string> GetItems(Func<KeyValuePair<string, string>, bool> filter = null)
        {
            Type t = Type.GetType(EnumType);
            if (!t.IsEnum)
            {
                throw new ArgumentException("EnumType");
            }
            return WebHelper.GetDescriptionalEnumInfo(t,filter);
        }

        public override void DataBind()
        {
            if (!string.IsNullOrEmpty(EnumType))
            {
                DataSource = GetItems();
            }
            base.DataBind();
        }

        public void DataBindFiltered(Func<KeyValuePair<string, string>, bool> filter)
        {
            if (!string.IsNullOrEmpty(EnumType))
            {
                DataSource = GetItems(filter);
            }
            base.DataBind();
        }
    }
}
