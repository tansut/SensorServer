using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Processing.Core.Sensor
{
    public class VirtualSensor : SensorProxy
    {
        private Communication.ConnectionInformation connectionInformation;
        private SensorMarshal sensorMarshal;


        private EventHandler<ResponseEventArgs> cmdResponseEvent;
        private EventHandler<NotificationEventArgs> deviceNotificationEvent;


        public VirtualSensor(Communication.ConnectionInformation connectionInformation, SensorMarshal sensorMarshaller)
        {
            this.connectionInformation = connectionInformation;
            this.sensorMarshal = sensorMarshaller;
            ProviderEventMarshal marshaller = new ProviderEventMarshal(new EventHandler<ResponseEventArgs>(this.proxyCmdResponseEvent), new EventHandler<NotificationEventArgs>(this.proxyDeviceNotificationEvent));
            sensorMarshaller.CmdResponseEvent += new EventHandler<ResponseEventArgs>(marshaller.proxyCmdResponseEvent);
            sensorMarshaller.DeviceNotificationEvent += new EventHandler<NotificationEventArgs>(marshaller.proxyDeviceNotificationEvent);
        }

        public override void Close()
        {
            sensorMarshal.Close();
        }

        public override event EventHandler<ResponseEventArgs> CmdResponseEvent
        {
            add
            {
                this.cmdResponseEvent = (EventHandler<ResponseEventArgs>)Delegate.Combine(this.cmdResponseEvent, value);
            }
            remove
            {
                this.cmdResponseEvent = (EventHandler<ResponseEventArgs>)Delegate.Remove(this.cmdResponseEvent, value);
            }
        }

        public override event EventHandler<NotificationEventArgs> DeviceNotificationEvent
        {
            add
            {
                this.deviceNotificationEvent = (EventHandler<NotificationEventArgs>)Delegate.Combine(this.deviceNotificationEvent, value);
            }
            remove
            {
                this.deviceNotificationEvent = (EventHandler<NotificationEventArgs>)Delegate.Remove(this.deviceNotificationEvent, value);
            }
        }

        internal void proxyCmdResponseEvent(object sender, ResponseEventArgs e)
        {
            var handler = this.cmdResponseEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        internal void proxyDeviceNotificationEvent(object sender, NotificationEventArgs e)
        {
            var handler = this.deviceNotificationEvent;
            if (handler != null)
            {
                //NotificationEventContext currentContext = this.m_deviceMarshaller.GetCurrentContext();
                //NotificationEventContext.Current = currentContext;
                try
                {
                    handler(this, e);
                }
                finally
                {
                    //if (currentContext != null)
                    //{
                    //    this.m_deviceMarshaller.SetCurrentContext(currentContext);
                    //    NotificationEventContext.Current = null;
                    //}
                }
            }
        }

 

 


        public override System.Collections.ObjectModel.Collection<string> GetPropertyGroupNames()
        {
            return sensorMarshal.GetPropertyGroupNames();
        }

        public override Dictionary<Sensors.Configuration.PropertyKey, Sensors.Configuration.DevicePropertyMetadata> GetPropertyMetadata(string propertyGroupName)
        {
            return sensorMarshal.GetPropertyMetadata(propertyGroupName);
        }

        public override Dictionary<string, Sensors.Configuration.PropertyList> GetSources()
        {
            return sensorMarshal.GetSources();
        }

        public override bool IsConnectionAlive()
        {
            return sensorMarshal.IsConnectionAlive();
        }

        public override void Reboot()
        {
            sensorMarshal.Reboot();
        }

        public override void SendCommand(Commands.SensorCommand command)
        {
            sensorMarshal.SendCommand(command);
        }

        public override void SendCommand(string sourceName, Commands.SensorCommand command)
        {
            sensorMarshal.SendCommand(sourceName, command);
        }

        public override Commands.ResponseEventArgs ExecuteCommand(Commands.SensorCommand command)
        {
            return sensorMarshal.ExecuteCommand(command);
            
        }

        public override Commands.ResponseEventArgs ExecuteCommand(string sourceName, Commands.SensorCommand command)
        {
            return sensorMarshal.ExecuteCommand(sourceName, command);
        }

        public override void SetupConnection(Security.AuthenticationInformation authenticationInfo)
        {
            
            sensorMarshal.SetupConnection(authenticationInfo);
        }

        public override SensorDeviceInformation DeviceInformation
        {
            get { return sensorMarshal.DeviceInformation; }
        }
    }
}
