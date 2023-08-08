namespace Kalitte.Sensors.Rfid.Commands
{
   using Kalitte.Sensors.Rfid;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Configuration;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Rfid.Core;

    [Serializable]
    public sealed class GetStandardizedPrintTemplateResponse : Response
    {
        private readonly List<PrintTemplateField> templateFields;

        public GetStandardizedPrintTemplateResponse(Collection<PrintTemplateField> fields)
        {
            if (fields != null)
            {
                this.templateFields = new List<PrintTemplateField>(fields);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getStandardizedPrintTemplateResponse>");
            if (this.templateFields != null)
            {
                builder.Append("<template>");
                foreach (PrintTemplateField field in this.templateFields)
                {
                    builder.Append(field.ToString());
                }
                builder.Append("</template>");
            }
            builder.Append("</getStandardizedPrintTemplateResponse>");
            return builder.ToString();
        }

        public ReadOnlyCollection<PrintTemplateField> StandardizedTemplate
        {
            get
            {
                if (this.templateFields != null)
                {
                    return new ReadOnlyCollection<PrintTemplateField>(this.templateFields);
                }
                return null;
            }
        }
    }
}
