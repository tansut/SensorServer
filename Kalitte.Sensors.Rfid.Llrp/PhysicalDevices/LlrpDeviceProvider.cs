using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Utilities;
using System.ServiceModel;
using Kalitte.Sensors.Rfid.Llrp.Helpers;
using System.ServiceModel.Channels;
using Kalitte.Sensors.Rfid;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using System.Runtime.CompilerServices;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Rfid.Llrp.Communication;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Rfid.Llrp.Configuration;
using Kalitte.Sensors.Events.Management;
using Kalitte.Sensors.Rfid.Llrp.Core;
using Kalitte.Sensors.Rfid.Llrp.Events;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Rfid.PhysicalDevices;
using System.Globalization;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Rfid.Utilities;
using Kalitte.Sensors.Rfid.Llrp.Utilities;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Security;

namespace Kalitte.Sensors.Rfid.Llrp.PhysicalDevices
{



    public class LlrpDeviceProvider : RfidDeviceProvider, IProviderExtensions
    {
        private IChannelFactory<IDuplexChannel> m_channelFactory;
        private CustomBinding m_customBinding;
        private string m_DiscoveryError;
        private ILlrpDeviceProxyManager m_llrpDeviceProxyManager;
        private ILogger m_logger;
        private PropertyList m_providerInitProperties;
        private string m_providerName;
        //private WSDiscoveryForDspi m_wsDiscovery;
        private EventHandler<NotificationEventArgs> providerNotificationEvent;
        private EventHandler<DiscoveryEventArgs> discoveryEvent;

