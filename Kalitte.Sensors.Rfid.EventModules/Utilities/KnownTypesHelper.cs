﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Rfid.Core;
using Kalitte.Sensors.Rfid.EventModules.Client.TagView;


namespace Kalitte.Sensors.Rfid.EventModules.Utilities
{
    public sealed class KnownTypesHelper
    {
        // Fields
        private static Type[] s_knownTypes;

        // Methods
        static KnownTypesHelper()
        {
            Collection<Type> knownTypes = new Collection<Type>();
            foreach (Type type in TypesHelper.GetKnownTypes(typeof(KnownTypesHelper).Assembly))
            {
                knownTypes.Add(type);
            }
            foreach (Type type in TypesHelper.GetKnownTypes(typeof(TagAppearedEvent).Assembly))
            {
                knownTypes.Add(type);
            }
            s_knownTypes = new Type[knownTypes.Count];
            knownTypes.CopyTo(s_knownTypes, 0);
        }

        private KnownTypesHelper()
        {
        }

        public static IEnumerable<Type> GetKnownTypes()
        {
            return s_knownTypes;
        }
    }
}
