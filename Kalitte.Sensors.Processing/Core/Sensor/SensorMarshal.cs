using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Processing.Core.Sensor
{
    public class SensorMarshal : MarshalBase
    {
        private SensorDevices.SensorProxy sensor;
        private ProviderMarshal providerMarshal;

        public event EventHandler<ResponseEventArgs> CmdResponseEvent;
        public event EventHandler<NotificationEventArgs> DeviceNotificationEvent;


        public SensorMarshal(SensorDevices.SensorProxy sensor, ProviderMarshal providerMarshal)
        {
            this.sensor = sensor;
            this.providerMarshal = providerMarshal;
            sensor.CmdResponseEvent += new EventHandler<Commands.ResponseEventArgs>(device_CmdResponseEvent);
            sensor.DeviceNotificationEvent += new EventHandler<Events.NotificationEventArgs>(device_DeviceNotificationEvent);
        }

        void device_DeviceNotificationEvent(object sender, Events.NotificationEventArgs e)
        {
            if (this.DeviceNotificationEvent != null)
            {
                this.DeviceNotificationEvent(this, e);
            }
        }

        void device_CmdResponseEvent(object sender, Commands.ResponseEventArgs e)
        {
            if (this.CmdResponseEvent != null)
            {
                this.CmdResponseEvent(this, e);
            }
        }

        internal void SetupConnection(Security.AuthenticationInformation authenticationInfo)
        {
            sensor.SetupConnection(authenticationInfo);
        }

        public SensorDevices.SensorDeviceInformation DeviceInformation
        {
            get
            {
                return sensor.DeviceInformation;
            }
        }

        internal Commands.ResponseEventArgs ExecuteCommand(SensorCommand command)
        {
            return sensor.ExecuteCommand(command);
        }

        internal Commands.ResponseEventArgs ExecuteCommand(string sourceName, SensorCommand command)
        {
            return sensor.ExecuteCommand(sourceName, command);
        }

        internal void Close()
        {
            sensor.Close();
        }

        internal void SendCommand(string sourceName, SensorCommand command)
        {
            sensor.SendCommand(sourceName, command);
        }

        internal void SendCommand(SensorCommand command)
        {
            sensor.SendCommand(command);
        }

        internal void Reboot()
        {
            sensor.Reboot();
        }

        internal bool IsConnectionAlive()
        {
            return sensor.IsConnectionAlive();
        }

        internal Dictionary<string, Sensors.Configuration.PropertyList> GetSources()
        {
            return sensor.GetSources();
        }

        internal Dictionary<Sensors.Configuration.PropertyKey, Sensors.Configuration.DevicePropertyMetadata> GetPropertyMetadata(string propertyGroupName)
        {
            return sensor.GetPropertyMetadata(propertyGroupName);
        }

        internal System.Collections.ObjectModel.Collection<string> GetPropertyGroupNames()
        {
            return sensor.GetPropertyGroupNames();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                IDisposable sensor = this.sensor as IDisposable;
                if (sensor != null)
                {
                    try
                    {
                        sensor.Dispose();
                    }
                    catch (System.Exception)
                    {
                        throw;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

 

    }
}