        public static ProviderMetadata GetMetadata()
        {
            ProviderInformation providerInformation = new ProviderInformation(LlrpResources.ProviderId, LlrpResources.ProviderDescription, typeof(LlrpDeviceProvider).Assembly.GetName().Version.ToString());
            Dictionary<PropertyKey, ProviderPropertyMetadata> providerPropertyMetadata = new Dictionary<PropertyKey, ProviderPropertyMetadata>();

            providerPropertyMetadata.Add(LlrpProviderConnectionGroup.PortKey, LlrpProviderConnectionGroup.PortMetadata);
            providerPropertyMetadata.Add(LlrpProviderGeneralGroup.LlrpVersionKey, LlrpProviderGeneralGroup.LlrpVersionMetadata);
            providerPropertyMetadata.Add(LlrpProviderManagementGroup.LlrpMessageTimeoutKey, LlrpProviderManagementGroup.LlrpMessageTimeoutMetadata);
            providerPropertyMetadata.Add(LlrpProviderManagementGroup.TcpKeepAliveTimeKey, LlrpProviderManagementGroup.TcpKeepAliveTimeMetadata);
            providerPropertyMetadata.Add(LlrpProviderConnectionGroup.ListenForIncomingConnectionsKey, LlrpProviderConnectionGroup.ListenForIncomingConnectionsMetadata);
            //providerPropertyMetadata.Add(LlrpProviderConnectionGroup.DeviceInitiatedConnectionDiscoveryHeartbeatKey, LlrpProviderConnectionGroup.DeviceInitiatedConnectionDiscoveryHeartbeatMetadata);
            //providerPropertyMetadata.Add(LlrpProviderDiscoveryGroup.MatchTimeoutKey, LlrpProviderDiscoveryGroup.MatchTimeoutMetadata);
            Collection<ProviderCapability> capabilities = new Collection<ProviderCapability>();
            capabilities.Add(ProviderCapability.TcpTransport);
            //capabilities.Add(ProviderCapability.ProviderDefunctEvent);
            //capabilities.Add(ProviderCapability.Discovery);
            //capabilities.Add(ProviderCapability.TriggeredDiscovery);
            Dictionary<VendorEntityKey, VendorEntityMetadata> vendorExtensionsEntityMetadata = new Dictionary<VendorEntityKey, VendorEntityMetadata>();
            Dictionary<string, VendorEntityParameterMetadata> subEntities = new Dictionary<string, VendorEntityParameterMetadata>();
            subEntities.Add("Hop Table Id", new VendorEntityParameterMetadata(typeof(ushort), LlrpResources.HoppingEventHopTableIdDescription, null, true));
            subEntities.Add("Next Channel Index", new VendorEntityParameterMetadata(typeof(ushort), LlrpResources.HoppingEventNextChannelIndexDescription, null, true));
            vendorExtensionsEntityMetadata.Add(new VendorEntityKey(typeof(VendorDefinedManagementEvent), typeof(HoppingEvent).Name, EntityType.ManagementEvent), new VendorEntityMetadata(LlrpEventTypes.HoppingEvent.Description, subEntities));
            Dictionary<string, VendorEntityParameterMetadata> dictionary4 = new Dictionary<string, VendorEntityParameterMetadata>();
            dictionary4.Add("Access spec Id", new VendorEntityParameterMetadata(typeof(uint), LlrpResources.ReaderExceptionEventAccessSpecDescription, null, false));
            dictionary4.Add("Antenna Name", new VendorEntityParameterMetadata(typeof(string), LlrpResources.ReaderExceptionEventAntennaNameDescription, null, false));
            dictionary4.Add("Inventory Parameter Spec Id", new VendorEntityParameterMetadata(typeof(ushort), LlrpResources.ReaderExceptionEventInventoryParameterSpecIdDescription, null, false));
            dictionary4.Add("Operation Spec Id", new VendorEntityParameterMetadata(typeof(ushort), LlrpResources.ReaderExceptionEventOperationSpecDescription, null, false));
            dictionary4.Add("Message", new VendorEntityParameterMetadata(typeof(string), LlrpResources.ReaderExceptionEventMessageDescription, null, true));
            dictionary4.Add("RO Spec Id", new VendorEntityParameterMetadata(typeof(uint), LlrpResources.ReaderExceptionEventROSpecIdDescription, null, false));
            dictionary4.Add("Spec Index", new VendorEntityParameterMetadata(typeof(ushort), LlrpResources.ReaderExceptionEventSpecIndexDescription, null, false));
            vendorExtensionsEntityMetadata.Add(new VendorEntityKey(typeof(VendorDefinedManagementEvent), typeof(ReaderExceptionEvent).Name, EntityType.ManagementEvent), new VendorEntityMetadata(LlrpResources.ReaderExceptionEventTypeDescription, dictionary4));
            Dictionary<string, VendorEntityParameterMetadata> dictionary5 = new Dictionary<string, VendorEntityParameterMetadata>();
            dictionary5.Add("Event Type", new VendorEntityParameterMetadata(typeof(string), LlrpResources.RFSurveyEventTypeDescription, null, true, Util.GetNames(typeof(RFSurveyEventType))));
            dictionary5.Add("RO Spec Id", new VendorEntityParameterMetadata(typeof(uint), LlrpResources.RFSurveyEventROSpecIdDescription, null, true));
            dictionary5.Add("Spec Index", new VendorEntityParameterMetadata(typeof(ushort), LlrpResources.RFSurveyEventSpecIndexDescription, null, true));
            vendorExtensionsEntityMetadata.Add(new VendorEntityKey(typeof(VendorDefinedManagementEvent), typeof(RFSurveyEvent).Name, EntityType.ManagementEvent), new VendorEntityMetadata(LlrpResources.RFSurveyEventDescription, dictionary5));
            Dictionary<string, VendorEntityParameterMetadata> dictionary6 = new Dictionary<string, VendorEntityParameterMetadata>();
            dictionary6.Add("Message", new VendorEntityParameterMetadata(typeof(string), LlrpResources.ConnectionAttemptEventStatusDescription, null, true, Util.GetNames(typeof(ConnectionAttemptEventType))));
            vendorExtensionsEntityMetadata.Add(new VendorEntityKey(typeof(VendorDefinedManagementEvent), typeof(ConnectionAttemptEvent).Name, EntityType.ManagementEvent), new VendorEntityMetadata(LlrpResources.ConnectionAttemptEventTypeDescription, dictionary6));
            Dictionary<string, VendorEntityParameterMetadata> dictionary7 = new Dictionary<string, VendorEntityParameterMetadata>();
            dictionary7.Add("Frequency RSSI Informations", new VendorEntityParameterMetadata(typeof(string), LlrpResources.RFSurveyReportFrequencyRssiDescription, null, true));
            dictionary7.Add("RO Spec Id", new VendorEntityParameterMetadata(typeof(uint), LlrpResources.RFSurveyReportROSpecIdDescription, null, false));
            dictionary7.Add("Spec Index", new VendorEntityParameterMetadata(typeof(ushort), LlrpResources.RFSurveyReportSpecIndexDescription, null, false));
            vendorExtensionsEntityMetadata.Add(new VendorEntityKey(typeof(VendorDefinedManagementEvent), typeof(RFSurveyReportData).Name, EntityType.ManagementEvent), new VendorEntityMetadata(LlrpResources.RFSurveyReportEventDescription, dictionary7));
            vendorExtensionsEntityMetadata.Add(new VendorEntityKey(typeof(VendorDefinedManagementEvent), typeof(MultipleNicWarningEvent).Name, EntityType.ManagementEvent), new VendorEntityMetadata(LlrpResources.MultipleNicWarningEventMessage, null));
            vendorExtensionsEntityMetadata.Add(new VendorEntityKey(typeof(VendorDefinedManagementEvent), typeof(NoNicWarningEvent).Name, EntityType.ManagementEvent), new VendorEntityMetadata(LlrpResources.NoNicWarningEventMessage, null));
            Dictionary<string, VendorEntityParameterMetadata> dictionary8 = new Dictionary<string, VendorEntityParameterMetadata>();
            dictionary8.Add(LlrpResources.LlrpMessageCommandKey, new VendorEntityParameterMetadata(typeof(byte[]), LlrpResources.LlrpMessageCommandKeyDescription, null, true));
            vendorExtensionsEntityMetadata.Add(new VendorEntityKey(typeof(VendorCommand), LlrpResources.LlrpMessageVendordefinedCommand, EntityType.Command), new VendorEntityMetadata(LlrpResources.LlrpMessageVendordefinedCommandDescription, dictionary8));
            Dictionary<string, VendorEntityParameterMetadata> dictionary9 = new Dictionary<string, VendorEntityParameterMetadata>();
            dictionary9.Add(LlrpResources.LlrpMessageResponseKey, new VendorEntityParameterMetadata(typeof(byte[]), LlrpResources.LlrpMessageResponseKeyDescription, null, true));
            vendorExtensionsEntityMetadata.Add(new VendorEntityKey(typeof(VendorResponse), LlrpResources.LlrpMessageVendordefinedCommand, EntityType.Response), new VendorEntityMetadata(LlrpResources.LlrpMessageVendordefinedResponseDescription, dictionary9));
            return new ProviderMetadata(providerInformation, capabilities, providerPropertyMetadata, vendorExtensionsEntityMetadata, Util.GetStandardLlrpDeviceMetadata());
        }


