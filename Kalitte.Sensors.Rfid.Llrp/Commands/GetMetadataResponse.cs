using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Commands;

namespace Kalitte.Sensors.Rfid.Llrp
{
internal sealed class GetMetadataResponse : Response
{
    // Fields
    private Dictionary<PropertyKey, DevicePropertyMetadata> m_metadata;

    // Methods
    internal GetMetadataResponse(Dictionary<PropertyKey, DevicePropertyMetadata> metadata)
    {
        this.m_metadata = metadata;
    }

    // Properties
    internal Dictionary<PropertyKey, DevicePropertyMetadata> Metadata
    {
        get
        {
            return this.m_metadata;
        }
    }
}


 

}
