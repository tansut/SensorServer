using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Rfid.Commands
{
[Serializable]
public sealed class FieldIncrementInfo
{
    // Fields
    private readonly long m_endPosition;
    private readonly long m_increment;
    private readonly DataFormat m_incrementFormat;
    private readonly long m_startPosition;

    // Methods
    public FieldIncrementInfo(long increment, long startPos, long endPos, DataFormat incrementFormat)
    {
        this.m_increment = increment;
        this.m_startPosition = startPos;
        this.m_endPosition = endPos;
        this.m_incrementFormat = incrementFormat;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<fieldIncrementInfo>");
        builder.Append("<increment>");
        builder.Append(this.Increment);
        builder.Append("</increment>");
        builder.Append("<startPosition>");
        builder.Append(this.StartPosition);
        builder.Append("</startPosition>");
        builder.Append("<endPosition>");
        builder.Append(this.EndPosition);
        builder.Append("</endPosition>");
        builder.Append("<incrementFormat>");
        builder.Append(this.IncrementFormat);
        builder.Append("</incrementFormat>");
        builder.Append("</fieldIncrementInfo>");
        return builder.ToString();
    }

    // Properties
    public long EndPosition
    {
        get
        {
            return this.m_endPosition;
        }
    }

    public long Increment
    {
        get
        {
            return this.m_increment;
        }
    }

    public DataFormat IncrementFormat
    {
        get
        {
            return this.m_incrementFormat;
        }
    }

    public long StartPosition
    {
        get
        {
            return this.m_startPosition;
        }
    }
}


 

}
