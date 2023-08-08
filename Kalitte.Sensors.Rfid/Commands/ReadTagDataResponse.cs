using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Utilities;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Rfid.Commands
{
[Serializable]
public sealed class ReadTagDataResponse : Response
{
    // Fields
    private readonly byte[] tagData;

    // Methods
    public ReadTagDataResponse(byte[] tagData)
    {
        this.tagData = tagData;
    }

    public byte[] GetTagData()
    {
        return CollectionsHelper.CloneByte(this.tagData);
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<getTagDataResponse>");
        builder.Append("<tagData>");
        builder.Append(this.tagData);
        builder.Append("</tagData>");
        builder.Append("</getTagDataResponse>");
        return builder.ToString();
    }
}


 

}
