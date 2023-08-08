namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Text;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class GetCurrentPrintTemplateNameResponse : Response
    {
        private readonly string templateName;

        public GetCurrentPrintTemplateNameResponse(string templateName)
        {
            this.templateName = templateName;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getCurrentPrintTemplateNameResponse>");
            builder.Append("<templateName>");
            builder.Append(this.templateName);
            builder.Append("</templateName>");
            builder.Append("</getCurrentPrintTemplateNameResponse>");
            return builder.ToString();
        }

        public string TemplateName
        {
            get
            {
                return this.templateName;
            }
        }
    }
}
