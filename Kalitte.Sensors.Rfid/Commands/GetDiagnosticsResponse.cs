namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Configuration;

    [Serializable]
    public sealed class GetDiagnosticsResponse : Response
    {
        private readonly PropertyList diagnosticsProperties;

        public GetDiagnosticsResponse(PropertyList diagnosticsProperties)
        {
            this.diagnosticsProperties = diagnosticsProperties;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getDiagnosticsResponse>");
            builder.Append("<diagnosticsProperties>");
            builder.Append(this.diagnosticsProperties);
            builder.Append("</diagnosticsProperties>");
            builder.Append("</getDiagnosticsResponse>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.diagnosticsProperties == null)
            {
                throw new ArgumentNullException("diagnosticsProperties");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public PropertyList DiagnosticsProperties
        {
            get
            {
                return this.diagnosticsProperties;
            }
        }
    }
}
