namespace Kalitte.Sensors.Rfid.Commands
{
   using Kalitte.Sensors.Rfid;
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Rfid.Utilities;
    using Kalitte.Sensors.Commands;

    [Serializable, MayChangeState]
    public sealed class AddPrintTemplateCommand : SensorCommand
    {
        private readonly byte[] m_template;
        private AddPrintTemplateResponse response;

        public AddPrintTemplateCommand(byte[] template)
        {
            this.m_template = template;
            this.ValidateParameters();
        }

        public byte[] GetTemplate()
        {
            return Kalitte.Sensors.Utilities.CollectionsHelper.CloneByte(this.m_template);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<addPrintTemplate>");
            builder.Append(base.ToString());
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</addPrintTemplate>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.m_template == null)
            {
                throw new ArgumentNullException("template");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public AddPrintTemplateResponse Response
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
