using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Processing
{
    [Serializable]
    public class ItemStateInfo
    {
        [DataMember]
        public ItemState State { get; set; }

        [DataMember]
        public LastException LastException { get; private set; }

        [DataMember]
        public string StateText { get; set; }

        public ItemStateInfo(ItemState status)
        {
            this.StateText = string.Empty;
            this.State = status;
        }

        public static ItemStateInfo Stopped
        {
            get
            {
                return new ItemStateInfo(ItemState.Stopped);
            }
        }

        public static ItemStateInfo Running
        {
            get
            {
                return new ItemStateInfo(ItemState.Running);
            }
        }

        public ItemStateInfo(ItemState status, string userStatus)
        {
            this.StateText = userStatus;
            this.State = status;
        }

        public ItemStateInfo(Exception lastException, string userStatus)
        {
            LastException = new Processing.LastException(lastException);
            this.StateText = userStatus;
            State = ItemState.Stopped;
        }

        public ItemStateInfo(Exception lastException)
        {
            LastException = new Processing.LastException(lastException);
            State = ItemState.Stopped;
            this.StateText = lastException.Message;
        }

        public void ClearException()
        {
            LastException = null;
        }

        public virtual void ClearStatus()
        {
            ClearException();
            StateText = string.Empty;
        }
    }
}
