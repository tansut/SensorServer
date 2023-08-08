using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Security
{
    [Serializable, KnownType("GetAuthInfoSubTypes")]
    public abstract class AuthenticationInformation
    {
        // Methods
        protected internal AuthenticationInformation()
        {
        }

        private static IEnumerable<Type> GetAuthInfoSubTypes()
        {
            return TypesHelper.GetCurrentAssemblyTypes(typeof(AuthenticationInformation));

        }
    }




}
