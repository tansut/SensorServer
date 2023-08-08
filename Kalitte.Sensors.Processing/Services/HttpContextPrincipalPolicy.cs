using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IdentityModel.Policy;
using System.Security.Principal;
using System.IdentityModel.Claims;

namespace Kalitte.Sensors.Processing.Services
{

    public class HttpContextPrincipalPolicy : IAuthorizationPolicy
    {
        public bool Evaluate(EvaluationContext evaluationContext, ref
                             object state)
        {
            HttpContext context = HttpContext.Current;

            if (context != null)
            {
                evaluationContext.Properties["Principal"] =
                context.User;
                evaluationContext.Properties["Identities"] =
                   new List<IIdentity>() { context.User.Identity };
            }

            return true;
        }

        public System.IdentityModel.Claims.ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        public string Id
        {
            get { return "HttpContextPrincipalPolicy"; }
        }
    }
}