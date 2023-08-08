using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Web.Business
{
    public class ServerAnalysisBusiness : BusinessBase
    {
        public override System.Collections.IList GetItems()
        {
            return this.GetCategories("");
        }

        public NameDescriptionList GetCategoryNames(string watch, ServerAnalyseItem related)
        {
            return SensorProxy.GetServerWatcherCategoryNames(watch, related);
        }

        public NameDescriptionList GetWatcherNames()
        {
            return SensorProxy.GetServerWatcherNames();
        }
        public NameDescriptionList GetCategories(string watch)
        {
            return SensorProxy.GetServerWatcherCategories(watch);
        }

        public NameDescriptionList GetInstanceNames(string watch, string category)
        {
            return SensorProxy.GetServerWatcherInstanceNames(watch, category);

        }
        public NameDescriptionList GetMeasureNames(string watch, string category)
        {
            return SensorProxy.GetServerWatcherMeasureNames(watch, category);
        }
        public float[] GetMeasureValues(string watch, string category, string instance, string[] measureNames)
        {
            return SensorProxy.GetServerWatcherMeasureValues(watch, category, instance, measureNames);
        }
    }


}
