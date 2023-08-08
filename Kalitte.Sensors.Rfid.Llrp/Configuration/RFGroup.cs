using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Helpers;
using Kalitte.Sensors.Rfid.Llrp.Core;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Rfid.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    internal sealed class RFGroup
    {
        // Fields
        internal static readonly PropertyKey AirProtocolsSupportedKey = new PropertyKey("RF", "Air Protocols Supported");
        internal static readonly DevicePropertyMetadata AirProtocolsSupportedMetadata = new DevicePropertyMetadata(RFPropertyGroup.AirProtocolsSupportedMetadata.Type, RFPropertyGroup.AirProtocolsSupportedMetadata.Description, SensorPropertyRelation.Source, null, RFPropertyGroup.AirProtocolsSupportedMetadata.IsWritable, RFPropertyGroup.AirProtocolsSupportedMetadata.IsMandatory, RFPropertyGroup.AirProtocolsSupportedMetadata.IsPersistent, RFPropertyGroup.AirProtocolsSupportedMetadata.RequiresRestart, Util.GetNames(typeof(AirProtocolId)));
    }

 

 

}
