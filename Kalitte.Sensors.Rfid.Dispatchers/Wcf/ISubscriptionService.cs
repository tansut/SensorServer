using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Kalitte.Sensors.Interfaces;

namespace Kalitte.Sensors.Dispatchers.Wcf
{
    [ServiceContract(CallbackContract = typeof(IEventDispatcher))]
    public interface ISubscriptionService
    {
        [OperationContract]
        void Subscribe(string logicalSource);

        [OperationContract]
        void Unsubscribe(string logicalSource);
    }
}
