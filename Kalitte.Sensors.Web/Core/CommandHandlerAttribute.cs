using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Web.Security;

namespace Kalitte.Sensors.Web.Core
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class CommandHandlerAttribute : Attribute
    {


        public CommandHandlerAttribute()
        {
            KnownCommand = Security.KnownCommand.None;
        }

        public KnownCommand KnownCommand { get; set; }
        public string CommandName { get; set; }
        public Type ControllerType { get; set; }
    }

}
