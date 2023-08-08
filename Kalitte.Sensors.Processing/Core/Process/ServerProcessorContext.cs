using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Interfaces;
using Kalitte.Sensors.Security;

namespace Kalitte.Sensors.Processing.Core.Process
{
    public sealed class ServerProcessorContext: ProcessorContext
    {
        private ProcessMarshall processMarshal;

        public ServerProcessorContext(Dictionary<string, object> contextObjects, ILogger logger, ProcessMarshall processMarshal)
            : base(contextObjects, logger)
        {
            this.processMarshal = processMarshal;
        }

        public override void Notify(string source, Events.SensorEventBase sensorEvent)
        {
            processMarshal.Notify(source, sensorEvent);
        }

        public override void AddEventToNextPipe(object sender, string source, Events.SensorEventBase sensorEvent)
        {
            processMarshal.AddEventToNextPipe(sender, source, sensorEvent);
        }

        public override void Stop(string reasonMessage)
        {
            processMarshal.Stop(reasonMessage);
        }


        public override void NotifyImmediate(string source, Events.SensorEventBase sensorEvent)
        {
            processMarshal.NotifyImmediate(source, sensorEvent);
        }

        public override void SetProperty(object sender, Configuration.EntityProperty property)
        {
            processMarshal.SetPropertyFromModule(sender, property);
        }
    }
}
