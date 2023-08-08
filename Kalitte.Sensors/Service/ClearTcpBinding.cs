using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Configuration;
using System.Net.Security;
using System.Xml;

namespace Kalitte.Sensors.Service
{
    public class ClearTcpBinding : CustomBinding
    {
        private long _maxReceivedMessageSize = 2147483647;

        public void SetMaxReceivedMessageSize(long value)
        {
            _maxReceivedMessageSize = value;
        }

        public override BindingElementCollection CreateBindingElements()
        {
            var res = new BindingElementCollection
                          {
                              new BinaryMessageEncodingBindingElement {MessageVersion = MessageVersion.Soap12WSAddressing10, ReaderQuotas = XmlDictionaryReaderQuotas.Max},
                              SecurityBindingElement.CreateUserNameOverTransportBindingElement(),
                              new AutoSecuredTcpTransportElement {MaxReceivedMessageSize = _maxReceivedMessageSize}
                          };
            return res;
        }

        public override string Scheme { get { return "net.tcp"; } }
    }

    public class ClearTcpBindingElement : StandardBindingElement
    {
        private ConfigurationPropertyCollection _properties;

        protected override void OnApplyConfiguration(Binding binding)
        {
            var b = (ClearTcpBinding)binding;
            b.SetMaxReceivedMessageSize(Convert.ToInt64(MaxReceivedMessageSize));
        }

        protected override Type BindingElementType
        {
            get { return typeof(ClearTcpBinding); }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                if (_properties == null)
                {
                    var properties = base.Properties;
                    properties.Add(new ConfigurationProperty("maxReceivedMessageSize", typeof(string), "65536"));
                    _properties = properties;
                }
                return _properties;
            }
        }

        public string MaxReceivedMessageSize
        {
            get { return (string)base["maxReceivedMessageSize"]; }
            set { base["maxReceivedMessageSize"] = value; }
        }
    }

    public class ClearTcpCollectionElement
        : StandardBindingCollectionElement<ClearTcpBinding, ClearTcpBindingElement>
    {
    }

    public class AutoSecuredTcpTransportElement : TcpTransportBindingElement, ITransportTokenAssertionProvider
    {
        public override T GetProperty<T>(BindingContext context)
        {
            if (typeof(T) == typeof(ISecurityCapabilities))
                return (T)(ISecurityCapabilities)new AutoSecuredTcpSecurityCapabilities();
            return base.GetProperty<T>(context);
        }

        public System.Xml.XmlElement GetTransportTokenAssertion()
        {
            return null;
        }
    }

    public class AutoSecuredTcpSecurityCapabilities : ISecurityCapabilities
    {
        public ProtectionLevel SupportedRequestProtectionLevel { get { return ProtectionLevel.EncryptAndSign; } }
        public ProtectionLevel SupportedResponseProtectionLevel { get { return ProtectionLevel.EncryptAndSign; } }
        public bool SupportsClientAuthentication { get { return false; } }
        public bool SupportsClientWindowsIdentity { get { return false; } }
        public bool SupportsServerAuthentication { get { return true; } }
    }
}
