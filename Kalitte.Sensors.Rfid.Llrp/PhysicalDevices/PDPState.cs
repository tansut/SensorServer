using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Core;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Rfid.Llrp;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Rfid.Llrp.Configuration;
using Kalitte.Sensors.Rfid.Llrp.Utilities;
using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
using Kalitte.Sensors.Rfid.Utilities;

namespace Kalitte.Sensors.Rfid.Llrp.PhysicalDevices
{

    internal sealed class PDPState
    {
        // Fields
        private AccessSpec m_accessSpec;
        private Dictionary<PropertyKey, DevicePropertyMetadata> m_deviceMetadata;
        private DateTime m_deviceStartTime = DateTime.UtcNow;
        private DuplicateEliminationHandler m_duplicateEliminationHandler;
        private bool m_fInventoryOn;
        private PropertyList m_llrpSpecificCapabilities;
        //private Dictionary<PropertyKey, DevicePropertyMetadata> m_microsoftCustomMetadata;
        private PropertyList m_providerMaintainedProperties = new PropertyList(LlrpResources.PropertyProfileName);
        private ROSpec m_roSpec;
        private Dictionary<string, PropertyList> m_sources;
        private static Collection<PropertyKey> s_deviceOverrideAbleProperties = new Collection<PropertyKey>();

        internal DuplicateEliminationHandler DuplicateEliminationHandler
        {
            get
            {
                return this.m_duplicateEliminationHandler;
            }
        }


        // Methods
        static PDPState()
        {
            s_deviceOverrideAbleProperties.Add(GeneralGroup.VendorKey);
            s_deviceOverrideAbleProperties.Add(GeneralGroup.FirmwareVersionKey);
            s_deviceOverrideAbleProperties.Add(GeneralGroup.RegulatoryRegionKey);
            s_deviceOverrideAbleProperties.Add(RFGroup.AirProtocolsSupportedKey);
            s_deviceOverrideAbleProperties.Add(TagReadGroup.DuplicateEliminationTimeKey);
        }

        internal PDPState()
        {
            this.Reset();
        }

        internal void Reset()
        {
            this.m_providerMaintainedProperties = new PropertyList(LlrpResources.PropertyProfileName);
            this.m_providerMaintainedProperties[NotificationGroup.EventModeKey] = true;
            this.m_fInventoryOn = false;
            this.m_providerMaintainedProperties[NotificationGroup.InventoryROSpecKey] = NotificationGroup.InventoryROSpecMetadata.DefaultValue;
            this.ROSpec = LlrpSerializationHelper.DeserializeFromXmlDataContract<ROSpec>((string) NotificationGroup.InventoryROSpecMetadata.DefaultValue);
            object obj2 = this.m_providerMaintainedProperties[NotificationGroup.InventoryAccessSpecKey] = NotificationGroup.InventoryAccessSpecMetadata.DefaultValue;
            this.InventoryAccessSpec = (obj2 == null) ? null : LlrpSerializationHelper.DeserializeFromXmlDataContract<AccessSpec>((string)obj2);
            long defaultValue = (long)TagReadGroup.DuplicateEliminationTimeMetadata.DefaultValue;
            this.m_providerMaintainedProperties[TagReadGroup.DuplicateEliminationTimeKey] = defaultValue;
            this.m_duplicateEliminationHandler = new DuplicateEliminationHandler(defaultValue, false, LlrpProviderContext.Logger);
            this.m_providerMaintainedProperties[LlrpManagementGroup.SynchronousCommandInventoryDurationKey] = LlrpManagementGroup.SynchronousCommandInventoryDurationMetadata.DefaultValue;
            this.m_providerMaintainedProperties[LlrpManagementGroup.LlrpMessageTimeoutKey] = LlrpProviderContext.LlrpMessageTimeout;
            this.m_providerMaintainedProperties[LlrpManagementGroup.SynchronousCommandInventoryOperationCountKey] = LlrpManagementGroup.SynchronousCommandInventoryOperationCountMetadata.DefaultValue;
            this.m_providerMaintainedProperties[LlrpTroubleshootGroup.CleanupSpecsKey] = false;
        }

        // Properties
        internal static Collection<PropertyKey> DeviceOverrideAbleProperties
        {
            get
            {
                return s_deviceOverrideAbleProperties;
            }
        }



        internal DateTime DeviceStartTime
        {
            get
            {
                return this.m_deviceStartTime;
            }
            set
            {
                this.m_deviceStartTime = value;
            }
        }



        internal AccessSpec InventoryAccessSpec
        {
            get
            {
                return this.m_accessSpec;
            }
            set
            {
                this.m_accessSpec = value;
            }
        }

        internal bool IsInventoryOn
        {
            get
            {
                return this.m_fInventoryOn;
            }
            set
            {
                this.m_fInventoryOn = value;
                this.ProviderMaintainedProperties[NotificationGroup.EventModeKey] = value;
            }
        }

        internal PropertyList LlrpSpecificCapabilities
        {
            get
            {
                return this.m_llrpSpecificCapabilities;
            }
            set
            {
                this.m_llrpSpecificCapabilities = value;
            }
        }

        internal Dictionary<PropertyKey, DevicePropertyMetadata> Metadata
        {
            get
            {
                return this.m_deviceMetadata;
            }
            set
            {
                this.m_deviceMetadata = value;
            }
        }


        internal PropertyList ProviderMaintainedProperties
        {
            get
            {
                return this.m_providerMaintainedProperties;
            }
        }

        internal ROSpec ROSpec
        {
            get
            {
                return this.m_roSpec;
            }
            set
            {
                this.m_roSpec = value;
            }
        }

        internal Dictionary<string, PropertyList> Sources
        {
            get
            {
                return this.m_sources;
            }
            set
            {
                this.m_sources = value;
            }
        }
    }





}
