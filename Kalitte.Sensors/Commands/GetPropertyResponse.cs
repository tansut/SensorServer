namespace Kalitte.Sensors.Commands
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Configuration;

    [Serializable]
    public sealed class GetPropertyResponse : Response
    {
        private readonly EntityProperty property;

        public GetPropertyResponse(EntityProperty entityProperty)
        {
            this.property = entityProperty;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getPropertyResponse>");
            builder.Append("<property>");
            builder.Append(this.property);
            builder.Append("</property>");
            builder.Append("</getPropertyResponse>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.property == null)
            {
                throw new ArgumentNullException("entityProperty");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public EntityProperty Property
        {
            get
            {
                return this.property;
            }
        }
    }
}
