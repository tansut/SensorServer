using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using System.Reflection;
using Kalitte.Sensors.Extensions;

namespace Kalitte.Sensors.Utilities
{
    public static class XmlHelper
    {
        public static ProviderSettingsCollection GetProviderSettings(XElement settings)
        {
            ProviderSettingsCollection psc = new ProviderSettingsCollection();
            var providerSettings = settings.Descendants("add").ToList();
            foreach (var item in providerSettings)
            {
                ProviderSettings ps = new ProviderSettings();
                var atts = item.Attributes();
                foreach (var att in atts)
                {
                    switch (att.Name.ToString())
                    {
                        case "name": ps.Name = att.Value; break;
                        case "type": ps.Type = att.Value; break;
                        default: ps.Parameters.Add(att.Name.ToString(), att.Value); break;
                    }
                }
                psc.Add(ps);
            }
            return psc;
        }

        public static T SetObjectFromXml<T>(XElement element, T obj) where T : class
        {
            var atts = element.Attributes();
            var pInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType.IsValueType || p.PropertyType.IsEnum || p.PropertyType == typeof(string)).ToList();
            foreach (var att in atts)
            {
                var pi = pInfos.Where(p => p.Name.ToLowerInvariant() == att.Name.ToString().ToLowerInvariant()).SingleOrDefault();
                if (pi != null)
                {
                    if (pi.PropertyType == typeof(string))
                        pi.SetValue(obj, att.Value, null);
                    else if (pi.PropertyType.IsEnum) pi.SetValue(obj, att.Value.ToEnum(pi.PropertyType), null);
                    else if (pi.PropertyType.IsValueType) pi.SetValue(obj, Convert.ChangeType(att.Value, pi.PropertyType), null);
                }
            }
            return obj;
        }
    }
}
