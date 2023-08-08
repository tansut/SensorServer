using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Kalitte.Sensors.Interfaces;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Dispatchers.Wcf
{
    class DispatcherProxy : System.ServiceModel.ClientBase<IEventDispatcher>, IEventDispatcher
    {
        #region IEventDispatcher Members

        public void Notify(string source, SensorEventBase sensorEvent)
        {
            Channel.Notify(source, sensorEvent);
        }


        #endregion

        public DispatcherProxy()
        { }

        public DispatcherProxy(string endpointConfigurationName)
            : base(endpointConfigurationName)
        { }

        public DispatcherProxy(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        { }

        public DispatcherProxy(string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        { }

        public DispatcherProxy(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        { }


    }
}
