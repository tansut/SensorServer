using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Utilities;
using System.Threading;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Rfid.Llrp.Events;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Rfid.Llrp.PhysicalDevices
{
internal class IncomingLlrpConnectionManager : IDisposable
{
    // Fields
    private readonly long DiscoveryHeartbeat;
    private Dictionary<ConnectionInformation, ProxyState> m_connectionToDeviceMapper;
    private object m_discoveryLock = new object();
    private Timer m_discoveryTimer;
    private volatile bool m_fDiscoveryIsInProgress;
    private object m_lock = new object();
    private ILogger m_logger;

    // Events
    internal event EventHandler<DiscoveryEventArgs> DiscoveryEvent;

    // Methods
    internal IncomingLlrpConnectionManager(ILogger logger)
    {
        this.m_logger = logger;
        this.m_connectionToDeviceMapper = new Dictionary<ConnectionInformation, ProxyState>();
        this.DiscoveryHeartbeat = LlrpProviderContext.DiscoveryHeartbeat * 0x3e8L;
        this.m_discoveryTimer = new Timer(new TimerCallback(this.DiscoveryThread), null, this.DiscoveryHeartbeat, this.DiscoveryHeartbeat);
    }

    internal void AddOrUpdateDevice(DiscoveryEventArgs discoveryEventArgs, LlrpDeviceProxy device)
    {
        lock (this.m_lock)
        {
            this.m_logger.Info("Device {0} discovered", new object[] { discoveryEventArgs });
            ConnectionInformation connectionInformation = discoveryEventArgs.DeviceInfo.ConnectionInformation;
            if (this.IsDevicePresent(connectionInformation))
            {
                this.m_logger.Info("Device is being discovered again");
            }
            this.m_connectionToDeviceMapper[connectionInformation] = new ProxyState(device, discoveryEventArgs);
        }
        this.OnDiscovery(this, discoveryEventArgs);
    }

    private void deviceProxy_OnConnectionClosedEvent(object sender, ConnectionCloseEventArgs e)
    {
        this.ReturnDevice(e.ConnectionInformation);
    }

    private void DiscoveryThread(object state)
    {
        this.m_logger.Verbose("In discovery thread, for sending discovery message again");
        lock (this.m_discoveryLock)
        {
            if (this.m_fDiscoveryIsInProgress)
            {
                this.m_logger.Verbose("Another discovery is in progress, thus returning");
                return;
            }
            this.m_fDiscoveryIsInProgress = true;
        }
        try
        {
            List<DiscoveryEventArgs> list = new List<DiscoveryEventArgs>();
            List<ConnectionInformation> list2 = new List<ConnectionInformation>();
            lock (this.m_lock)
            {
                this.m_logger.Verbose("Starting discovery thread, for sending discovery message again");
                foreach (KeyValuePair<ConnectionInformation, ProxyState> pair in this.m_connectionToDeviceMapper)
                {
                    try
                    {
                        DiscoveryEventArgs discoveryEventArgs = pair.Value.DiscoveryEventArgs;
                        if (pair.Value.UsedByService)
                        {
                            this.m_logger.Verbose("Device {0} used by the service, thus not raising the discovery event for the same", new object[] { discoveryEventArgs.DeviceInfo });
                        }
                        else if (this.IsConnectionStale(pair.Value.DeviceProxy))
                        {
                            this.m_logger.Info("Connection to the device {0} has gone stale, removing from the pool", new object[] { discoveryEventArgs.DeviceInfo });
                            list2.Add(pair.Key);
                        }
                        else if (pair.Value.FirstCycle)
                        {
                            this.m_logger.Info("This is the first cycle for the device {0}. Thus not raising discovery", new object[] { discoveryEventArgs.DeviceInfo });
                        }
                        else
                        {
                            list.Add(discoveryEventArgs);
                        }
                        continue;
                    }
                    finally
                    {
                        pair.Value.FirstCycle = false;
                    }
                }
                foreach (ConnectionInformation information in list2)
                {
                    this.RemoveInformation(information);
                }
            }
            foreach (DiscoveryEventArgs args2 in list)
            {
                this.OnDiscovery(this, args2);
            }
        }
        catch (Exception exception)
        {
            this.m_logger.Error("Error {0} during detecting stale connection and raising discovery message for reader initiated connection", new object[] { exception });
        }
        finally
        {
            lock (this.m_discoveryLock)
            {
                this.m_fDiscoveryIsInProgress = false;
            }
        }
    }

    public void Dispose()
    {
        lock (this.m_lock)
        {
            if (this.m_discoveryTimer != null)
            {
                this.m_connectionToDeviceMapper.Clear();
                this.m_discoveryTimer.Dispose();
                this.m_discoveryTimer = null;
            }
        }
    }

    private bool IsConnectionStale(LlrpDeviceProxy deviceProxy)
    {
        try
        {
            if (deviceProxy.IsConnectionAlive())
            {
                return false;
            }
        }
        catch (Exception)
        {
        }
        return true;
    }

    private bool IsDevicePresent(ConnectionInformation connectionInformation)
    {
        return this.m_connectionToDeviceMapper.ContainsKey(connectionInformation);
    }

    private void OnDiscovery(object sender, DiscoveryEventArgs args)
    {
        EventHandler<DiscoveryEventArgs> discoveryEvent = this.DiscoveryEvent;
        if (discoveryEvent != null)
        {
            discoveryEvent(this, args);
        }
    }

    private void RemoveInformation(ConnectionInformation connectionInformation)
    {
        this.m_connectionToDeviceMapper[connectionInformation].Dispose();
        this.m_connectionToDeviceMapper.Remove(connectionInformation);
    }

    private void ReturnDevice(ConnectionInformation connectionInformation)
    {
        lock (this.m_lock)
        {
            if (this.m_connectionToDeviceMapper.ContainsKey(connectionInformation))
            {
                LlrpDeviceProxy deviceProxy = this.m_connectionToDeviceMapper[connectionInformation].DeviceProxy;
                deviceProxy.OnConnectionClosedEvent -= new EventHandler<ConnectionCloseEventArgs>(this.deviceProxy_OnConnectionClosedEvent);
                if (this.IsConnectionStale(deviceProxy))
                {
                    this.RemoveInformation(connectionInformation);
                }
                else
                {
                    this.m_logger.Info("Adding the device {0} back to the reader initiated pool", new object[] { connectionInformation });
                    this.SetUsedState(this.m_connectionToDeviceMapper[connectionInformation], false);
                }
            }
        }
    }

    private void SetUsedState(ProxyState proxyState, bool state)
    {
        proxyState.UsedByService = state;
    }

    internal bool TryGetDevice(ConnectionInformation connectionInformation, out LlrpDeviceProxy deviceProxy)
    {
        deviceProxy = null;
        lock (this.m_lock)
        {
            if (this.IsDevicePresent(connectionInformation))
            {
                deviceProxy = this.m_connectionToDeviceMapper[connectionInformation].DeviceProxy;
                if (this.IsConnectionStale(deviceProxy))
                {
                    this.m_logger.Info("The device {0} did establish connection to the provider but the connection is stale now. Thus not using the old connection handle", new object[] { connectionInformation });
                    deviceProxy = null;
                    return false;
                }
                this.SetUsedState(this.m_connectionToDeviceMapper[connectionInformation], true);
                deviceProxy.OnConnectionClosedEvent += new EventHandler<ConnectionCloseEventArgs>(this.deviceProxy_OnConnectionClosedEvent);
                this.m_logger.Info("Returning a discovered device {0}", new object[] { connectionInformation });
                return true;
            }
        }
        return false;
    }

    // Nested Types
    private class ProxyState : IDisposable
    {
        // Fields
        private LlrpDeviceProxy m_deviceProxy;
        private DiscoveryEventArgs m_discoveryEventArgs;
        private bool m_fBeingUsed;
        private bool m_firstCycle;

        // Methods
        internal ProxyState(LlrpDeviceProxy deviceProxy, DiscoveryEventArgs discoveryEventArgs)
        {
            this.m_deviceProxy = deviceProxy;
            this.m_discoveryEventArgs = discoveryEventArgs;
            this.m_fBeingUsed = false;
            this.m_firstCycle = true;
        }

        public void Dispose()
        {
            if (this.m_deviceProxy != null)
            {
                this.m_deviceProxy.CleanUp();
            }
        }

        // Properties
        internal LlrpDeviceProxy DeviceProxy
        {
            get
            {
                return this.m_deviceProxy;
            }
        }

        internal DiscoveryEventArgs DiscoveryEventArgs
        {
            get
            {
                return this.m_discoveryEventArgs;
            }
        }

        internal bool FirstCycle
        {
            get
            {
                return this.m_firstCycle;
            }
            set
            {
                this.m_firstCycle = value;
            }
        }

        internal bool UsedByService
        {
            get
            {
                return this.m_fBeingUsed;
            }
            set
            {
                this.m_fBeingUsed = value;
            }
        }
    }
}

 
 

}
