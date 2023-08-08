using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable]
    public class NameDescriptionList : List<NameDescription>
    {
        public NameDescriptionList(IDictionary<string, string> source)
        {
            foreach (var item in source)
            {
                Add(new NameDescription(item.Key, item.Value));
            }
        }

        public NameDescriptionList(IEnumerable<string> source)
        {
            foreach (var item in source)
            {
                Add(new NameDescription(item, item));
            }
        }

        public NameDescriptionList()
        {
        }
    }
}
