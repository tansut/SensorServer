namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Text;

    [Serializable]
    public sealed class GetPrintLabelPreviewResponse : GetPreviewImageResponse
    {
        public GetPrintLabelPreviewResponse(byte[] previewImage) : base(previewImage)
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getPrintLabelPreviewResponse>");
            builder.Append(base.ToString());
            builder.Append("</getPrintLabelPreviewResponse>");
            return builder.ToString();
        }
    }
}
