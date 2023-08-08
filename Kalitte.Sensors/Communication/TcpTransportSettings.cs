using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Communication
{
    [Serializable]
    public sealed class TcpTransportSettings : TransportSettings
    {
        // Fields
        private readonly string host;
        private readonly int port;

        // Methods
        public TcpTransportSettings(string host, int port)
        {
            this.host = host;
            this.port = port;
            this.ValidateParameters();
        }

        public override bool Equals(object obj)
        {
            TcpTransportSettings settings = obj as TcpTransportSettings;
            if (settings == null)
            {
                return false;
            }
            return (((base.Equals(settings) && (this.host != null)) && this.host.Equals(settings.host)) && (this.port == settings.port));
        }

        public override int GetHashCode()
        {
            if (this.host == null)
            {
                return 0;
            }
            return (this.host.GetHashCode() * this.port);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<tcpTransportSettings>");
            builder.Append("<host>");
            builder.Append(this.host);
            builder.Append("</host>");
            builder.Append("<port>");
            builder.Append(this.port);
            builder.Append("</port>");
            builder.Append("</tcpTransportSettings>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.host == null) || (this.host.Length == 0))
            {
                throw new ArgumentNullException("host");
            }
            if (0 >= this.port)
            {
                throw new ArgumentException("InvalidPort");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public string Host
        {
            get
            {
                return this.host;
            }
        }

        public int Port
        {
            get
            {
                return this.port;
            }
        }
    }



}
