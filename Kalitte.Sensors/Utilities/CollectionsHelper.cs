using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using System.Collections.ObjectModel;

namespace Kalitte.Sensors.Utilities
{
    public sealed class CollectionsHelper
    {

        public static bool CompareArrays<T>(T[] a1, T[] a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }

        private static bool CompareArrays(object[] value, object[] value2)
        {
            if (value != null)
            {
                if (value2 != null)
                {
                    if (value.Length != value2.Length)
                    {
                        return false;
                    }
                    int index = 0;
                    foreach (object obj2 in value)
                    {
                        if (obj2 != null)
                        {
                            if (value2[index] == null)
                            {
                                return false;
                            }
                            if (obj2.GetType().IsArray)
                            {
                                if (value2[index].GetType().IsArray)
                                {
                                    if (!CompareArrays((object[])obj2, (object[])value2[index]))
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            if (!obj2.Equals(value2[index]))
                            {
                                return false;
                            }
                        }
                        else if (value2[index] != null)
                        {
                            return false;
                        }
                        index++;
                    }
                    return true;
                }
            }
            else if (value2 == null)
            {
                return true;
            }
            return false;
        }

        public static bool CompareDictionaries(VendorData dictionary1, VendorData dictionary2)
        {
            if (dictionary1 != null)
            {
                if (dictionary2 != null)
                {
                    if (dictionary1.Count != dictionary2.Count)
                    {
                        return false;
                    }
                    foreach (KeyValuePair<string, object> pair in dictionary1)
                    {
                        if (dictionary2.ContainsKey(pair.Key))
                        {
                            object obj2 = dictionary2[pair.Key];
                            if (pair.Value != null)
                            {
                                if (obj2 == null)
                                {
                                    return false;
                                }
                                if (pair.Value.GetType().IsArray)
                                {
                                    if (obj2.GetType().IsArray)
                                    {
                                        if (!CompareArrays((object[])pair.Value, (object[])obj2))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                if (!pair.Value.Equals(obj2))
                                {
                                    return false;
                                }
                                continue;
                            }
                            if (obj2 == null)
                            {
                                continue;
                            }
                        }
                        return false;
                    }
                    return true;
                }
            }
            else if (dictionary2 == null)
            {
                return true;
            }
            return false;
        }

        //public static object[] CreateArrayFromCollection(Collection<object> paramsCollection)
        //{
        //    if (paramsCollection == null)
        //    {
        //        return null;
        //    }
        //    object[] array = new object[paramsCollection.Count];
        //    paramsCollection.CopyTo(array, 0);
        //    return array;
        //}

        public static Collection<object> CreateParamsCollectionFromArray(object[] parameters)
        {
            if (parameters == null)
            {
                return null;
            }
            Collection<object> collection = new Collection<object>();
            foreach (object obj2 in parameters)
            {
                collection.Add(obj2);
            }
            return collection;
        }

        public static byte[] CloneByte(byte[] value)
        {
            if (value == null)
            {
                return null;
            }
            return (byte[])value.Clone();
        }
    }
}
