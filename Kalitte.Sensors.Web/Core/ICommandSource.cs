using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Web.Business;

namespace Kalitte.Sensors.Web.Core
{
    public interface ICommandSource
    {
        BusinessBase ControllerObject { get; }
    }
}
