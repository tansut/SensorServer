namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Rfid.Core;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class GetTagMetadataResponse : Response
    {
        private readonly TagMetadata tagMetadata;

        public GetTagMetadataResponse(TagMetadata tagMetadata)
        {
            this.tagMetadata = tagMetadata;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getTagMetaDataResponse>");
            builder.Append("<tagMetadata>");
            builder.Append(this.tagMetadata);
            builder.Append("</tagMetadata>");
            builder.Append("</getTagMetaDataResponse>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.tagMetadata == null)
            {
                throw new ArgumentNullException("tagMetadata");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public TagMetadata TagMetadata
        {
            get
            {
                return this.tagMetadata;
            }
        }
    }
}
