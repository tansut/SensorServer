using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Rfid.Llrp.Events;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Rfid.Llrp.PhysicalDevices
{
    internal class LlrpDeviceProxyManager : ILlrpDeviceProxyManager, IDisposable
    {
        // Fields
        private bool m_fDisposed;
        private IncomingLlrpConnectionListener m_incomingConnectionListener;
        private IncomingLlrpConnectionManager m_incomingConnectionManager;
        private object m_lock = new object();
        private ILogger m_logger;
        private IProviderExtensions m_providerExtension;

        // Events
        public event EventHandler<DiscoveryEventArgs> Discovery;

        public event EventHandler<NotificationEventArgs> Notification;

        // Methods
        internal LlrpDeviceProxyManager(bool enableListen, string providerName, IProviderExtensions providerExtension, ILogger logger)
        {
            this.m_logger = logger;
            this.m_providerExtension = providerExtension;
            if (enableListen)
            {
                this.m_incomingConnectionListener = new IncomingLlrpConnectionListener(providerName, providerExtension, this.m_logger);
                this.m_incomingConnectionListener.DiscoveryEvent += new DiscoveryEventHandler(this.IncomingConnectionDiscoveryEventHandler);
                this.m_incomingConnectionListener.ConnectionNotAcceptingEvent += new EventHandler<NotificationEventArgs>(this.ConnectionNoLongerAcceptedEventHandler);
            }
            this.m_incomingConnectionManager = new IncomingLlrpConnectionManager(logger);
            this.m_incomingConnectionManager.DiscoveryEvent += new EventHandler<DiscoveryEventArgs>(this.OnDiscovery);

            this.m_fDisposed = false;
        }

        private void ConnectionNoLongerAcceptedEventHandler(object sender, NotificationEventArgs args)
        {
            this.OnProviderNotification(sender, args);
        }

        public void Dispose()
        {
            lock (this.m_lock)
            {
                if (!this.m_fDisposed)
                {
                    try
                    {
                        this.m_logger.Info("Disposing the connection manager.....");
                        if (this.m_incomingConnectionManager != null)
                        {
                            this.m_incomingConnectionManager.DiscoveryEvent -= new EventHandler<DiscoveryEventArgs>(this.OnDiscovery);
                            this.m_incomingConnectionManager.Dispose();
                        }
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        this.m_logger.Info("Closing the channel listener if any.....");
                        if (this.m_incomingConnectionListener != null)
                        {
                            this.m_incomingConnectionListener.ConnectionNotAcceptingEvent -= new EventHandler<NotificationEventArgs>(this.ConnectionNoLongerAcceptedEventHandler);
                            this.m_incomingConnectionListener.DiscoveryEvent -= new DiscoveryEventHandler(this.IncomingConnectionDiscoveryEventHandler);
                            this.m_incomingConnectionListener.Dispose();
                        }
                    }
                    catch (Exception)
                    {
                    }
                    this.m_fDisposed = true;
                }
            }
        }

        public LlrpDeviceProxy GetProxy(ConnectionInformation connectionInformation)
        {
            lock (this.m_lock)
            {
                this.m_logger.Info("Getting PDP for {0}", new object[] { connectionInformation });
                this.ThrowIfDisposed();
                LlrpDeviceProxy deviceProxy = null;
                if (!this.m_incomingConnectionManager.TryGetDevice(connectionInformation, out deviceProxy))
                {
                    deviceProxy = this.m_providerExtension.GetDeviceProxy(connectionInformation);
                }
                return deviceProxy;
            }
        }

        private void IncomingConnectionDiscoveryEventHandler(DiscoveryEventArgs discoveryEventArgs, LlrpDeviceProxy device)
        {
            this.m_incomingConnectionManager.AddOrUpdateDevice(discoveryEventArgs, device);
        }

        private void OnDiscovery(object sender, DiscoveryEventArgs args)
        {
            this.m_logger.Verbose("Raising the discovery event for device {0} from factory class", new object[] { args });
            EventHandler<DiscoveryEventArgs> discovery = this.Discovery;
            if (discovery != null)
            {
                discovery(this, args);
            }
        }

        private void OnProviderNotification(object sender, NotificationEventArgs args)
        {
            EventHandler<NotificationEventArgs> notification = this.Notification;
            if (notification != null)
            {
                notification(sender, args);
            }
        }

        private void ThrowIfDisposed()
        {
            lock (this.m_lock)
            {
                if (this.m_fDisposed)
                {
                    throw new ObjectDisposedException(base.GetType().Name);
                }
            }
        }
    }





}
