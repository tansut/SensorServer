using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using Ext.Net;
using Kalitte.Sensors.Web.Security;
using Kalitte.Sensors.Web.Business;
using System.Net;

namespace Kalitte.Sensors.Web.Modules
{
    public class AppModule : IHttpModule
    {

        public AppModule() { }


        public void Init(System.Web.HttpApplication context)
        {
            context.Error += new EventHandler(context_Error);
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
        }

        void context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            if (application.Request.IsAuthenticated)
            {



            }
        }

        void context_Error(object sender, EventArgs e)
        {

            HttpApplication Application = (HttpApplication)sender;
            HttpResponse Response = Application.Context.Response;
            HttpRequest Request = Application.Context.Request;
            Exception exc = Application.Server.GetLastError();
            if (exc == null)
                return;
            Exception convertedExc;
            if (!((exc is HttpException && ((System.Web.HttpException)(exc)).GetHttpCode() == 404)))
            {
                try
                {
                    convertedExc = ExceptionManager.Manage(exc);
                }
                catch (Exception cExc)
                {
                    convertedExc = cExc;
                }

                if (X.IsAjaxRequest)
                {
                    Application.Server.ClearError();
                    var response = new Ext.Net.Response(false);
                    response.Message = convertedExc.Message;
                    Response.ClearContent();
                    Response.Write(new Ext.Net.ClientConfig().Serialize(response));
                    Response.End();
                }
                else
                {
                    if (exc is HttpException)
                    {
                        Application.Context.Items.Add("lastException", convertedExc);
                        Application.Server.Transfer("~/Pages/Shared/UnhandledError.aspx");
                    }
                    else
                    {
                        Response.Write("<h2>Global Page Error</h2>\n");
                        Response.Write(
                            "<p>" + exc.Message + "</p>\n");
                        Response.Write("Return to the <a href='Default.aspx'>" +
                            "Default Page</a>\n");
                    }

                }
            }
        }


        public void Dispose() { }
    }
}
