namespace Kalitte.Sensors.Rfid.Llrp.Communication
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Core;

    internal sealed class LlrpChannelFactory<TChannel> : ChannelFactoryBase<IDuplexChannel>
    {
        private BindingContext m_bindingContext;
        private ILogger m_logger;
        private TransportBindingElement m_transportBindingElement;

        internal LlrpChannelFactory(TransportBindingElement element, BindingContext context, ILogger logger)
        {
            this.m_transportBindingElement = element;
            this.m_bindingContext = context;
            this.m_logger = logger;
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return null;
        }

        protected override void OnClose(TimeSpan timeout)
        {
        }

        protected override IDuplexChannel OnCreateChannel(EndpointAddress address, Uri via)
        {
            return new LlrpDuplexChannel(address, this.m_bindingContext, this.m_logger);
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
        }

        protected override void OnOpen(TimeSpan timeout)
        {
        }
    }
}
