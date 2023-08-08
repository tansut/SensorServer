namespace Kalitte.Sensors.Rfid.Commands
{
   using Kalitte.Sensors.Rfid;
    using System;
    using System.Text;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.UI;

    [Serializable, MayChangeState, DedicatedCommand]
    [SensorCommandEditor("Rfid/ResetConfigurationToFactorySettingsCommandEditor.ascx")]    
    public sealed class ResetConfigurationToFactorySettingsCommand : SensorCommand
    {
        private ResetConfigurationToFactorySettingsResponse m_response;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<resetConfigurationToFactorySettings>");
            builder.Append(base.ToString());
            builder.Append("<response>");
            builder.Append(this.m_response);
            builder.Append("</response>");
            builder.Append("</resetConfigurationToFactorySettings>");
            return builder.ToString();
        }

        public ResetConfigurationToFactorySettingsResponse Response
        {
            get
            {
                return this.m_response;
            }
            set
            {
                this.m_response = value;
            }
        }
    }
}
