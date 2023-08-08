using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Configuration
{
    public static class MetadataKeys
    {
        public readonly static PropertyKey LocationNameKey = new PropertyKey("Location", "Name");
        public readonly static PropertyKey LocationAdressLine1Key = new PropertyKey("Location", "AdressLine1");
        public readonly static PropertyKey LocationAdressLine2Key = new PropertyKey("Location", "AdressLine2");
        public readonly static PropertyKey LocationZipcodeKey = new PropertyKey("Location", "Zipcode");
        public readonly static PropertyKey LocationCityKey = new PropertyKey("Location", "City");
        public readonly static PropertyKey LocationCountryKey = new PropertyKey("Location", "Country");
        public readonly static PropertyKey LocationXKey = new PropertyKey("Location", "X");
        public readonly static PropertyKey LocationYKey = new PropertyKey("Location", "Y");
        public readonly static PropertyKey LocationZKey = new PropertyKey("Location", "Z");
        public readonly static PropertyKey LocationDescriptionKey = new PropertyKey("Location", "Description");


        static MetadataKeys()
        {

        }
    }
}
