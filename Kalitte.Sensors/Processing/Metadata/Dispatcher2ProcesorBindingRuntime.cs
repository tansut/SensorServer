using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]
    public class Dispatcher2ProcesorBindingRuntime
    {
        public static Dispatcher2ProcesorBindingRuntime Empty
        {
            get
            {
                return new Dispatcher2ProcesorBindingRuntime();
            }
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }
    }
}
