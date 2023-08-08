using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Kalitte.Sensors.Rfid.Utilities;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.UI;

namespace Kalitte.Sensors.Rfid.Commands
{
    [Serializable]
    [SensorCommandEditor("Rfid/WriteTagCommandEditor.ascx")]
    public sealed class WriteTagCommand : TagCommand
    {
        // Fields
        private readonly byte[] m_tagData;
        private readonly byte[] m_tagId;
        private readonly int offset;
        private WriteTagResponse response;
        private readonly SeekOrigin seekOrigin;
        private readonly int memoryBank;


        // Methods
        public WriteTagCommand(byte[] passCode, byte[] tagId, int memoryBank, byte[] tagData, SeekOrigin seekOrigin, int offset)
            : base(passCode)
        {
            this.m_tagId = tagId;
            this.m_tagData = tagData;
            this.seekOrigin = seekOrigin;
            this.offset = offset;
            this.memoryBank = memoryBank;
            this.ValidateParameters();
        }

        public byte[] GetTagData()
        {
            return CollectionsHelper.CloneByte(this.m_tagData);
        }

        public byte[] GetTagId()
        {
            return CollectionsHelper.CloneByte(this.m_tagId);
        }

        public int MemoryBank
        {
            get
            {
                return this.memoryBank;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<writeTagCommand>");
            builder.Append(base.ToString());
            builder.Append("<tagId>");
            if (this.m_tagId != null)
            {
                //builder.Append(Convert.ToBase64String(this.m_tagId));
                builder.Append(HexHelper.HexEncode(this.m_tagId));
            }
            builder.Append("</tagId>");
            builder.Append("<tagData>");
            if (this.m_tagData != null)
            {
                //builder.Append(Convert.ToBase64String(this.m_tagData));
                builder.Append(HexHelper.HexEncode(this.m_tagData));
            }
            builder.Append("</tagData>");
            builder.Append("<memoryBank>");
            builder.Append(this.memoryBank);
            builder.Append("</memoryBank>");
            builder.Append("<seekOrigin>");
            builder.Append(this.seekOrigin);
            builder.Append("</seekOrigin>");
            builder.Append("<offset>");
            builder.Append(this.offset);
            builder.Append("</offset>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</writeTagCommand>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.m_tagId == null) || (this.m_tagId.Length == 0))
            {
                throw new ArgumentNullException("tagId");
            }
            if ((this.m_tagData == null) || (this.m_tagData.Length == 0))
            {
                throw new ArgumentNullException("tagData");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public int Offset
        {
            get
            {
                return this.offset;
            }
        }

        public WriteTagResponse Response
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


        public SeekOrigin SeekOrigin
        {
            get
            {
                return this.seekOrigin;
            }
        }
    }





}
