using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web;
using Kalitte.Sensors.Web.Security;
using Kalitte.Sensors.Utilities;
using System.Security.Cryptography;
using System.Threading;

namespace Kalitte.Sensors.Web.Business
{
    public static class AuthenticationBusiness
    {
        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }


        public static Logindata GetLoginData()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null)
                throw new BusinessException("Cannot retreive credentials. Please signoff and logon again.");
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            byte[] encData = Convert.FromBase64String(ticket.UserData);
            byte[] plain = ProtectedData.Unprotect(encData, null, DataProtectionScope.LocalMachine);
            Logindata login = (Logindata)SerializationHelper.BinaryDeSerializeFromByteArray(plain);
            return login;
        }

        public static void Login(string userName, string password, string host, int port,  bool remember)
        {
            if (Membership.ValidateUser(userName, password))
            {
                Logindata login = new Logindata() { Password = password, Port = port, ServerHost = host };
                byte[] plain = SerializationHelper.BinarySerializeToByteArray(login);
                byte[] enc = ProtectedData.Protect(plain, null, DataProtectionScope.LocalMachine);
                string cookieData = Convert.ToBase64String(enc);
                HttpCookie cookie = FormsAuthentication.GetAuthCookie(userName, remember);
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                FormsAuthenticationTicket newticket = new FormsAuthenticationTicket(ticket.Version, ticket.Name,
                    ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, cookieData);
                cookie.Value = FormsAuthentication.Encrypt(newticket);
                HttpContext.Current.Response.Cookies.Set(cookie);
                HttpContext.Current.Response.Redirect("~/");
            }
            else
            {
                throw new BusinessException("Invalid username/password combination.");
            }
        }
    }
}
