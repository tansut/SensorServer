using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]
    public class DispatcherProperty : EntityPropertyBase
    {
        [DataMember]
        public LogLevelInformation LogLevel { get; set; }


        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }

        public DispatcherProperty(PropertyList profile, PropertyList extendedProfile, ItemStartupType startup)
            : base(profile, extendedProfile, startup)
        {
            LogLevel = new LogLevelInformation(true, Security.LogLevel.Error);
        }

        public DispatcherProperty(ItemStartupType startup)
            : base(startup)
        {
            LogLevel = new LogLevelInformation(true, Security.LogLevel.Error);
        }
    }
}
