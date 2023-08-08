using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Rfid.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    internal sealed class TagReadGroup
    {
        // Fields
        internal static readonly PropertyKey DuplicateEliminationTimeKey = new PropertyKey("Tag Read", "Duplicate Elimination Time");
        internal static readonly DevicePropertyMetadata DuplicateEliminationTimeMetadata = new DevicePropertyMetadata(TagReadPropertyGroup.DuplicationEliminationTimeMetadata.Type, TagReadPropertyGroup.DuplicationEliminationTimeMetadata.Description, TagReadPropertyGroup.DuplicationEliminationTimeMetadata.PropertyTargets, 0x1388L, true, false, false, false, TagReadPropertyGroup.DuplicationEliminationTimeMetadata.LowerRange, TagReadPropertyGroup.DuplicationEliminationTimeMetadata.HigherRange);
    }

 

 

}
