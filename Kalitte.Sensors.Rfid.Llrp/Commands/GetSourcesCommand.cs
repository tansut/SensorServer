using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Commands;

namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
internal sealed class GetSourcesCommand : SensorCommand
{
    // Fields
    private GetSourcesResponse m_response;

    // Methods
    internal GetSourcesCommand()
    {
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<GetSourcesCommand>");
        if (this.Response != null)
        {
            builder.Append(this.Response.ToString());
        }
        builder.Append("</GetSourcesCommand>");
        return builder.ToString();
    }

    // Properties
    internal GetSourcesResponse Response
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
