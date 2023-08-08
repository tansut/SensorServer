using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTEnumCombo: TTComboBox
    {
        public TTEnumCombo()
            : base()
        {
            DisplayField = "Value";
            ValueField = "Key";
            Editable = false;
            AllowBlank = false;
            
        }

        public T GetSelectedAsType<T>() where T: struct
        {
            Type t = typeof(T);
            T result;
            if (Enum.TryParse<T>(SelectedAsString, out result))
                return result;
            else throw new IndexOutOfRangeException("Invalid Enum Value:" + SelectedAsString);
        }

    }
}
