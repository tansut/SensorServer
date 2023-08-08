using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Configuration
{
public sealed class RFPropertyGroup
{
    // Fields
    public const string AirProtocolsInUse = "Air Protocols In Use";
    public static readonly DevicePropertyMetadata AirProtocolsInUseMetadata = new DevicePropertyMetadata(typeof(string[]), "AirProtocolsInUse", SensorPropertyRelation.DeviceAndSource, null, true, false, true, false, GetAirProtocolsValueSet());
    public const string AirProtocolsSupported = "Air Protocols Supported";
    public static readonly DevicePropertyMetadata AirProtocolsSupportedMetadata = new DevicePropertyMetadata(typeof(string[]), "AirProtocolsSupported", SensorPropertyRelation.DeviceAndSource, null, false, false, true, false, GetAirProtocolsValueSet());
    public const string AntennaSequence = "Antenna sequence";
    public static readonly DevicePropertyMetadata AntennaSequenceMetadata = new DevicePropertyMetadata(typeof(string[]), "AntennaSequence", SensorPropertyRelation.Device, null, true, false, true, false);
    public const string DutyCycle = "Duty cycle";
    public static readonly DevicePropertyMetadata DutyCycleMetadata = new DevicePropertyMetadata(typeof(float), "PowerLevel", SensorPropertyRelation.DeviceAndSource, null, true, false, true, false, 0.0, 100.0);
    public const string EffectiveRange = "Effective Range";
    public static readonly DevicePropertyMetadata EffectiveRangeMetadata = new DevicePropertyMetadata(typeof(double), "EffectiveRange", SensorPropertyRelation.Source, null, false, false, true, false, 0.0, 5.0);
    public const string Frequency = "Frequency";
    public static readonly DevicePropertyMetadata FrequencyMetadata = new DevicePropertyMetadata(typeof(double), "Frequency", SensorPropertyRelation.Source, null, false, false, true, false, 13.56, 960.0);
    public const string NoiseLevel = "Noise Level";
    public static readonly DevicePropertyMetadata NoiseLevelMetadata = new DevicePropertyMetadata(typeof(float), "NoiseLevel", SensorPropertyRelation.Source, null, false, false, true, false, 0.0, 100.0);
    public const string NoiseLevelThreshold = "Noise Level Threshold";
    public static readonly DevicePropertyMetadata NoiseLevelThresholdMetadata = new DevicePropertyMetadata(typeof(float), "NoiseLevelThreshold", SensorPropertyRelation.Source, null, true, false, true, false, 0.0, 100.0);
    public const string OperationEnvironment = "Operation Environment";
    public static readonly DevicePropertyMetadata OperationEnvironmentMetadata = new DevicePropertyMetadata(typeof(string), "OperationEnvironment", SensorPropertyRelation.DeviceAndSource, null, true, false, true, false, GetOperationEnvironmentValueSet());
    public const string PowerLevel = "Power Level";
    public static readonly DevicePropertyMetadata PowerLevelMetadata = new DevicePropertyMetadata(typeof(float), "PowerLevel", SensorPropertyRelation.Source, null, true, false, true, false, 0.0, 100.0);
    public const string RFModeContinuous = "RF Mode continuous";
    public static readonly DevicePropertyMetadata RFModeContinuousMetadata = new DevicePropertyMetadata(typeof(bool), "RFModeContinuous", SensorPropertyRelation.Device, true, true, false, true, false);
    public const string SessionId = "Session id";
    public static readonly DevicePropertyMetadata SessionIdMetadata = new DevicePropertyMetadata(typeof(string), "SessionID", SensorPropertyRelation.DeviceAndSource, null, true, false, true, false);

    // Methods
    private RFPropertyGroup()
    {
    }

    private static Collection<object> GetAirProtocolsValueSet()
    {
        Collection<object> collection = new Collection<object>();
        collection.Add("EpcClass0");
        collection.Add("EpcClass1Gen1");
        collection.Add("EpcClass1Gen2");
        collection.Add("IsoA");
        collection.Add("IsoB");
        collection.Add("Iso14443");
        collection.Add("Iso15693");
        collection.Add("Barcode");
        return collection;
    }

    private static Collection<object> GetOperationEnvironmentValueSet()
    {
        Collection<object> collection = new Collection<object>();
        collection.Add("single");
        collection.Add("multiple");
        collection.Add("dense");
        return collection;
    }
}

 

 

}
