namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Text;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class GetCurrentPrintTemplateNameCommand : SensorCommand
    {
        private GetCurrentPrintTemplateNameResponse response;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getCurrentTemplateName>");
            builder.Append(base.ToString());
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</getCurrentTemplateName>");
            return builder.ToString();
        }

        public GetCurrentPrintTemplateNameResponse Response
        {
            get
            {
                return this.response;
            }
            set
            {
                this.response = value;
            }
        }
    }
}
