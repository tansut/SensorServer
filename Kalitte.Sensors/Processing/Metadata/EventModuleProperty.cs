using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable]
    public class EventModuleProperty: EntityPropertyBase
    {
        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }

        public EventModuleProperty(PropertyList profile, PropertyList extendedProfile, ItemStartupType startup)
            : base(profile, extendedProfile, startup)
        {
        }

        public EventModuleProperty(ItemStartupType startup)
            : base(startup)
        {
        }
    }
}
