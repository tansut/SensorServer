using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Web.Controls
{
    public class SetPropertyProfileEventArgs: EventArgs
    {
        public PropertyList UpdatedProfile { get; set; }

        public SetPropertyProfileEventArgs(PropertyList profile)
        {
            this.UpdatedProfile = profile;
        }
    }
}
