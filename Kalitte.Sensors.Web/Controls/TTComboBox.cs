using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;


namespace Kalitte.Sensors.Web.Controls
{
    public class TTComboBox : ComboBox
    {
        public TTComboBox()
            : base()
        {
            Editable = true;
            ForceSelection = true;
            TriggerAction = TriggerAction.All;
            Mode = DataLoadMode.Local;
            SelectOnFocus = true;
            TypeAhead = true;
        }

        public int SelectedAsInt
        {
            get
            {
                return Convert.ToInt32(SelectedItem.Value);
            }
            set
            {
                SetValueAndFireSelect(value);
            }
        }

        public long SelectedAsLong
        {
            get
            {
                return Convert.ToInt64(SelectedItem.Value);
            }
            set
            {
                SetValueAndFireSelect(value);
            }
        }

        public string SelectedAsString
        {
            get
            {

                if (SelectedItem.Value == null)
                    return string.Empty;
                else return SelectedItem.Value.ToString();
            }
            set
            {
                if (value == null)
                    SetValueAndFireSelect(string.Empty);
                else
                    SetValueAndFireSelect(value);
            }
        }


        public long? SelectedAsNullableLong
        {
            get
            {
                if (SelectedItem.Value == null || string.IsNullOrEmpty(SelectedItem.Value.ToString()))
                    return null;
                else return SelectedAsLong;
            }
            set
            {
                if (!value.HasValue)
                {
                    Clear();
                }
                else SelectedAsLong = value.Value;
            }
        }

        public int? SelectedAsNullableInt
        {
            get
            {
                if (SelectedItem.Value == null || string.IsNullOrEmpty(SelectedItem.Value.ToString()))
                    return null;
                else return SelectedAsInt;
            }
            set
            {
                if (!value.HasValue)
                {
                    Clear();
                }
                else SelectedAsInt = value.Value;
            }
        }

    }
}
