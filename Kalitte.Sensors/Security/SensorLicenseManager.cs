using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Security
{
    internal sealed class SensorLicenseManager
    {
        
        internal SensorLicenseManager()
        {
        }


        internal void Validate()
        {
             DateTime max = new DateTime(2012, 12, 1);
            if (DateTime.Now > max)
            {
                Console.WriteLine("Beta expired");
                Environment.Exit(1);
            }
        }
    }
}
