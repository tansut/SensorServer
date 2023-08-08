using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Kalitte.Sensors.Configuration;
using System.Configuration;
using System.ServiceModel.Description;
using Kalitte.Sensors.Processing;


namespace Kalitte.Sensors.Utilities
{
    public sealed class TypesHelper
    {
        // Fields
        private static Dictionary<Type, Type> s_knownTypeDict;
        private static Type[] s_knownTypes;
        private static Dictionary<Type, Type> s_primitiveKnownTypeDict;
        private static System.Threading.ReaderWriterLock listLock = new System.Threading.ReaderWriterLock();
        private static bool isInited = false;
        private static object initLock = new object();


        // Methods

        private static void checkAndInit()
        {
            lock (initLock)
            {
                if (!isInited)
                    Init();
            }
        }

        public static void Init()
        {
            lock (initLock)
            {
                if (isInited)
                    throw new InvalidOperationException("Types helper already inited");
                Collection<Type> knownTypes = new Collection<Type>();
                AddPrimitivesToKnownTypes(knownTypes);
                AddArraysToKnownTypes(knownTypes);
                AddCurrentDllKnownTypes(knownTypes);
                AddKnownFrameworkTypes(knownTypes);
                AddConfiguredKnownTypesProviders(knownTypes);


                Dictionary<Type, Type> dictionary = new Dictionary<Type, Type>();
                foreach (Type type in knownTypes)
                {
                    dictionary[type] = type;
                }
                s_knownTypes = new Type[knownTypes.Count];
                knownTypes.CopyTo(s_knownTypes, 0);
                s_knownTypeDict = dictionary;
                Collection<Type> collection2 = new Collection<Type>();
                AddPrimitivesToKnownTypes(collection2);
                Dictionary<Type, Type> dictionary2 = new Dictionary<Type, Type>();
                foreach (Type type2 in collection2)
                {
                    dictionary2[type2] = type2;
                }
                s_primitiveKnownTypeDict = dictionary2;
                isInited = true;
            }
        }

        static TypesHelper()
        {


        }


        private static void AddConfiguredKnownTypesProviders(Collection<Type> knownTypes)
        {
            SensorConfigurationSection section = ConfigurationManager.GetSection("KalitteSensorFramework") as SensorConfigurationSection;
            if (section == null)
                return;
            var s_Providers = section.KnownTypesProviders;
            foreach (ProviderSettings provider in s_Providers)
            {
                var result = provider.Parameters["useSeperateAppDomain"];
                if (string.IsNullOrWhiteSpace(result)) result = "False";
                string app = AppDomain.CurrentDomain.BaseDirectory;
                if (result.ToUpperInvariant().Trim() == "FALSE")
                {
                    Type t = Type.GetType(provider.Type);
                    if (t == null)
                        throw new ConfigurationErrorsException(string.Format("Cannot create type {0}", provider.Type));
                    MethodInfo method = t.GetMethod("GetKnownTypes");
                    if (method == null)
                        throw new ConfigurationErrorsException(string.Format("Cannot find GetKnownTypes method on type {0}", provider.Type));
                    IEnumerable<Type> obj = method.Invoke(null, new object[] { }) as IEnumerable<Type>;
                    if (obj == null)
                        throw new ConfigurationErrorsException(string.Format("GetKnownTypes method on type {0} should return IEnumerable<Type>", provider.Type));
                    foreach (var knwoType in obj)
                        knownTypes.Add(knwoType);
                }
                else
                {
                    VirtualMarshal marshal = new VirtualMarshal(provider.Type);
                    try
                    {
                        var listOfTypes = marshal.GetStaticMethodResult<IEnumerable<Type>>("GetKnownTypes", true);
                        foreach (var knwoType in listOfTypes)
                            knownTypes.Add(knwoType);
                    }
                    finally
                    {
                        marshal.Close();
                    }
                }

            }
        }



        private TypesHelper()
        {
        }

        private static void AddArraysToKnownTypes(Collection<Type> knownTypes)
        {
            knownTypes.Add(typeof(bool[]));
            knownTypes.Add(typeof(byte[]));
            knownTypes.Add(typeof(sbyte[]));
            knownTypes.Add(typeof(char[]));
            knownTypes.Add(typeof(short[]));
            knownTypes.Add(typeof(int[]));
            knownTypes.Add(typeof(long[]));
            knownTypes.Add(typeof(double[]));
            knownTypes.Add(typeof(float[]));
            knownTypes.Add(typeof(string[]));
            knownTypes.Add(typeof(uint[]));
            knownTypes.Add(typeof(ushort[]));
            knownTypes.Add(typeof(ulong[]));
            knownTypes.Add(typeof(Collection<object>));
        }


        private static void AddKnownFrameworkTypes(Collection<Type> knownTypes)
        {
            knownTypes.Add(typeof(RegexOptions));
            knownTypes.Add(typeof(DateTime));
        }

        private static void AddPrimitivesToKnownTypes(Collection<Type> knownTypes)
        {
            knownTypes.Add(typeof(bool));
            knownTypes.Add(typeof(byte));
            knownTypes.Add(typeof(sbyte));
            knownTypes.Add(typeof(char));
            knownTypes.Add(typeof(short));
            knownTypes.Add(typeof(int));
            knownTypes.Add(typeof(long));
            knownTypes.Add(typeof(double));
            knownTypes.Add(typeof(float));
            knownTypes.Add(typeof(string));
            knownTypes.Add(typeof(uint));
            knownTypes.Add(typeof(ushort));
            knownTypes.Add(typeof(ulong));
        }

