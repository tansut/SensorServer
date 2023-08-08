using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]
    public class SensorProviderRuntime
    {
        public static SensorProviderRuntime Empty
        {
            get
            {
                return new SensorProviderRuntime();
            }
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }
    }
}
