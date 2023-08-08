using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Rfid.Utilities;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.UI;

namespace Kalitte.Sensors.Rfid.Commands
{

    [Serializable]
    [SensorCommandEditor("Rfid/WriteTagDataCommandEditor.ascx")]
    public sealed class WriteTagDataCommand : TagCommand
    {
        // Fields
        private readonly byte[] m_tagData;
        private readonly byte[] m_tagId;
        private WriteTagDataResponse response;

        // Methods
        public WriteTagDataCommand(byte[] passCode, byte[] tagId, byte[] tagData)
            : base(passCode)
        {
            this.m_tagId = tagId;
            this.m_tagData = tagData;
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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<writeTagData>");
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
                //builder.Append(Convert.ToBase64String(this.m_tagId));
                builder.Append(HexHelper.HexEncode(this.m_tagId));
            }
            builder.Append("</tagData>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</writeTagData>");
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
        public WriteTagDataResponse Response
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
