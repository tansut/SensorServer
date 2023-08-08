using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable]
    public class ItemMonitoringData: ICloneable, IEquatable<ItemMonitoringData>
    {
        public bool Enabled { get; set; }
        public int CheckInterval { get; set; }
        public int MaxRetryCount { get; set; }

        public ItemMonitoringData()
        {
            Enabled = false;
            CheckInterval = 5000;
            MaxRetryCount = 0;
        }

        #region ICloneable Members

        public object Clone()
        {
            var copy = new ItemMonitoringData();
            copy.Enabled = this.Enabled;
            copy.CheckInterval = this.CheckInterval;
            copy.MaxRetryCount = this.MaxRetryCount;
            return copy;
        }

        #endregion

        #region IEquatable<RunningStateKeepData> Members

        public bool Equals(ItemMonitoringData other)
        {
            return (this.Enabled == other.Enabled &&
                this.CheckInterval == other.CheckInterval &&
                this.MaxRetryCount == other.MaxRetryCount);
        }

        #endregion
    }
}
