namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Text;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class GetAllPrintTemplateNamesCommand : SensorCommand
    {
        private GetAllPrintTemplateNamesResponse response;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getAllPrintTemplateNames>");
            builder.Append(base.ToString());
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</getAllPrintTemplateNames>");
            return builder.ToString();
        }

        public GetAllPrintTemplateNamesResponse Response
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
