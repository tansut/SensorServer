namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Rfid.Core;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class GetPrintLabelPreviewCommand : SensorCommand
    {
        private readonly PrintLabel printLabel;
        private GetPrintLabelPreviewResponse response;
        private readonly bool retrieveThumbnailOnly;

        public GetPrintLabelPreviewCommand(PrintLabel printLabel, bool thumbnailOnly)
        {
            this.printLabel = printLabel;
            this.retrieveThumbnailOnly = thumbnailOnly;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getPrintLabelPreview>");
            builder.Append(base.ToString());
            builder.Append("<printLabel>");
            builder.Append(this.printLabel);
            builder.Append("</printLabel>");
            builder.Append("<retrieveThumbnailOnly>");
            builder.Append(this.retrieveThumbnailOnly);
            builder.Append("</retrieveThumbnailOnly>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</getPrintLabelPreview>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.printLabel == null)
            {
                throw new ArgumentNullException("printLabel");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public PrintLabel Label
        {
            get
            {
                return this.printLabel;
            }
        }

        public GetPrintLabelPreviewResponse Response
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
    }
}