        public LlrpDeviceProvider()
        {
            this.m_providerInitProperties = new PropertyList(LlrpResources.PropertyProfileName);
            this.m_providerInitProperties.Add(LlrpProviderConnectionGroup.PortKey, LlrpProviderConnectionGroup.PortMetadata.DefaultValue);
            this.m_providerInitProperties.Add(LlrpProviderDiscoveryGroup.MatchTimeoutKey, LlrpProviderDiscoveryGroup.MatchTimeoutMetadata.DefaultValue);
            this.m_providerInitProperties.Add(LlrpProviderGeneralGroup.LlrpVersionKey, LlrpProviderGeneralGroup.LlrpVersionMetadata.DefaultValue);
        }



        static LlrpDeviceProvider()
        {
            //TypesHelper.RegisterKnownType(LlrpKnownTypeHelper.KnownType.ToArray());
        }




        private void HandleProviderInitProperties(PropertyList providerInitParameters)
        {
            if ((providerInitParameters != null) && (providerInitParameters.Count != 0))
            {
                foreach (PropertyKey key in providerInitParameters.Keys)
                {
                    this.SetProperty(new EntityProperty(key, providerInitParameters[key]));
                }
            }
        }





        public override void Shutdown()
        {
            this.m_logger.Info("Provider is shutting down.....");
            //this.m_logger.Info("Disposing the proxy factory...");
            if (this.m_llrpDeviceProxyManager != null)
            {
                this.m_llrpDeviceProxyManager.Discovery -= new EventHandler<DiscoveryEventArgs>(this.OnDiscovery);
                this.m_llrpDeviceProxyManager.Notification -= new EventHandler<NotificationEventArgs>(this.OnProviderNotification);
                this.m_llrpDeviceProxyManager.Dispose();
            }
            //this.m_logger.Info("Closing the channel factory if any.....");
            if (this.m_channelFactory != null)
            {
                try
                {
                    this.m_channelFactory.Close();
                }
                catch (Exception exception)
                {
                    this.m_logger.Error("Error during closing the channel factory {0}", new object[] { exception });
                }
            }
            //this.m_logger.Info("Closing the WS-Discovery...");
            //if (this.m_wsDiscovery != null)
            //{
            //    this.m_wsDiscovery.OnDeviceAdded -= new EventHandler<WSDiscoveryEventArgs>(this.m_wsDiscovery_OnDeviceAdded);
            //    this.m_wsDiscovery.SearchCompleted -= new AsyncCompletedEventHandler(this.m_wsDiscovery_SearchCompleted);
            //    this.m_wsDiscovery.Dispose();
            //}
            this.m_logger.Info("Provider shut down done");
        }

