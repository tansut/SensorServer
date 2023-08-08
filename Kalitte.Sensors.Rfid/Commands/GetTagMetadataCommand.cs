namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Rfid.Utilities;
    using Kalitte.Sensors.Utilities;

    [Serializable]
    public sealed class GetTagMetadataCommand : TagCommand
    {
        private readonly byte[] m_tagId;
        private GetTagMetadataResponse response;

        public GetTagMetadataCommand(byte[] passCode, byte[] tagId) : base(passCode)
        {
            this.m_tagId = tagId;
            this.ValidateParameters();
        }

        public byte[] GetTagId()
        {
            return CollectionsHelper.CloneByte(this.m_tagId);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getTagMetaData>");
            builder.Append(base.ToString());
            builder.Append("<tagId>");
            builder.Append(this.m_tagId);
            builder.Append("</tagId>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</getTagMetaData>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.m_tagId == null) || (this.m_tagId.Length == 0))
            {
                throw new ArgumentNullException("tagId");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public GetTagMetadataResponse Response
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
