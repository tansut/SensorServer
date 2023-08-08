using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpReaderEventNotificationSpecGroup
    {
        // Fields
        public const string AISpecEndEvent = "AI Spec End Event";
        internal static readonly PropertyKey AISpecEndEventKey = new PropertyKey("LLRP Reader Event Notification Specification", "AI Spec End Event");
        internal static readonly DevicePropertyMetadata AISpecEndEventMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.AISpecEndEventDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string AISpecEndWithSingulationEvent = "AI Spec End with Singulation Event";
        internal static readonly PropertyKey AISpecEndWithSingulationEventKey = new PropertyKey("LLRP Reader Event Notification Specification", "AI Spec End with Singulation Event");
        internal static readonly DevicePropertyMetadata AISpecEndWithSingulationEventMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.AISpecEndWithSingulationEventDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string AntennaEvent = "Antenna Event";
        internal static readonly PropertyKey AntennaEventKey = new PropertyKey("LLRP Reader Event Notification Specification", "Antenna Event");
        internal static readonly DevicePropertyMetadata AntennaEventMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.AntennaEventDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string BufferFillWarningEvent = "Buffer Fill Warning Event";
        internal static readonly PropertyKey BufferFillWarningEventKey = new PropertyKey("LLRP Reader Event Notification Specification", "Buffer Fill Warning Event");
        internal static readonly DevicePropertyMetadata BufferFillWarningEventMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.BufferFillWarningEventDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string GpiEvent = "Gpi Event";
        internal static readonly PropertyKey GpiEventKey = new PropertyKey("LLRP Reader Event Notification Specification", "Gpi Event");
        internal static readonly DevicePropertyMetadata GpiEventMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.GpiEventDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string HoppingEvent = "Hopping Event";
        internal static readonly PropertyKey HoppingEventKey = new PropertyKey("LLRP Reader Event Notification Specification", "Hopping Event");
        internal static readonly DevicePropertyMetadata HoppingEventMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.HoppingEventDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string ReaderExceptionEvent = "Reader Exception Event";
        internal static readonly PropertyKey ReaderExceptionEventKey = new PropertyKey("LLRP Reader Event Notification Specification", "Reader Exception Event");
        internal static readonly DevicePropertyMetadata ReaderExceptionEventMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.ReaderExceptionEventDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string RFSurveyEvent = "RF Survey event";
        internal static readonly PropertyKey RFSurveyEventKey = new PropertyKey("LLRP Reader Event Notification Specification", "RF Survey event");
        internal static readonly DevicePropertyMetadata RFSurveyEventMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.RFSurveyEventDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string ROSpecEvent = "RO Spec Event";
        internal static readonly PropertyKey ROSpecEventKey = new PropertyKey("LLRP Reader Event Notification Specification", "RO Spec Event");
        internal static readonly DevicePropertyMetadata ROSpecEventMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.ROSpecEventDescription, SensorPropertyRelation.Device, false, true, false, false, false);
    }

 

 

}
