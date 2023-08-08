using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Threading;
using Kalitte.Sensors.Client.Proxy;

namespace Kalitte.Sensors.Web.Security
{
    public class ExceptionManager
    {
        public static string GetExceptionMessage(Exception exc)
        {
            if (exc is SensorClientException && ((SensorClientException)exc).Message == "RemoteException")
                return exc.InnerException.Message;
            else return exc.Message;
        }

        private static ApplicationException Convert(Exception exc)
        {
            if (exc is TechnicalException)
                return (TechnicalException)exc;
            else if (exc is BusinessException)
                return (BusinessException)exc;
            else if (exc is HttpUnhandledException && exc.InnerException != null)
                exc = exc.InnerException;

            if (exc is TargetInvocationException)
            {
                while (exc.InnerException != null && exc is TargetInvocationException)
                    exc = exc.InnerException;
            }

            if (exc is SensorClientException)
            {
                if (exc.InnerException != null)
                    exc = new BusinessException(exc.InnerException.Message, exc.InnerException);
                else exc = new BusinessException(exc.Message, exc);
            }

            if (exc is FormatException)
                exc = new BusinessException("Please check input datas. We got a format exception.");
            if (!(exc is BusinessException)) exc = new TechnicalException("Technical exception thrown. Contact system administrator.", exc);


            return (ApplicationException)exc;
        }

        public static string ExceptionDebugDetails(Exception exc)
        {
            Exception temp = exc;
            StringBuilder sb = new StringBuilder();
            do
            {
                sb.Append(string.Format("Exception: <b>{0}</b>, User: {1}<br/>",
                    temp.GetType().Name, Thread.CurrentPrincipal.Identity.Name));
                sb.Append(string.Format("Exception: <b>{0}</b><br/>", temp.Message));
                sb.Append(string.Format("StackTrace: {0}<br/>", temp.StackTrace));
                sb.Append("------------------------------------------------<br/>");

                temp = temp.InnerException;

            } while (temp != null);
            return sb.ToString();

        }

        public static Exception Manage(Exception exc)
        {
            Exception converted = Convert(exc);
            if (converted is BusinessException)
            {
                return converted;
            }
            else
            {
                if (HttpContext.Current != null)
                {
                    converted.Data.Add("Page", HttpContext.Current.Request.Url.ToString());
                    converted.Data.Add("IP", HttpContext.Current.Request.UserHostAddress);

                    //try
                    //{
                    //    if (exc is System.Reflection.ReflectionTypeLoadException)
                    //    {
                    //        System.Reflection.ReflectionTypeLoadException refExc = (System.Reflection.ReflectionTypeLoadException)exc;
                    //        StringBuilder sb = new StringBuilder();
                    //        if (refExc.LoaderExceptions != null)
                    //        {
                    //            foreach (var ex in refExc.LoaderExceptions)
                    //                sb.Append(ex.Message + Environment.NewLine);
                    //        }
                    //        converted.Data.Add("LoaderExceptions", sb.ToString());
                    //    }
                    //}
                    //catch (Exception cExc)
                    //{

                    //    converted.Data.Add("LoaderExc", cExc.Message);
                    //}
                }

#if !DEBUG
                //ExceptionPolicy.HandleException(converted, "default");
                return converted;
#else
                //ExceptionPolicy.HandleException(converted, "debug");

#endif
                return converted;
            }
        }
    }
}
