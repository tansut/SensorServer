namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class LockPartialTagDataResponse : Response
    {
        public override string ToString()
        {
            return "<lockPartialTagDataResponse/>";
        }
    }
}
