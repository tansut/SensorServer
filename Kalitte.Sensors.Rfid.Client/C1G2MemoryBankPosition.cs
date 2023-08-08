using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Core;

namespace Kalitte.Sensors.Rfid.Client
{
    internal class C1G2MemoryBankPosition
    {
        public C1G2MemoryBank MemoryBank { get; private set; }
        public int Start { get; private set; }
        public int Length { get; private set; }
        public C1G2MemoryBankPosition(C1G2MemoryBank bank, int start, int length)
        {
            this.MemoryBank = bank;
            this.Start = start;
            this.Length = length;
        }
    }
}
