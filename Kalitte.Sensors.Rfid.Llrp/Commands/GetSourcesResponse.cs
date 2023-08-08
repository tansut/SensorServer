using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Commands;

namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
internal sealed class GetSourcesResponse : Response
{
    // Fields
    private Dictionary<string, PropertyList> m_sources;

    // Methods
    internal GetSourcesResponse(Dictionary<string, PropertyList> sources)
    {
        if (sources == null)
        {
            sources = new Dictionary<string, PropertyList>();
        }
        this.m_sources = sources;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<GetSourcesResponse>");
        foreach (string str in this.Sources.Keys)
        {
            builder.Append("<Source>");
            builder.Append(str);
            builder.Append("</Source>");
            builder.Append("<Profile>");
            if (this.Sources[str] != null)
            {
                builder.Append(this.Sources[str].ToString());
            }
            builder.Append("</Profile>");
        }
        builder.Append("</GetSourcesResponse>");
        return builder.ToString();
    }

    // Properties
    internal Dictionary<string, PropertyList> Sources
    {
        get
        {
            return this.m_sources;
        }
    }
}

 

}
