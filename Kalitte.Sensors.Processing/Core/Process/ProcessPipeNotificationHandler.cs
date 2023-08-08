using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Processing.Core.Process
{

    [Serializable]
    public class ProcessPipeNotificationEventArgs : EventArgs
    {
        public KeyValuePair<string, SensorEventBase> ProcessPipeEvent { get; set; }
        public ProcessPipeNotificationEventArgs(KeyValuePair<string, SensorEventBase> processPipeEvent)
        {
            this.ProcessPipeEvent = processPipeEvent;
        }
    }

    public class ProcessPipeNotificationHandler : MarshalBase
    {
        public EventHandler<ProcessPipeNotificationEventArgs> onNotification;

        public ProcessPipeNotificationHandler(EventHandler<ProcessPipeNotificationEventArgs> onNotification)
            : base()
        {
            this.onNotification = onNotification;
        }

        public void NotificationEvent(object sender, ProcessPipeNotificationEventArgs e)
        {
            this.onNotification(sender, e);
        }
    }

}
