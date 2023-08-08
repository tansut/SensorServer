using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Communication
{
    [Serializable, KnownType("GetTransportSettingsSubTypes")]
    public abstract class TransportSettings
    {
        // Fields
        private VendorData m_vendorSpecificData;

        // Methods
        protected TransportSettings()
        {
        }

        public override bool Equals(object obj)
        {
            TransportSettings settings = obj as TransportSettings;
            return ((settings != null) && CollectionsHelper.CompareDictionaries(this.m_vendorSpecificData, settings.m_vendorSpecificData));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private static IEnumerable<Type> GetTransportSettingsSubTypes()
        {
            return TypesHelper.GetCurrentAssemblyTypes(typeof(TransportSettings));
        }

        public override string ToString()
        {
            return "<transportSettings/>";
        }

        // Properties
        public VendorData VendorSpecificData
        {
            get
            {
                return this.m_vendorSpecificData;
            }
            set
            {
                this.m_vendorSpecificData = value;
            }
        }
    }




}
