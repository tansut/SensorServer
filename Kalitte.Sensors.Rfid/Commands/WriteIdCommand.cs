using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Utilities;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.UI;

namespace Kalitte.Sensors.Rfid.Commands
{
    [Serializable]
    [SensorCommandEditor("Rfid/WriteIdCommandEditor.ascx")]
    public sealed class WriteIdCommand : TagCommand
    {
        // Fields
        private readonly byte[] m_tagId;
        private readonly byte[] newAccessCode;
        private readonly byte[] newKillCode;
        private WriteIdResponse response;

        // Methods
        public WriteIdCommand(byte[] passCode, byte[] tagId, byte[] newAccessCode, byte[] newKillCode)
            : base(passCode)
        {
            this.m_tagId = tagId;
            this.newAccessCode = newAccessCode;
            this.newKillCode = newKillCode;
        }

        public byte[] GetNewAccessCode()
        {
            return CollectionsHelper.CloneByte(this.newAccessCode);
        }

        public byte[] GetNewKillCode()
        {
            return CollectionsHelper.CloneByte(this.newKillCode);
        }

        public byte[] GetTagId()
        {
            return CollectionsHelper.CloneByte(this.m_tagId);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<writeId>");
            builder.Append(base.ToString());
            builder.Append("<tagId>");
            if (this.m_tagId != null)
            {
                //builder.Append(Convert.ToBase64String(this.m_tagId));
                builder.Append(HexHelper.HexEncode(this.m_tagId));
            }
            builder.Append("</tagId>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</writeId>");
            return builder.ToString();
        }

        // Properties
        public WriteIdResponse Response
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
