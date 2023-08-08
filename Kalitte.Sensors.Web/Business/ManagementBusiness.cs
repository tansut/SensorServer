using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Web.Business
{
    public class ManagementBusiness: BusinessBase
    {
        public override System.Collections.IList GetItems()
        {
            return null;
        }

        public void SetLastEventFilter(ProcessingItem itemType, string itemName, LastEventFilter filter)
        {
            SensorProxy.SetLastEventFilter(itemType, itemName, filter);
        }

        public LastEventFilter GetLastEventFilter(ProcessingItem itemType, string itemName)
        {
            return SensorProxy.GetLastEventFilter(itemType, itemName);
        }
    }
}
