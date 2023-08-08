namespace Kalitte.Sensors.Commands
{
    using System;
    using System.Text;


    [Serializable]
    public sealed class GetActivePropertyListCommand : SensorCommand
    {
        private GetActivePropertyListResponse response;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getCurrentPropertyProfileCommand>");
            builder.Append(base.ToString());
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</getCurrentPropertyProfileCommand>");
            return builder.ToString();
        }

        public GetActivePropertyListResponse Response
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
