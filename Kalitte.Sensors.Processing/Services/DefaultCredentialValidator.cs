using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.Threading;
using System.Security.Principal;
using Kalitte.Sensors.Security;
using System.Web.Security;
using System.IdentityModel.Tokens;

namespace Kalitte.Sensors.Processing.Services
{
    class DefaultCredentialValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (!Membership.ValidateUser(userName, password))
                throw new SecurityTokenException("Unknown Username or Password");
            var roles = Roles.GetRolesForUser(userName);
            var identity = new GenericIdentity(userName, "Sensor Authentication");            
            Thread.CurrentPrincipal = new GenericPrincipal(identity, roles );
        }
    }
}
