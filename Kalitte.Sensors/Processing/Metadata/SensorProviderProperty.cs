using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]
    public class SensorProviderProperty : EntityPropertyBase
    {
        private ProviderDiscoveryBehaviour discoveryBehaviour;


        [DataMember(IsRequired = false)]
        public ProviderDiscoveryBehaviour DiscoveryBehavior
        {
            get
            {
                if (discoveryBehaviour == null)
                    discoveryBehaviour = new ProviderDiscoveryBehaviour();
                return discoveryBehaviour;
            }
            set { discoveryBehaviour = value; }
        }


        [DataMember]
        public LogLevelInformation LogLevel { get; set; }

        public SensorProviderProperty(PropertyList profile, PropertyList extendedProfile, ItemStartupType startup)
            : base(profile, extendedProfile, startup)
        {
            LogLevel = new LogLevelInformation(true, Security.LogLevel.Error);
        }

        public SensorProviderProperty(ItemStartupType startup)
            : base(startup)
        {
            LogLevel = new LogLevelInformation(true, Security.LogLevel.Error);
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }
    }
}
