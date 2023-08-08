using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Principal;
using System.Web.Security;

namespace Kalitte.Sensors.Processing.Services
{
    public class BasicHttpMembershipAuthenticationModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication application)
        {
            application.AuthenticateRequest += new EventHandler(this.OnAuthenticateRequest);
            application.EndRequest += new EventHandler(this.OnEndRequest);
        }

        public void OnAuthenticateRequest(object source, EventArgs eventArgs)
        {

            HttpApplication app = (HttpApplication)source;

            //the Authorization header is checked if present
            string authHeader = app.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authHeader))
            {
                string authStr = app.Request.Headers["Authorization"];

                if (authStr == null || authStr.Length == 0)
                {
                    // No credentials; anonymous request
                    return;
                }

                authStr = authStr.Trim();
                if (authStr.IndexOf("Basic", 0) != 0)
                {
                    // header is not correct...we'll pass it along and 
                    // assume someone else will handle it
                    return;
                }


                authStr = authStr.Trim();


                string encodedCredentials = authStr.Substring(6);

                byte[] decodedBytes =
                Convert.FromBase64String(encodedCredentials);
                string s = new ASCIIEncoding().GetString(decodedBytes);

                string[] userPass = s.Split(new char[] { ':' });
                string username = userPass[0];
                string password = userPass[1];
                //the user is validated against the 
                //SqlMemberShipProvider
                //If it is validated then the roles are retrieved from 
                // the role provider and a generic principal is created
                //the generic principal is assigned to the user context
                // of the application


                if (Membership.ValidateUser(username, password))
                {
                    string[] roles = Roles.GetRolesForUser(username);
                    app.Context.User = new GenericPrincipal(new
                    GenericIdentity(username, "Membership Provider"),
                                    roles);

                }
                else
                {
                    DenyAccess(app);
                    return;

                }
            }
            else
            {
                app.Response.StatusCode = 401;
                app.Response.End();


            }
        }
        public void OnEndRequest(object source, EventArgs eventArgs)
        {
            //the authorization header is not present
            //the status of response is set to 401 and it ended
            //the end request will check if it is 401 and add
            //the authentication header so the client knows
            //it needs to send credentials to authenticate
            if (HttpContext.Current.Response.StatusCode == 401)
            {
                HttpContext context = HttpContext.Current;
                context.Response.StatusCode = 401;
                context.Response.AddHeader("WWW-Authenticate", "Basic Realm");
            }
        }

        private void DenyAccess(HttpApplication app)
        {
            app.Response.StatusCode = 401;
            app.Response.StatusDescription = "Access Denied";

            // Write to response stream as well, to give user visual 
            // indication of error during development
            app.Response.Write("401 Access Denied");

            app.CompleteRequest();
        }



    }

}