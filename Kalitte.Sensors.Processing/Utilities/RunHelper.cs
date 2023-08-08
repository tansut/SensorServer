using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;

namespace Kalitte.Sensors.Processing.Utilities
{
    public static class RunHelper
    {
        private class CallResult
        {
            public object[] parameters;
            public object result;
            public object objectTobeInvoke;
            public MethodInfo method;
            public System.Exception exc;

            public void RunInOtherThread(object state)
            {
                CallResult result = (CallResult)state;
                try
                {
                    result.result = method.Invoke(result.objectTobeInvoke, result.parameters);
                }
                catch (TargetInvocationException exc)
                {
                    result.exc = exc.InnerException;
                }
                catch (System.Exception exc)
                {
                    result.exc = exc;
                }
            }
        }

        public static object Execute(object instance, MethodInfo methodInfo, int timeOut, params object[] param)
        {
            var callResult = new CallResult() { method = methodInfo, objectTobeInvoke = instance, parameters = param };
            var callThread = new Thread(new ParameterizedThreadStart(callResult.RunInOtherThread));
            callThread.Start(callResult);
            bool callSuccess = callThread.Join(timeOut);

            if (!callSuccess)
            {
                callThread.Abort();
                throw new Exception(string.Format("Timeout ({0}) call on object {1} method {2}.", timeOut, instance.GetType().FullName, methodInfo.Name));
            }

            if (callResult.exc != null)
                throw callResult.exc;
            else return callResult.result;
        }


        public static object Execute(object instance, string methodName, int timeOut, params object[] param)
        {
            var methodRef = instance.GetType().GetMethod(methodName);
            return Execute(instance, methodRef, timeOut, param);

        }
    }
}
