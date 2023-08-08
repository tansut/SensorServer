namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Rfid.Utilities;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.UI;

    [Serializable]
    [SensorCommandEditor("Rfid/ReadTagCommandEditor.ascx")]
    public sealed class ReadTagCommand : TagCommand
    {
        private readonly int length;
        private readonly byte[] m_tagId;
        private readonly int offset;
        private ReadTagResponse response;
        private readonly System.IO.SeekOrigin seekOrigin;
        private readonly int memoryBank;

        public ReadTagCommand(byte[] passCode, byte[] tagId, int memoryBank, SeekOrigin seekOrigin, int offset, int length)
            : base(passCode)
        {
            this.m_tagId = tagId;
            this.seekOrigin = seekOrigin;
            this.memoryBank = memoryBank;
            this.offset = offset;
            this.length = length;
            this.ValidateParameters();
        }

        public byte[] GetTagId()
        {
            return CollectionsHelper.CloneByte(this.m_tagId);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<readTagCommand>");
            builder.Append(base.ToString());
            builder.Append("<tagId>");
            if (this.m_tagId != null)
            {
                //builder.Append(Convert.ToBase64String(this.m_tagId));
                builder.Append(HexHelper.HexEncode(this.m_tagId));

            }
            builder.Append("</tagId>");
            builder.Append("<memoryBank>");
            builder.Append(this.memoryBank);
            builder.Append("</memoryBank>");
            builder.Append("<seekOrigin>");
            builder.Append(this.seekOrigin);
            builder.Append("</seekOrigin>");
            builder.Append("<offset>");
            builder.Append(this.offset);
            builder.Append("</offset>");
            builder.Append("<length>");
            builder.Append(this.length);
            builder.Append("</length>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</readTagCommand>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.m_tagId == null) || (this.m_tagId.Length == 0))
            {
                throw new ArgumentNullException("tagId");
            }
            if (this.length < 0)
            {
                throw new ArgumentException("InvalidLength");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public int Length
        {
            get
            {
                return this.length;
            }
        }

        public int Offset
        {
            get
            {
                return this.offset;
            }
        }

        public ReadTagResponse Response
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

        public System.IO.SeekOrigin SeekOrigin
        {
            get
            {
                return this.seekOrigin;
            }
        }

        public int MemoryBank
        {
            get
            {
                return this.memoryBank;
            }
        }
    }
}
