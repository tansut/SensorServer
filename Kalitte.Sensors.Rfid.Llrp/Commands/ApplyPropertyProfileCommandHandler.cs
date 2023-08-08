namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Commands;
    using System.Runtime.InteropServices;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Configuration;
    using Kalitte.Sensors.Configuration;
    using Kalitte.Sensors.Rfid.Llrp.Configuration;
    using Kalitte.Sensors.Exceptions;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Commands;

    internal sealed class ApplyPropertyProfileCommandHandler : CommandHandler
    {
        private Dictionary<PropertyKey, DevicePropertyMetadata> m_customMetadata;
        private Dictionary<PropertyKey, CommandError> m_detailedErrors;
        private PropertyList m_profileFromDevice;
        private Collection<PropertyKey> m_propertiesConsumed;
        private static Collection<PropertyKey> s_AccessReportSpecGroup;
        private static Collection<PropertyKey> s_AirProtocolSpecificEpcMemorySelectorParameterGroup;
        private static Collection<PropertyKey> s_AntennaPropertiesGroup;
        private static Collection<PropertyKey> s_C1G2FilterGroup;
        private static Collection<PropertyKey> s_C1G2InventoryCommandSettingGroup;
        private static Collection<PropertyKey> s_C1G2RFControlGroup;
        private static Collection<PropertyKey> s_C1G2SingulationControlGroup;
        private static Collection<PropertyKey> s_C1G2TagInventoryStateAwareSingulationActionGroup;
        private static Collection<PropertyKey> s_EventNotificationGroup = new Collection<PropertyKey>();
        private static Collection<PropertyKey> s_EventsAndReportsGroup;
        private static Collection<PropertyKey> s_GpiPortCurrentStateGroup;
        private static Collection<PropertyKey> s_GpoWriteDataGroup;
        private static Collection<PropertyKey> s_KeepaliveSpecGroup;
        private static Collection<PropertyKey> s_RFReceiverGroup;
        private static Collection<PropertyKey> s_RFTransmitterGroup;
        private static Collection<PropertyKey> s_ROReportSpecGroup;
        private static Collection<PropertyKey> s_SetReaderConfigurationGroup;
        private static Collection<PropertyKey> s_TagReportContentSelector;

        static ApplyPropertyProfileCommandHandler()
        {
            s_EventNotificationGroup.Add(LlrpReaderEventNotificationSpecGroup.AISpecEndEventKey);
            s_EventNotificationGroup.Add(LlrpReaderEventNotificationSpecGroup.AISpecEndWithSingulationEventKey);
            s_EventNotificationGroup.Add(LlrpReaderEventNotificationSpecGroup.AntennaEventKey);
            s_EventNotificationGroup.Add(LlrpReaderEventNotificationSpecGroup.BufferFillWarningEventKey);
            s_EventNotificationGroup.Add(LlrpReaderEventNotificationSpecGroup.GpiEventKey);
            s_EventNotificationGroup.Add(LlrpReaderEventNotificationSpecGroup.HoppingEventKey);
            s_EventNotificationGroup.Add(LlrpReaderEventNotificationSpecGroup.ReaderExceptionEventKey);
            s_EventNotificationGroup.Add(LlrpReaderEventNotificationSpecGroup.RFSurveyEventKey);
            s_EventNotificationGroup.Add(LlrpReaderEventNotificationSpecGroup.ROSpecEventKey);
            s_AntennaPropertiesGroup = new Collection<PropertyKey>();
            s_AntennaPropertiesGroup.Add(LlrpAntennaPropertiesGroup.AntennaGainKey);
            s_AntennaPropertiesGroup.Add(SourceGroup.EnabledKey);
            s_C1G2InventoryCommandSettingGroup = new Collection<PropertyKey>();
            s_C1G2InventoryCommandSettingGroup.Add(LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareKey);
            s_C1G2FilterGroup = new Collection<PropertyKey>();
            s_C1G2FilterGroup.Add(LlrpC1G2InventoryCommandGroup.FiltersKey);
            s_C1G2RFControlGroup = new Collection<PropertyKey>();
            s_C1G2RFControlGroup.Add(LlrpC1G2InventoryCommandGroup.ModeIndexKey);
            s_C1G2RFControlGroup.Add(LlrpC1G2InventoryCommandGroup.TariKey);
            s_C1G2SingulationControlGroup = new Collection<PropertyKey>();
            s_C1G2SingulationControlGroup.Add(LlrpC1G2InventoryCommandGroup.SessionKey);
            s_C1G2SingulationControlGroup.Add(LlrpC1G2InventoryCommandGroup.TagPopulationKey);
            s_C1G2SingulationControlGroup.Add(LlrpC1G2InventoryCommandGroup.TagTransitTimeKey);
            s_C1G2TagInventoryStateAwareSingulationActionGroup = new Collection<PropertyKey>();
            s_C1G2TagInventoryStateAwareSingulationActionGroup.Add(LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareSingulationActionSLFlagKey);
            s_C1G2TagInventoryStateAwareSingulationActionGroup.Add(LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareSingulationActionStateKey);
            s_RFReceiverGroup = new Collection<PropertyKey>();
            s_RFReceiverGroup.Add(LlrpAntennaConfigurationGroup.ReceiverSensitivityIndexKey);
            s_RFTransmitterGroup = new Collection<PropertyKey>();
            s_RFTransmitterGroup.Add(LlrpAntennaConfigurationGroup.ChannelIndexKey);
            s_RFTransmitterGroup.Add(LlrpAntennaConfigurationGroup.HopTableIdKey);
            s_RFTransmitterGroup.Add(LlrpAntennaConfigurationGroup.TransmitPowerIndexKey);
            s_AirProtocolSpecificEpcMemorySelectorParameterGroup = new Collection<PropertyKey>();
            s_AirProtocolSpecificEpcMemorySelectorParameterGroup.Add(LlrpROReportSpecGroup.C1G2EnableCrcKey);
            s_AirProtocolSpecificEpcMemorySelectorParameterGroup.Add(LlrpROReportSpecGroup.C1G2EnablePCKey);
            s_TagReportContentSelector = new Collection<PropertyKey>();
            s_TagReportContentSelector.Add(LlrpROReportSpecGroup.EnableAccessSpecIdKey);
            s_TagReportContentSelector.Add(LlrpROReportSpecGroup.EnableAntennaIdKey);
            s_TagReportContentSelector.Add(LlrpROReportSpecGroup.EnableChannelIndexKey);
            s_TagReportContentSelector.Add(LlrpROReportSpecGroup.EnableFirstSeenTimestampKey);
            s_TagReportContentSelector.Add(LlrpROReportSpecGroup.EnableInventoryParameterSpecIdKey);
            s_TagReportContentSelector.Add(LlrpROReportSpecGroup.EnableLastSeenTimestampKey);
            s_TagReportContentSelector.Add(LlrpROReportSpecGroup.EnablePeakRssiKey);
            s_TagReportContentSelector.Add(LlrpROReportSpecGroup.EnableROSpecIdKey);
            s_TagReportContentSelector.Add(LlrpROReportSpecGroup.EnableSpecIndexKey);
            s_TagReportContentSelector.Add(LlrpROReportSpecGroup.EnableTagSeenCountKey);
            s_ROReportSpecGroup = new Collection<PropertyKey>();
            s_ROReportSpecGroup.Add(LlrpROReportSpecGroup.NumberOfTagReportDataKey);
            s_ROReportSpecGroup.Add(LlrpROReportSpecGroup.TriggerKey);
            s_AccessReportSpecGroup = new Collection<PropertyKey>();
            s_AccessReportSpecGroup.Add(LlrpAccessReportSpecGroup.TriggerKey);
            s_KeepaliveSpecGroup = new Collection<PropertyKey>();
            s_KeepaliveSpecGroup.Add(LlrpKeepAliveSpecGroup.SendKeepaliveKey);
            s_KeepaliveSpecGroup.Add(LlrpKeepAliveSpecGroup.TimeIntervalKey);
            s_GpoWriteDataGroup = new Collection<PropertyKey>();
            s_GpoWriteDataGroup.Add(SourceGroup.PortOutputValueKey);
            s_GpiPortCurrentStateGroup = new Collection<PropertyKey>();
            s_GpiPortCurrentStateGroup.Add(SourceGroup.EnabledKey);
            s_EventsAndReportsGroup = new Collection<PropertyKey>();
            s_EventsAndReportsGroup.Add(LlrpEventsAndReportGroup.HoldsEventsAndReportKey);
            s_SetReaderConfigurationGroup = new Collection<PropertyKey>();
            s_SetReaderConfigurationGroup.Add(LlrpTroubleshootGroup.ResetToFactoryDefaultKey);
        }

        internal ApplyPropertyProfileCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger)
            : base(sourcName, command, state, device, logger)
        {
            this.m_detailedErrors = new Dictionary<PropertyKey, CommandError>();
            this.m_propertiesConsumed = new Collection<PropertyKey>();
        }

        private void AddNotificationStateIfKeyPresent(EventNotificationStateEventType state, PropertyKey targetKey, PropertyList clientProfile, ref Collection<EventNotificationState> states, ref Collection<PropertyKey> propertiesUsed)
        {
            if (clientProfile.ContainsKey(targetKey))
            {
                propertiesUsed.Add(targetKey);
                states.Add(new EventNotificationState(state, (bool)clientProfile[targetKey]));
            }
        }

        private void ApplyPropertiesOnDeviceIfAny(PropertyList clientProfile)
        {
            SetReaderConfigurationMessage message = null;
            Collection<PropertyKey> propertiesUsed = null;
            try
            {
                message = this.ConstructReaderConfigurationMessage(clientProfile, out propertiesUsed);
                if (message == null)
                {
                    base.Logger.Info("No LLRP device specific properties to apply on device {0}", new object[] { base.Device.DeviceName });
                    return;
                }
                SetReaderConfigurationResponse response = base.Device.Request(message) as SetReaderConfigurationResponse;
                Util.ThrowIfNull(response, LlrpMessageType.SetReaderConfig);
                Util.ThrowIfFailed(response.Status);
            }
            catch (SensorProviderException exception)
            {
                this.HandleExceptionOnApply(propertiesUsed, exception);
            }
            catch (TimeoutException exception2)
            {
                this.HandleExceptionOnApply(propertiesUsed, exception2);
            }
            this.CopyCollection(propertiesUsed, this.m_propertiesConsumed);
        }

        private AccessReportSpec ConstructAccessReportSpec(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            if (!this.ProfileContainsOneOrMoreKey(clientProfile, s_AccessReportSpecGroup))
            {
                return null;
            }
            propertiesUsed.Add(LlrpAccessReportSpecGroup.TriggerKey);
            string str = (string)clientProfile[LlrpAccessReportSpecGroup.TriggerKey];
            return new AccessReportSpec(Util.GetEnumInstance<AccessReportTrigger>(str));
        }

        private Collection<AirProtocolInventoryCommandSettings> ConstructAirProtocolInventoryCommandSettings(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            Collection<AirProtocolInventoryCommandSettings> collection = new Collection<AirProtocolInventoryCommandSettings>();
            C1G2InventoryCommand item = this.ConstructC1G2InventoryCommand(clientProfile, ref propertiesUsed);
            if (item != null)
            {
                collection.Add(item);
            }
            return collection;
        }

        private AirProtocolSpecificEpcMemorySelectorParameter ConstructAirProtocolSpecificEpcMemorySelectorParameter(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            if (this.ProfileContainsOneOrMoreKey(clientProfile, s_AirProtocolSpecificEpcMemorySelectorParameterGroup))
            {
                object obj2;
                Collection<PropertyKey> keysConsumed = new Collection<PropertyKey>();
                if (this.TryGetObject(clientProfile, LlrpROReportSpecGroup.C1G2EnableCrcKey, keysConsumed, out obj2))
                {
                    bool flag2;
                    bool enableCRC = (bool)obj2;
                    if (this.TryGetObject(clientProfile, LlrpROReportSpecGroup.C1G2EnablePCKey, keysConsumed, out obj2))
                    {
                        flag2 = (bool)obj2;
                    }
                    else
                    {
                        return null;
                    }
                    this.CopyCollection(keysConsumed, propertiesUsed);
                    return new C1G2EpcMemorySelector(enableCRC, flag2);
                }
            }
            return null;
        }

        private AntennaConfiguration ConstructAntennaConfiguration(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            LLRPSourceType type;
            bool flag = false;
            if (Util.IsValidSourceName(base.SourceName, out type))
            {
                flag = type == LLRPSourceType.Antenna;
            }
            if (flag)
            {
                ushort sourceId = Util.GetSourceId(base.SourceName);
                RFReceiver rfReceiver = this.ConstructRFReceiver(clientProfile, ref propertiesUsed);
                RFTransmitter rfTransmitter = this.ConstructRFTransmitter(clientProfile, ref propertiesUsed);
                Collection<AirProtocolInventoryCommandSettings> airProtocolInventoryCommandParameter = this.ConstructAirProtocolInventoryCommandSettings(clientProfile, ref propertiesUsed);
                if (((rfReceiver != null) || (rfTransmitter != null)) || ((airProtocolInventoryCommandParameter != null) && (airProtocolInventoryCommandParameter.Count != 0)))
                {
                    return new AntennaConfiguration(sourceId, rfReceiver, rfTransmitter, airProtocolInventoryCommandParameter);
                }
            }
            return null;
        }

        private AntennaProperties ConstructAntennaProperties(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            LLRPSourceType type;
            bool flag = false;
            if (Util.IsValidSourceName(base.SourceName, out type))
            {
                flag = type == LLRPSourceType.Antenna;
            }
            if (!flag || !this.ProfileContainsOneOrMoreKey(clientProfile, s_AntennaPropertiesGroup))
            {
                return null;
            }
            Collection<PropertyKey> keysConsumed = new Collection<PropertyKey>();
            ushort sourceId = Util.GetSourceId(base.SourceName);
            object result = null;
            short antennaGain = 0;
            if (this.TryGetObject(clientProfile, LlrpAntennaPropertiesGroup.AntennaGainKey, keysConsumed, out result))
            {
                antennaGain = (short)result;
            }
            else
            {
                return null;
            }
            bool isConnected = false;
            if (this.TryGetObject(clientProfile, SourceGroup.EnabledKey, keysConsumed, out result))
            {
                isConnected = (bool)result;
            }
            else
            {
                return null;
            }
            this.CopyCollection(keysConsumed, propertiesUsed);
            return new AntennaProperties(sourceId, isConnected, antennaGain);
        }

        private Collection<C1G2Filter> ConstructC1G2Filters(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            if (this.ProfileContainsOneOrMoreKey(clientProfile, s_C1G2FilterGroup))
            {
                propertiesUsed.Add(LlrpC1G2InventoryCommandGroup.FiltersKey);
                string xml = (string)clientProfile[LlrpC1G2InventoryCommandGroup.FiltersKey];
                if (xml != null)
                {
                    try
                    {
                        return LlrpSerializationHelper.DeserializeFromXmlDataContract<Collection<C1G2Filter>>(xml);
                    }
                    catch (Exception exception)
                    {
                        base.Logger.Error("Error {0} during deserializing the filters on device {1}", new object[] { exception, base.Device.DeviceName });
                        this.m_detailedErrors[LlrpC1G2InventoryCommandGroup.FiltersKey] = new CommandError(ErrorCode.InvalidParameter, exception.Message, ErrorCode.InvalidParameter.Description, null);
                    }
                }
            }
            return null;
        }

        private C1G2InventoryCommand ConstructC1G2InventoryCommand(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            object obj2;
            Collection<PropertyKey> collection = new Collection<PropertyKey>();
            Collection<C1G2Filter> filters = this.ConstructC1G2Filters(clientProfile, ref collection);
            C1G2RFControl rfControl = this.ConstructC1G2RFControl(clientProfile, ref collection);
            C1G2SingulationControl singulationControl = this.ConstructC1G2SingulationControl(clientProfile, ref collection);
            if (((filters == null) && (rfControl == null)) && ((singulationControl == null) && !this.ProfileContainsOneOrMoreKey(clientProfile, s_C1G2InventoryCommandSettingGroup)))
            {
                base.Logger.Info("No properties corresponding to c1g2inventorycommand settings for device {0} to apply", new object[] { base.Device.DeviceName });
                return null;
            }
            bool stateAware = false;
            if (this.TryGetObject(clientProfile, LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareKey, collection, out obj2))
            {
                stateAware = (bool)obj2;
            }
            else
            {
                return null;
            }
            this.CopyCollection(collection, propertiesUsed);
            return new C1G2InventoryCommand(stateAware, filters, rfControl, singulationControl, null);
        }

        private C1G2RFControl ConstructC1G2RFControl(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            if (this.ProfileContainsOneOrMoreKey(clientProfile, s_C1G2RFControlGroup))
            {
                object obj2;
                Collection<PropertyKey> keysConsumed = new Collection<PropertyKey>();
                ushort modeIndex = 0;
                if (this.TryGetObject(clientProfile, LlrpC1G2InventoryCommandGroup.ModeIndexKey, keysConsumed, out obj2))
                {
                    modeIndex = (ushort)obj2;
                    short tari = 0;
                    if (this.TryGetObject(clientProfile, LlrpC1G2InventoryCommandGroup.TariKey, keysConsumed, out obj2))
                    {
                        tari = (short)obj2;
                    }
                    else
                    {
                        return null;
                    }
                    this.CopyCollection(keysConsumed, propertiesUsed);
                    return new C1G2RFControl(modeIndex, tari);
                }
            }
            return null;
        }

        private C1G2SingulationControl ConstructC1G2SingulationControl(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            object obj2;
            Collection<PropertyKey> collection = new Collection<PropertyKey>();
            C1G2TagInventoryStateAwareSingulationAction action = this.ConstructC1G2TagInventoryStateAwareSingulationAction(clientProfile, ref collection);
            if ((action == null) && !this.ProfileContainsOneOrMoreKey(clientProfile, s_C1G2SingulationControlGroup))
            {
                base.Logger.Info("No properties corresponding to C1G2SingulationControl settings for device {0} to apply", new object[] { base.Device.DeviceName });
                return null;
            }
            TagSession enumInstance = TagSession.S0;
            if (this.TryGetObject(clientProfile, LlrpC1G2InventoryCommandGroup.SessionKey, collection, out obj2))
            {
                try
                {
                    enumInstance = Util.GetEnumInstance<TagSession>((string)obj2);
                    ushort tagPopulation = 0;
                    if (this.TryGetObject(clientProfile, LlrpC1G2InventoryCommandGroup.TagPopulationKey, collection, out obj2))
                    {
                        tagPopulation = (ushort)obj2;
                    }
                    else
                    {
                        return null;
                    }
                    uint tagTransitTime = 0;
                    if (this.TryGetObject(clientProfile, LlrpC1G2InventoryCommandGroup.TagTransitTimeKey, collection, out obj2))
                    {
                        tagTransitTime = (uint)obj2;
                    }
                    else
                    {
                        return null;
                    }
                    this.CopyCollection(collection, propertiesUsed);
                    return new C1G2SingulationControl(enumInstance, tagPopulation, tagTransitTime, action);
                }
                catch (Exception exception)
                {
                    base.Logger.Error("Invalid tag session {0} on device {1}. Error {2}", new object[] { obj2, base.Device.DeviceName, exception });
                    return null;
                }
            }
            return null;
        }

        private C1G2TagInventoryStateAwareSingulationAction ConstructC1G2TagInventoryStateAwareSingulationAction(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            if (this.ProfileContainsOneOrMoreKey(clientProfile, s_C1G2TagInventoryStateAwareSingulationActionGroup))
            {
                object obj2;
                Collection<PropertyKey> keysConsumed = new Collection<PropertyKey>();
                TagInventoryState stateA = TagInventoryState.StateA;
                if (this.TryGetObject(clientProfile, LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareSingulationActionStateKey, keysConsumed, out obj2))
                {
                    try
                    {
                        stateA = Util.GetEnumInstance<TagInventoryState>((string)obj2);
                        TagSLState assert = TagSLState.Assert;
                        if (this.TryGetObject(clientProfile, LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareSingulationActionSLFlagKey, keysConsumed, out obj2))
                        {
                            try
                            {
                                assert = Util.GetEnumInstance<TagSLState>((string)obj2);
                                this.CopyCollection(keysConsumed, propertiesUsed);
                                return new C1G2TagInventoryStateAwareSingulationAction(stateA, assert);
                            }
                            catch (Exception exception2)
                            {
                                base.Logger.Error("Invalid tag inventory SL state {0} on device {1}. Error {2}", new object[] { obj2, base.Device.DeviceName, exception2 });
                                return null;
                            }
                        }
                        return null;
                    }
                    catch (Exception exception)
                    {
                        base.Logger.Error("Invalid tag inventory state {0} on device {1}. Error {2}", new object[] { obj2, base.Device.DeviceName, exception });
                        return null;
                    }
                }
            }
            return null;
        }

        private Collection<CustomParameterBase> ConstructCustomParameters(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            //Dictionary<PropertyKey, DevicePropertyMetadata> microsoftSpecificMetadata = null;
            //MicrosoftDevicePropertiesParameter parameter;
            //lock (base.DeviceState)
            //{
            //    microsoftSpecificMetadata = base.DeviceState.MicrosoftSpecificMetadata;
            //}
            //CommandError cmdError = null;
            //if ((microsoftSpecificMetadata == null) && base.TryPopulateReaderCapabilities(out cmdError))
            //{
            //    lock (base.DeviceState)
            //    {
            //        microsoftSpecificMetadata = base.DeviceState.MicrosoftSpecificMetadata;
            //    }
            //}
            //if ((microsoftSpecificMetadata == null) || (microsoftSpecificMetadata.Count == 0))
            //{
            //    return null;
            //}
            Collection<PropertyKey> source = new Collection<PropertyKey>();
            PropertyList profile = new PropertyList(LlrpResources.PropertyProfileName);
            //foreach (PropertyKey key in clientProfile.Keys)
            //{
            //    if (microsoftSpecificMetadata.ContainsKey(key))
            //    {
            //        profile[key] = clientProfile[key];
            //        source.Add(key);
            //    }
            //}
            PropertyList deviceProfile = null;
            Dictionary<int, PropertyList> antennaProfiles = null;
            Dictionary<int, PropertyList> gpiProfiles = null;
            Dictionary<int, PropertyList> gpoProfiles = null;
            if (base.SourceName == null)
            {
                deviceProfile = profile;
            }
            else
            {
                LLRPSourceType type;
                Util.IsValidSourceName(base.SourceName, out type);
                ushort sourceId = Util.GetSourceId(base.SourceName, type);
                switch (type)
                {
                    case LLRPSourceType.Antenna:
                        antennaProfiles = new Dictionary<int, PropertyList>();
                        antennaProfiles.Add(sourceId, profile);
                        goto Label_0152;

                    case LLRPSourceType.GPI:
                        gpiProfiles = new Dictionary<int, PropertyList>();
                        gpiProfiles.Add(sourceId, profile);
                        goto Label_0152;

                    case LLRPSourceType.GPO:
                        gpoProfiles = new Dictionary<int, PropertyList>();
                        gpoProfiles.Add(sourceId, profile);
                        goto Label_0152;
                }
                return null;
            }
        Label_0152:
            //parameter = null;
            try
            {
                //parameter = new MicrosoftDevicePropertiesParameter(new RfidProperties(deviceProfile, antennaProfiles, gpiProfiles, gpoProfiles));
            }
            catch (ArgumentOutOfRangeException exception)
            {
                //base.Logger.Error("Could not create custom parameters for apply property command on the device {0} for the reason being {1}", new object[] { base.Device.DeviceName, exception });
                //foreach (PropertyKey key2 in source)
                //{
                //    this.m_detailedErrors[key2] = new CommandError(ErrorCode.ApplyPropertyProfileFailed, LlrpResources.ApplyPropertyFailedDueToLotOfProperties, ErrorCode.ApplyPropertyProfileFailed.Description, null);
                //}
                //return null;
            }
            finally
            {
                this.CopyCollection(source, propertiesUsed);
            }
            Collection<CustomParameterBase> collection2 = new Collection<CustomParameterBase>();
            //collection2.Add(parameter);
            return collection2;
        }

        private EventsAndReport ConstructEventsAndReport(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            if (!this.ProfileContainsOneOrMoreKey(clientProfile, s_EventsAndReportsGroup))
            {
                return null;
            }
            propertiesUsed.Add(LlrpEventsAndReportGroup.HoldsEventsAndReportKey);
            return new EventsAndReport((bool)clientProfile[LlrpEventsAndReportGroup.HoldsEventsAndReportKey]);
        }

        private GpiPortCurrentState ConstructGpiPortCurrentState(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            bool flag = false;
            LLRPSourceType antenna = LLRPSourceType.Antenna;
            if (Util.IsValidSourceName(base.SourceName, out antenna))
            {
                flag = antenna == LLRPSourceType.GPI;
            }
            if (!flag || !this.ProfileContainsOneOrMoreKey(clientProfile, s_GpiPortCurrentStateGroup))
            {
                return null;
            }
            ushort sourceId = Util.GetSourceId(base.SourceName);
            propertiesUsed.Add(SourceGroup.EnabledKey);
            return new GpiPortCurrentState(sourceId, (bool)clientProfile[SourceGroup.EnabledKey], GpiState.Unknown);
        }

        private GpoWriteData ConstructGpoWriteData(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            bool flag = false;
            LLRPSourceType antenna = LLRPSourceType.Antenna;
            if (Util.IsValidSourceName(base.SourceName, out antenna))
            {
                flag = antenna == LLRPSourceType.GPO;
            }
            if (!flag || !this.ProfileContainsOneOrMoreKey(clientProfile, s_GpoWriteDataGroup))
            {
                return null;
            }
            ushort sourceId = Util.GetSourceId(base.SourceName);
            propertiesUsed.Add(SourceGroup.PortOutputValueKey);
            byte[] buffer = clientProfile[SourceGroup.PortOutputValueKey] as byte[];
            CommandError cmdError = null;
            if (!TryValidatePortOutputValue(buffer, out cmdError))
            {
                this.m_detailedErrors[SourceGroup.PortOutputValueKey] = cmdError;
                return null;
            }
            return new GpoWriteData(sourceId, GetPortOutputValue(buffer));
        }

        private KeepAliveSpec ConstructKeepaliveSpec(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            if (this.ProfileContainsOneOrMoreKey(clientProfile, s_KeepaliveSpecGroup))
            {
                object obj2;
                Collection<PropertyKey> keysConsumed = new Collection<PropertyKey>();
                KeepAliveTriggerType @null = KeepAliveTriggerType.Null;
                if (this.TryGetObject(clientProfile, LlrpKeepAliveSpecGroup.SendKeepaliveKey, keysConsumed, out obj2))
                {
                    uint num;
                    if ((bool)obj2)
                    {
                        @null = KeepAliveTriggerType.Periodic;
                    }
                    if (this.TryGetObject(clientProfile, LlrpKeepAliveSpecGroup.TimeIntervalKey, keysConsumed, out obj2))
                    {
                        num = (uint)obj2;
                    }
                    else
                    {
                        return null;
                    }
                    this.CopyCollection(keysConsumed, propertiesUsed);
                    return new KeepAliveSpec(@null, num);
                }
            }
            return null;
        }

        private SetReaderConfigurationMessage ConstructReaderConfigurationMessage(PropertyList clientProfile, out Collection<PropertyKey> propertiesUsed)
        {
            propertiesUsed = new Collection<PropertyKey>();
            ReaderEventNotificationSpec readerEvent = this.ConstructReaderEventNotificationSpec(clientProfile, ref propertiesUsed);
            AntennaProperties item = this.ConstructAntennaProperties(clientProfile, ref propertiesUsed);
            AntennaConfiguration configuration = this.ConstructAntennaConfiguration(clientProfile, ref propertiesUsed);
            ROReportSpec roReport = this.ConstructROReportSpec(clientProfile, ref propertiesUsed);
            AccessReportSpec accessReport = this.ConstructAccessReportSpec(clientProfile, ref propertiesUsed);
            KeepAliveSpec keepAlive = this.ConstructKeepaliveSpec(clientProfile, ref propertiesUsed);
            GpoWriteData data = this.ConstructGpoWriteData(clientProfile, ref propertiesUsed);
            GpiPortCurrentState state = this.ConstructGpiPortCurrentState(clientProfile, ref propertiesUsed);
            EventsAndReport eventsAndReport = this.ConstructEventsAndReport(clientProfile, ref propertiesUsed);
            Collection<CustomParameterBase> customs = this.ConstructCustomParameters(clientProfile, ref propertiesUsed);
            if ((propertiesUsed.Count == 0) && !this.ProfileContainsOneOrMoreKey(clientProfile, s_SetReaderConfigurationGroup))
            {
                return null;
            }
            bool resetToFactoryDefault = false;
            if (clientProfile.ContainsKey(LlrpTroubleshootGroup.ResetToFactoryDefaultKey))
            {
                propertiesUsed.Add(LlrpTroubleshootGroup.ResetToFactoryDefaultKey);
                resetToFactoryDefault = (bool)clientProfile[LlrpTroubleshootGroup.ResetToFactoryDefaultKey];
            }
            Collection<AntennaProperties> antennaProperties = null;
            if (item != null)
            {
                antennaProperties = new Collection<AntennaProperties>();
                antennaProperties.Add(item);
            }
            Collection<AntennaConfiguration> antennaConfig = null;
            if (configuration != null)
            {
                antennaConfig = new Collection<AntennaConfiguration>();
                antennaConfig.Add(configuration);
            }
            Collection<GpoWriteData> gpoWriteData = null;
            if (data != null)
            {
                gpoWriteData = new Collection<GpoWriteData>();
                gpoWriteData.Add(data);
            }
            Collection<GpiPortCurrentState> gpiCurrentStates = null;
            if (state != null)
            {
                gpiCurrentStates = new Collection<GpiPortCurrentState>();
                gpiCurrentStates.Add(state);
            }
            return new SetReaderConfigurationMessage(resetToFactoryDefault, readerEvent, antennaProperties, antennaConfig, roReport, accessReport, keepAlive, gpiCurrentStates, gpoWriteData, eventsAndReport, customs);
        }

        private ReaderEventNotificationSpec ConstructReaderEventNotificationSpec(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            if (!this.ProfileContainsOneOrMoreKey(clientProfile, s_EventNotificationGroup))
            {
                return null;
            }
            Collection<EventNotificationState> states = new Collection<EventNotificationState>();
            this.AddNotificationStateIfKeyPresent(EventNotificationStateEventType.AISpecEvent, LlrpReaderEventNotificationSpecGroup.AISpecEndEventKey, clientProfile, ref states, ref propertiesUsed);
            this.AddNotificationStateIfKeyPresent(EventNotificationStateEventType.AISpecWithSingulation, LlrpReaderEventNotificationSpecGroup.AISpecEndWithSingulationEventKey, clientProfile, ref states, ref propertiesUsed);
            this.AddNotificationStateIfKeyPresent(EventNotificationStateEventType.AntennaEvent, LlrpReaderEventNotificationSpecGroup.AntennaEventKey, clientProfile, ref states, ref propertiesUsed);
            this.AddNotificationStateIfKeyPresent(EventNotificationStateEventType.Gpi, LlrpReaderEventNotificationSpecGroup.GpiEventKey, clientProfile, ref states, ref propertiesUsed);
            this.AddNotificationStateIfKeyPresent(EventNotificationStateEventType.HoppingToNextChannel, LlrpReaderEventNotificationSpecGroup.HoppingEventKey, clientProfile, ref states, ref propertiesUsed);
            this.AddNotificationStateIfKeyPresent(EventNotificationStateEventType.ReaderExceptionEvent, LlrpReaderEventNotificationSpecGroup.ReaderExceptionEventKey, clientProfile, ref states, ref propertiesUsed);
            this.AddNotificationStateIfKeyPresent(EventNotificationStateEventType.ReportBufferFillWarning, LlrpReaderEventNotificationSpecGroup.BufferFillWarningEventKey, clientProfile, ref states, ref propertiesUsed);
            this.AddNotificationStateIfKeyPresent(EventNotificationStateEventType.RFSurveyEvent, LlrpReaderEventNotificationSpecGroup.RFSurveyEventKey, clientProfile, ref states, ref propertiesUsed);
            this.AddNotificationStateIfKeyPresent(EventNotificationStateEventType.ROSpec, LlrpReaderEventNotificationSpecGroup.ROSpecEventKey, clientProfile, ref states, ref propertiesUsed);
            return new ReaderEventNotificationSpec(states);
        }

        private RFReceiver ConstructRFReceiver(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            if (!this.ProfileContainsOneOrMoreKey(clientProfile, s_RFReceiverGroup))
            {
                return null;
            }
            propertiesUsed.Add(LlrpAntennaConfigurationGroup.ReceiverSensitivityIndexKey);
            return new RFReceiver((ushort)clientProfile[LlrpAntennaConfigurationGroup.ReceiverSensitivityIndexKey]);
        }

        private RFTransmitter ConstructRFTransmitter(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            if (this.ProfileContainsOneOrMoreKey(clientProfile, s_RFTransmitterGroup))
            {
                object obj2;
                Collection<PropertyKey> keysConsumed = new Collection<PropertyKey>();
                if (this.TryGetObject(clientProfile, LlrpAntennaConfigurationGroup.ChannelIndexKey, keysConsumed, out obj2))
                {
                    ushort num2;
                    ushort num3;
                    ushort channelIndex = (ushort)obj2;
                    if (this.TryGetObject(clientProfile, LlrpAntennaConfigurationGroup.HopTableIdKey, keysConsumed, out obj2))
                    {
                        num2 = (ushort)obj2;
                    }
                    else
                    {
                        return null;
                    }
                    if (this.TryGetObject(clientProfile, LlrpAntennaConfigurationGroup.TransmitPowerIndexKey, keysConsumed, out obj2))
                    {
                        num3 = (ushort)obj2;
                    }
                    else
                    {
                        return null;
                    }
                    this.CopyCollection(keysConsumed, propertiesUsed);
                    return new RFTransmitter(num3, num2, channelIndex);
                }
            }
            return null;
        }

        private ROReportSpec ConstructROReportSpec(PropertyList clientProfile, ref Collection<PropertyKey> propertiesUsed)
        {
            object obj2;
            bool flag;
            bool flag2;
            bool flag3;
            bool flag4;
            bool flag5;
            bool flag6;
            bool flag7;
            bool flag8;
            bool flag9;
            bool flag10;
            string str;
            ushort num;
            Collection<PropertyKey> collection = new Collection<PropertyKey>();
            AirProtocolSpecificEpcMemorySelectorParameter item = this.ConstructAirProtocolSpecificEpcMemorySelectorParameter(clientProfile, ref collection);
            if (((this.ProfileContainsOneOrMoreKey(clientProfile, s_ROReportSpecGroup) || this.ProfileContainsOneOrMoreKey(clientProfile, s_TagReportContentSelector)) || (item != null)) && this.TryGetObject(clientProfile, LlrpROReportSpecGroup.EnableAccessSpecIdKey, collection, out obj2))
            {
                flag = (bool)obj2;
            }
            else
            {
                return null;
            }
            if (this.TryGetObject(clientProfile, LlrpROReportSpecGroup.EnableAntennaIdKey, collection, out obj2))
            {
                flag2 = (bool)obj2;
            }
            else
            {
                return null;
            }
            if (this.TryGetObject(clientProfile, LlrpROReportSpecGroup.EnableChannelIndexKey, collection, out obj2))
            {
                flag3 = (bool)obj2;
            }
            else
            {
                return null;
            }
            if (this.TryGetObject(clientProfile, LlrpROReportSpecGroup.EnableFirstSeenTimestampKey, collection, out obj2))
            {
                flag4 = (bool)obj2;
            }
            else
            {
                return null;
            }
            if (this.TryGetObject(clientProfile, LlrpROReportSpecGroup.EnableInventoryParameterSpecIdKey, collection, out obj2))
            {
                flag5 = (bool)obj2;
            }
            else
            {
                return null;
            }
            if (this.TryGetObject(clientProfile, LlrpROReportSpecGroup.EnableLastSeenTimestampKey, collection, out obj2))
            {
                flag6 = (bool)obj2;
            }
            else
            {
                return null;
            }
            if (this.TryGetObject(clientProfile, LlrpROReportSpecGroup.EnablePeakRssiKey, collection, out obj2))
            {
                flag7 = (bool)obj2;
            }
            else
            {
                return null;
            }
            if (this.TryGetObject(clientProfile, LlrpROReportSpecGroup.EnableROSpecIdKey, collection, out obj2))
            {
                flag8 = (bool)obj2;
            }
            else
            {
                return null;
            }
            if (this.TryGetObject(clientProfile, LlrpROReportSpecGroup.EnableSpecIndexKey, collection, out obj2))
            {
                flag9 = (bool)obj2;
            }
            else
            {
                return null;
            }
            if (this.TryGetObject(clientProfile, LlrpROReportSpecGroup.EnableTagSeenCountKey, collection, out obj2))
            {
                flag10 = (bool)obj2;
            }
            else
            {
                return null;
            }
            if (this.TryGetObject(clientProfile, LlrpROReportSpecGroup.TriggerKey, collection, out obj2))
            {
                str = (string)obj2;
            }
            else
            {
                return null;
            }
            if (this.TryGetObject(clientProfile, LlrpROReportSpecGroup.NumberOfTagReportDataKey, collection, out obj2))
            {
                num = (ushort)obj2;
            }
            else
            {
                return null;
            }
            this.CopyCollection(collection, propertiesUsed);
            Collection<AirProtocolSpecificEpcMemorySelectorParameter> memorySelector = null;
            if (item != null)
            {
                memorySelector = new Collection<AirProtocolSpecificEpcMemorySelectorParameter>();
                memorySelector.Add(item);
            }
            return new ROReportSpec(Util.GetEnumInstance<ROReportTrigger>(str), num, new TagReportContentSelector(flag8, flag9, flag5, flag2, flag3, flag7, flag4, flag6, flag10, flag, memorySelector), null);
        }

        private void CopyCollection(Collection<PropertyKey> source, Collection<PropertyKey> destination)
        {
            foreach (PropertyKey key in source)
            {
                destination.Add(key);
            }
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            base.Logger.Info("Executing apply property profile command on device {0}", new object[] { base.Device.DeviceName });
            PropertyList providerMaintainedPropertiesCache = new PropertyList(LlrpResources.PropertyProfileName);
            ApplyPropertyListCommand command = base.Command as ApplyPropertyListCommand;
            command.Response = new ApplyPropertyListResponse();
            PropertyList propertyProfile = command.PropertyProfile;
            if ((propertyProfile != null) && (propertyProfile.Count > 0))
            {
                this.HandleCleanupSpec(providerMaintainedPropertiesCache, propertyProfile);
                if (propertyProfile.ContainsKey(LlrpManagementGroup.SynchronousCommandInventoryDurationKey))
                {
                    this.m_propertiesConsumed.Add(LlrpManagementGroup.SynchronousCommandInventoryDurationKey);
                    providerMaintainedPropertiesCache[LlrpManagementGroup.SynchronousCommandInventoryDurationKey] = propertyProfile[LlrpManagementGroup.SynchronousCommandInventoryDurationKey];
                }
                if (propertyProfile.ContainsKey(LlrpManagementGroup.SynchronousCommandInventoryOperationCountKey))
                {
                    this.m_propertiesConsumed.Add(LlrpManagementGroup.SynchronousCommandInventoryOperationCountKey);
                    providerMaintainedPropertiesCache[LlrpManagementGroup.SynchronousCommandInventoryOperationCountKey] = propertyProfile[LlrpManagementGroup.SynchronousCommandInventoryOperationCountKey];
                }
                if (propertyProfile.ContainsKey(LlrpManagementGroup.LlrpMessageTimeoutKey))
                {
                    this.m_propertiesConsumed.Add(LlrpManagementGroup.LlrpMessageTimeoutKey);
                    base.Device.MessageTimeout = (int)propertyProfile[LlrpManagementGroup.LlrpMessageTimeoutKey];
                    providerMaintainedPropertiesCache[LlrpManagementGroup.LlrpMessageTimeoutKey] = base.Device.MessageTimeout;
                }
                this.HandleEventMode(providerMaintainedPropertiesCache, propertyProfile);
                this.HandleInventoryROSpec(providerMaintainedPropertiesCache, propertyProfile);
                this.HandleInventoryAccessSpec(providerMaintainedPropertiesCache, propertyProfile);
                this.HandleDuplicateElimination(providerMaintainedPropertiesCache, propertyProfile);
                if (this.m_propertiesConsumed.Count < propertyProfile.Count)
                {
                    this.ApplyPropertiesOnDeviceIfAny(propertyProfile);
                }
                foreach (PropertyKey key in propertyProfile.Keys)
                {
                    if (!this.m_propertiesConsumed.Contains(key))
                    {
                        this.m_detailedErrors[key] = new CommandError(ErrorCode.PropertyInvalid, LlrpResources.PropertyNotSupportedOrDependentPropertyMissing, ErrorCode.PropertyInvalid.Description, null);
                    }
                }
                lock (base.DeviceState)
                {
                    PropertyList providerMaintainedProperties = base.DeviceState.ProviderMaintainedProperties;
                    Util.AppendPropertyProfile(providerMaintainedPropertiesCache, ref providerMaintainedProperties);
                }
            }
            if (this.m_detailedErrors.Count == 0)
            {
                return new ResponseEventArgs(command);
            }
            return new ResponseEventArgs(command, new ApplyPropertyListFailedError(command.PropertyProfile, this.m_detailedErrors));
        }

        private CommandError GetCommandError(Exception ex)
        {
            if (ex is SensorProviderException)
            {
                return new CommandError(ErrorCode.ApplyPropertyListFailed, (SensorProviderException)ex, ex.Message, ErrorCode.ApplyPropertyListFailed.Description, null);
            }
            return new CommandError(ErrorCode.ApplyPropertyListFailed, ex.Message, ErrorCode.ApplyPropertyListFailed.Description, null);
        }

        private static bool GetPortOutputValue(byte[] value)
        {
            return (value[0] != 0);
        }

        private void HandleCleanupSpec(PropertyList providerMaintainedPropertiesCache, PropertyList clientprofile)
        {
            CommandError cmdError = null;
            if (clientprofile.ContainsKey(LlrpTroubleshootGroup.CleanupSpecsKey))
            {
                this.m_propertiesConsumed.Add(LlrpTroubleshootGroup.CleanupSpecsKey);
                if ((bool)clientprofile[LlrpTroubleshootGroup.CleanupSpecsKey])
                {
                    if (!base.CleanupSpecs(out cmdError))
                    {
                        this.m_detailedErrors[LlrpTroubleshootGroup.CleanupSpecsKey] = cmdError;
                        if (cmdError.ErrorCode.Equals(LlrpErrorCode.DeletingROSpecFailed))
                        {
                            return;
                        }
                        lock (base.DeviceState)
                        {
                            base.DeviceState.IsInventoryOn = false;
                            return;
                        }
                    }
                    lock (base.DeviceState)
                    {
                        base.DeviceState.IsInventoryOn = false;
                    }
                }
            }
        }

        private void HandleDuplicateElimination(PropertyList providerMaintainedPropertiesCache, PropertyList clientprofile)
        {
            PropertyKey duplicateEliminationTimeKey = TagReadGroup.DuplicateEliminationTimeKey;
            if (clientprofile.ContainsKey(duplicateEliminationTimeKey) && !this.CustomMetadata.ContainsKey(duplicateEliminationTimeKey))
            {
                this.m_propertiesConsumed.Add(duplicateEliminationTimeKey);
                try
                {
                    long num = (long)clientprofile[duplicateEliminationTimeKey];
                    providerMaintainedPropertiesCache[duplicateEliminationTimeKey] = num;
                    lock (base.DeviceState)
                    {
                        base.DeviceState.DuplicateEliminationHandler.DupElimTimeInMillis = num;
                    }
                }
                catch (Exception exception)
                {
                    this.m_detailedErrors[duplicateEliminationTimeKey] = new CommandError(ErrorCode.ApplyPropertyListFailed, exception.Message, ErrorCode.ApplyPropertyListFailed.Description, null);
                }
            }
        }

        private void HandleEventMode(PropertyList providerMaintainedPropertiesCache, PropertyList clientprofile)
        {
            if (clientprofile.ContainsKey(NotificationGroup.EventModeKey))
            {
                bool flag = false;
                lock (base.DeviceState)
                {
                    flag = base.DeviceState.IsInventoryOn;
                }
                this.m_propertiesConsumed.Add(NotificationGroup.EventModeKey);
                PropertyKey eventModeKey = NotificationGroup.EventModeKey;
                base.Logger.Verbose("Event mode property in apply profile");
                bool flag2 = (bool)clientprofile[eventModeKey];
                if (flag2 == flag)
                {
                    base.Logger.Info("Current inventory mode : {0} is the same as desired event mode. No action taken", new object[] { flag });
                    lock (base.DeviceState)
                    {
                        base.DeviceState.IsInventoryOn = flag2;
                        return;
                    }
                }
                if (flag2)
                {
                    providerMaintainedPropertiesCache[eventModeKey] = flag2;
                }
                else
                {
                    base.Logger.Info("Disabling Event Mode");
                    ResponseEventArgs args = new StopInventoryCommandHandler(base.SourceName, new StopInventoryCommand(), base.DeviceState, base.Device, base.Logger).ExecuteCommand();
                    if (args.CommandError == null)
                    {
                        flag = false;
                    }
                    else
                    {
                        this.m_detailedErrors[eventModeKey] = args.CommandError;
                    }
                }
            }
        }

        private void HandleExceptionOnApply(Collection<PropertyKey> keysConsumed, Exception ex)
        {
            base.Logger.Error("Error {0} while applying device property on device {1}", new object[] { ex, base.Device.DeviceName });
            if (keysConsumed != null)
            {
                foreach (PropertyKey key in keysConsumed)
                {
                    this.m_detailedErrors[key] = this.GetCommandError(ex);
                }
            }
        }

        private void HandleInventoryAccessSpec(PropertyList providerMaintainedPropertiesCache, PropertyList clientprofile)
        {
            if (clientprofile.ContainsKey(NotificationGroup.InventoryAccessSpecKey))
            {
                bool flag = false;
                lock (base.DeviceState)
                {
                    flag = base.DeviceState.IsInventoryOn;
                }
                PropertyKey item = NotificationGroup.InventoryAccessSpecKey;
                this.m_propertiesConsumed.Add(item);
                if (flag)
                {
                    base.Logger.Warning("Not Changing access spec on the device as the event mode could not be set to false");
                    this.m_detailedErrors[item] = new CommandError(ErrorCode.ApplyPropertyListFailed, LlrpResources.CantChangeSpecificationAsExistingSpecificationCouldNotBeDeleted, ErrorCode.ApplyPropertyListFailed.Description, null);
                }
                else
                {
                    AccessSpec spec = null;
                    string xml = (string)clientprofile[item];
                    try
                    {
                        if (xml != null)
                        {
                            spec = LlrpSerializationHelper.DeserializeFromXmlDataContract<AccessSpec>(xml);
                        }
                        providerMaintainedPropertiesCache[item] = xml;
                        lock (base.DeviceState)
                        {
                            base.DeviceState.InventoryAccessSpec = spec;
                        }
                    }
                    catch (Exception exception)
                    {
                        base.Logger.Error("Error {0} deserializing access spec {1} on the device {2}", new object[] { exception, xml, base.Device.DeviceName });
                        this.m_detailedErrors[item] = new CommandError(ErrorCode.InvalidParameter, exception.Message, ErrorCode.InvalidParameter.Description, null);
                    }
                }
            }
        }

        private void HandleInventoryROSpec(PropertyList providerMaintainedPropertiesCache, PropertyList clientprofile)
        {
            if (clientprofile.ContainsKey(NotificationGroup.InventoryROSpecKey))
            {
                bool flag = false;
                lock (base.DeviceState)
                {
                    flag = base.DeviceState.IsInventoryOn;
                }
                PropertyKey item = NotificationGroup.InventoryROSpecKey;
                this.m_propertiesConsumed.Add(item);
                if (flag)
                {
                    base.Logger.Warning("Not Changing ro spec on the device as the event mode could not be set to false");
                    this.m_detailedErrors[item] = new CommandError(ErrorCode.ApplyPropertyListFailed, LlrpResources.CantChangeSpecificationAsExistingSpecificationCouldNotBeDeleted, ErrorCode.ApplyPropertyListFailed.Description, null);
                }
                else
                {
                    ROSpec spec = null;
                    string roSpecInXml = (string)clientprofile[item];
                    try
                    {
                        spec = Util.ValidateAndGetInventoryROSpec(roSpecInXml);
                        providerMaintainedPropertiesCache[item] = roSpecInXml;
                        lock (base.DeviceState)
                        {
                            base.DeviceState.ROSpec = spec;
                        }
                    }
                    catch (Exception exception)
                    {
                        base.Logger.Error("Error {0} while deserializing ro spec {1} on the device {2}", new object[] { exception, roSpecInXml, base.Device.DeviceName });
                        this.m_detailedErrors[item] = new CommandError(ErrorCode.InvalidParameter, exception.Message, ErrorCode.InvalidParameter.Description, null);
                    }
                }
            }
        }

        private bool ProfileContainsOneOrMoreKey(PropertyList profile, Collection<PropertyKey> keysToCheck)
        {
            foreach (PropertyKey key in keysToCheck)
            {
                if (profile.ContainsKey(key))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TryGetObject(PropertyList profileToApply, PropertyKey keyToCheck, Collection<PropertyKey> keysConsumed, out object result)
        {
            result = null;
            if (profileToApply.ContainsKey(keyToCheck))
            {
                keysConsumed.Add(keyToCheck);
                result = profileToApply[keyToCheck];
                return true;
            }
            if (this.m_profileFromDevice == null)
            {
                CommandError cmdError = null;
                if (!base.GetReaderConfigurationAndCreateProfile(out cmdError, ref this.m_profileFromDevice))
                {
                    base.Logger.Warning("Could not retreive profile from device {0}. Which is required to create the composite object", new object[] { base.Device.DeviceName });
                    this.m_profileFromDevice = new PropertyList(LlrpResources.PropertyProfileName);
                }
            }
            if (this.m_profileFromDevice.ContainsKey(keyToCheck))
            {
                result = this.m_profileFromDevice[keyToCheck];
                return true;
            }
            base.Logger.Error("Could not get the property {0} for device {1} in cache, thus not creating the concerned group", new object[] { keyToCheck, base.Device.DeviceName });
            return false;
        }

        private static bool TryValidatePortOutputValue(byte[] value, out CommandError cmdError)
        {
            cmdError = null;
            if (((value != null) && (value.Length == 1)) && ((value[0] == 0) || (value[0] == 1)))
            {
                return true;
            }
            cmdError = new CommandError(ErrorCode.InvalidParameter, LlrpResources.InvalidPortOutputValue, ErrorCode.InvalidParameter.Description, null);
            return false;
        }

        private Dictionary<PropertyKey, DevicePropertyMetadata> CustomMetadata
        {
            get
            {
                if (this.m_customMetadata == null)
                {
                    this.m_customMetadata = base.GetCustomMetadata();
                }
                return this.m_customMetadata;
            }
        }

        internal override bool IsConcurrentToInventoryOperation
        {
            get
            {
                bool flag = true;
                ApplyPropertyListCommand command = (ApplyPropertyListCommand)base.Command;
                PropertyList propertyProfile = command.PropertyProfile;
                bool flag2 = false;
                bool flag3 = false;
                bool flag4 = false;
                bool flag5 = false;
                bool flag6 = false;
                lock (base.DeviceState)
                {
                    flag3 = base.DeviceState.IsInventoryOn;
                }
                flag2 = flag3;
                if (propertyProfile != null)
                {
                    if (propertyProfile.ContainsKey(NotificationGroup.EventModeKey))
                    {
                        flag2 = (bool)propertyProfile[NotificationGroup.EventModeKey];
                    }
                    if (propertyProfile.ContainsKey(LlrpTroubleshootGroup.CleanupSpecsKey) && ((bool)propertyProfile[LlrpTroubleshootGroup.CleanupSpecsKey]))
                    {
                        flag6 = true;
                    }
                }
                if ((propertyProfile != null) && (propertyProfile.ContainsKey(NotificationGroup.InventoryAccessSpecKey) || propertyProfile.ContainsKey(NotificationGroup.InventoryROSpecKey)))
                {
                    flag5 = true;
                }
                if (flag2 != flag3)
                {
                    flag4 = true;
                }
                return ((!flag2 || ((!flag4 && !flag5) && !flag6)) && flag);
            }
        }
    }
}
