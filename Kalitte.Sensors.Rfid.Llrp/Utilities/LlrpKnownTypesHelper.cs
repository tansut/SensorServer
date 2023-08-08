namespace Kalitte.Sensors.Rfid.Llrp.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Core;

    public sealed class LlrpKnownTypesHelper
    {
        private static Collection<Type> s_knownTypes = new Collection<Type>();

        static LlrpKnownTypesHelper()
        {
            s_knownTypes = TypesHelper.GetKnownTypes(typeof(LlrpKnownTypesHelper).Assembly);
        }

        public static IEnumerable<Type> GetKnownTypes()
        {
            return s_knownTypes;
        }

    }
}
