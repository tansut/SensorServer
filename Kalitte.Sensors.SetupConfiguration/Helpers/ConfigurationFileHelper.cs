using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;
using Kalitte.Sensors.SetupConfiguration.Core.Exception;

namespace Kalitte.Sensors.SetupConfiguration.Helpers
{
    public class ConfigurationFileHelper
    {
        XDocument Document = null;
        public string FilePath { get; set; }
        public ConfigurationFileHelper(string configFilePath)
        {
            if (File.Exists(configFilePath))
            {
                FilePath = configFilePath;
                Document = XDocument.Load(configFilePath);
            }
            else throw new UserException(string.Format("{0} is not found", configFilePath));
        }
        public void SetElementValue(string xPath, string value)
        {
            var element = Document.XPathSelectElement(xPath);
            if (element != null)
                element.Value = value;
        }

        public void SetAttributeValueOfInnerElement(string xPath, string filterAttributeName, string filterAttributeValue, string attributeName, string value)
        {
            var element = Document.XPathSelectElement(xPath);
            if (element != null)
            {
                if (element.HasElements)
                {
                    var elems = element.Elements().ToList();
                    var elem = elems.Where(p => p.Attribute(filterAttributeName) != null && p.Attribute(filterAttributeName).Value == filterAttributeValue).SingleOrDefault();
                    if (elem != null)
                    {
                        var att = elem.Attribute(attributeName);
                        if (att != null)
                            att.Value = value;
                    }
                }
            }
        }

        public void SetAttributeValue(string xPath, string attributeName, string value)
        {
            var element = Document.XPathSelectElement(xPath);
            if (element != null)
            {
                var att = element.Attribute(attributeName);
                if (att != null) att.Value = value;
            }
        }

        public void Save()
        {
            Document.Save(FilePath, SaveOptions.None);
        }
    }
}