        public override event EventHandler<DiscoveryEventArgs> DiscoveryEvent
        {
            add
            {
                this.discoveryEvent = (EventHandler<DiscoveryEventArgs>)Delegate.Combine(this.discoveryEvent, value);
            }
            remove
            {
                this.discoveryEvent = (EventHandler<DiscoveryEventArgs>)Delegate.Remove(this.discoveryEvent, value);
            }
        }

        public override event EventHandler<NotificationEventArgs> ProviderNotificationEvent
        {
            add
            {
                this.providerNotificationEvent = (EventHandler<NotificationEventArgs>)Delegate.Combine(this.providerNotificationEvent, value);
            }
            remove
            {
                this.providerNotificationEvent = (EventHandler<NotificationEventArgs>)Delegate.Remove(this.providerNotificationEvent, value);
            }
        }

        protected virtual void OnDiscovery(object sender, DiscoveryEventArgs args)
        {
            this.m_logger.Verbose("Raising the discovery event for device {0}", new object[] { args });
            EventHandler<DiscoveryEventArgs> discoveryEvent = this.discoveryEvent;
            if (discoveryEvent != null)
            {
                discoveryEvent(this, args);
            }
        }







        protected virtual void OnProviderNotification(object sender, NotificationEventArgs args)
        {
            EventHandler<NotificationEventArgs> providerNotificationEvent = this.providerNotificationEvent;
            if (providerNotificationEvent != null)
            {
                providerNotificationEvent(sender, args);
            }
        }



        public override SensorProxy GetPhysicalSensor(ConnectionInformation connectionInformation)
        {
            if (connectionInformation == null)
            {
                throw new ArgumentNullException("connectionInformation");
            }
            if (!(connectionInformation.TransportSettings is TcpTransportSettings))
            {
                throw new ArgumentException(LlrpResources.NotSupportedConnectionType);
            }
            return this.m_llrpDeviceProxyManager.GetProxy(connectionInformation);
        }













        private object GetProviderProperty(PropertyKey key)
        {
            lock (this.m_providerInitProperties)
            {
                return this.m_providerInitProperties[key];
            }
        }


        public virtual LlrpDeviceProxy GetDeviceProxy(ConnectionInformation connectionInformation)
        {
            if (connectionInformation == null)
            {
                throw new ArgumentNullException("connectionInformation");
            }
            return new LlrpDeviceProxy(connectionInformation, this.GetChannel(connectionInformation), LlrpProviderContext.Logger);
        }


        public virtual LlrpDeviceProxy GetDeviceProxy(ConnectionInformation connectionInformation, IDuplexChannel duplexChannel)
        {
            if (connectionInformation == null)
            {
                throw new ArgumentNullException("connectionInformation");
            }
            if (duplexChannel == null)
            {
                throw new ArgumentNullException("duplexChannel");
            }
            return new LlrpDeviceProxy(connectionInformation, duplexChannel, LlrpProviderContext.Logger);
        }






