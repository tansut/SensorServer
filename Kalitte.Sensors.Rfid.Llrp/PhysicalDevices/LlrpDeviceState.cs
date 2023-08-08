using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Llrp.Commands;

namespace Kalitte.Sensors.Rfid.Llrp.PhysicalDevices
{
internal class LlrpDeviceState
{
    // Fields
    private LastTagReportInformation m_lastTagInformation = new LastTagReportInformation();

    // Properties
    internal LastTagReportInformation LastTagInformation
    {
        get
        {
            return this.m_lastTagInformation;
        }
    }
}

 

 

}
