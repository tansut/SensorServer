namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class AddPrintTemplateResponse : Response
    {
        public override string ToString()
        {
            return "<addPrintTemplateResponse/>";
        }
    }
}
