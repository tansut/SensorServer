namespace Kalitte.Sensors.Commands {
    using System;
    using System.Runtime.Serialization;
    using System.Text;

    using Kalitte.Sensors.Configuration;

    [Serializable]
    public sealed class GetActivePropertyListResponse : Response
    {
        private readonly PropertyList currentProfile;

        public GetActivePropertyListResponse(PropertyList currentProfile)
        {
            this.currentProfile = currentProfile;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getCurrentPropertyProfileResponse>");
            builder.Append("<currentProfile>");
            builder.Append(this.currentProfile);
            builder.Append("</currentProfile>");
            builder.Append("</getCurrentPropertyProfileResponse>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.currentProfile == null)
            {
                throw new ArgumentNullException("currentProfile");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public PropertyList CurrentProfile
        {
            get
            {
                return this.currentProfile;
            }
        }
    }
}
