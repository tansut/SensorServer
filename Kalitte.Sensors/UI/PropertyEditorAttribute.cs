using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.UI
{
    public sealed class PropertyEditorAttribute : WebEditorAttribute
    {
        public PropertyEditorAttribute(string userControlPath)
            : base(userControlPath)
        {
        }
    }
}
