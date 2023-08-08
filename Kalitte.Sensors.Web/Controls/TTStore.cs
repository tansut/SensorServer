using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTStore : Store
    {

        public TTStore()
            : base()
        {
        }

        protected void CheckAndAddBaseParameter(string name, string value)
        {
            if (!BaseParams.Any(p => p.Name == name))
            {
                BaseParams.Add(new Parameter(name, value, ParameterMode.Raw)); 
            }
        }

    }
}
