using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Rfid.Utilities;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Rfid.Events
{
[Serializable]
public sealed class IOPortValueChangedEvent : RfidObservation
{
    // Fields
    private readonly string portName;
    private readonly byte[] portValue;

    // Methods
    public IOPortValueChangedEvent(string portName, byte[] portValue) : base(null)
    {
        this.portName = portName;
        this.portValue = portValue;
        this.ValidateParameters();
    }

    public byte[] GetPortValue()
    {
        if (this.portValue == null)
        {
            return null;
        }
        return CollectionsHelper.CloneByte(this.portValue);
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<iOPortValueChangedEvent>");
        builder.Append(base.ToString());
        builder.Append("<portName>");
        builder.Append(this.portName);
        builder.Append("</portName>");
        builder.Append("<portValue>");
        builder.Append(this.portValue);
        builder.Append("</portValue>");
        builder.Append("</iOPortValueChangedEvent>");
        return builder.ToString();
    }

    private void ValidateParameters()
    {
        if ((this.portName == null) || (this.portName.Length == 0))
        {
            throw new ArgumentNullException("portName");
        }
        if (this.portValue == null)
        {
            throw new ArgumentNullException("portValue");
        }
    }

    [OnDeserialized]
    private void ValidateParameters(StreamingContext context)
    {
        this.ValidateParameters();
    }

    // Properties
    public string PortName
    {
        get
        {
            return this.portName;
        }
    }
}

 
 

}
