using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Rfid.Commands
{
[Serializable]
public sealed class VendorDefinedTagCommand : TagCommand
{
    // Fields
    private readonly string name;
    private VendorResponse response;
    private readonly string vendorCommand;

    // Methods
    public VendorDefinedTagCommand(byte[] passCode, string name, string command, VendorData parameters) : base(passCode)
    {
        this.name = name;
        this.vendorCommand = command;
        base.VendorDefinedParameters = parameters;
        this.ValidateParameters();
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<vendorDefinedTagCommand>");
        builder.Append(base.ToString());
        builder.Append("<name>");
        builder.Append(this.name);
        builder.Append("</name>");
        builder.Append("<command>");
        builder.Append(this.vendorCommand);
        builder.Append("</command>");
        builder.Append("<response>");
        builder.Append(this.response);
        builder.Append("</response>");
        builder.Append("</vendorDefinedTagCommand>");
        return builder.ToString();
    }

    private void ValidateParameters()
    {
        if ((this.name == null) || (this.name.Length == 0))
        {
            throw new ArgumentNullException("name");
        }
        if ((this.vendorCommand == null) || (this.vendorCommand.Length == 0))
        {
            throw new ArgumentNullException("command");
        }
    }

    [OnDeserialized]
    private void ValidateParameters(StreamingContext context)
    {
        this.ValidateParameters();
    }

    // Properties
    public string Command
    {
        get
        {
            return this.vendorCommand;
        }
    }

    public string Name
    {
        get
        {
            return this.name;
        }
    }

    public VendorResponse Response
    {
        get
        {
            return this.response;
        }
        set
        {
            this.response = value;
        }
    }
}

 

 

}
