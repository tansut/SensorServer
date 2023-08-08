using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Management.Instrumentation;

namespace Kalitte.Sensors.Events.Management
{
[Serializable]
public sealed class FreeMemoryLowEvent : DeviceManagementEvent
{
    // Fields
    [IgnoreMember]
    private readonly int freeMemoryPercentage;

    // Methods
    public FreeMemoryLowEvent(EventLevel eventLevel, string description, int freeMemoryPercentage) : base(eventLevel, EventType.FreeMemoryLow, description)
    {
        this.freeMemoryPercentage = freeMemoryPercentage;
        this.ValidateParameters();
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<freeMemoryLowEvent>");
        builder.Append(base.ToString());
        builder.Append("<freeMemoryPercentage>");
        builder.Append(this.freeMemoryPercentage);
        builder.Append("</freeMemoryPercentage>");
        builder.Append("</freeMemoryLowEvent>");
        return builder.ToString();
    }

    private void ValidateParameters()
    {
        if ((0 > this.freeMemoryPercentage) || (100 < this.freeMemoryPercentage))
        {
            throw new ArgumentException("InvalidMemoryPercent");
        }
    }

    [OnDeserialized]
    private void ValidateParameters(StreamingContext context)
    {
        this.ValidateParameters();
    }

    // Properties
    public int FreeMemoryPercentage
    {
        get
        {
            return this.freeMemoryPercentage;
        }
    }
}

 
 

}
