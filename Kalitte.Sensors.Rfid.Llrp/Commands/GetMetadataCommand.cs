using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Commands;

namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
internal sealed class GetMetadataCommand : SensorCommand
{
    // Fields
    private string m_groupName;
    private GetMetadataResponse m_response;

    // Methods
    internal GetMetadataCommand(string groupName)
    {
        this.m_groupName = groupName;
    }

    // Properties
    internal string GroupName
    {
        get
        {
            return this.m_groupName;
        }
    }

    internal GetMetadataResponse Response
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
