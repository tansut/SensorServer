using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable,  DataContract]
    public class ProcessorProperty : EntityPropertyBase
    {
        [DataMember]  
        public NonExistEventHandlerBehavior ModuleNonExistEventHandlerBehavior { get; set; }

        [DataMember]        
        public PipeNullEventBehavior PipeNullEventBehavior { get; set; }

        [DataMember]
        public LogLevelInformation LogLevel { get; set; }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }

        public ProcessorProperty(PropertyList profile, PropertyList extendedProfile, ItemStartupType startup)
            : base(profile, extendedProfile, startup)
        {
            LogLevel = new LogLevelInformation(true, Security.LogLevel.Error);
        }

        public ProcessorProperty(ItemStartupType startup)
            : base(startup)
        {
            LogLevel = new LogLevelInformation(true, Security.LogLevel.Error);
        }


    }
}
