using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using System.Globalization;
using System.Management.Instrumentation;
using Kalitte.Sensors.Utilities;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Events
{
    [Serializable, KnownType("GetSensorEventBaseSubTypes")]
    public abstract class SensorEventBase
    {
        internal const string eventIDKey = "EventID";
        private object m_lockObject;
        private VendorData vendorData;

        // Methods
        protected internal SensorEventBase()
        {
            this.m_lockObject = new object();
        }

        protected SensorEventBase(VendorData values)
        {
            this.m_lockObject = new object();
            this.vendorData = values;
        }

        internal string GetEventID()
        {
            string str = null;
            if (this.VendorSpecificData.ContainsKey("EventID") && (this.VendorSpecificData["EventID"] != null))
            {
                str = (string)this.VendorSpecificData["EventID"];
            }
            return str;
        }

        private static IEnumerable<Type> GetSensorEventBaseSubTypes()
        {
            return TypesHelper.GetCurrentAssemblyTypes(typeof(SensorEventBase));
        }

        internal void SetEventID()
        {
            if (!this.VendorSpecificData.ContainsKey("EventID"))
            {
                string str = Guid.NewGuid().ToString("", CultureInfo.InvariantCulture);
                this.VendorSpecificData.Add("EventID", str);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<SensorEventBase>");
            if (this.HasVendorData())
            {
                builder.Append(this.vendorData.ToString());
            }
            builder.Append("</SensorEventBase>");
            return builder.ToString();
        }

        public bool HasVendorData()
        {
            return this.vendorData != null;
        }

        // Properties
        [IgnoreMember]
        public VendorData VendorSpecificData
        {
            get
            {
                if (this.vendorData == null)
                {
                    lock (this.m_lockObject)
                    {
                        if (this.vendorData == null)
                        {
                            this.vendorData = new VendorData();
                        }
                    }
                }
                return this.vendorData;
            }
        }
    }
}
