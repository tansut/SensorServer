namespace Kalitte.Sensors.Rfid.Core
{
    using System;
    using System.Text;

    [Serializable]
    public sealed class FilterDataSelector : TagDataSelector
    {
        public FilterDataSelector()
        {
            this.IsSource = false;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<filterDataSelector>");
            builder.Append("<isId>");
            builder.Append(base.IsId);
            builder.Append("</isId>");
            builder.Append("<isData>");
            builder.Append(base.IsData);
            builder.Append("</isData>");
            builder.Append("<isType>");
            builder.Append(base.IsType);
            builder.Append("</isType>");
            builder.Append("<isSource>");
            builder.Append(this.IsSource);
            builder.Append("</isSource>");
            builder.Append("<isTime>");
            builder.Append(base.IsTime);
            builder.Append("</isTime>");
            builder.Append("<isNumberingSystemIdentifier>");
            builder.Append(base.IsNumberingSystemIdentifier);
            builder.Append("</isNumberingSystemIdentifier>");
            builder.Append("</filterDataSelector>");
            return builder.ToString();
        }

        public bool IsSource
        {
            get
            {
                return base.IsSource;
            }
            set
            {
                base.IsSource = value;
            }
        }
    }
}
