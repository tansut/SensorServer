namespace Kalitte.Sensors.Rfid.Llrp.Helpers
{
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Configuration;
    using Kalitte.Sensors.Configuration;
    using Kalitte.Sensors.Exceptions;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Rfid.Llrp.Configuration;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;
    using Kalitte.Sensors.Rfid.Core;
    using Kalitte.Sensors.Rfid.Utilities;

    internal class Util
    {
        internal static byte[] GetTagData(C1G2ReadOPSpecResult readResult)
        {
            return BitHelper.GetByte(readResult.GetReadData());
        }

        internal static Dictionary<PropertyKey, DevicePropertyMetadata> GetStandardDeviceMetadata(GetReaderCapabilitiesResponse response)
        {
            bool writable = true;
            bool flag2 = true;
            bool flag3 = true;
            if (response != null)
            {
                if (response.GeneralDeviceCapabilities != null)
                {
                    GeneralDeviceCapabilities generalDeviceCapabilities = response.GeneralDeviceCapabilities;
                    writable = generalDeviceCapabilities.CanSetAntennaProperties;
                    flag2 = (generalDeviceCapabilities.ReceiveTableEntries != null) && (generalDeviceCapabilities.ReceiveTableEntries.Count > 1);
                }
                if (((response.RegulatoryCapabilities != null) && (response.RegulatoryCapabilities.UhfBandCapabilities != null)) && (response.RegulatoryCapabilities.UhfBandCapabilities.FrequencyInformation != null))
                {
                    flag3 = response.RegulatoryCapabilities.UhfBandCapabilities.FrequencyInformation.IsHopping;
                }
            }
            Dictionary<PropertyKey, DevicePropertyMetadata> dictionary = new Dictionary<PropertyKey, DevicePropertyMetadata>();
            dictionary[LlrpGeneralCapabilitiesGroup.AntennaSensitivityMaximumIndexKey] = LlrpGeneralCapabilitiesGroup.AntennaSensitivityMaximumIndexMetadata;
            dictionary[LlrpGeneralCapabilitiesGroup.AntennaSensitivityMinimumIndexKey] = LlrpGeneralCapabilitiesGroup.AntennaSensitivityMinimumIndexMetadata;
            dictionary[LlrpGeneralCapabilitiesGroup.CanSetAntennaPropertiesKey] = LlrpGeneralCapabilitiesGroup.CanSetAntennaPropertiesMetadata;
            dictionary[LlrpGeneralCapabilitiesGroup.HasUtcClockCapabilityKey] = LlrpGeneralCapabilitiesGroup.HasUtcClockCapabilityMetadata;
            dictionary[LlrpGeneralCapabilitiesGroup.MaxAntennaSupportedKey] = LlrpGeneralCapabilitiesGroup.MaxAntennaSupportedMetadata;
            dictionary[LlrpGeneralCapabilitiesGroup.ModelNameKey] = LlrpGeneralCapabilitiesGroup.ModelNameMetadata;
            dictionary[LlrpGeneralCapabilitiesGroup.NumberOfGpiKey] = LlrpGeneralCapabilitiesGroup.NumberOfGpiMetadata;
            dictionary[LlrpGeneralCapabilitiesGroup.NumberOfGpoKey] = LlrpGeneralCapabilitiesGroup.NumberOfGPOMetadata;
            dictionary[LlrpGeneralCapabilitiesGroup.ReceiveSensitivityTableKey] = LlrpGeneralCapabilitiesGroup.ReceiveSensitivityTableMetadata;
            dictionary[LlrpCapabilitiesGroup.CanDoRFSurveyKey] = LlrpCapabilitiesGroup.CanDoRFSurveyMetadata;
            dictionary[LlrpCapabilitiesGroup.CanDOTagInventoryStateAwareSingulationKey] = LlrpCapabilitiesGroup.CanDOTagInventoryStateAwareSingulationMetadata;
            dictionary[LlrpCapabilitiesGroup.CanReportBufferFillWarningKey] = LlrpCapabilitiesGroup.CanReportBufferFillWarningMetadata;
            dictionary[LlrpCapabilitiesGroup.ClientRequestOperationSpecTimeoutKey] = LlrpCapabilitiesGroup.ClientRequestOperationSpecTimeoutMetadata;
            dictionary[LlrpCapabilitiesGroup.MaximumAccessSpecKey] = LlrpCapabilitiesGroup.MaximumAccessSpecMetadata;
            dictionary[LlrpCapabilitiesGroup.MaximumInventoryPerAIKey] = LlrpCapabilitiesGroup.MaximumInventoryPerAIMetadata;
            dictionary[LlrpCapabilitiesGroup.MaximumOperationSpecPerAccessSpecKey] = LlrpCapabilitiesGroup.MaximumOperationSpecPerAccessSpecMetadata;
            dictionary[LlrpCapabilitiesGroup.MaximumPriorityLevelSupportedKey] = LlrpCapabilitiesGroup.MaximumPriorityLevelSupportedMetadata;
            dictionary[LlrpCapabilitiesGroup.MaximumROSpecKey] = LlrpCapabilitiesGroup.MaximumROSpecMetadata;
            dictionary[LlrpCapabilitiesGroup.MaximumSpecsPerROSpecKey] = LlrpCapabilitiesGroup.MaximumSpecsPerROSpecMetadata;
            dictionary[LlrpCapabilitiesGroup.SupportsClientOperationSpecKey] = LlrpCapabilitiesGroup.SupportsClientOperationSpecMetadata;
            dictionary[LlrpCapabilitiesGroup.SupportsEventAndReportHoldingKey] = LlrpCapabilitiesGroup.SupportsEventAndReportHoldingMetadata;
            dictionary[LlrpRegulatoryCapabilitiesGroup.CountryCodeKey] = LlrpRegulatoryCapabilitiesGroup.CountryCodeMetadata;
            dictionary[LlrpRegulatoryCapabilitiesGroup.FixedFrequencyTableKey] = LlrpRegulatoryCapabilitiesGroup.FixedFrequencyTableMetadata;
            if (flag3)
            {
                dictionary[LlrpRegulatoryCapabilitiesGroup.FrequencyHopTableKey] = LlrpRegulatoryCapabilitiesGroup.FrequencyHopTableMetadata;
            }
            dictionary[LlrpRegulatoryCapabilitiesGroup.HoppingKey] = LlrpRegulatoryCapabilitiesGroup.HoppingMetadata;
            dictionary[LlrpRegulatoryCapabilitiesGroup.RFModeTableKey] = LlrpRegulatoryCapabilitiesGroup.RFModeTableMetadata;
            dictionary[LlrpRegulatoryCapabilitiesGroup.TransmitPowerTableKey] = LlrpRegulatoryCapabilitiesGroup.TransmitPowerTableMetadata;
            dictionary[LlrpC1G2CapabilitiesGroup.MaximumSelectFiltersPerQueryKey] = LlrpC1G2CapabilitiesGroup.MaximumSelectFiltersPerQueryMetadata;
            dictionary[LlrpC1G2CapabilitiesGroup.SupportsBlockEraseKey] = LlrpC1G2CapabilitiesGroup.SupportsBlockEraseMetadata;
            dictionary[LlrpC1G2CapabilitiesGroup.SupportsBlockWriteKey] = LlrpC1G2CapabilitiesGroup.SupportsBlockWriteMetadata;
            dictionary[LlrpConfigurationStateGroup.StateKey] = LlrpConfigurationStateGroup.StateMetadata;
            dictionary[LlrpKeepAliveSpecGroup.SendKeepaliveKey] = LlrpKeepAliveSpecGroup.SendKeepaliveMetadata;
            dictionary[LlrpKeepAliveSpecGroup.TimeIntervalKey] = LlrpKeepAliveSpecGroup.TimeIntervalMetatdata;
            dictionary[LlrpEventsAndReportGroup.HoldsEventsAndReportKey] = LlrpEventsAndReportGroup.HoldsEventsAndReportMetadata;
            dictionary[LlrpAntennaPropertiesGroup.AntennaGainKey] = new DevicePropertyMetadata(typeof(short), LlrpResources.AntennaGainDescription, SensorPropertyRelation.Source, (short)0, writable, false, false, false);
            dictionary[LlrpAntennaConfigurationGroup.ChannelIndexKey] = LlrpAntennaConfigurationGroup.ChannelIndexMetadata;
            if (flag3)
            {
                dictionary[LlrpAntennaConfigurationGroup.HopTableIdKey] = LlrpAntennaConfigurationGroup.HopTableIdMetadata;
            }
            dictionary[LlrpAntennaConfigurationGroup.ReceiverSensitivityIndexKey] = new DevicePropertyMetadata(typeof(ushort), LlrpResources.ReceiverSensitivityIndexDescription, SensorPropertyRelation.Source, (ushort)0, flag2, false, false, false);
            dictionary[LlrpAntennaConfigurationGroup.TransmitPowerIndexKey] = LlrpAntennaConfigurationGroup.TransmitPowerIndexMetadata;
            dictionary[LlrpC1G2InventoryCommandGroup.FiltersKey] = LlrpC1G2InventoryCommandGroup.FiltersMetadata;
            dictionary[LlrpC1G2InventoryCommandGroup.ModeIndexKey] = LlrpC1G2InventoryCommandGroup.ModeIndexMetadata;
            dictionary[LlrpC1G2InventoryCommandGroup.SessionKey] = LlrpC1G2InventoryCommandGroup.SessionMetadata;
            dictionary[LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareKey] = LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareMetadata;
            dictionary[LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareSingulationActionSLFlagKey] = LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareSingulationActionSLFlagMetadata;
            dictionary[LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareSingulationActionStateKey] = LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareSingulationActionStateMetadata;
            dictionary[LlrpC1G2InventoryCommandGroup.TagPopulationKey] = LlrpC1G2InventoryCommandGroup.TagPopulationMetadata;
            dictionary[LlrpC1G2InventoryCommandGroup.TagTransitTimeKey] = LlrpC1G2InventoryCommandGroup.TagTransitTimeMetadata;
            dictionary[LlrpC1G2InventoryCommandGroup.TariKey] = LlrpC1G2InventoryCommandGroup.TariMetadata;
            dictionary[LlrpROReportSpecGroup.C1G2EnableCrcKey] = LlrpROReportSpecGroup.C1G2EnableCrcMetadata;
            dictionary[LlrpROReportSpecGroup.C1G2EnablePCKey] = LlrpROReportSpecGroup.C1G2EnablePCMetadata;
            dictionary[LlrpROReportSpecGroup.EnableAccessSpecIdKey] = LlrpROReportSpecGroup.EnableAccessSpecIdMetadata;
            dictionary[LlrpROReportSpecGroup.EnableAntennaIdKey] = LlrpROReportSpecGroup.EnableAntennaIdMetadata;
            dictionary[LlrpROReportSpecGroup.EnableChannelIndexKey] = LlrpROReportSpecGroup.EnableChannelIndexMetadata;
            dictionary[LlrpROReportSpecGroup.EnableFirstSeenTimestampKey] = LlrpROReportSpecGroup.EnableFirstSeenTimestampMetadata;
            dictionary[LlrpROReportSpecGroup.EnableInventoryParameterSpecIdKey] = LlrpROReportSpecGroup.EnableInventoryParameterSpecIdMetadata;
            dictionary[LlrpROReportSpecGroup.EnableLastSeenTimestampKey] = LlrpROReportSpecGroup.EnableLastSeenTimestampMetadata;
            dictionary[LlrpROReportSpecGroup.EnablePeakRssiKey] = LlrpROReportSpecGroup.EnablePeakRssiMetadata;
            dictionary[LlrpROReportSpecGroup.EnableROSpecIdKey] = LlrpROReportSpecGroup.EnableROSpecIdMetadata;
            dictionary[LlrpROReportSpecGroup.EnableSpecIndexKey] = LlrpROReportSpecGroup.EnableSpecIndexMetadata;
            dictionary[LlrpROReportSpecGroup.EnableTagSeenCountKey] = LlrpROReportSpecGroup.EnableTagSeenCountMetadata;
            dictionary[LlrpROReportSpecGroup.NumberOfTagReportDataKey] = LlrpROReportSpecGroup.NumberOfTagReportDataMetadata;
            dictionary[LlrpROReportSpecGroup.TriggerKey] = LlrpROReportSpecGroup.TriggerMetadata;
            dictionary[LlrpAccessReportSpecGroup.TriggerKey] = LlrpAccessReportSpecGroup.TriggerMetadata;
            dictionary[LlrpReaderEventNotificationSpecGroup.AISpecEndEventKey] = LlrpReaderEventNotificationSpecGroup.AISpecEndEventMetadata;
            dictionary[LlrpReaderEventNotificationSpecGroup.AISpecEndWithSingulationEventKey] = LlrpReaderEventNotificationSpecGroup.AISpecEndWithSingulationEventMetadata;
            dictionary[LlrpReaderEventNotificationSpecGroup.AntennaEventKey] = LlrpReaderEventNotificationSpecGroup.AntennaEventMetadata;
            dictionary[LlrpReaderEventNotificationSpecGroup.BufferFillWarningEventKey] = LlrpReaderEventNotificationSpecGroup.BufferFillWarningEventMetadata;
            dictionary[LlrpReaderEventNotificationSpecGroup.GpiEventKey] = LlrpReaderEventNotificationSpecGroup.GpiEventMetadata;
            dictionary[LlrpReaderEventNotificationSpecGroup.HoppingEventKey] = LlrpReaderEventNotificationSpecGroup.HoppingEventMetadata;
            dictionary[LlrpReaderEventNotificationSpecGroup.ReaderExceptionEventKey] = LlrpReaderEventNotificationSpecGroup.ReaderExceptionEventMetadata;
            dictionary[LlrpReaderEventNotificationSpecGroup.RFSurveyEventKey] = LlrpReaderEventNotificationSpecGroup.RFSurveyEventMetadata;
            dictionary[LlrpReaderEventNotificationSpecGroup.ROSpecEventKey] = LlrpReaderEventNotificationSpecGroup.ROSpecEventMetadata;
            dictionary[NotificationGroup.EventModeKey] = NotificationGroup.EventModeMetadata;
            dictionary[NotificationGroup.InventoryAccessSpecKey] = NotificationGroup.InventoryAccessSpecMetadata;
            dictionary[NotificationGroup.InventoryROSpecKey] = NotificationGroup.InventoryROSpecMetadata;
            dictionary[GeneralGroup.NameKey] = GeneralGroup.NameMetadata;
            dictionary[GeneralGroup.FirmwareVersionKey] = GeneralGroup.FirmwareVersionMetadata;
            dictionary[GeneralGroup.RegulatoryRegionKey] = GeneralGroup.RegulatoryRegionMetadata;
            dictionary[GeneralGroup.VendorKey] = GeneralGroup.VendorMetadata;
            dictionary[RFGroup.AirProtocolsSupportedKey] = RFGroup.AirProtocolsSupportedMetadata;
            dictionary[SourceGroup.EnabledKey] = SourceGroup.EnabledMetadata;
            dictionary[SourceGroup.PortInputValueKey] = SourceGroup.PortInputValueMetadata;
            dictionary[SourceGroup.PortOutputValueKey] = SourceGroup.PortOutputValueMetadata;
            dictionary[SourceGroup.SourceTypeKey] = SourceGroup.SourceTypeMetadata;
            dictionary[SourceGroup.SystemEnabledKey] = SourceGroup.SystemEnabledMetadata;
            dictionary[SourceGroup.LlrpSourceIdKey] = SourceGroup.LlrpSourceIdMetadata;
            dictionary[TagReadGroup.DuplicateEliminationTimeKey] = TagReadGroup.DuplicateEliminationTimeMetadata;
            dictionary[LlrpManagementGroup.SynchronousCommandInventoryDurationKey] = LlrpManagementGroup.SynchronousCommandInventoryDurationMetadata;
            dictionary[LlrpManagementGroup.LlrpMessageTimeoutKey] = LlrpManagementGroup.LlrpMessageTimeoutMetadata;
            dictionary[LlrpManagementGroup.SynchronousCommandInventoryOperationCountKey] = LlrpManagementGroup.SynchronousCommandInventoryOperationCountMetadata;
            dictionary[LlrpTroubleshootGroup.CleanupSpecsKey] = LlrpTroubleshootGroup.CleanupSpecsMetadata;
            dictionary[LlrpTroubleshootGroup.ResetToFactoryDefaultKey] = LlrpTroubleshootGroup.ResetToFactoryDefaultMetadata;
            return dictionary;
        }

        internal static Dictionary<PropertyKey, DevicePropertyMetadata> GetStandardLlrpDeviceMetadata()
        {
            return GetStandardDeviceMetadata(null);
        }









        internal static void AppendPropertyProfile(PropertyList source, ref PropertyList destination)
        {
            if (source != null)
            {
                if (destination == null)
                {
                    destination = new PropertyList(source.Name);
                }
                foreach (PropertyKey key in source.Keys)
                {
                    destination[key] = source[key];
                }
            }
        }


        internal static void AppendProperties(PropertyList destinationProfile, PropertyList sourceProfile)
        {
            AppendProperties(destinationProfile, sourceProfile, null);
        }


        internal static void AppendProperties(PropertyList destination, PropertyList source, Dictionary<PropertyKey, DevicePropertyMetadata> overrideAbleProperties)
        {
            if (overrideAbleProperties == null)
            {
                overrideAbleProperties = new Dictionary<PropertyKey, DevicePropertyMetadata>();
            }
            foreach (PropertyKey key in source.Keys)
            {
                if (destination.ContainsKey(key))
                {
                    if (overrideAbleProperties.ContainsKey(key))
                    {
                        LlrpProviderContext.Logger.Info("Device has the property {0}, which is override able. Thus updating the profile", new object[] { key });
                    }
                    else
                    {
                        LlrpProviderContext.Logger.Info("profile already contains the key {0}. Thus not adding the value from the custom param", new object[] { key });
                        continue;
                    }
                }
                destination[key] = source[key];
            }
        }










        internal static Collection<TagReadEvent> GetAsyncTags(PDPState deviceState, ROAccessReport rOAccessReport, ILogger logger)
        {
            Collection<TagReadEvent> collection = GetTags(deviceState, rOAccessReport, deviceState.ROSpec, null, TagDataSelector.All, logger);
            Collection<TagReadEvent> collection2 = new Collection<TagReadEvent>();
            if (deviceState.ProviderMaintainedProperties.ContainsKey(TagReadGroup.DuplicateEliminationTimeKey))
            {
                if (collection != null)
                {
                    foreach (TagReadEvent event2 in collection)
                    {
                        if (!deviceState.DuplicateEliminationHandler.IsDuplicate(event2))
                        {
                            collection2.Add(event2);
                        }
                        else
                        {
                            logger.Verbose(event2.Source + HexHelper.HexEncode(event2.GetId()) + " dublicate eliminated.");
                        }
                    }
                }
            }
            else
            {
                collection2 = collection;
            }
            if (collection2 == null)
            {
                collection2 = new Collection<TagReadEvent>();
            }
            return collection2;
        }






        private static TagReadEvent GetTag(TagReportData tagReport, PDPState deviceState, TagDataSelector dataSelector, string sourceName, ILogger logger)
        {
            TagReadEvent tag = null;
            if ((tagReport != null) && ((tagReport.EPC96 != null) || (tagReport.EpcData != null)))
            {
                string str = (tagReport.AntennaId != null) ? ((string)((tagReport.AntennaId.Id == 0) ? null : GetAntennaName(tagReport.AntennaId.Id))) : null;
                if ((sourceName != null) && !sourceName.Equals(str, StringComparison.OrdinalIgnoreCase))
                {
                    logger.Warning("Source name {0} does not match expected source {1}. Thus ignoring", new object[] { str, sourceName });
                    return null;
                }
                byte[] tagId = null;
                if (dataSelector.IsId)
                {
                    tagId = (tagReport.EPC96 != null) ? tagReport.EPC96.GetData() : tagReport.EpcData.GetData();
                }
                DateTime ? utcNow = null;
                if (dataSelector.IsTime)
                {
                    if (tagReport.FirstSeenTimestampUtc != null)
                    {
                        utcNow = ConstantValues.LlrpUtcStartTime.Add(new TimeSpan((long)(tagReport.FirstSeenTimestampUtc.Microseconds * ((ulong)10L))));
                    }
                    else if (tagReport.FirstSeenTimestampUptime != null)
                    {
                        TimeSpan span = new TimeSpan((long)(tagReport.FirstSeenTimestampUptime.Microseconds * ((ulong)10L)));
                        utcNow = deviceState.DeviceStartTime.Add(span);
                    }
                    else
                    {
                        logger.Verbose("Indicates the case that the tag did not have any time parameter in it from device. Setting the time as the current time");
                    }
                }
                byte[] numberingSystemIdentifier = null;
                if (dataSelector.IsNumberingSystemIdentifier && (tagReport.AirProtocolTagData != null))
                {
                    foreach (AirProtocolTagData data in tagReport.AirProtocolTagData)
                    {
                        if (data is C1G2PC)
                        {
                            C1G2PC cgpc = (C1G2PC)data;
                            numberingSystemIdentifier = new byte[] { (byte)((cgpc.PCBits & 0x100) >> 8), (byte)(cgpc.PCBits & 0xff) };
                            break;
                        }
                    }
                }
                byte[] tagData = null;
                if (dataSelector.IsData)
                {
                    TryGetTagData(tagReport, out tagData);
                }
                TagType tagType = GetTagType(tagReport);
                tag = new TagReadEvent(tagId, tagType, tagData, str, utcNow.HasValue ?  utcNow.Value.ToLocalTime(): DateTime.UtcNow.ToLocalTime(), numberingSystemIdentifier, dataSelector);
                //PopulateVendorSpecificInformation(tag, tagReport, logger);
                if (tagReport.PeakRssi != null)
                {
                    float rssi = tagReport.PeakRssi.Rssi;
                    tag.VendorSpecificData[TagReadEvent.Rssi] = rssi;
                }
                if (tagReport.TagSeenCount != null)
                {
                    tag.VendorSpecificData[TagReadEvent.ReadCount] = tagReport.TagSeenCount.TagCount;
                }

                //tansu: eklendi
                if (dataSelector.IsTime)
                {
                    DateTime ? lastSeen =null;
                    if (tagReport.LastSeenTimestampUtc != null)
                    {
                        lastSeen = ConstantValues.LlrpUtcStartTime.Add(new TimeSpan((long)(tagReport.LastSeenTimestampUtc.Microseconds * ((ulong)10L))));
                    }
                    else if (tagReport.LastSeenTimestampUptime != null)
                    {
                        TimeSpan span = new TimeSpan((long)(tagReport.LastSeenTimestampUptime.Microseconds * ((ulong)10L)));
                        lastSeen = deviceState.DeviceStartTime.Add(span);
                    }
                    if (lastSeen.HasValue)
                        tag.VendorSpecificData[TagReadEvent.LastSeen] = lastSeen.Value.ToLocalTime();
                }
            }
            return tag;
        }



        private static TagType GetTagType(TagReportData tagReport)
        {
            TagType type = TagType.EpcClass1Gen2;
            //if (tagReport.CustomParameters != null)
            //{
            //    MicrosoftRfidTagTypeParameter parameter = null;
            //    foreach (CustomParameterBase base2 in tagReport.CustomParameters)
            //    {
            //        parameter = base2 as MicrosoftRfidTagTypeParameter;
            //        if (parameter != null)
            //        {
            //            return parameter.TagType;
            //        }
            //    }
            //}
            return type;
        }


        internal static bool TryGetTagData(AirProtocolSpecificOPSpecResult opSpecResult, out byte[] tagData)
        {
            tagData = null;
            C1G2ReadOPSpecResult readResult = opSpecResult as C1G2ReadOPSpecResult;
            if (readResult == null)
            {
                return false;
            }
            if (readResult.ResultType != C1G2ReadOPSpecResultType.Success)
            {
                return false;
            }
            tagData = GetTagData(readResult);
            return true;
        }



        private static bool TryGetTagData(Collection<AirProtocolSpecificOPSpecResult> opSpecResults, out byte[] tagData)
        {
            tagData = null;
            if ((opSpecResults != null) && (opSpecResults.Count != 0))
            {
                foreach (AirProtocolSpecificOPSpecResult result in opSpecResults)
                {
                    if (TryGetTagData(result, out tagData))
                    {
                        return true;
                    }
                }
            }
            return false;
        }








        private static bool TryGetTagData(TagReportData tagReport, out byte[] tagData)
        {
            tagData = null;
            if (!TryGetTagData(tagReport.AirProtocolSpecificOPSpecResults, out tagData))
            {
                return TryGetTagData(tagReport.CustomParameters, out tagData);
            }
            return true;
        }


        private static bool TryGetTagData(Collection<CustomParameterBase> customParameters, out byte[] tagData)
        {
            tagData = null;
            //if ((customParameters != null) && (customParameters.Count != 0))
            //{
            //    MicrosoftRfidTagDataParameter parameter = null;
            //    foreach (CustomParameterBase base2 in customParameters)
            //    {
            //        parameter = base2 as MicrosoftRfidTagDataParameter;
            //        if (parameter != null)
            //        {
            //            tagData = parameter.GetTagData();
            //            return true;
            //        }
            //    }
            //}
            return false;
        }





















        internal static Collection<TagReadEvent> GetTags(PDPState deviceState, ROAccessReport rOAccessReport, ROSpec matchingROSpec, string sourceName, TagDataSelector selector, ILogger logger)
        {
            Collection<TagReadEvent> collection = new Collection<TagReadEvent>();
            if (rOAccessReport.TagReports != null)
            {
                foreach (TagReportData data in rOAccessReport.TagReports)
                {
                    if ((data.ROSpecId == null) || (matchingROSpec != null && data.ROSpecId.Id != matchingROSpec.Id))
                    {
                        logger.Warning("Ignoring reports as it is not part of the notification spec {0} but part of {1}", new object[] { matchingROSpec.Id, (data.ROSpecId != null) ? data.ROSpecId.Id.ToString() : "unknown" });
                    }
                    else
                    {
                        TagReadEvent item = GetTag(data, deviceState, selector, sourceName, logger);
                        if (item != null)
                        {
                            collection.Add(item);
                        }
                    }
                }
            }
            return collection;
        }

        internal static void AppendBytesAndPadToByteBoundary(LLRPMessageStream stream, byte[] bytesToCopy, uint bitsToConsider, bool bigEndian)
        {
            if ((bytesToCopy != null) && (bytesToCopy.Length != 0))
            {
                uint bitsOfInterest = BitsToPad((int)bitsToConsider);
                stream.Append(bytesToCopy, bitsToConsider, bigEndian);
                if (bitsOfInterest > 0)
                {
                    stream.Append((ulong)0L, bitsOfInterest, true);
                }
            }
        }


        internal static ushort BitsToPad(int bits)
        {
            return (((bits % 8) == 0) ? ((ushort)0) : ((ushort)(8 - (bits % 8))));
        }

        internal static void CheckCollectionForNonNullElement<T>(Collection<T> llrpParameters) where T : LlrpParameterBase
        {
            if (llrpParameters != null)
            {
                for (int i = 0; i < llrpParameters.Count; i++)
                {
                    if (llrpParameters[i] == null)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, LlrpResources.CollectionElementNull, new object[] { i + 1, typeof(T).FullName }));
                    }
                }
            }
        }

        internal static string ConvertByteArrayToHexString(byte[] input, int offset, uint count)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if ((offset < 0) || (offset > input.Length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0)
            {
                throw new ArgumentException("Count is less than zero.");
            }
            if ((offset + count) > input.Length)
            {
                throw new ArgumentException("Count is more than the number of bytes remaining in the buffer from offset.");
            }
            byte[] destinationArray = new byte[count];
            Array.Copy(input, offset, destinationArray, 0, destinationArray.Length);
            return HexHelper.HexEncode(destinationArray, true);
        }

        internal static byte[] ConvertUnicodeToUTF8(string input)
        {
            if (input == null)
            {
                return null;
            }
            return Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Encoding.Unicode.GetBytes(input));
        }

        internal static string ConvertUTF8ToUnicodeString(byte[] input)
        {
            if (input != null)
            {
                byte[] bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, input);
                if (bytes != null)
                {
                    return Encoding.Unicode.GetString(bytes, 0, bytes.Length);
                }
            }
            return null;
        }


        internal static void Encode<T>(Collection<T> llrpParameters, LLRPMessageStream stream) where T : LlrpParameterBase
        {
            if (llrpParameters != null)
            {
                foreach (LlrpParameterBase base2 in llrpParameters)
                {
                    Encode(base2, stream);
                }
            }
        }

        internal static void Encode(LlrpParameterBase llrpParameter, LLRPMessageStream stream)
        {
            if (llrpParameter != null)
            {
                llrpParameter.Encode(stream);
            }
        }

        internal static string GetAntennaName(ushort antennaId)
        {
            return ("Antenna_" + antennaId);
        }

        internal static T[] GetArrayClone<T>(T[] value) where T : struct
        {
            if (value == null)
            {
                return null;
            }
            return (T[])value.Clone();
        }


        internal static uint GetBitLengthOfParam(LlrpParameterBase llrpParameter)
        {
            uint parameterLength = 0;
            if (llrpParameter != null)
            {
                parameterLength = llrpParameter.ParameterLength;
            }
            return parameterLength;
        }

        internal static byte[] GetByteArrayClone(byte[] value)
        {
            return GetArrayClone<byte>(value);
        }

        internal static byte[] GetBytesFromUTF8(string input)
        {
            if (input == null)
            {
                return null;
            }
            return Encoding.UTF8.GetBytes(input);
        }

        internal static string GetDeviceId(IdentificationParameter parameter)
        {
            if (parameter == null)
            {
                return null;
            }
            if (parameter.IdentificationType != IdentificationType.Epc)
            {
                return ("MAC:" + GetString<byte>(parameter.GetReaderId()));
            }
            return ("EPC:" + GetString<byte>(parameter.GetReaderId()));
        }

        internal static T GetEnumInstance<T>(string value)
        {
            try
            {
                return (T)System.Enum.Parse(typeof(T), value, false);
            }
            catch (ArgumentException)
            {
            }
            return (T)System.Enum.ToObject(typeof(T), 0);
        }

        internal static string GetGpiName(ushort gpiId)
        {
            return ("GPI_" + gpiId);
        }

        internal static string GetGpoName(ushort gpoId)
        {
            return ("GPO_" + gpoId);
        }

        private static string GetHostNameAppropriateForUri(string hostName)
        {
            IPAddress address;
            if (IPAddress.TryParse(hostName, out address) && (address.AddressFamily == AddressFamily.InterNetworkV6))
            {
                return ("[" + address.ToString() + "]");
            }
            return hostName;
        }

        internal static byte[] GetInitialisedByteArray(int length)
        {
            byte[] buffer = new byte[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = 0xff;
            }
            return buffer;
        }

        internal static string GetLlrpUriAddress(string hostName, int port)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("llrp.bin");
            builder.Append("://");
            builder.Append(GetHostNameAppropriateForUri(hostName));
            builder.Append(":");
            builder.Append(port);
            return builder.ToString();
        }


        internal static Collection<object> GetNames(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            Collection<object> collection = new Collection<object>();
            FieldInfo[] fields = enumType.GetFields();
            if (fields != null)
            {
                foreach (FieldInfo info in fields)
                {
                    if (info.FieldType.Equals(enumType))
                    {
                        collection.Add(info.Name);
                    }
                }
            }
            return collection;
        }

        internal static ushort GetSourceId(string sourceName)
        {
            LLRPSourceType type;
            IsValidSourceName(sourceName, out type);
            return GetSourceId(sourceName, type);
        }

        internal static ushort GetSourceId(string sourceName, LLRPSourceType sourceType)
        {
            if (sourceType == LLRPSourceType.Antenna)
            {
                return ushort.Parse(sourceName.Substring("Antenna_".Length), NumberStyles.None, CultureInfo.CurrentCulture);
            }
            if (sourceType == LLRPSourceType.GPI)
            {
                return ushort.Parse(sourceName.Substring("GPI_".Length), NumberStyles.None, CultureInfo.CurrentCulture);
            }
            if (sourceType == LLRPSourceType.GPO)
            {
                return ushort.Parse(sourceName.Substring("GPO_".Length), NumberStyles.None, CultureInfo.CurrentCulture);
            }
            return 0;
        }




        internal static string GetString<T>(IEnumerable<T> enumerator)
        {
            if (enumerator == null)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            foreach (T local in enumerator)
            {
                builder.Append(local);
            }
            return builder.ToString();
        }






        internal static ulong GetTimeSinceEpoch(DateTime time)
        {
            try
            {
                TimeSpan span = (TimeSpan)(time.ToUniversalTime() - ConstantValues.LlrpUtcStartTime);
                return (ulong)(span.Ticks / 10L);
            }
            catch (ArgumentOutOfRangeException)
            {
                return (ulong)(TimeSpan.MaxValue.Ticks / 10L);
            }
        }

        internal static uint GetTotalBitLengthOfParam<T>(Collection<T> llrpParameters) where T : LlrpParameterBase
        {
            uint num = 0;
            if (llrpParameters != null)
            {
                foreach (LlrpParameterBase base2 in llrpParameters)
                {
                    if (base2 != null)
                    {
                        num += base2.ParameterLength;
                    }
                }
            }
            return num;
        }


        internal static string GetUTF8FromBytes(byte[] input)
        {
            if (input == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(input, 0, input.Length);
        }

        internal static bool HostSupportsIPv4()
        {
            bool flag;
            try
            {
                using (new TcpClient(AddressFamily.InterNetwork))
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        internal static bool HostSupportsIPv6()
        {
            bool flag;
            try
            {
                using (new TcpClient(AddressFamily.InterNetworkV6))
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        internal static bool IsLocked(object lockingObject)
        {
            return true;
        }

        internal static bool IsValidSourceName(string sourceName, out LLRPSourceType sourceType)
        {
            sourceType = LLRPSourceType.Antenna;
            if (sourceName != null)
            {
                uint result = 0;
                if (sourceName.StartsWith("Antenna_", StringComparison.OrdinalIgnoreCase))
                {
                    sourceType = LLRPSourceType.Antenna;
                    return uint.TryParse(sourceName.Substring("Antenna_".Length), out result);
                }
                if (sourceName.StartsWith("GPI_", StringComparison.OrdinalIgnoreCase))
                {
                    sourceType = LLRPSourceType.GPI;
                    return uint.TryParse(sourceName.Substring("GPI_".Length), out result);
                }
                if (sourceName.StartsWith("GPO_", StringComparison.OrdinalIgnoreCase))
                {
                    sourceType = LLRPSourceType.GPO;
                    return uint.TryParse(sourceName.Substring("GPO_".Length), out result);
                }
            }
            return false;
        }

        internal static void LogThreadPoolStatus(ILogger logger)
        {
            int num;
            int num2;
            ThreadPool.GetMaxThreads(out num, out num2);
            logger.Verbose("ThreadPool: Max worker threads: {0}", new object[] { num });
            ThreadPool.GetAvailableThreads(out num, out num2);
            logger.Verbose("ThreadPool: Available worker threads: {0}", new object[] { num });
        }





        internal static void ThrowIfFailed(LlrpMessageBase message)
        {
            if (message != null)
            {
                LlrpMessageResponseBase base2 = message as LlrpMessageResponseBase;
                if (base2 != null)
                {
                    ThrowIfFailed(base2.Status);
                }
            }
        }

        internal static void ThrowIfFailed(LlrpStatus status)
        {
            if ((status != null) && !status.IsSuccess)
            {
                throw new SensorProviderException(status.ToString());
            }
        }

        internal static void ThrowIfNull(LlrpMessageBase message, LlrpMessageType messageType)
        {
            if (message == null)
            {
                throw new SensorProviderException(string.Format(CultureInfo.CurrentCulture, LlrpResources.NullResponseForMessage, new object[] { messageType }));
            }
        }

        internal static void ToString(LlrpParameterBase llrpParameter, StringBuilder strBuilder)
        {
            if (llrpParameter != null)
            {
                strBuilder.Append(llrpParameter.ToString());
            }
        }

        internal static void ToString(LlrpParameterType parameterType, StringBuilder strBuilder)
        {
            strBuilder.Append(LlrpResources.LlrpParameterTypeToStringHeader);
            strBuilder.Append(parameterType);
            strBuilder.Append(LlrpResources.LlrpParameterTypeToStringFooter);
        }

        internal static void ToString<T>(IEnumerable<T> llrpParameters, StringBuilder strBuilder) where T : LlrpParameterBase
        {
            if (llrpParameters != null)
            {
                foreach (LlrpParameterBase base2 in llrpParameters)
                {
                    ToString(base2, strBuilder);
                }
            }
        }

        internal static void ToString(StatusCode statusCode, StringBuilder strBuilder)
        {
            strBuilder.Append(LlrpResources.StatusCodeToStringHeader);
            strBuilder.Append(statusCode);
            strBuilder.Append(LlrpResources.StatusCodeToStringFooter);
        }

        internal static void ToStringBase(LlrpParameterBase llrpParameter, StringBuilder str)
        {
            if (llrpParameter != null)
            {
                str.Append("<LLRP Parameter Base>");
                str.Append("<Type>");
                str.Append(llrpParameter.ParameterType);
                str.Append("</Type>");
                str.Append("<Length>");
                str.Append((uint)(llrpParameter.ParameterLength / 8));
                str.Append("</Length>");
                str.Append("</LLRP Parameter Base>");
            }
        }

        internal static void ToStringSerialize(LlrpParameterBase llrpParameters, StringBuilder strBuilder)
        {
            if (llrpParameters != null)
            {
                strBuilder.Append(LlrpSerializationHelper.SerializeToXmlDataContract(llrpParameters, false));
            }
        }

        internal static void ToStringSerialize<T>(IEnumerable<T> llrpParameters, StringBuilder strBuilder) where T : LlrpParameterBase
        {
            if (llrpParameters != null)
            {
                foreach (LlrpParameterBase base2 in llrpParameters)
                {
                    ToStringSerialize(base2, strBuilder);
                }
            }
        }








        internal static ROSpec ValidateAndGetInventoryROSpec(string roSpecInXml)
        {
            if (roSpecInXml == null)
            {
                throw new ArgumentException(LlrpResources.InventoryROSpecNull);
            }
            return LlrpSerializationHelper.DeserializeFromXmlDataContract<ROSpec>(roSpecInXml);
        }

        internal static void PopulateDeviceCustomProperties(GetReaderConfigurationResponse response, ref PropertyList profile, Dictionary<PropertyKey, DevicePropertyMetadata> customMetadata)
        {
            if (((response != null) && (response.CustomParameters != null)) && (response.CustomParameters.Count != 0))
            {
                foreach (CustomParameterBase base2 in response.CustomParameters)
                {
                    //if (base2 is MicrosoftDevicePropertiesParameter)
                    {

                        CustomParameterBase parameter = base2;
                        //MicrosoftDevicePropertiesParameter parameter = (MicrosoftDevicePropertiesParameter)base2;
                        //if (((parameter != null) && (parameter.RfidProperties != null)) && (parameter.RfidProperties.DeviceProfile != null))
                        //{
                        //    PropertyProfile deviceProfile = parameter.RfidProperties.DeviceProfile;
                        //    AppendProperties(profile, deviceProfile, customMetadata);
                        //}
                    }
                }
            }
        }




    }
}
