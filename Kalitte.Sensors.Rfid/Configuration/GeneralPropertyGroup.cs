using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Configuration
{
    public sealed class GeneralPropertyGroup
    {
        // Fields
        public const string Description = "Description";
        public static readonly DevicePropertyMetadata DescriptionMetadata = new DevicePropertyMetadata(typeof(string), "Description", SensorPropertyRelation.DeviceAndSource, null, true, false, true, false);

        public const string FirmwareVersion = "Firmware version";
        public static readonly DevicePropertyMetadata FirmwareVersionMetadata = new DevicePropertyMetadata(typeof(string), "FirmwareVersion", SensorPropertyRelation.Device, null, false, false, true, false);
        public const string Location = "Location";
        public static readonly DevicePropertyMetadata LocationMetadata = new DevicePropertyMetadata(typeof(string), "Location", SensorPropertyRelation.DeviceAndSource, null, true, false, true, false);
        public const string Name = "Name";
        public static readonly DevicePropertyMetadata NameMetadata = new DevicePropertyMetadata(typeof(string), "Name", SensorPropertyRelation.DeviceAndSource, null, true, false, true, false);
        public const string NtpServer = "NTP server";
        public static readonly DevicePropertyMetadata NtpServerMetadata = new DevicePropertyMetadata(typeof(string), "NtpServer", SensorPropertyRelation.Device, null, true, false, true, false);
        public const string RegulatoryRegion = "Regulatory region";
        public static readonly DevicePropertyMetadata RegulatoryRegionMetadata = new DevicePropertyMetadata(typeof(string), "RegulatoryRegion", SensorPropertyRelation.Device, null, true, false, true, false);
        public const string Time = "Time";
        public static readonly DevicePropertyMetadata TimeMetadata = new DevicePropertyMetadata(typeof(string), "Time", SensorPropertyRelation.Device, null, true, false, true, false);
        public const string Vendor = "Vendor";
        public static readonly DevicePropertyMetadata VendorMetadata = new DevicePropertyMetadata(typeof(string), "Vendor", SensorPropertyRelation.Device, null, false, false, true, false);
    }





}
