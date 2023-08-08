namespace Kalitte.Sensors.Commands
{
    using System;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class ApplyPropertyListResponse : Response
    {
        public override string ToString()
        {
            return "<applyPropertyProfileResponse/>";
        }
    }
}
