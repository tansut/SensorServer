namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Text;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class GetAllPrintTemplatesCommand : SensorCommand
    {
        private GetAllPrintTemplatesResponse response;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getAllPrintTemplates>");
            builder.Append(base.ToString());
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</getAllPrintTemplates>");
            return builder.ToString();
        }

        public GetAllPrintTemplatesResponse Response
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
