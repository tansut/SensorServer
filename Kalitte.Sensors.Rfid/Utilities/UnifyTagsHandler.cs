using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Rfid.Core;

namespace Kalitte.Sensors.Rfid.Utilities
{
    public enum UnifiyTagSelectStrategy
    {
        SelectFirstTag,
        SelectLastTag
    }

    public sealed class UnifyTagsHandler
    {
        public UnifiyTagSelectStrategy TagSelectStrategy { get; private set; }
        private readonly object m_lock;
        private Dictionary<TagIdKey, DateTime> m_events;
        private List<TagReadEvent> tagList;

        public UnifyTagsHandler(IList<TagReadEvent> tagList)
            : this(UnifiyTagSelectStrategy.SelectLastTag, tagList)
        {

        }

        public UnifyTagsHandler(UnifiyTagSelectStrategy strategy, IList<TagReadEvent> tagList)
        {
            this.TagSelectStrategy = strategy;
            m_lock = new object();
            m_events = new Dictionary<TagIdKey, DateTime>();
            this.tagList = new List<TagReadEvent>(tagList);
        }


        public IList<TagReadEvent> FilterDuplicates()
        {

            lock (this.m_lock)
            {
                List<TagReadEvent> tags = new List<TagReadEvent>();
                foreach (TagReadEvent event2 in tagList)
                {
                    if (!this.IsDuplicate(event2))
                    {
                        tags.Add(event2);
                    }
                    else if (TagSelectStrategy == UnifiyTagSelectStrategy.SelectLastTag)
                    {
                        TagIdKey keyToSearch = new TagIdKey(event2.GetId());
                        var firstItemIndex = tags.FindIndex((item) => { var tagIdOfItem = new TagIdKey(item.GetId()); return keyToSearch.Equals(tagIdOfItem); });
                        tags[firstItemIndex] = event2;
                    }
                }
                return tags;
            }

        }

        public bool IsDuplicate(Events.TagReadEvent tre)
        {
            TagIdKey key = new TagIdKey(tre.GetId());
            if (m_events.ContainsKey(key))
                return true;
            else
            {
                m_events.Add(key, DateTime.Now);
                return false;
            }
        }
    }
}
