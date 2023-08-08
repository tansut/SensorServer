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
    public class Logical2SensorBindingProperty: EntityPropertyBase
    {
        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }

        public Logical2SensorBindingProperty(PropertyList profile, PropertyList extendedProfile, ItemStartupType startup)
            : base(profile, extendedProfile, startup)
        {
        }

        public Logical2SensorBindingProperty(ItemStartupType startup)
            : base(startup)
        {
        } 
    }
}
