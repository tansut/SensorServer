namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class GetDiagnosticsCommand : SensorCommand
    {
        private GetDiagnosticsResponse response;

        public override string ToString()
        {
            return "<getDiagnostics/>";
        }

        public GetDiagnosticsResponse Response
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
