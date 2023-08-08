using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing
{
    public class VirtualMarshal
    {
        AppDomain Domain;
        MarshalLoader loader;
        public VirtualMarshal(string type)
        {
            Type t = typeof(MarshalLoader);
            Domain = MarshalHelper.CreateAppDomanin("temp");
            loader = (MarshalLoader)Domain.CreateInstanceAndUnwrap(
                t.Assembly.FullName,
                t.FullName, false, System.Reflection.BindingFlags.CreateInstance,
                null, new object[] { type }, null, null);
        }

        public T GetStaticMethodResult<T>(string methodName, bool throwIfNotMethodExists, params object[] parameters)
        {
            return loader.GetStaticMethodResult<T>(methodName, throwIfNotMethodExists, parameters);
        }

        public void Close()
        {
            if (loader is IDisposable)
                ((IDisposable)loader).Dispose();
            AppDomain.Unload(Domain);
        }

    }
}
