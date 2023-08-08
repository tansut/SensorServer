namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class GetPrintTemplatePreviewCommand : SensorCommand
    {
        private GetPrintTemplatePreviewResponse response;
        private readonly bool retrieveThumbnailOnly;
        private readonly string templateName;

        public GetPrintTemplatePreviewCommand(string templateName, bool thumbnailOnly)
        {
            this.templateName = templateName;
            this.retrieveThumbnailOnly = thumbnailOnly;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getPrintTemplatePreview>");
            builder.Append(base.ToString());
            builder.Append("<templateName>");
            builder.Append(this.templateName);
            builder.Append("</templateName>");
            builder.Append("<retrieveThumbnailOnly>");
            builder.Append(this.retrieveThumbnailOnly);
            builder.Append("</retrieveThumbnailOnly>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</getPrintTemplatePreview>");
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

        public GetPrintTemplatePreviewResponse Response
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

        public bool RetrieveThumbnailOnly
        {
            get
            {
                return this.retrieveThumbnailOnly;
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
