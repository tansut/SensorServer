namespace Kalitte.Sensors.Commands
{
    using System;

    [Serializable]
    public sealed class SetPropertyResponse : Response
    {
        public override string ToString()
        {
            return "<setPropertyResponse/>";
        }
    }
}
