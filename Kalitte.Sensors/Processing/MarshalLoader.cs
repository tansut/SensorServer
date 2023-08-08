using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing
{
    internal class MarshalLoader: MarshalBase
    {
        private Type loadedType;
        private Assembly loadedAssembly;

        public MarshalLoader(string type)
        {
            TypeParser parser = new TypeParser(type);
            loadedAssembly = Assembly.Load(parser.AssemblyName);
            loadedType = loadedAssembly.GetType(parser.Type);
            if (loadedType == null)
                throw new ArgumentException("Unable to load type", type);
        }


        internal T GetStaticMethodResult<T>(string methodName, bool throwIfNotMethodExists, object[] parameters)
        {
            MethodInfo method = loadedType.GetMethod(methodName);
            if (method == null)
            {
                if (throwIfNotMethodExists)
                    throw new ArgumentException("Unable to get method using type " + loadedType.FullName , methodName);
                else return default(T);
            }
            else return (T)method.Invoke(method, parameters);
        }
    }
}
