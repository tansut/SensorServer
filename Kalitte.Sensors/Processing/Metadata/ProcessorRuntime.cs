using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]
    public class ProcessorRuntime
    {
        public static ProcessorRuntime Empty
        {
            get
            {
                return new ProcessorRuntime();
            }
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }
    }
}
