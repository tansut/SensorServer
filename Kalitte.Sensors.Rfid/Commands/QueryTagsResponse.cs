namespace Kalitte.Sensors.Rfid.Commands
{
    
    using System;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class QueryTagsResponse : Response
    {
        private readonly Collection<TagReadEvent> tags;

        public QueryTagsResponse(Collection<TagReadEvent> tags)
        {
            this.tags = tags;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<queryTagsResponse>");
            builder.Append("<tags>");
            if (this.tags != null)
            {
                foreach (TagReadEvent event2 in this.tags)
                {
                    builder.Append(event2);
                }
            }
            builder.Append("</tags>");
            builder.Append("</queryTagsResponse>");
            return builder.ToString();
        }

        public ReadOnlyCollection<TagReadEvent> Tags
        {
            get
            {
                if (this.tags != null)
                {
                    return new ReadOnlyCollection<TagReadEvent>(this.tags);
                }
                return null;
            }
        }
    }
}
