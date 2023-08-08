using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable]
    public class LogicalSensorRuntime
    {
        public static LogicalSensorRuntime Empty
        {
            get
            {
                return new LogicalSensorRuntime();
            }
        }

        public object Clone()
        {
            var serialized = SerializationHelper.SerializeToXmlDataContract(this, false);
            return SerializationHelper.DeserializeFromXmlDataContract<LogicalSensorRuntime>(serialized);
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }
    }
}
