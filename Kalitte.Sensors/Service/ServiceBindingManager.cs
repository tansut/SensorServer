using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Xml;

namespace Kalitte.Sensors.Service
{
    public class ServiceBindingManager
    {

        private static Binding GetDefaultTcpBinding()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.MaxConnections = 0x400;
            binding.ReliableSession.Enabled = false;
            binding.TransactionFlow = false;
            binding.MaxReceivedMessageSize = 0x7fffffffL;
            binding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
            return binding;
        }

        private static Binding GetDefaultClearTcpBinding()
        {
            ClearTcpBinding binding = new ClearTcpBinding();
            return binding;
        }

        public static Binding GetDefaultBinding()
        {
            return GetDefaultClearTcpBinding();
        }
    }
}
