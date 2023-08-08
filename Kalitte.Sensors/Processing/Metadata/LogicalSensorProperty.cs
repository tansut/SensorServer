using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using System.Runtime.Serialization;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable]
    public class LogicalSensorProperty: EntityPropertyBase, ICloneable
    {
        #region ICloneable Members

        public object Clone()
        {
            var serialized = SerializationHelper.SerializeToXmlDataContract(this, false);
            return SerializationHelper.DeserializeFromXmlDataContract<LogicalSensorProperty>(serialized);
        }

        public LogicalSensorProperty(PropertyList profile, PropertyList extendedProfile, ItemStartupType startup)
            : base(profile, extendedProfile, startup)
        {
        }

        public LogicalSensorProperty(ItemStartupType startup)
            : base(startup)
        {
        } 

        #endregion

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }
    }
}
