using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Rfid.Core;

namespace Kalitte.Sensors.Rfid.Configuration
{
public sealed class SourcePropertyGroup
{
    // Fields
    public const string ContinuousRead = "ContinuousRead";
    public static readonly DevicePropertyMetadata ContinuousReadMetadata = new DevicePropertyMetadata(typeof(bool), "ContinuousRead" , SensorPropertyRelation.Source, false, true, false, false, false);
    public const string Enabled = "Enabled";
    public static readonly DevicePropertyMetadata EnabledMetadata = new DevicePropertyMetadata(typeof(bool), "Enabled"   , SensorPropertyRelation.Source, true, true, false, true, false);
    public const string PortInputValue = "Port Input Value";
    public static readonly DevicePropertyMetadata PortInputValueMetadata = new DevicePropertyMetadata(typeof(byte[]), "PortInputValue"   , SensorPropertyRelation.Source, null, false, false, true, false);
    public const string PortOutputValue = "Port Output Value";
    public static readonly DevicePropertyMetadata PortOutputValueMetadata = new DevicePropertyMetadata(typeof(byte[]), "PortOutputValue", SensorPropertyRelation.Source, null, true, false, true, false);
    public const string SourceType = "Source Type";
    public static readonly DevicePropertyMetadata SourceTypeMetadata = new DevicePropertyMetadata(typeof(RfidSourceType), "SourceType", SensorPropertyRelation.Source, null, false, false, true, false, GetSourceTypeValueSet());
    public const string SystemEnabled = "System Enabled";
    public static readonly DevicePropertyMetadata SystemEnabledMetadata = new DevicePropertyMetadata(typeof(bool), "SystemEnabled", SensorPropertyRelation.Source, true, false, false, true, false);

    // Methods
    private SourcePropertyGroup()
    {
    }

    private static Collection<object> GetSourceTypeValueSet()
    {
        Collection<object> collection = new Collection<object>();
        collection.Add(Kalitte.Sensors.Rfid.Core.RfidSourceType.Antenna);
        collection.Add(Kalitte.Sensors.Rfid.Core.RfidSourceType.IOPort);
        return collection;
    }
}


 

}
