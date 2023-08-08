using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Core;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Rfid.EventModules.Client.TagView;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Rfid.EventModules.Movement
{
    class RssiCalculationData
    {
        public int SampleCount { get; private set; }
        public float Rssi { get; private set; }
        public float InitialRssi { get; private set; }
        public float LastRssi { get; private set; }

        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }
        public TagReadEvent TagRead { get; private set; }
        public ILogger Logger { get; set; }

        public string Name { get; set; }

        public RssiCalculationData(string name, TagReadEvent tagRead, ILogger logger)
        {
            this.Name = name;
            this.TagRead = tagRead;
            this.InitialRssi = (float)tagRead.VendorSpecificData[TagReadEvent.Rssi];
            this.Rssi = InitialRssi;
            this.SampleCount = 1;
            FirstSeen = DateTime.Now;
            LastSeen = FirstSeen;
            this.Logger = logger;
        }

        public bool HasEnoughData(int rsiiCalcInterval, int rsiiCalcLimit)
        {
            bool doCalculation = rsiiCalcLimit != 0 && SampleCount >= rsiiCalcLimit;
            if (!doCalculation)
            {
                doCalculation = (DateTime.Now - FirstSeen) >= TimeSpan.FromMilliseconds(rsiiCalcInterval);
                Logger.VerboseIf(doCalculation, string.Format("{0} timeout occured. Asuming enough data", Name),"",0);
            } 
            return doCalculation;
        }

        public void AddSample(float rssi)
        {
            this.LastSeen = DateTime.Now;
            SampleCount++;
            LastRssi = rssi;
            Rssi = (Rssi + LastRssi) / SampleCount;

        }

        public void Reset(float rssi)
        {
            SampleCount = 1;
            LastRssi = rssi;
            Rssi = LastRssi;

        }

        public double Change
        {
            get
            {
                return ((LastRssi - Rssi) / LastRssi) * 100.0;
            }
        }


    }
}
