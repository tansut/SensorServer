using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract, KnownType("GetSubTypes")]
    public abstract class EntityPropertyBase
    {
        private ItemMonitoringData monitoringData = null;

        private static IEnumerable<Type> GetSubTypes()
        {
            return TypesHelper.GetTypes(typeof(EntityPropertyBase));
        }

        [DataMember(IsRequired = false)]
        public ItemMonitoringData MonitoringData
        {
            get
            {
                if (monitoringData == null)
                    monitoringData = new ItemMonitoringData();
                return monitoringData;
            }
            set
            {
                monitoringData = value;
            }
        }

        [DataMember]
        public ItemStateInfo StateInfo { get; set; }

        [DataMember]
        public ItemStartupType Startup { get; set; }

        [DataMember]
        public PropertyList Profile { get; set; }

        [DataMember]
        public PropertyList ExtendedProfile { get; set; }

        public virtual void ResetState()
        {
            StateInfo = new ItemStateInfo(ItemState.Stopped);
        }

        public EntityPropertyBase(ItemStartupType startup)
        {
            ResetState();
            this.Startup = startup;
            this.Profile = new PropertyList();
            this.ExtendedProfile = new PropertyList();
            MonitoringData = new ItemMonitoringData();
        }

        public EntityPropertyBase(PropertyList profile, PropertyList extendedProfile, ItemStartupType startup)
        {
            ResetState();
            this.Startup = startup;
            this.Profile = profile;
            this.ExtendedProfile = extendedProfile;
            MonitoringData = new ItemMonitoringData();

        }
    }
}
