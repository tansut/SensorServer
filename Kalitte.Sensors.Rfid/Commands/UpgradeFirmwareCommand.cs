namespace Kalitte.Sensors.Rfid.Commands
{
    using Kalitte.Sensors.Rfid;
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.UI;

    [Serializable, DedicatedCommand, MayChangeState]
    [SensorCommandEditor("Rfid/UpgradeFirmwareCommandEditor.ascx")]    
    public sealed class UpgradeFirmwareCommand : SensorCommand
    {
        private string m_firmwareLocation;
        private UpgradeFirmwareResponse m_response;

        public UpgradeFirmwareCommand(string firmwareLocation)
        {
            this.m_firmwareLocation = firmwareLocation;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<upgradeFirmware>");
            builder.Append(base.ToString());
            builder.Append("<firmwareLocation>");
            builder.Append(this.m_firmwareLocation);
            builder.Append("</firmwareLocation>");
            builder.Append("<response>");
            builder.Append(this.m_response);
            builder.Append("</response>");
            builder.Append("</upgradeFirmware>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.m_firmwareLocation == null) || (this.m_firmwareLocation.Length == 0))
            {
                throw new ArgumentNullException("firmwareLocation");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public string FirmwareUpgradePath
        {
            get
            {
                return this.m_firmwareLocation;
            }
        }

        public UpgradeFirmwareResponse Response
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
