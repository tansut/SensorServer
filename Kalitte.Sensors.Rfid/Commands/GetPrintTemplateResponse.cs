namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Rfid.Utilities;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Utilities;

    [Serializable]
    public sealed class GetPrintTemplateResponse : Response
    {
        private readonly byte[] m_template;

        public GetPrintTemplateResponse(byte[] template)
        {
            this.m_template = template;
            this.ValidateParameters();
        }

        public byte[] GetTemplate()
        {
            return CollectionsHelper.CloneByte(this.m_template);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getPrintTemplateResponse>");
            builder.Append("</getPrintTemplateResponse>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.m_template == null) || (this.m_template.Length == 0))
            {
                throw new ArgumentNullException("template");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }
    }
}
