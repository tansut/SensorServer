namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Text;

    [Serializable]
    public sealed class GetPrintTemplatePreviewResponse : GetPreviewImageResponse
    {
        public GetPrintTemplatePreviewResponse(byte[] previewImage) : base(previewImage)
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getPrintTemplatePreviewResponse>");
            builder.Append(base.ToString());
            builder.Append("</getPrintTemplatePreviewResponse>");
            return builder.ToString();
        }
    }
}
