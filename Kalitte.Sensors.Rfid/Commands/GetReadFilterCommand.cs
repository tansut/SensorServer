namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Text;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class GetReadFilterCommand : SensorCommand
    {
        private GetReadFilterResponse response;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getReadFilter>");
            builder.Append(base.ToString());
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</getReadFilter>");
            return builder.ToString();
        }

        public GetReadFilterResponse Response
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
