namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class ResetConfigurationToFactorySettingsResponse : Response
    {
        public override string ToString()
        {
            return "<resetConfigurationToFactorySettings/>";
        }
    }
}
