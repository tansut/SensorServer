using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kalitte.Sensors.Utilities
{
    public class TypeParser
    {
        public string AssemblyName { get; private set; }
        public string AssemblyNameWithExtension { get; private set; }
        public string Type { get; set; }

        public TypeParser(string type)
        {
            Validate(type);
            string [] partsOfType = type.Split(',');
            AssemblyName = partsOfType[1].Trim();
            Type = partsOfType[0].Trim();
            if (AssemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
                AssemblyNameWithExtension = AssemblyName;
            else AssemblyNameWithExtension = string.Format("{0}.dll", AssemblyName);
            
        }



        public static void Validate(string type)
        {
            string[] partsOfType = type.Split(',');
            if (partsOfType.Length < 2)
                throw new ArgumentException("Invalid type information format.", type);
        }
    }
}
