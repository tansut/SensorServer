using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Security;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Events;



namespace Kalitte.Sensors.SensorDevices
{
    public abstract class SensorProxy
    {
        // Events
        public abstract event EventHandler<ResponseEventArgs> CmdResponseEvent;

        public abstract event EventHandler<NotificationEventArgs> DeviceNotificationEvent;

        // Methods
        protected SensorProxy()
        {
        }

        //public abstract FirmwareComparisonInformation CheckFirmwareCompatibility(string firmwareLocation);
        public abstract void Close();
        //public abstract Collection<DeviceCapability> GetDeviceCapabilities();
        public abstract Collection<string> GetPropertyGroupNames();
        public abstract Dictionary<PropertyKey, DevicePropertyMetadata> GetPropertyMetadata(string propertyGroupName);
        public abstract Dictionary<string, PropertyList> GetSources();
        public abstract bool IsConnectionAlive();
        public abstract void Reboot();
        public abstract void SendCommand(SensorCommand command);
        public abstract void SendCommand(string sourceName, SensorCommand command);
        public abstract ResponseEventArgs ExecuteCommand(SensorCommand command);
        public abstract ResponseEventArgs ExecuteCommand(string sourceName, SensorCommand command);
        public abstract void SetupConnection(AuthenticationInformation authenticationInfo);

        // Properties
        public abstract SensorDeviceInformation DeviceInformation { get; }


    }

 

}