        public IDuplexChannel GetChannel(ConnectionInformation connectionInformation)
        {
            if (connectionInformation == null)
            {
                throw new ArgumentNullException("connectionInformation");
            }
            TcpTransportSettings transportSettings = connectionInformation.TransportSettings as TcpTransportSettings;
            if (transportSettings == null)
            {
                throw new ArgumentException("NotSupportedConnectionType");
            }
            return this.m_channelFactory.CreateChannel(new EndpointAddress(Util.GetLlrpUriAddress(transportSettings.Host, transportSettings.Port)));
        }

        private void SetProviderProperty(PropertyKey key, object value)
        {
            lock (this.m_providerInitProperties)
            {
                this.m_providerInitProperties[key] = value;
            }
        }






        public override void SetProperty(EntityProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            this.m_logger.Info("SetProviderProperty called with property {0}", new object[] { property });
            PropertyKey key = property.Key;
            if (key.Equals(LlrpProviderGeneralGroup.LlrpVersionKey))
            {
                this.SetProviderProperty(key, property.PropertyValue);
            }
            else if (key.Equals(LlrpProviderConnectionGroup.PortKey))
            {
                this.SetProviderProperty(key, property.PropertyValue);
            }
            else if (key.Equals(LlrpProviderConnectionGroup.ListenForIncomingConnectionsKey))
            {
                this.SetProviderProperty(key, property.PropertyValue);
            }
            else if (key.Equals(LlrpProviderConnectionGroup.DeviceInitiatedConnectionDiscoveryHeartbeatKey))
            {
                LlrpProviderContext.DiscoveryHeartbeat = (long)property.PropertyValue;
            }
            else if (key.Equals(LlrpProviderManagementGroup.LlrpMessageTimeoutKey))
            {
                LlrpProviderContext.LlrpMessageTimeout = (int)property.PropertyValue;
            }
            else if (key.Equals(LlrpProviderManagementGroup.TcpKeepAliveTimeKey))
            {
                LlrpProviderContext.TcpKeepAliveTime = (int)property.PropertyValue;
            }
            else
            {
                if (!key.Equals(LlrpProviderDiscoveryGroup.MatchTimeoutKey))
                {
                    throw new SensorProviderException(string.Format(CultureInfo.CurrentCulture, LlrpResources.UnsupportedProperty, new object[] { property.Key }));
                }
                this.SetProviderProperty(key, property.PropertyValue);
            }
            this.m_logger.Info("Returning from SetProviderProperty");
        }

        public virtual IChannelListener<IDuplexChannel> GetChannelListener()
        {
            return this.m_customBinding.BuildChannelListener<IDuplexChannel>(new BindingParameterCollection());
        }

