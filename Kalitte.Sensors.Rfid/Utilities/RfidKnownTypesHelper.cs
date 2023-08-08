using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Rfid.Core;


namespace Kalitte.Sensors.Rfid.Utilities
{
    public sealed class RfidKnownTypesHelper
    {
        // Fields
        private static Type[] s_knownTypes;

        // Methods
        static RfidKnownTypesHelper()
        {
            Collection<Type> knownTypes = new Collection<Type>();
            foreach (Type type in TypesHelper.GetKnownTypes(typeof(RfidKnownTypesHelper).Assembly))
            {
                knownTypes.Add(type);
            }
            AddCeSupportedKnownTypes(knownTypes);
            s_knownTypes = new Type[knownTypes.Count];
            knownTypes.CopyTo(s_knownTypes, 0);
        }

        private RfidKnownTypesHelper()
        {
        }


        private static void AddCeSupportedKnownTypes(Collection<Type> types)
        {
            types.Add(typeof(TagDataSelector));
            types.Add(typeof(RfidSourceType));
        }

        public static IEnumerable<Type> GetKnownTypes()
        {
            return s_knownTypes;
        }
    }





}
