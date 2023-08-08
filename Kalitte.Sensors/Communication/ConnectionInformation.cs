using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Communication
{
[Serializable]
public sealed class ConnectionInformation
{
    // Fields
    private readonly string provider;
    private readonly TransportSettings transportSettings;

    // Methods
    public ConnectionInformation(string provider, TransportSettings transportSettings)
    {
        this.provider = provider;
        this.transportSettings = transportSettings;
        this.ValidateParameters();
    }

    public override bool Equals(object obj)
    {
        ConnectionInformation information = obj as ConnectionInformation;
        if (information == null)
        {
            return false;
        }
        return ((((this.provider != null) && this.provider.Equals(information.provider)) && (this.transportSettings != null)) && this.transportSettings.Equals(information.transportSettings));
    }

    public override int GetHashCode()
    {
        if ((this.provider != null) && (this.transportSettings != null))
        {
            return (this.provider.GetHashCode() * this.transportSettings.GetHashCode());
        }
        return 0;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<connectionInformation>");
        builder.Append("<provider>");
        builder.Append(this.provider);
        builder.Append("</provider>");
        builder.Append("<transportSettings>");
        builder.Append(this.transportSettings);
        builder.Append("</transportSettings>");
        builder.Append("</connectionInformation>");
        return builder.ToString();
    }

    private void ValidateParameters()
    {
        if ((this.provider == null) || (this.provider.Length == 0))
        {
            throw new ArgumentNullException("provider");
        }
        if (this.transportSettings == null)
        {
            throw new ArgumentNullException("transportSettings");
        }
    }

    [OnDeserialized]
    private void ValidateParameters(StreamingContext context)
    {
        this.ValidateParameters();
    }

    // Properties
    public string Provider
    {
        get
        {
            return this.provider;
        }
    }

    public TransportSettings TransportSettings
    {
        get
        {
            return this.transportSettings;
        }
    }
}

 
 

}
