namespace Kalitte.Sensors.Commands
{
    using System;
    using System.Text;
    using Kalitte.Sensors.Configuration;


    [Serializable]
    public sealed class GetPropertyCommand : SensorCommand
    {
        private readonly PropertyKey propertyKey;
        private GetPropertyResponse response;

        public GetPropertyCommand(PropertyKey propertyKey)
        {
            this.propertyKey = propertyKey;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getProperty>");
            builder.Append(base.ToString());
            builder.Append("<propertyKey>");
            builder.Append(this.propertyKey);
            builder.Append("</propertyKey>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</getProperty>");
            return builder.ToString();
        }

        public PropertyKey PropertyKey
        {
            get
            {
                return this.propertyKey;
            }
        }

        public GetPropertyResponse Response
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