        public override void Startup(SensorProviderContext providerContext, string providerName, PropertyList providerInitParameters)
        {
            if (providerContext == null)
            {
                throw new ArgumentNullException("providerContext");
            }
            var licenseManager = new SensorLicenseManager();
            licenseManager.Validate();
            LlrpProviderContext.Logger = this.m_logger = providerContext.Logger;
            this.m_providerName = providerName;
            this.m_logger.Info("{0} being intialized", new object[] { providerName });
            this.HandleProviderInitProperties(providerInitParameters);
            int providerProperty = (int)this.GetProviderProperty(LlrpProviderConnectionGroup.PortKey);
            bool listen = (bool)this.GetProviderProperty(LlrpProviderConnectionGroup.ListenForIncomingConnectionsKey);

            LlrpTransportBindingElement element = new LlrpTransportBindingElement(providerProperty, this.m_logger);
            this.m_customBinding = new CustomBinding(new BindingElement[] { element });
            this.m_channelFactory = this.m_customBinding.BuildChannelFactory<IDuplexChannel>(new BindingParameterCollection());
            this.m_channelFactory.Open();

            this.m_llrpDeviceProxyManager = new LlrpDeviceProxyManager(listen, providerName, this, this.m_logger);
            this.m_llrpDeviceProxyManager.Discovery += new EventHandler<DiscoveryEventArgs>(this.OnDiscovery);
            this.m_llrpDeviceProxyManager.Notification += new EventHandler<NotificationEventArgs>(this.OnProviderNotification);
            //try
            //{
            //    this.m_wsDiscovery = new WSDiscoveryForDspi(providerName);
            //    this.m_wsDiscovery.OnDeviceAdded += new EventHandler<WSDiscoveryEventArgs>(this.m_wsDiscovery_OnDeviceAdded);
            //    this.m_wsDiscovery.SearchCompleted += new AsyncCompletedEventHandler(this.m_wsDiscovery_SearchCompleted);
            //}
            //catch (InvalidOperationException exception)
            //{
            //    if (!exception.Message.Equals(WSDiscoveryResources.TooManyRelevantNics, StringComparison.CurrentCulture))
            //    {
            //        if (!exception.Message.Equals(WSDiscoveryResources.NoRelevantNics, StringComparison.CurrentCulture))
            //        {
            //            throw;
            //        }
            //        this.m_logger.Warning("Discovery of devices using WS-Discovery is not turned on by default if there are no operational network with UDP multicast support. Ensure that the computer has a minimum of one operational network adapter with UDP multicast support. Please refer to BizTalk RFID documentation to enable WS-Discovery.");
            //        this.OnProviderNotification(this, new NotificationEventArgs(new Notification(new VendorDefinedManagementEvent(EventLevel.Warning, LlrpEventTypes.NoNicWarningEvent, LlrpResources.NoNicWarningEventMessage, typeof(NoNicWarningEvent).Name, null))));
            //        this.m_DiscoveryError = LlrpResources.NoNicWarningEventMessage;
            //    }
            //    else
            //    {
            //        this.m_logger.Warning("Discovery of devices using WS-Discovery is not turned on by default in a multiple network adapter configuration. Please refer to BizTalk RFID documentation to enable WS-Discovery.");
            //        this.OnProviderNotification(this, new NotificationEventArgs(new Notification(new VendorDefinedManagementEvent(EventLevel.Warning, LlrpEventTypes.MultipleNicWarningEvent, LlrpResources.MultipleNicWarningEventMessage, typeof(MultipleNicWarningEvent).Name, null))));
            //        this.m_DiscoveryError = LlrpResources.MultipleNicWarningEventMessage;
            //    }
            //}
        }






        //public override void StartDiscovery()
        //{
        //    //if (this.m_wsDiscovery != null)
        //    //{
        //    //    this.m_logger.Info("Starting the WS-Discovery");
        //    //    this.m_wsDiscovery.StartDiscovery(LlrpWSDiscovery.LlrpMatchCriteria);
        //    //    this.m_logger.Info("Starting the WS-Discovery completed");
        //    //}
        //}

        //public override void StopDiscovery()
        //{
        //    //if (this.m_wsDiscovery != null)
        //    //{
        //    //    this.m_logger.Info("Stopping the WS-Discovery");
        //    //    this.m_wsDiscovery.StopDiscovery();
        //    //    this.m_logger.Info("Stopping the WS-Discovery done");
        //    //}
        //}


        //public override void TriggerDiscovery()
        //{
        //    //if (this.m_wsDiscovery != null)
        //    //{
        //    //    this.m_logger.Info("Triggering the WS-Discovery");
        //    //    int providerProperty = (int)this.GetProviderProperty(LlrpProviderDiscoveryGroup.MatchTimeoutKey);
        //    //    this.m_logger.Info("Match timeout is {0}", new object[] { providerProperty });
        //    //    this.m_wsDiscovery.MatchTimeout = providerProperty;
        //    //    this.m_wsDiscovery.SearchAsync(LlrpWSDiscovery.LlrpMatchCriteria);
        //    //    this.m_logger.Info("Triggering the WS-Discovery done");
        //    //}
        //    //else
        //    //{
        //    //    SensorProviderException exception = new SensorProviderException(this.m_DiscoveryError);
        //    //    this.m_logger.Error(exception.ToString());
        //    //    throw exception;
        //    //}
        //}

    }
}
