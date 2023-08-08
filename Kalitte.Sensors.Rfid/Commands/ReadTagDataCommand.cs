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
    [SensorCommandEditor("Rfid/ReadTagDataCommandEditor.ascx")]
    public sealed class ReadTagDataCommand : TagCommand
    {
        // Fields
        private readonly byte[] m_tagId;
        private ReadTagDataResponse response;

        // Methods
        public ReadTagDataCommand(byte[] passCode, byte[] tagId)
            : base(passCode)
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
            builder.Append("<readTagData>");
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
            builder.Append("</readTagData>");
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

        // Properties
        public ReadTagDataResponse Response
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