        private static void AddCurrentDllKnownTypes(Collection<Type> knownTypes)
        {
            foreach (Type type in GetKnownTypes(typeof(TypesHelper).Assembly))
            {
                knownTypes.Add(type);
            }
        }

        public static IEnumerable<Type> GetKnownTypeEnumerator()
        {
            checkAndInit();
            return s_knownTypes.Select(p => p).AsEnumerable();
        }

        public static Collection<Type> GetKnownTypes(Assembly assembly)
        {
            Collection<Type> collection = new Collection<Type>();
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsGenericType)
                {
                    if (type.IsEnum)
                    {
                        collection.Add(type);
                    }
                    else if (type.IsSerializable)
                    {
                        collection.Add(type);
                    }
                    else
                    {
                        object[] customAttributes = type.GetCustomAttributes(typeof(DataContractAttribute), false);
                        if ((customAttributes != null) && (customAttributes.Length > 0))
                        {
                            collection.Add(type);
                        }
                    }
                }
            }
            return collection;
        }

        private static Type[] GetKnownTypes(ICustomAttributeProvider knownTypeAttributeTarget)
        {
            checkAndInit();
            return s_knownTypes;
        }

        public static Type GetType(string fullName)
        {
            Type t = Type.GetType(fullName);
            if (t != null)
                return t;
            checkAndInit();
            listLock.AcquireReaderLock(1000);
            try
            {
                return s_knownTypes.FirstOrDefault(p => p.FullName == fullName);
            }
            finally
            {
                listLock.ReleaseReaderLock();
            }

        }

        public static void RegisterKnownType(Type[] typesToRegister)
        {
            checkAndInit();
            listLock.AcquireWriterLock(1000);
            try
            {
                List<Type> temp = new List<Type>(s_knownTypes);
                foreach (Type t in typesToRegister)
                {
                    if (!IsKnownType(t))
                    {
                        temp.Add(t);
                        s_knownTypeDict[t] = t;
                    }

                }
                s_knownTypes = new Type[temp.Count];
                temp.CopyTo(s_knownTypes, 0);

            }
            finally
            {
                listLock.ReleaseWriterLock();
            }
        }

        public static void AddKnownTypes(ServiceEndpoint endpoint)
        {
            checkAndInit();
            listLock.AcquireReaderLock(1000);
            try
            {
                if (((endpoint != null) && (endpoint.Contract != null)) && (endpoint.Contract.Operations != null))
                {
                    foreach (OperationDescription description in endpoint.Contract.Operations)
                    {
                        foreach (Type type in KnownTypes)
                        {
                            //                            if (!IsKnownPrimitiveType(type) &&
                            //    (type.Assembly.GetName().Name.ToLowerInvariant() == "kalitte.sensors" || type.Assembly.GetName().Name.ToLowerInvariant() == "kalitte.sensors.rfid"))
                            //{
                            if (!IsKnownPrimitiveType(type))
                            {
                                description.KnownTypes.Add(type);
                            }
                        }
                    }
                }
            }
            finally
            {
                listLock.ReleaseReaderLock();
            }

        }




        public static bool IsKnownPrimitiveType(Type type)
        {
            checkAndInit();
            return s_primitiveKnownTypeDict.ContainsKey(type);
        }

        public static bool IsKnownType(Type type)
        {
            checkAndInit();
            listLock.AcquireReaderLock(1000);
            try
            {
                var result = s_knownTypeDict.ContainsKey(type);
                return result;
            }
            finally
            {
                listLock.ReleaseReaderLock();
            }

        }


        public static bool IsKnownTypeObject(object obj)
        {
            checkAndInit();
            if (obj != null)
            {
                return IsKnownType(obj.GetType());
            }
            return true;
        }

        public static IEnumerable<Type> GetCurrentAssemblyTypes(Type baseType)
        {
            Assembly currentAss = Assembly.GetExecutingAssembly();
            Type[] types = currentAss.GetTypes();

            List<Type> result = new List<Type>();
            foreach (var type in types)
            {
                if (baseType != type && baseType.IsAssignableFrom(type))
                    result.Add(type);
            }

            return result;
        }

        public static IEnumerable<Type> GetTypes(Type baseType)
        {
            checkAndInit();
            listLock.AcquireReaderLock(1000);
            List<Type> result = new List<Type>();

            try
            {
                foreach (var type in s_knownTypeDict)

                    if (baseType != type.Key && baseType.IsAssignableFrom(type.Key))
                        result.Add(type.Key);

            }
            finally
            {
                listLock.ReleaseReaderLock();
            }

            return result;
        }

        public static bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                    return true;
            }
            return false;
        }

        public static bool IsSerializable(Type t)
        {
            return t.IsSerializable;
        }


        // Properties
        public static Type[] KnownTypes
        {
            get
            {
                checkAndInit();
                listLock.AcquireReaderLock(1000);
                try
                {
                    //foreach (var t in s_knownTypes)
                    //{
                    //    if (t.Name.Contains("C1G2EpcMemorySelector"))
                    //    {
                    //        int j = 12;
                    //    }
                    //}
                    return s_knownTypes;
                }
                finally
                {
                    listLock.ReleaseReaderLock();
                }
            }
        }
    }





}
