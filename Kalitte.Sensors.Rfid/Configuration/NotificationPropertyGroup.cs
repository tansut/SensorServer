using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Rfid.Core;


namespace Kalitte.Sensors.Rfid.Configuration
{
public static class NotificationPropertyGroup
{
    // Fields
    public const string EventMode = "Event Mode";
    public static readonly DevicePropertyMetadata EventModeMetadata = new DevicePropertyMetadata(typeof(bool), "EventMode", SensorPropertyRelation.DeviceAndSource, true, true, false, true, false);
    public const string NotificationHost = "Notification Host";
    public static readonly DevicePropertyMetadata NotificationHostMetadata = new DevicePropertyMetadata(typeof(string), "NotificationHost", SensorPropertyRelation.Device, null, true, false, true, false);
    public const string NotificationPort = "Notification Port";
    public static readonly DevicePropertyMetadata NotificationPortMetadata = new DevicePropertyMetadata(typeof(int), "NotificationPort", SensorPropertyRelation.Device, null, true, false, true, false, 1.0, 2147483647.0);
    public const string OnTriggerPull = "On Trigger Pull";
    public static readonly DevicePropertyMetadata OnTriggerPullMetadata = new DevicePropertyMetadata(typeof(string), "OnTriggerPull", SensorPropertyRelation.Device, "ScanRfid", true, false, false, false, GetOnTriggerPullValueSet());
    public const string TagDataSelector = "Tag Data Selector";
    public static readonly DevicePropertyMetadata TagDataSelectorMetadata = new DevicePropertyMetadata(typeof(TagDataSelector), 
        "TagDataSelector", SensorPropertyRelation.DeviceAndSource, Kalitte.Sensors.Rfid.Core.TagDataSelector.All, true, false, true, false);

    // Methods
    private static Collection<object> GetOnTriggerPullValueSet()
    {
        Collection<object> collection = new Collection<object>();
        collection.Add("ScanRfid");
        collection.Add("ScanBarcode");
        collection.Add("ScanBoth");
        collection.Add("ScanNone");
        return collection;
    }
}


 

}
