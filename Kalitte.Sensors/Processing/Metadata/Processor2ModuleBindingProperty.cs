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
    public class Processor2ModuleBindingProperty: EntityPropertyBase
    {
        [DataMember]
        public bool InheritNonExistEventHandlerBehavior { get; set; }

        [DataMember]
        public NonExistEventHandlerBehavior ModuleNonExistEventHandlerBehavior { get; set; }

        [DataMember]
        public bool InheritNullEventBehaviorBehavior { get; set; }

        [DataMember]
        public PipeNullEventBehavior ModuleNullEventBehavior { get; set; }
        

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }

        public Processor2ModuleBindingProperty(PropertyList profile, PropertyList extendedProfile, ItemStartupType startup)
            : base(profile, extendedProfile, startup)
        {
            InheritNonExistEventHandlerBehavior = true;
            InheritNullEventBehaviorBehavior = true;
        }

        public Processor2ModuleBindingProperty(ItemStartupType startup)
            : base(startup)
        {
            InheritNonExistEventHandlerBehavior = true;
            InheritNullEventBehaviorBehavior = true;
        }  
    }
}
