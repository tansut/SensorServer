using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]
    public class Logical2ProcessorBindingRuntime
    {
        public static Logical2ProcessorBindingRuntime Empty
        {
            get
            {
                return new Logical2ProcessorBindingRuntime();
            }
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }
    }
}
