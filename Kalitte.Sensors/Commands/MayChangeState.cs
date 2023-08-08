using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Commands
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class MayChangeStateAttribute : Attribute
    {
        // Fields
        private bool device = true;

        // Properties
        public bool Device
        {
            get
            {
                return this.device;
            }
        }
    }


}
