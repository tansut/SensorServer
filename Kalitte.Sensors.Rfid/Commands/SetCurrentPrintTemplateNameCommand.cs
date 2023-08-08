namespace Kalitte.Sensors.Rfid.Commands
{
   using Kalitte.Sensors.Rfid;
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Commands;

    [Serializable, MayChangeState]
    public sealed class SetCurrentPrintTemplateNameCommand : SensorCommand
    {
        private SetCurrentPrintTemplateNameResponse response;
        private readonly string templateName;

        public SetCurrentPrintTemplateNameCommand(string templateName)
        {
            this.templateName = templateName;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<setCurrentPrintTemplateName>");
            builder.Append(base.ToString());
            builder.Append("<templateName>");
            builder.Append(this.templateName);
            builder.Append("</templateName>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</setCurrentPrintTemplateName>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.templateName == null) || (this.templateName.Length == 0))
            {
                throw new ArgumentNullException("templateName");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public SetCurrentPrintTemplateNameResponse Response
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

        public string TemplateName
        {
            get
            {
                return this.templateName;
            }
        }
    }
}
