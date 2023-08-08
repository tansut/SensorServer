namespace Kalitte.Sensors.Commands
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Configuration;
    using Kalitte.Sensors.Commands;

    [Serializable, MayChangeState]
    public sealed class ApplyPropertyListCommand : SensorCommand
    {
        private readonly PropertyList propertyProfile;
        private ApplyPropertyListResponse response;

        public ApplyPropertyListCommand(PropertyList propertyProfile)
        {
            this.propertyProfile = propertyProfile;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<applyPropertyList>");
            builder.Append(base.ToString());
            builder.Append("<propertyList>");
            builder.Append(this.propertyProfile);
            builder.Append("</propertyList>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</applyPropertyList>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.propertyProfile == null)
            {
                throw new ArgumentNullException("propertyProfile");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public PropertyList PropertyProfile
        {
            get
            {
                return this.propertyProfile;
            }
        }

        public ApplyPropertyListResponse Response
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
