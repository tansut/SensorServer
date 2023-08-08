using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Kalitte.Sensors.Interfaces;

namespace Kalitte.Sensors.Dispatchers.Wcf
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    class SubscriptionService :
       SubscriptionServiceBase<IEventDispatcher>, ISubscriptionService { 
    }
}