namespace Kalitte.Sensors.Commands
{
    using System;
    using System.Text;


    [Serializable]
    public sealed class GetDefaultPropertyListCommand : SensorCommand
    {
        private GetDefaultPropertyGroupResponse response;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getDefaultPropertyProfileCommand>");
            builder.Append(base.ToString());
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</getDefaultPropertyProfileCommand>");
            return builder.ToString();
        }

        public GetDefaultPropertyGroupResponse Response
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
