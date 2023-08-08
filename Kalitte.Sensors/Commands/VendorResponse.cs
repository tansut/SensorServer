using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Commands
{
[Serializable]
public sealed class VendorResponse : Response
{
    // Fields
    private readonly string name;

    // Methods
    public VendorResponse(string name, VendorData vendorDefinedReplies)
    {
        this.name = name;
        base.VendorReplies = vendorDefinedReplies;
        this.ValidateParameters();
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<vendorDefinedResponse>");
        builder.Append(base.ToString());
        builder.Append("<name>");
        builder.Append(this.name);
        builder.Append("</name>");
        builder.Append("</vendorDefinedResponse>");
        return builder.ToString();
    }

    private void ValidateParameters()
    {
        if ((this.name == null) || (this.name.Length == 0))
        {
            throw new ArgumentNullException("name");
        }
    }

    [OnDeserialized]
    private void ValidateParameters(StreamingContext context)
    {
        this.ValidateParameters();
    }

    // Properties
    public string Name
    {
        get
        {
            return this.name;
        }
    }
}

 

 

}
