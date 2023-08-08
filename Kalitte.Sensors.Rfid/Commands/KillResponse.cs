namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class KillResponse : Response
    {
        public override string ToString()
        {
            return "<killResponse/>";
        }
    }
}
