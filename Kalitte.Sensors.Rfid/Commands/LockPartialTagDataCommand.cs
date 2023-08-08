namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Rfid.Utilities;
    using Kalitte.Sensors.Utilities;

    [Serializable]
    public sealed class LockPartialTagDataCommand : TagCommand
    {
        private readonly int length;
        private readonly byte[] m_tagId;
        private readonly int offset;
        private readonly bool permanent;
        private LockPartialTagDataResponse response;
        private readonly System.IO.SeekOrigin seekOrigin;

        public LockPartialTagDataCommand(byte[] passCode, byte[] tagId, System.IO.SeekOrigin seekOrigin, int offset, int length) : base(passCode)
        {
            this.m_tagId = tagId;
            this.seekOrigin = seekOrigin;
            this.offset = offset;
            this.length = length;
            this.ValidateParameters();
        }

        public LockPartialTagDataCommand(byte[] passCode, byte[] tagId, System.IO.SeekOrigin seekOrigin, int offset, int length, bool permanent) : this(passCode, tagId, seekOrigin, offset, length)
        {
            this.permanent = permanent;
        }

        public byte[] GetTagId()
        {
            return CollectionsHelper.CloneByte(this.m_tagId);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<lockPartialTagData>");
            builder.Append(base.ToString());
            builder.Append("<tagId>");
            if (this.m_tagId != null)
            {
                //builder.Append(Convert.ToBase64String(this.m_tagId));
                builder.Append(HexHelper.HexEncode(this.m_tagId));
            }
            builder.Append("</tagId>");
            builder.Append("<seekOrigin>");
            builder.Append(this.seekOrigin);
            builder.Append("</seekOrigin>");
            builder.Append("<offset>");
            builder.Append(this.offset);
            builder.Append("</offset>");
            builder.Append("<length>");
            builder.Append(this.length);
            builder.Append("</length>");
            builder.Append("<permanent>");
            builder.Append(this.permanent);
            builder.Append("</permanent>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</lockPartialTagData>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.m_tagId == null) || (this.m_tagId.Length == 0))
            {
                throw new ArgumentNullException("tagId");
            }
            if (0 >= this.length)
            {
                throw new ArgumentException("InvalidLength");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public bool IsPermanent
        {
            get
            {
                return this.permanent;
            }
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

        public LockPartialTagDataResponse Response
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
    }
}
