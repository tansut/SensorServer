using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Rfid.Events
{
[Serializable]
public sealed class TagListEvent : RfidObservation
{
    // Fields
    private List<TagReadEvent> m_tags;

    // Methods
    public TagListEvent(IList<TagReadEvent> tags) : this(tags, null)
    {
    }

    public TagListEvent(IList<TagReadEvent> tags, string sourceName)
    {
        base.Source = sourceName;
        this.m_tags = new List<TagReadEvent>(tags);
        this.ValidateParameters();
    }

    public void SetTags(List<TagReadEvent> tags)
    {
        if (tags == null)
        {
            throw new ArgumentNullException("tags");
        }
        this.m_tags = tags;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<tagListEvent>");
        builder.Append(base.ToString());
        if (this.m_tags != null)
        {
            builder.Append("<tags>");
            foreach (TagReadEvent event2 in this.m_tags)
            {
                if (event2 != null)
                {
                    builder.Append(event2.ToString());
                }
            }
            builder.Append("</tags>");
        }
        builder.Append("</tagListEvent>");
        return builder.ToString();
    }

    private void ValidateParameters()
    {
        if (this.m_tags == null)
        {
            throw new ArgumentNullException("tags");
        }
        foreach (TagReadEvent event2 in this.m_tags)
        {
            if (!(event2.Source == base.Source) && (event2.Source != null))
            {
                throw new ArgumentException("TagListEventTagsSameSource");
            }
        }
    }

    [OnDeserialized]
    private void ValidateParameters(StreamingContext context)
    {
        this.ValidateParameters();
    }

    // Properties
    public override string DeviceName
    {
        get
        {
            return base.DeviceName;
        }
        set
        {
            base.DeviceName = value;
            if (this.m_tags != null)
            {
                foreach (TagReadEvent event2 in this.m_tags)
                {
                    if (event2 != null)
                    {
                        event2.DeviceName = value;
                    }
                }
            }
        }
    }

    public ReadOnlyCollection<TagReadEvent> Tags
    {
        get
        {
            if (this.m_tags != null)
            {
                return new ReadOnlyCollection<TagReadEvent>(this.m_tags);
            }
            return null;
        }
    }
}

 

 

}
