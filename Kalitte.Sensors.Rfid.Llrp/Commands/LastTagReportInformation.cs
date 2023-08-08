using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Llrp.Core;
using System.Collections.ObjectModel;

namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
internal class LastTagReportInformation
{
    // Fields
    private AccessSpecId m_accessSpecId;
    private AntennaId m_antennaId;
    private ChannelIndex m_channelIndex;
    private FirstSeenTimestampUptime m_firstSeenUptime;
    private FirstSeenTimestampUtc m_firstSeenUTC;
    private InventoryParameterSpecId m_inventoryParamSpecId;
    private LastSeenTimestampUptime m_lastSeenUptime;
    private LastSeenTimestampUtc m_lastSeenUTC;
    private PeakRssi m_peakRSSI;
    private ROSpecId m_roSpecId;
    private SpecIndex m_specIndex;
    private TagSeenCount m_tagSeenCount;

    // Methods
    internal void PopulateTagReportData(Collection<TagReportData> tagReport)
    {
        if (tagReport != null)
        {
            for (int i = 0; i < tagReport.Count; i++)
            {
                tagReport[i] = this.PopulateTagReportData(tagReport[i]);
            }
        }
    }

    private TagReportData PopulateTagReportData(TagReportData tagReportData)
    {
        if (tagReportData == null)
        {
            return null;
        }
        TagReportData data = new TagReportData(tagReportData.EPC96, tagReportData.EpcData, (tagReportData.ROSpecId != null) ? tagReportData.ROSpecId : this.RoSpecId, (tagReportData.SpecIndex != null) ? tagReportData.SpecIndex : this.SpecIndex, (tagReportData.InventoryParameterSpecId != null) ? tagReportData.InventoryParameterSpecId : this.InventoryParamSpecId, (tagReportData.AntennaId != null) ? tagReportData.AntennaId : this.AntennaId, (tagReportData.PeakRssi != null) ? tagReportData.PeakRssi : this.PeakRSSI, (tagReportData.ChannelIndex != null) ? tagReportData.ChannelIndex : this.ChannelIndex, (tagReportData.FirstSeenTimestampUtc != null) ? tagReportData.FirstSeenTimestampUtc : this.FirstSeenUTC, (tagReportData.FirstSeenTimestampUptime != null) ? tagReportData.FirstSeenTimestampUptime : this.FirstSeenUptime, (tagReportData.LastSeenTimestampUtc != null) ? tagReportData.LastSeenTimestampUtc : this.LastSeenUTC, (tagReportData.LastSeenTimestampUptime != null) ? tagReportData.LastSeenTimestampUptime : this.LastSeenUptime, (tagReportData.TagSeenCount != null) ? tagReportData.TagSeenCount : this.TagSeenCount, tagReportData.AirProtocolTagData, (tagReportData.AccessSpecId != null) ? tagReportData.AccessSpecId : this.AccessSpecId, tagReportData.AirProtocolSpecificOPSpecResults, tagReportData.ClientRequestOPSpecResults, tagReportData.CustomParameters);
        this.UpdateTagInformation(tagReportData);
        return data;
    }

    private void UpdateTagInformation(TagReportData tagReportData)
    {
        if (tagReportData != null)
        {
            if (tagReportData.ROSpecId != null)
            {
                this.RoSpecId = (ROSpecId) tagReportData.ROSpecId.Clone();
            }
            if (tagReportData.SpecIndex != null)
            {
                this.SpecIndex = (SpecIndex) tagReportData.SpecIndex.Clone();
            }
            if (tagReportData.InventoryParameterSpecId != null)
            {
                this.InventoryParamSpecId = (InventoryParameterSpecId) tagReportData.InventoryParameterSpecId.Clone();
            }
            if (tagReportData.AntennaId != null)
            {
                this.AntennaId = (AntennaId) tagReportData.AntennaId.Clone();
            }
            if (tagReportData.PeakRssi != null)
            {
                this.PeakRSSI = (PeakRssi) tagReportData.PeakRssi.Clone();
            }
            if (tagReportData.ChannelIndex != null)
            {
                this.ChannelIndex = (ChannelIndex) tagReportData.ChannelIndex.Clone();
            }
            if (tagReportData.FirstSeenTimestampUptime != null)
            {
                this.FirstSeenUptime = (FirstSeenTimestampUptime) tagReportData.FirstSeenTimestampUptime.Clone();
            }
            if (tagReportData.FirstSeenTimestampUtc != null)
            {
                this.FirstSeenUTC = (FirstSeenTimestampUtc) tagReportData.FirstSeenTimestampUtc.Clone();
            }
            if (tagReportData.LastSeenTimestampUptime != null)
            {
                this.LastSeenUptime = (LastSeenTimestampUptime) tagReportData.LastSeenTimestampUptime.Clone();
            }
            if (tagReportData.LastSeenTimestampUtc != null)
            {
                this.LastSeenUTC = (LastSeenTimestampUtc) tagReportData.LastSeenTimestampUtc.Clone();
            }
            if (tagReportData.TagSeenCount != null)
            {
                this.TagSeenCount = (TagSeenCount) tagReportData.TagSeenCount.Clone();
            }
            if (tagReportData.AccessSpecId != null)
            {
                this.AccessSpecId = (AccessSpecId) tagReportData.AccessSpecId.Clone();
            }
        }
    }

    // Properties
    private AccessSpecId AccessSpecId
    {
        get
        {
            return this.m_accessSpecId;
        }
        set
        {
            this.m_accessSpecId = value;
        }
    }

    private AntennaId AntennaId
    {
        get
        {
            return this.m_antennaId;
        }
        set
        {
            this.m_antennaId = value;
        }
    }

    private ChannelIndex ChannelIndex
    {
        get
        {
            return this.m_channelIndex;
        }
        set
        {
            this.m_channelIndex = value;
        }
    }

    private FirstSeenTimestampUptime FirstSeenUptime
    {
        get
        {
            return this.m_firstSeenUptime;
        }
        set
        {
            this.m_firstSeenUptime = value;
        }
    }

    private FirstSeenTimestampUtc FirstSeenUTC
    {
        get
        {
            return this.m_firstSeenUTC;
        }
        set
        {
            this.m_firstSeenUTC = value;
        }
    }

    private InventoryParameterSpecId InventoryParamSpecId
    {
        get
        {
            return this.m_inventoryParamSpecId;
        }
        set
        {
            this.m_inventoryParamSpecId = value;
        }
    }

    private LastSeenTimestampUptime LastSeenUptime
    {
        get
        {
            return this.m_lastSeenUptime;
        }
        set
        {
            this.m_lastSeenUptime = value;
        }
    }

    private LastSeenTimestampUtc LastSeenUTC
    {
        get
        {
            return this.m_lastSeenUTC;
        }
        set
        {
            this.m_lastSeenUTC = value;
        }
    }

    private PeakRssi PeakRSSI
    {
        get
        {
            return this.m_peakRSSI;
        }
        set
        {
            this.m_peakRSSI = value;
        }
    }

    private ROSpecId RoSpecId
    {
        get
        {
            return this.m_roSpecId;
        }
        set
        {
            this.m_roSpecId = value;
        }
    }

    private SpecIndex SpecIndex
    {
        get
        {
            return this.m_specIndex;
        }
        set
        {
            this.m_specIndex = value;
        }
    }

    private TagSeenCount TagSeenCount
    {
        get
        {
            return this.m_tagSeenCount;
        }
        set
        {
            this.m_tagSeenCount = value;
        }
    }
}

 

 

}
