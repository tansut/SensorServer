using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Events
{
    [Serializable]
    public sealed class NotificationEventArgs : EventArgs
    {
        // Fields
        private readonly Notification notification;

        // Methods
        public NotificationEventArgs(Notification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException("notification");
            }
            this.notification = notification;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<notificationEventArgs>");
            builder.Append("<notification>");
            builder.Append(this.notification);
            builder.Append("</notification>");
            builder.Append("</notificationEventArgs>");
            return builder.ToString();
        }

        // Properties
        public Notification Notification
        {
            get
            {
                return this.notification;
            }
        }
    }




}
