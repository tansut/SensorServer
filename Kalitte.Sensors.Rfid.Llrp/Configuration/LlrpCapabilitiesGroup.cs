using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpCapabilitiesGroup
    {
        // Fields
        public const string CanDoRFSurvey = "Can Do RF Survey";
        internal static readonly PropertyKey CanDoRFSurveyKey = new PropertyKey("LLRP Capabilities", "Can Do RF Survey");
        internal static readonly DevicePropertyMetadata CanDoRFSurveyMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.CanDoRFSurveyDescription, SensorPropertyRelation.Device, true, false, false, true, false);
        public const string CanDOTagInventoryStateAwareSingulation = "Can Do Tag Inventory State Aware Singulation";
        internal static readonly PropertyKey CanDOTagInventoryStateAwareSingulationKey = new PropertyKey("LLRP Capabilities", "Can Do Tag Inventory State Aware Singulation");
        internal static readonly DevicePropertyMetadata CanDOTagInventoryStateAwareSingulationMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.CanDOTagInventoryStateAwareSingulationDescription, SensorPropertyRelation.Device, true, false, false, true, false);
        public const string CanReportBufferFillWarning = "Can Report Buffer Fill Warning";
        internal static readonly PropertyKey CanReportBufferFillWarningKey = new PropertyKey("LLRP Capabilities", "Can Report Buffer Fill Warning");
        internal static readonly DevicePropertyMetadata CanReportBufferFillWarningMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.CanReportBufferFillWarningDescription, SensorPropertyRelation.Device, true, false, false, true, false);
        public const string ClientRequestOperationSpecTimeout = "Client Request Operation Spec Timeout";
        internal static readonly PropertyKey ClientRequestOperationSpecTimeoutKey = new PropertyKey("LLRP Capabilities", "Client Request Operation Spec Timeout");
        internal static readonly DevicePropertyMetadata ClientRequestOperationSpecTimeoutMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.ClientRequestOperationSpecTimeoutDescription, SensorPropertyRelation.Device, (ushort)0, false, false, true, false);
        public const string MaximumAccessSpec = "Maximum Access Spec";
        internal static readonly PropertyKey MaximumAccessSpecKey = new PropertyKey("LLRP Capabilities", "Maximum Access Spec");
        internal static readonly DevicePropertyMetadata MaximumAccessSpecMetadata = new DevicePropertyMetadata(typeof(uint), LlrpResources.MaximumAccessSpecDescription, SensorPropertyRelation.Device, (uint)0, false, false, true, false);
        public const string MaximumInventoryPerAI = "Maximum Inventory per AI";
        internal static readonly PropertyKey MaximumInventoryPerAIKey = new PropertyKey("LLRP Capabilities", "Maximum Inventory per AI");
        internal static readonly DevicePropertyMetadata MaximumInventoryPerAIMetadata = new DevicePropertyMetadata(typeof(uint), LlrpResources.MaximumInventoryPerAIDescription, SensorPropertyRelation.Device, (uint)0, false, false, true, false);
        public const string MaximumOperationSpecPerAccessSpec = "Maximum Operation Spec per Access Spec";
        internal static readonly PropertyKey MaximumOperationSpecPerAccessSpecKey = new PropertyKey("LLRP Capabilities", "Maximum Operation Spec per Access Spec");
        internal static readonly DevicePropertyMetadata MaximumOperationSpecPerAccessSpecMetadata = new DevicePropertyMetadata(typeof(uint), LlrpResources.MaximumOperationSpecPerAccessSpecDescription, SensorPropertyRelation.Device, (uint)0, false, false, true, false);
        public const string MaximumPriorityLevelSupported = "Maximum Priority Level supported";
        internal static readonly PropertyKey MaximumPriorityLevelSupportedKey = new PropertyKey("LLRP Capabilities", "Maximum Priority Level supported");
        internal static readonly DevicePropertyMetadata MaximumPriorityLevelSupportedMetadata = new DevicePropertyMetadata(typeof(byte), LlrpResources.MaximumPriorityLevelSupportedDescription, SensorPropertyRelation.Device, (byte)7, false, false, true, false, 0.0, 7.0);
        public const string MaximumROSpec = "Maximum RO Spec";
        internal static readonly PropertyKey MaximumROSpecKey = new PropertyKey("LLRP Capabilities", "Maximum RO Spec");
        internal static readonly DevicePropertyMetadata MaximumROSpecMetadata = new DevicePropertyMetadata(typeof(uint), LlrpResources.MaximumROSpecDescription, SensorPropertyRelation.Device, (uint)0, false, false, true, false);
        public const string MaximumSpecsPerROSpec = "Maximum Specs per RO Spec";
        internal static readonly PropertyKey MaximumSpecsPerROSpecKey = new PropertyKey("LLRP Capabilities", "Maximum Specs per RO Spec");
        internal static readonly DevicePropertyMetadata MaximumSpecsPerROSpecMetadata = new DevicePropertyMetadata(typeof(uint), LlrpResources.MaximumSpecsPerROSpecDescription, SensorPropertyRelation.Device, (uint)0, false, false, true, false);
        public const string SupportsClientOperationSpec = "Supports Client Operation Spec";
        internal static readonly PropertyKey SupportsClientOperationSpecKey = new PropertyKey("LLRP Capabilities", "Supports Client Operation Spec");
        internal static readonly DevicePropertyMetadata SupportsClientOperationSpecMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.SupportsClientOperationSpecDescription, SensorPropertyRelation.Device, true, false, false, true, false);
        public const string SupportsEventAndReportHolding = "Supports Event And Report Holding";
        internal static readonly PropertyKey SupportsEventAndReportHoldingKey = new PropertyKey("LLRP Capabilities", "Supports Event And Report Holding");
        internal static readonly DevicePropertyMetadata SupportsEventAndReportHoldingMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.SupportsEventAndReportHoldingDescription, SensorPropertyRelation.Device, true, false, false, true, false);
    }

 

 

}
