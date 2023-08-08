using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpManagementGroup
    {
        // Fields
        internal const int DefaultMessageTimeout = 0xafc8;
        public const string LlrpMessageTimeout = "LLRP Message timeout";
        internal static readonly PropertyKey LlrpMessageTimeoutKey = new PropertyKey("LLRP Management", "LLRP Message timeout");
        internal static readonly DevicePropertyMetadata LlrpMessageTimeoutMetadata = new DevicePropertyMetadata(typeof(int), LlrpResources.LlrpMessageTimeoutDescription, SensorPropertyRelation.Device, DefaultMessageTimeout, true, false, false, false, 10000.0, 2147483647.0);
        internal const int MaximumMessageTimeout = 0x7fffffff;
        internal const int MinimumMessageTimeout = 0x2710;
        public const string SynchronousCommandInventoryDuration = "Synchronous command Inventory duration";
        internal static readonly PropertyKey SynchronousCommandInventoryDurationKey = new PropertyKey("LLRP Management", "Synchronous command Inventory duration");
        internal static readonly DevicePropertyMetadata SynchronousCommandInventoryDurationMetadata = new DevicePropertyMetadata(typeof(int), LlrpResources.SynchronousCommandInventoryDurationDescription, SensorPropertyRelation.Device, 0x1388, true, false, false, false, 1.0, 300000.0);
        public const string SynchronousCommandInventoryOperationCount = "Synchronous command Inventory operation count";
        internal static readonly PropertyKey SynchronousCommandInventoryOperationCountKey = new PropertyKey("LLRP Management", "Synchronous command Inventory operation count");
        internal static readonly DevicePropertyMetadata SynchronousCommandInventoryOperationCountMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.SynchronousCommandOperationCountDescription, SensorPropertyRelation.Device, (ushort)1, true, false, false, false, 0.0, 65535.0);
    }
}
