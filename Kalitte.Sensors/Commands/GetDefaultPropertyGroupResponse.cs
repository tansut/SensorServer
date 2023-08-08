namespace Kalitte.Sensors.Commands
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Configuration;

    [Serializable]
    public sealed class GetDefaultPropertyGroupResponse : Response
    {
        private readonly PropertyList defaultProfile;

        public GetDefaultPropertyGroupResponse(PropertyList defaultProfile)
        {
            this.defaultProfile = defaultProfile;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getDefaultPropertyProfileResponse>");
            builder.Append("<defaultProfile>");
            builder.Append(this.defaultProfile);
            builder.Append("</defaultProfile>");
            builder.Append("</getDefaultPropertyProfileResponse>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.defaultProfile == null)
            {
                throw new ArgumentNullException("defaultProfile");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public PropertyList DefaultProfile
        {
            get
            {
                return this.defaultProfile;
            }
        }
    }
}
