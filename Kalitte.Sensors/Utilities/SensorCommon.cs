using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Exceptions;
using System.IO;

namespace Kalitte.Sensors.Utilities
{
    public class SensorCommon
    {
        public static string GetDetailedErrorMessage(Exception ex, bool exposeStackTrace, out string errorMessageWithStack)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            while (ex != null)
            {
                builder.Append(ex.Message);
                builder2.Append(ex.Message);
                if (exposeStackTrace)
                {
                    builder2.Append(ex.ToString());
                }
                ex = ex.InnerException;
                if (ex != null)
                {
                    builder.AppendLine();
                    builder.Append("Inner Exception");
                    builder2.AppendLine();
                    builder2.Append("Inner Exception");
                }
            }
            errorMessageWithStack = builder2.ToString();
            return builder.ToString();
        }

        


        public static SensorFault GetFaultFromException(Exception e, bool exposeStackTrace)
        {
            SensorFault fault = new SensorFault();
            string errorMessageWithStack = null;
            fault.RemoteErrorMessage = GetDetailedErrorMessage(e, exposeStackTrace, out errorMessageWithStack);
            SensorException exception = e as SensorException;
            if (exception != null)
            {
                fault.RemoteErrorCode = exception.ErrorCode;
                fault.SetRemoteParameters(exception.Parameters);
            }
            fault.RemoteStackTrace = errorMessageWithStack;
            SensorException exception2 = e as SensorException;
            if (exception2 != null)
            {
                fault.RemoteException = exception2;
            }
            return fault;
        }

 


        public static Dictionary<PropertyKey, EntityMetadata> GetEntityMetadata<T>(Dictionary<PropertyKey, T> source)
        {
            if (source == null)
                return null;
            Dictionary<PropertyKey, EntityMetadata> result = new Dictionary<PropertyKey, EntityMetadata>();
            foreach (var item in source)
                result.Add(item.Key, item.Value as EntityMetadata);
            return result;
        }

        public static void ValidateParamsAreNonNull(object[] obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
        }

        public static string RemoveNonCharsAndDigits(string name)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char ch in name)
            {
                if (char.IsLetterOrDigit(ch))
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }



        public static void AppendFormat(StringBuilder sb, string format, params object[] p)
        {
            PreProcessParams(p);
            sb.AppendFormat(null, format, p);
        }

        internal static void PreProcessParams(object[] objs)
        {
            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    Exception ex = objs[i] as Exception;
                    if (ex != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        AppendAllInnerExceptions(sb, ex);
                        objs[i] = sb.ToString();
                    }
                }
            }
        }

        public static void AppendAllInnerExceptions(StringBuilder sb, Exception ex)
        {
            while (ex != null)
            {
                sb.Append(ex.Message);
                sb.Append("\r\n");
                sb.Append(ex.StackTrace);
                sb.Append("\r\n");
                ex = ex.InnerException;
                if (ex != null)
                {
                    sb.Append("---> ");
                }
            }
        }










        public static PropertyList CreateProfile(Dictionary<PropertyKey, EntityMetadata> dictionary)
        {
            if (dictionary == null)
                return new PropertyList();
            else
            {
                PropertyList profile = new PropertyList();
                foreach (var item in dictionary)
                {
                    profile.Add(item.Key, item.Value.DefaultValue);
                }
                return profile;
            }
        }

        public static PropertyList CreateExtendedProfile(ExtendedMetadata extendedMetadata)
        {
            if (extendedMetadata == null)
                return new PropertyList();
            else
            {
                PropertyList profile = new PropertyList();
                foreach (var item in extendedMetadata.PropertyMetadata)
                {
                    profile.Add(item.Key, item.Value.DefaultValue);
                }
                return profile;
            }
        }

        public static IEnumerable<string> GetFiles(string directory, string pattern)
        {
            var list = Directory.EnumerateFiles(directory, pattern + "*", SearchOption.TopDirectoryOnly);
            foreach (var item in list)
            {
                yield return Path.GetFileName(item);
            }
        }
    }
}
