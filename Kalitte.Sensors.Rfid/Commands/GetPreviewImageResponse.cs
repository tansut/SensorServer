namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Text;
    using Kalitte.Sensors.Commands;

    [Serializable, StrongNameIdentityPermission(SecurityAction.InheritanceDemand)]
    public abstract class GetPreviewImageResponse : Response
    {
        private readonly byte[] previewImage;

        protected GetPreviewImageResponse(byte[] previewImage)
        {
            this.previewImage = previewImage;
            this.ValidateParameters();
        }

        public byte[] GetPreviewImageByRef()
        {
            return this.previewImage;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getPreviewImageResponse>");
            if (this.previewImage != null)
            {
                builder.Append(this.previewImage.GetLength(0));
            }
            builder.Append("</getPreviewImageResponse>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.previewImage == null) || (this.previewImage.Length == 0))
            {
                throw new ArgumentNullException("previewImage");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }
    }
}
