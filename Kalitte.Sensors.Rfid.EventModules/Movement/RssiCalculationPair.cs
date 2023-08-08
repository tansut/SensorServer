using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Rfid.EventModules.Client.Movement;


namespace Kalitte.Sensors.Rfid.EventModules.Movement
{
    public class RssiCalculationPair
    {
        RssiCalculationData First { get; set; }
        RssiCalculationData Next { get; set; }

        public DateTime LastTagReadEventReceived { get; set; }
        public DateTime LastEventGenerated { get; set; }


        public RssiCalculationPair(TagReadEvent tre, ILogger logger)
        {
            First = new RssiCalculationData("First Pick", tre, logger);
            Next = null;
            LastTagReadEventReceived = DateTime.Now;
            LastEventGenerated = DateTime.MinValue;
        }

        public TagMovementEvent AddSample(TagReadEvent tre, TagMovementModule.ModuleSettings settings)
        {
            LastTagReadEventReceived = DateTime.Now;
            float rssi = (float)tre.VendorSpecificData[TagReadEvent.Rssi];
            if (First.HasEnoughData(settings.CalculationIntervalLimit, settings.CalculationTagLimit))
            {
                if (Next == null)
                {
                    Next = new RssiCalculationData("Next Pick", tre, First.Logger);
                }
                else if (Next.HasEnoughData(settings.CalculationIntervalLimit, settings.CalculationTagLimit))
                {
                    double change = ((Next.Rssi - First.Rssi) / Math.Abs(Next.Rssi)) * 100.0;
                    First.Logger.Verbose("Change : {0} Rssi: {1} NextRssi: {2}", change, First.Rssi, Next.Rssi);
                    TagMovementEvent evt = null;
                    var changeAbs = Math.Abs(change);
                    ILogger logger = First.Logger;
                    if (change < 0 && changeAbs >= settings.MinMoveAwayPercentage && changeAbs <= settings.MaxMoveAwayPercentage)
                        evt = new TagMovingAwayEvent(First.TagRead, tre, First.Rssi, Next.Rssi);
                    else if (change > 0 && changeAbs >= settings.MinApproachPercentage && changeAbs <= settings.MaxApproachPercentage)
                        evt = new TagApproachingEvent(First.TagRead, tre, First.Rssi, Next.Rssi);
                    if (evt != null)
                    {

                        if (LastEventGenerated != DateTime.MinValue && settings.MinEventInterval != 0)
                        {
                            var milisecondsFromLastEvent = (DateTime.Now - LastEventGenerated).TotalMilliseconds;
                            if (milisecondsFromLastEvent <= settings.MinEventInterval)
                            {
                                evt = null;
                                logger.Verbose("Probably dublicate event. Ignored. MilisecondsFromLastEvent is: {0}", milisecondsFromLastEvent);
                            }
                        }
                        if (evt != null)
                        {
                            LastEventGenerated = DateTime.Now;
                            First.Logger.Info("Event Fired. {0}. Change : {1} Rssi: {2} NextRssi: {3}", evt.GetType().Name, change, First.Rssi, Next.Rssi);
                        }
                    }
                    First = new RssiCalculationData("First Pick", tre, logger);
                    Next = null;
                    return evt;
                }
                else Next.AddSample(rssi);
            }
            else First.AddSample(rssi);
            return null;
        }



    }
}
