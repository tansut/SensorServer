using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Rfid.Utilities;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Security;
using Kalitte.Sensors.Rfid.Core;

namespace Kalitte.Sensors.Rfid.Utilities
{
    public sealed class DuplicateEliminationHandler : IDuplicateEliminationHandler
    {
        // Fields
        private readonly ILogger Logger;
        private long m_dupElimTimeInMillis;
        private Dictionary<TagIdKey, DateTime> m_events;
        private DateTime m_lastShrinkTime;
        private DateTime m_latestEventTime;
        private readonly object m_lock;
        private readonly int m_maxEventsToHold;
        private readonly bool m_useTagReceivedTime;

        

        // Methods
        public DuplicateEliminationHandler(long dupElimMillis, bool useTagReceivedTime, ILogger logger)
        {
            this.m_events = new Dictionary<TagIdKey, DateTime>();
            this.m_lock = new object();
            this.m_latestEventTime = DateTime.MinValue;
            this.m_lastShrinkTime = DateTime.MinValue;
            this.m_maxEventsToHold = 100;
            ValidateDupElimTime(dupElimMillis);
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            this.Logger = logger;
            this.m_dupElimTimeInMillis = dupElimMillis;
            this.m_useTagReceivedTime = useTagReceivedTime;
        }

        public DuplicateEliminationHandler(long dupElimMillis, bool useTagReceivedTime, int maxEventsToHold, ILogger logger)
            : this(dupElimMillis, useTagReceivedTime, logger)
        {
            if (maxEventsToHold < 0)
            {
                throw new ArgumentOutOfRangeException("maxEventsToHold");
            }
            this.m_maxEventsToHold = maxEventsToHold;
        }

        public void FilterDuplicates(TagListEvent tle)
        {
            if (tle != null)
            {
                lock (this.m_lock)
                {
                    List<TagReadEvent> tags = new List<TagReadEvent>();
                    foreach (TagReadEvent event2 in tle.Tags)
                    {
                        if (!this.IsDuplicate(event2))
                        {
                            tags.Add(event2);
                        }
                    }
                    tle.SetTags(tags);
                }
            }
        }

        public bool IsDuplicate(TagReadEvent tre)
        {
            if (this.m_dupElimTimeInMillis == 0L)
            {
                if (this.Logger.CurrentLevel == LogLevel.Verbose)
                {
                    this.Logger.Verbose("DuplicateEliminationHandler: Tag received not being considered for filtering because the duplicate elimination interval is set to 0.");
                }
                return false;
            }
            byte[] id = tre.GetId();
            if (!TagIdKey.IsValidId(id))
            {
                this.Logger.Verbose("DuplicateEliminationHandler: Tag with an invalid Id received. It won't be considered a duplicate.");
                return false;
            }
            bool flag = false;
            DateTime tagTime = this.m_useTagReceivedTime ? DateTime.Now : tre.Time;
            if (tagTime > this.m_latestEventTime)
            {
                this.m_latestEventTime = tagTime;
            }
            TagIdKey key = new TagIdKey(id);
            lock (this.m_lock)
            {
                if (this.m_events.ContainsKey(key) && this.IsDuplicate(tagTime, this.m_events[key]))
                {
                    flag = true;
                }
                else
                {
                    this.UpdateEventsTableIfLaterTimestamp(key, tagTime);
                }
            }
            if (this.Logger.CurrentLevel == LogLevel.Verbose)
            {
                this.Logger.Verbose("DuplicateEliminationHandler: Checking Id {0}: isDuplicate = {1}", new object[] { HexHelper.HexEncode(id), flag });
            }
            return flag;
        }

        private bool IsDuplicate(DateTime tagTime, DateTime lastSeenTime)
        {
            TimeSpan span = (TimeSpan)(tagTime - lastSeenTime);
            return (Math.Abs(span.TotalMilliseconds) < this.m_dupElimTimeInMillis);
        }

        private void ShrinkIfRequired()
        {
            if (this.m_events.Count > this.m_maxEventsToHold)
            {
                TimeSpan span = (TimeSpan)(DateTime.Now - this.m_lastShrinkTime);
                long totalMilliseconds = (long)span.TotalMilliseconds;
                if (totalMilliseconds > this.m_dupElimTimeInMillis)
                {
                    this.m_lastShrinkTime = DateTime.Now;
                    this.Logger.Info("Attempting to shrink the list. Current list size = {0}", new object[] { this.m_events.Count });
                    Collection<TagIdKey> collection = new Collection<TagIdKey>();
                    foreach (TagIdKey key in this.m_events.Keys)
                    {
                        if (!this.IsDuplicate(this.m_latestEventTime, this.m_events[key]))
                        {
                            collection.Add(key);
                        }
                    }
                    foreach (TagIdKey key2 in collection)
                    {
                        this.m_events.Remove(key2);
                    }
                    this.Logger.Info("List size after shrinking: {0}", new object[] { this.m_events.Count });
                }
            }
        }

        private void UpdateEventsTableIfLaterTimestamp(TagIdKey key, DateTime tagTime)
        {
            if (!this.m_events.ContainsKey(key) || (tagTime >= this.m_events[key]))
            {
                this.m_events[key] = tagTime;
                this.ShrinkIfRequired();
            }
        }

        private static void ValidateDupElimTime(long dupElimMillis)
        {
            if (dupElimMillis < 0L)
            {
                throw new ArgumentOutOfRangeException("dupElimMillis");
            }
        }

        // Properties
        public long DupElimTimeInMillis
        {
            get
            {
                lock (this.m_lock)
                {
                    return this.m_dupElimTimeInMillis;
                }
            }
            set
            {
                lock (this.m_lock)
                {
                    ValidateDupElimTime(value);
                    this.m_dupElimTimeInMillis = value;
                }
            }
        }

        internal int EventCount
        {
            get
            {
                return this.m_events.Count;
            }
        }
    }




}
