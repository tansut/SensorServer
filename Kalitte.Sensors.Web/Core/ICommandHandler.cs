using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Web.Controls;

namespace Kalitte.Sensors.Web.Core
{
    public interface ICommandHandler
    {
        bool ProcessCommand(object sender, CommandInfo cmd);
    }
}
