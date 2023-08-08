using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.UI
{
    public sealed class SensorCommandEditorAttribute: WebEditorAttribute
    {
        public SensorCommandEditorAttribute(string userControlPath)
            : base(userControlPath)
        {
        }
    }
}
