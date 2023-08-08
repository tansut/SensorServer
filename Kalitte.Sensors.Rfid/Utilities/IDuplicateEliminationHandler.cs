using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Events;

namespace Kalitte.Sensors.Rfid.Utilities
{
    internal interface IDuplicateEliminationHandler
    {
        // Methods
        void FilterDuplicates(TagListEvent tle);
        bool IsDuplicate(TagReadEvent tre);
    }

 

 

}
