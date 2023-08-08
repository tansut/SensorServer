using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kalitte.Sensors.Rfid.Utilities;
using Kalitte.Sensors.Rfid.Core;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Rfid.Events
{
[Serializable]
public sealed class TagReadEvent : RfidObservation
{
    // Fields
    private readonly byte[] data;
    private readonly TagDataSelector dataSelector;
    private readonly byte[] id;
    private readonly byte[] numberingSystemIdentifier;
    public const string ReadCount = "Read count";
    public const string Rssi = "rssi";
    public const string LastSeen = "Last seen";
    private readonly TagType type;

    // Methods
    public TagReadEvent(byte[] tagId, TagType tagType, byte[] tagData, string tagSource, DateTime tagTime, byte[] numberingSystemIdentifier, TagDataSelector dataSelector)
    {
        this.id = tagId;
        this.type = tagType;
        this.data = tagData;
        base.Source = tagSource;
        base.Time = tagTime;
        this.numberingSystemIdentifier = numberingSystemIdentifier;
        this.dataSelector = dataSelector;
    }

    public byte[] GetData()
    {
        return CollectionsHelper.CloneByte(this.data);
    }

    public byte[] GetId()
    {
        return CollectionsHelper.CloneByte(this.id);
    }

    public byte[] GetNumberingSystemIdentifier()
    {
        return CollectionsHelper.CloneByte(this.numberingSystemIdentifier);
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<tag>");
        builder.Append(base.ToString());
        if (this.id != null)
        {
            builder.Append("<tagId>");
            //builder.Append(Convert.ToBase64String(this.id));
            builder.Append(HexHelper.HexEncode(this.id));
            builder.Append("</tagId>");
        }
        builder.Append("<tagType>");
        builder.Append(this.type);
        builder.Append("</tagType>");
        if (this.data != null)
        {
            builder.Append("<tagData>");
            //builder.Append(Convert.ToBase64String(this.data));
            builder.Append(HexHelper.HexEncode(this.data));
            builder.Append("</tagData>");
        }
        builder.Append("<tagSource>");
        builder.Append(base.Source);
        builder.Append("</tagSource>");
        builder.Append("<tagTime>");
        builder.Append(base.Time);
        builder.Append("</tagTime>");
        builder.Append("<dataSelector>");
        builder.Append(this.dataSelector);
        builder.Append("</dataSelector>");
        builder.Append("</tag>");
        return builder.ToString();
    }

    // Properties
    public TagDataSelector DataSelector
    {
        get
        {
            return this.dataSelector;
        }
    }

    public TagType Type
    {
        get
        {
            return this.type;
        }
    }
}

 

 

}
