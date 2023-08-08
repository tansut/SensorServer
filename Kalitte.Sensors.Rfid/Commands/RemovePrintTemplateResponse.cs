namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class RemovePrintTemplateResponse : Response
    {
        public override string ToString()
        {
            return "<removePrintTemplateResponse/>";
        }
    }
}
