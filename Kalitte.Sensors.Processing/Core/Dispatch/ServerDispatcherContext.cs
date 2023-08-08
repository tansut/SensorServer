using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Processing.Core.Dispatch
{
    public class ServerDispatcherContext : DispatcherContext
    {
        private DispatchMarshal marshal;

        public ServerDispatcherContext(Dictionary<string, object> contextObjects, ILogger logger, DispatchMarshal marshal)
            : base(contextObjects, logger)
        {
            this.marshal = marshal;
        }

        public override void Stop(string reasonMessage)
        {
            marshal.Stop(reasonMessage);
        }

        public override void SetProperty(object sender, Configuration.EntityProperty property)
        {
            marshal.SetPropertyFromModule(sender, property);
        }
    }
}
