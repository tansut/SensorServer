using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using System.Globalization;
using Kalitte.Sensors.Rfid.Utilities;
using System.Management.Instrumentation;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Rfid.Events
{
    [Serializable, KnownType("GetRfidEventBaseSubTypes")]
    public abstract class RfidEventBase : SensorEventBase
    {

        // Methods
        protected internal RfidEventBase()
            : base()
        {

        }

        protected RfidEventBase(VendorData values)
            : base(values)
        {

        }



        private static IEnumerable<Type> GetRfidEventBaseSubTypes()
        {
            return TypesHelper.GetCurrentAssemblyTypes(typeof(RfidEventBase));
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<rfidEventBase>");
            if (this.HasVendorData())
            {
                builder.Append(this.VendorSpecificData.ToString());
            }
            builder.Append("</rfidEventBase>");
            return builder.ToString();
        }
    }


}
