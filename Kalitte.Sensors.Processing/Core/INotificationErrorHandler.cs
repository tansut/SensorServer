using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Kalitte.Sensors.Processing.Core
{
    public interface INotificationErrorHandler
    {
        void HandleError(object sender, ExceptionEventArgs exc);
    }
}
