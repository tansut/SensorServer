namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class GetAllPrintTemplateNamesResponse : Response
    {
        private Collection<string> templateNames;

        public GetAllPrintTemplateNamesResponse(Collection<string> templateNames)
        {
            this.templateNames = templateNames;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getAllPrintTemplateNamesResponse>");
            if (this.templateNames != null)
            {
                foreach (string str in this.templateNames)
                {
                    builder.Append("<templateName>");
                    builder.Append(str);
                    builder.Append("</templateName>");
                }
            }
            builder.Append("</getAllPrintTemplateNamesResponse>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.templateNames != null)
            {
                for (int i = 0; i < this.templateNames.Count; i++)
                {
                    if (string.IsNullOrEmpty(this.templateNames[i]))
                    {
                        throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture, "templateNames[{0}]", new object[] { i }));
                    }
                }
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public ReadOnlyCollection<string> TemplateNames
        {
            get
            {
                if (this.templateNames != null)
                {
                    return new ReadOnlyCollection<string>(this.templateNames);
                }
                return null;
            }
        }
    }
}
