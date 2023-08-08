using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable]
    public class NameDescription
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public NameDescription(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
