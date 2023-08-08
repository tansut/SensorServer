using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Commands;

namespace Kalitte.Sensors.Processing.Core.Sensor
{
    internal class ProviderEventMarshal : MarshalBase
{
    // Fields
    internal EventHandler<DiscoveryEventArgs> m_discoveryEvent;
    internal EventHandler<NotificationEventArgs> m_notificationEvent;
    internal EventHandler<NotificationEventArgs> m_providerNotificationEvent;
    internal EventHandler<ResponseEventArgs> m_responseEvent;

    // Methods
    public ProviderEventMarshal(EventHandler<DiscoveryEventArgs> discoveryDelegate, EventHandler<NotificationEventArgs> notificationDelegate)
    {
        this.m_discoveryEvent = discoveryDelegate;
        this.m_providerNotificationEvent = notificationDelegate;
    }

    public ProviderEventMarshal(EventHandler<ResponseEventArgs> responseDelegate, EventHandler<NotificationEventArgs> notificationDelegate)
    {
        this.m_responseEvent = responseDelegate;
        this.m_notificationEvent = notificationDelegate;
    }

    public void proxyCmdResponseEvent(object sender, ResponseEventArgs e)
    {
        this.m_responseEvent(sender, e);
    }

    public void proxyDeviceNotificationEvent(object sender, NotificationEventArgs e)
    {
        this.m_notificationEvent(sender, e);
    }

    public void proxyDiscoveryEvent(object sender, DiscoveryEventArgs e)
    {
        this.m_discoveryEvent(sender, e);
    }

    public void proxyProviderNotificationEvent(object sender, NotificationEventArgs e)
    {
        this.m_providerNotificationEvent(sender, e);
    }
}


 

}
