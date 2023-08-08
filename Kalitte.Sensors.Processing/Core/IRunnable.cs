using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Processing.Core
{
    internal interface IRunnable
    {        
        ItemState GetState();
        void RunItem();
    }
}
