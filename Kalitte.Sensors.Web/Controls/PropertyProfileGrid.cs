using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;
using Kalitte.Sensors.Configuration;
using System.ComponentModel;
using Kalitte.Sensors.Web.Utility;
using Kalitte.Sensors.Web.Security;
using Kalitte.Sensors.Utilities;
using System.Web.Script.Serialization;

namespace Kalitte.Sensors.Web.Controls
{
    public class PropertyProfileGrid : PropertyGrid
    {
        private string getValueAsString(object value)
        {
            if (value == null)
                return string.Empty;
            else return value.ToString();
        }

        public PropertyProfileGrid()
            : base()
        {
            Region = Ext.Net.Region.Center;

        }

        public void Edit(Configuration.PropertyList propertyProfile)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
                Title = "Properties";
            base.OnInit(e);
        }

        public override PropertyGridParameterCollection Source
        {
            get
            {
                if (base.Source == null || base.Source.Count == 0)
                {
                    base.Source.Add(new PropertyGridParameter("(name)", ""));
                }
                return base.Source;
            }
        }

        protected virtual TTPropertyGridParameter CreateParameter(string name, object defaultValue)
        {
            var mode = ParameterMode.Value;
            if (defaultValue != null)
            {
                //if (defaultValue is bool)
                //    mode = ParameterMode.Raw;
            }

            var item = new TTPropertyGridParameter(new PropertyGridParameter.Config() { Name = name, Mode = mode });
            return item;
        }


        public void ClearProperties()
        {
            string[] propertyNames = Source.Select(p => ((PropertyGridParameter)p).Name).ToArray();
            foreach (string name in propertyNames)
                RemoveProperty(name);
        }

        internal void Edit(Configuration.PropertyList profile, Dictionary<PropertyKey, EntityMetadata> metaData, string filterGroup)
        {
            ClearProperties();
            this.Show();
            Dictionary<string, string> nameDescList = new Dictionary<string, string>();
            foreach (var item in metaData)
            {
                if (item.Key.GroupName == filterGroup)
                {
                    object value = item.Value.DefaultValue;
                    if (profile.ContainsKey(item.Key))
                    {
                        value = profile[item.Key];
                        if (item.Value.HasCustomPropertyEditor())
                        {
                            if (value != null)
                            {
                                value = SerializationHelper.SerializeToXmlDataContract(value);
                            }
                        }
                    }

                    TTPropertyGridParameter p = CreateParameter(item.Key.PropertyName, item.Value.DefaultValue);
                    p.Metadata = item.Value;
                    p.PropertyKey = item.Key;
                    p.Owner = this;
                    p.Params.Add(new Parameter("desc", item.Value.Description, ParameterMode.Value));

                    AddProperty(p);
                    nameDescList.Add(item.Key.PropertyName, item.Value.Description);
                    if (value != null)
                        UpdateProperty(p.Name, value);
                }
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();


            this.ResourceManager.AddScript(string.Format("Ext.getCmp('{0}').descriptions = Ext.decode('{1}');", this.ClientID, jss.Serialize(RemoveEscapeChars(nameDescList))));

        }

        private Dictionary<string, string> RemoveEscapeChars(Dictionary<string, string> text)
        {
            Dictionary<string, string>  result = new Dictionary<string,string>();
            foreach (var item in text)
            {
                result.Add(item.Key, item.Value.Replace("\"", "\\\""));
            }
            return result;
        }

        internal void SaveProfile(Configuration.PropertyList profile, Dictionary<PropertyKey, EntityMetadata> metaData, string filterGroup, bool onlyChanges = false)
        {
            List<PropertyKey> keys = new List<PropertyKey>();

            foreach (var item in metaData)
            {
                if (string.IsNullOrEmpty(filterGroup) || item.Key.GroupName == filterGroup)
                    keys.Add(item.Key);
            }

            for (int i = 0; i < keys.Count; i++)
            {
                foreach (PropertyGridParameter param in Source)
                {
                    if (param.Name == keys[i].PropertyName)
                        SetProfileValue(profile, metaData, keys[i], param, onlyChanges);
                }
            }
        }




        private void SetProfileValue(PropertyList profile, Dictionary<PropertyKey, EntityMetadata> metaData,
            Configuration.PropertyKey metaDataKey, PropertyGridParameter param, bool onlyChanges = false)
        {
            object existingValue = null;
            bool valueExists = profile.TryGetValue(metaDataKey, out existingValue);
            var item = metaData[metaDataKey];
            if (param.IsChanged || !onlyChanges)
            {
                if (item.IsWritable)
                {
                    if (item.IsMandatory && string.IsNullOrWhiteSpace(param.Value)) throw new BusinessException(string.Format("{0} cannot be blank", param.Name));
                    if (TypesHelper.IsNumericType(item.Type))
                    {
                        object valueAsNumeric = ConvertHelper.ConvertType(item.Type, param.Value, metaData[metaDataKey].DefaultValue);
                        if (valueAsNumeric != null)
                            valueAsNumeric = ConvertHelper.ConvertType(typeof(double), valueAsNumeric, valueAsNumeric);
                        if (((IComparable)valueAsNumeric).CompareTo(item.HigherRange) > 0 || ((IComparable)valueAsNumeric).CompareTo(item.LowerRange) < 0)
                            throw new BusinessException(string.Format("{0} must be between {1} and {2}", param.Name, item.LowerRange, item.HigherRange));
                    }
                    object value = null;
                    if (item.HasCustomPropertyEditor())
                    {
                        if (string.IsNullOrWhiteSpace(param.Value))
                            value = null;
                        else value = SerializationHelper.DeserializeFromXmlDataContract(param.Value, item.Type);
                    }
                    else value = ConvertHelper.ConvertType(metaData[metaDataKey].Type, param.Value, existingValue);
                    profile[metaDataKey] = value;
                }
            }
        }

        internal void Clear()
        {
            ClearProperties();
        }
    }
}
