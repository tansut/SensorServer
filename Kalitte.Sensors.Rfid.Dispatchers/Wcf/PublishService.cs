using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Kalitte.Sensors.Interfaces;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Dispatchers.Wcf
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    class PublishService : PublishServiceBase<IEventDispatcher>, IEventDispatcher
    {

        #region IEventDispatcher Members

        public void Notify(string source, SensorEventBase sensorEvent)
        {
            FireEvent(source, sensorEvent);
        }

        #endregion
    }
}
