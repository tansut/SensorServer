namespace Kalitte.Sensors.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Kalitte.Sensors.Configuration;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;

    [Serializable]
    public sealed class ApplyPropertyListFailedError : CommandError
    {
        private Dictionary<PropertyKey, CommandError> detailedErrors;
        private readonly PropertyList inputProfile;

        public ApplyPropertyListFailedError(PropertyList inputProfile, Dictionary<PropertyKey, CommandError> detailedErrors) : base(ErrorCode.ApplyPropertyListFailed, ErrorCode.ApplyPropertyListFailed.Description, "ApplyPropertyProfileFailed", new object[0])
        {
            this.detailedErrors = new Dictionary<PropertyKey, CommandError>();
            if (inputProfile == null)
            {
                throw new ArgumentNullException("inputProfile");
            }
            this.inputProfile = inputProfile;
            this.detailedErrors = detailedErrors;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<applyPropertyProfileFailedError>");
            builder.Append("<inputProfile>");
            builder.Append(this.inputProfile);
            builder.Append("</inputProfile>");
            builder.Append("<properties>");
            if (this.detailedErrors != null)
            {
                foreach (KeyValuePair<PropertyKey, CommandError> pair in this.detailedErrors)
                {
                    builder.Append("<error>");
                    builder.Append("<key>");
                    builder.Append(pair.Key);
                    builder.Append("</key>");
                    builder.Append("<value>");
                    builder.Append(pair.Value);
                    builder.Append("</value>");
                    builder.Append("</error>");
                }
            }
            builder.Append("</properties>");
            builder.Append("</applyPropertyProfileFailedError>");
            return builder.ToString();
        }

        public Dictionary<PropertyKey, CommandError> DetailedErrors
        {
            get
            {
                return this.detailedErrors;
            }
        }

        public PropertyList InputProfile
        {
            get
            {
                return this.inputProfile;
            }
        }
    }
}
