using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Utilities
{
    public sealed class ServerKnownTypesHelper
    {
        private static Collection<Type> s_knownTypes = new Collection<Type>();

        static ServerKnownTypesHelper()
        {
            s_knownTypes = TypesHelper.GetKnownTypes(typeof(ServerKnownTypesHelper).Assembly);
        }

        public static IEnumerable<Type> GetKnownTypes()
        {
            return s_knownTypes;
        }

    }
}
