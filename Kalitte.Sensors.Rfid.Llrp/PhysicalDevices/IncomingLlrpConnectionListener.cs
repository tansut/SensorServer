using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Rfid;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using System.Net.Sockets;
using Kalitte.Sensors.Communication;
using System.Globalization;
using Kalitte.Sensors.Rfid.Llrp.Core;
using Kalitte.Sensors.Rfid.Llrp.Helpers;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Rfid.Llrp.Events;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Events.Management;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Rfid.Llrp.PhysicalDevices
{

    public interface IProviderExtensions
    {
        // Methods
        IDuplexChannel GetChannel(ConnectionInformation connectionInformation);
        IChannelListener<IDuplexChannel> GetChannelListener();
        LlrpDeviceProxy GetDeviceProxy(ConnectionInformation connectionInformation);
        LlrpDeviceProxy GetDeviceProxy(ConnectionInformation connectionInformation, IDuplexChannel duplexChannel);
    }

 


    internal class IncomingLlrpConnectionListener : IDisposable
    {
        // Fields
        private const int LlrpInboundPort = 0x13dc;
        private bool m_canAcceptConnection = true;
        private IChannelListener<IDuplexChannel> m_channelListener;
        private object m_lock = new object();
        private ILogger m_logger;
        private IProviderExtensions m_provider;
        private string m_providerName;

        // Events
        internal event EventHandler<NotificationEventArgs> ConnectionNotAcceptingEvent;

        internal event DiscoveryEventHandler DiscoveryEvent;

        // Methods
        internal IncomingLlrpConnectionListener(string providerName, IProviderExtensions provider, ILogger logger)
        {
            this.m_providerName = providerName;
            this.m_provider = provider;
            this.m_logger = logger;
            try
            {
                this.m_channelListener = provider.GetChannelListener();
                this.m_channelListener.Open();
            }
            catch (Exception exception)
            {
                throw new SensorProviderException(LlrpResources.ErrorwhileBindingToListeningPort, exception);
            }
            this.BeginAcceptConnection();
        }

        private void BeginAcceptConnection()
        {
            try
            {
                lock (this.m_lock)
                {
                    if (!this.CanAcceptConnection)
                    {
                        this.m_logger.Info("Stopping handling more connection");
                    }
                    else
                    {
                        this.m_channelListener.BeginAcceptChannel(new AsyncCallback(this.CallbackAcceptConnection), null);
                    }
                }
            }
            catch (SocketException exception)
            {
                this.HandleChannelException(exception);
            }
            catch (ObjectDisposedException exception2)
            {
                this.HandleChannelException(exception2);
            }
            catch (Exception exception3)
            {
                this.HandleChannelException(exception3);
            }
        }

        private void CallbackAcceptConnection(IAsyncResult result)
        {
            IDuplexChannel duplexChannel = null;
            try
            {
                lock (this.m_lock)
                {
                    if (!this.CanAcceptConnection)
                    {
                        return;
                    }
                    duplexChannel = this.m_channelListener.EndAcceptChannel(result);
                }
            }
            catch (SocketException exception)
            {
                this.HandleChannelException(exception);
                return;
            }
            catch (ObjectDisposedException exception2)
            {
                this.HandleChannelException(exception2);
                return;
            }
            catch (Exception exception3)
            {
                this.HandleChannelException(exception3);
                return;
            }
            LlrpDevice physicalDevice = null;
            LlrpDeviceProxy deviceProxy = null;
            try
            {
                this.m_logger.Info("Connection accepted from device {0}", new object[] { duplexChannel.RemoteAddress.Uri });
                ConnectionInformation connectionInformation = new ConnectionInformation(this.m_providerName, new TcpTransportSettings(duplexChannel.RemoteAddress.Uri.Host, 0x13dc));
                deviceProxy = this.m_provider.GetDeviceProxy(connectionInformation, duplexChannel);
                physicalDevice = deviceProxy.PhysicalDevice;
                physicalDevice.StartReading();
                physicalDevice.WaitForConnectionAttemptEvent();
                GetReaderConfigurationResponse message = physicalDevice.Request(new GetReaderConfigurationMessage(ReaderConfigurationRequestedData.Identification, 0, 0, 0, null)) as GetReaderConfigurationResponse;
                Util.ThrowIfNull(message, LlrpMessageType.GetReaderConfig);
                Util.ThrowIfFailed(message.Status);
                string deviceId = Util.GetDeviceId(message.Identification);
                PropertyList profile = new PropertyList(LlrpResources.PropertyProfileName);
                //Util.PopulateDeviceCustomProperties(message, ref profile, null);
                this.RaiseDiscoveryEvent(new DiscoveryEventArgs(new SensorDeviceInformation(deviceId, connectionInformation,  null), profile), deviceProxy);
            }
            catch (Exception exception4)
            {
                if (this.CanAcceptConnection)
                {
                    this.m_logger.Error("Error {0} during discovering device {1}", new object[] { exception4, duplexChannel.RemoteAddress });
                    try
                    {
                        if (physicalDevice != null)
                        {
                            physicalDevice.ActuallyClose(false);
                        }
                    }
                    catch (Exception exception5)
                    {
                        this.m_logger.Warning("Error {0} during closing connection to discovered device", new object[] { exception5 });
                    }
                }
            }
            finally
            {
                this.BeginAcceptConnection();
            }
        }

        public void Dispose()
        {
            lock (this.m_lock)
            {
                try
                {
                    this.CanAcceptConnection = false;
                    if (this.m_channelListener != null)
                    {
                        this.m_channelListener.Close();
                    }
                }
                catch (Exception exception)
                {
                    this.m_logger.Warning("Error {0} while closing the channel listener in the provider for incoming connection", new object[] { exception });
                }
                this.m_channelListener = null;
            }
        }

        private void HandleChannelException(Exception ex)
        {
            lock (this.m_lock)
            {
                if (!this.CanAcceptConnection)
                {
                    return;
                }
                this.m_logger.Error("Exception {0} during accept reader connection in provider", new object[] { ex });
                this.Dispose();
            }
            this.RaiseErrorEvent(ex);
        }

        private void RaiseDiscoveryEvent(DiscoveryEventArgs discoveryEventArgs, LlrpDeviceProxy device)
        {
            if (this.CanAcceptConnection)
            {
                this.m_logger.Info("Device with info {0} discovered in incoming connection handler", new object[] { discoveryEventArgs });
                DiscoveryEventHandler discoveryEvent = this.DiscoveryEvent;
                if (discoveryEvent != null)
                {
                    //discoveryEventArgs.DeviceInfo.ProviderData.Add("LLRPDeviceProxy", device);
                    discoveryEvent(discoveryEventArgs, device);
                }
            }
        }

        private void RaiseErrorEvent(Exception ex)
        {
            EventHandler<NotificationEventArgs> connectionNotAcceptingEvent = this.ConnectionNotAcceptingEvent;
            if (connectionNotAcceptingEvent != null)
            {
                ProviderDefunctEvent managementEvent = new ProviderDefunctEvent(string.Format(CultureInfo.CurrentCulture, LlrpResources.ReaderInitiatedConnectionStoppedDescription, new object[] { ex.Message }));
                managementEvent.ProviderName = this.m_providerName;
                connectionNotAcceptingEvent(this, new NotificationEventArgs(new Notification(managementEvent)));
            }
        }

        // Properties
        private bool CanAcceptConnection
        {
            get
            {
                lock (this.m_lock)
                {
                    return this.m_canAcceptConnection;
                }
            }
            set
            {
                lock (this.m_lock)
                {
                    this.m_canAcceptConnection = value;
                }
            }
        }
    }



}
