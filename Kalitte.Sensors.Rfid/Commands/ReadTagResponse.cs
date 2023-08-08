namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Text;
    using Kalitte.Sensors.Rfid.Utilities;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Utilities;

    [Serializable]
    public sealed class ReadTagResponse : Response
    {
        private readonly byte[] tagData;

        public ReadTagResponse(byte[] tagData)
        {
            this.tagData = tagData;
        }

        public byte[] GetReadData()
        {
            return CollectionsHelper.CloneByte(this.tagData);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<readTagResponse>");
            if (this.tagData != null)
            {
                builder.Append("<tagData>");
                //builder.Append(Convert.ToBase64String(this.tagData));
                builder.Append(HexHelper.HexEncode(this.tagData));
                builder.Append("</tagData>");
            }
            builder.Append("</readTagResponse>");
            return builder.ToString();
        }
    }
}
