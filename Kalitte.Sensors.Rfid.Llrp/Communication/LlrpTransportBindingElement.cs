namespace Kalitte.Sensors.Rfid.Llrp.Communication
{

    using System;
    using System.ServiceModel.Channels;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Core;

  internal class LlrpTransportBindingElement : TransportBindingElement
{
    // Fields
    private ILogger m_logger;
    private int m_portToBind;

    // Methods
    public LlrpTransportBindingElement(ILogger logger) : this(0x13dc, logger)
    {
    }

    public LlrpTransportBindingElement(int portToBind, ILogger logger)
    {
        if (logger == null)
        {
            throw new ArgumentNullException("logger");
        }
        if ((portToBind <= 0) || (portToBind > 0xffff))
        {
            throw new ArgumentOutOfRangeException("portToBind");
        }
        this.m_logger = logger;
        this.m_portToBind = portToBind;
    }

    public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
    {
        return new LlrpChannelFactory<IDuplexChannel>(this, context, this.m_logger) as IChannelFactory<TChannel>;
    }

    public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context) 
    {
        return new LlrpChannelListener<IDuplexChannel>(context, this.m_portToBind, this.m_logger) as IChannelListener<TChannel>;
    }

    public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
    {
        return (typeof(TChannel) == typeof(IDuplexChannel));
    }

    public override bool CanBuildChannelListener<TChannel>(BindingContext context) 
    {
        return (typeof(TChannel) == typeof(IDuplexChannel));
    }

    public override BindingElement Clone()
    {
        return new LlrpTransportBindingElement(this.m_portToBind, this.m_logger);
    }

    // Properties
    public override string Scheme
    {
        get
        {
            return "llrp.bin";
        }
    }
}


 

}
