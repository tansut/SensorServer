using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.UI
{
    public interface ICustomPropertyEditor
    {
        void Edit(string serializedValue);
        string EndEdit();
    }
}
