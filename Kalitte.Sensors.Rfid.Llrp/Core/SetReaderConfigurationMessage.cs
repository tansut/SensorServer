namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class SetReaderConfigurationMessage : LlrpMessageRequestBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.AccessReportSpec m_accessReportSpec;
        private Collection<AntennaConfiguration> m_antennaConfigurations;
        private Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties> m_antennaProps;
        private Collection<CustomParameterBase> m_customs;
        private EventsAndReport m_eventAndReport;
        private Collection<GpiPortCurrentState> m_gpiCurrentStates;
        private Collection<GpoWriteData> m_gpoDatas;
        private KeepAliveSpec m_keepAlive;
        private ReaderEventNotificationSpec m_readerEventNotification;
        private bool m_resetToFactoryDefault;
        private Kalitte.Sensors.Rfid.Llrp.Core.ROReportSpec m_roReportSpec;

        internal SetReaderConfigurationMessage(BitArray bitArray) : base(LlrpMessageType.SetReaderConfig, bitArray)
        {
            int index = 80;
            bool resetToFactoryDefault = bitArray[index++];
            index += 7;
            ReaderEventNotificationSpec readerEvent = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ReaderEventNotificationSpec, bitArray, index))
            {
                readerEvent = new ReaderEventNotificationSpec(bitArray, ref index);
            }
            Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties> antennaProperties = new Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AntennaProperties, bitArray, index))
            {
                antennaProperties.Add(new Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties(bitArray, ref index));
            }
            Collection<AntennaConfiguration> antennaConfig = new Collection<AntennaConfiguration>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AntennaConfiguration, bitArray, index))
            {
                antennaConfig.Add(new AntennaConfiguration(bitArray, ref index));
            }
            Kalitte.Sensors.Rfid.Llrp.Core.ROReportSpec roReport = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ROReportSpec, bitArray, index))
            {
                roReport = new Kalitte.Sensors.Rfid.Llrp.Core.ROReportSpec(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.AccessReportSpec accessReport = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AccessReportSpec, bitArray, index))
            {
                accessReport = new Kalitte.Sensors.Rfid.Llrp.Core.AccessReportSpec(bitArray, ref index);
            }
            KeepAliveSpec keepAlive = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.KeepAliveSpec, bitArray, index))
            {
                keepAlive = new KeepAliveSpec(bitArray, ref index);
            }
            Collection<GpoWriteData> gpoWriteDatas = new Collection<GpoWriteData>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.GpoWriteData, bitArray, index))
            {
                gpoWriteDatas.Add(new GpoWriteData(bitArray, ref index));
            }
            Collection<GpiPortCurrentState> gpiCurrentStates = new Collection<GpiPortCurrentState>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.GpiPortCurrentState, bitArray, index))
            {
                gpiCurrentStates.Add(new GpiPortCurrentState(bitArray, ref index));
            }
            EventsAndReport eventsAndReport = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.EventsAndReports, bitArray, index))
            {
                eventsAndReport = new EventsAndReport(bitArray, ref index);
            }
            Collection<CustomParameterBase> customs = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index))
            {
                customs.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            this.Init(resetToFactoryDefault, readerEvent, antennaProperties, antennaConfig, roReport, accessReport, keepAlive, gpiCurrentStates, gpoWriteDatas, eventsAndReport, customs);
        }

        public SetReaderConfigurationMessage(bool resetToFactoryDefault, ReaderEventNotificationSpec readerEvent, Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties> antennaProperties, Collection<AntennaConfiguration> antennaConfig, Kalitte.Sensors.Rfid.Llrp.Core.ROReportSpec roReport, Kalitte.Sensors.Rfid.Llrp.Core.AccessReportSpec accessReport, KeepAliveSpec keepAlive, Collection<GpiPortCurrentState> gpiCurrentStates, Collection<GpoWriteData> gpoWriteData, EventsAndReport eventsAndReport, Collection<CustomParameterBase> customs) : base(LlrpMessageType.SetReaderConfig)
        {
            this.Init(resetToFactoryDefault, readerEvent, antennaProperties, antennaConfig, roReport, accessReport, keepAlive, gpiCurrentStates, gpoWriteData, eventsAndReport, customs);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = this.CreateHeaderStream();
            stream.Append(this.ResetToFactoryDefault, 1, true);
            stream.Append((ulong) 0L, 7, true);
            Util.Encode(this.ReaderEventNotification, stream);
            Util.Encode<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties>(this.AntennaProperties, stream);
            Util.Encode<AntennaConfiguration>(this.AntennaConfigurations, stream);
            Util.Encode(this.ROReportSpec, stream);
            Util.Encode(this.AccessReportSpec, stream);
            Util.Encode(this.KeepAlive, stream);
            Util.Encode<GpoWriteData>(this.GpoData, stream);
            Util.Encode<GpiPortCurrentState>(this.GpiCurrentStates, stream);
            Util.Encode(this.EventAndReport, stream);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
            return stream.Merge();
        }

        private void Init(bool resetToFactoryDefault, ReaderEventNotificationSpec readerEvent, Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties> antennaProperties, Collection<AntennaConfiguration> antennaConfig, Kalitte.Sensors.Rfid.Llrp.Core.ROReportSpec roReport, Kalitte.Sensors.Rfid.Llrp.Core.AccessReportSpec accessReport, KeepAliveSpec keepAlive, Collection<GpiPortCurrentState> gpiCurrentStates, Collection<GpoWriteData> gpoWriteDatas, EventsAndReport eventsAndReport, Collection<CustomParameterBase> customs)
        {
            Util.CheckCollectionForNonNullElement<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties>(antennaProperties);
            Util.CheckCollectionForNonNullElement<AntennaConfiguration>(antennaConfig);
            Util.CheckCollectionForNonNullElement<GpiPortCurrentState>(gpiCurrentStates);
            Util.CheckCollectionForNonNullElement<GpoWriteData>(gpoWriteDatas);
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customs);
            this.m_resetToFactoryDefault = resetToFactoryDefault;
            this.m_readerEventNotification = readerEvent;
            this.m_antennaProps = antennaProperties;
            this.m_antennaConfigurations = antennaConfig;
            this.m_roReportSpec = roReport;
            this.m_accessReportSpec = accessReport;
            this.m_keepAlive = keepAlive;
            this.m_gpiCurrentStates = gpiCurrentStates;
            this.m_gpoDatas = gpoWriteDatas;
            this.m_eventAndReport = eventsAndReport;
            this.m_customs = customs;
            this.MessageLength = (((((((((8 + Util.GetBitLengthOfParam(this.m_readerEventNotification)) + Util.GetTotalBitLengthOfParam<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties>(this.m_antennaProps)) + Util.GetTotalBitLengthOfParam<AntennaConfiguration>(this.m_antennaConfigurations)) + Util.GetBitLengthOfParam(this.m_roReportSpec)) + Util.GetBitLengthOfParam(this.m_accessReportSpec)) + Util.GetBitLengthOfParam(this.m_keepAlive)) + Util.GetTotalBitLengthOfParam<GpiPortCurrentState>(this.m_gpiCurrentStates)) + Util.GetTotalBitLengthOfParam<GpoWriteData>(this.m_gpoDatas)) + Util.GetBitLengthOfParam(this.m_eventAndReport)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.m_customs);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Set Reader Configuration Message>");
            strBuilder.Append(base.ToString());
            strBuilder.Append("<Reset to factory default>");
            strBuilder.Append(this.ResetToFactoryDefault);
            strBuilder.Append("</Reset to factory default>");
            Util.ToString(this.ReaderEventNotification, strBuilder);
            Util.ToString<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties>(this.AntennaProperties, strBuilder);
            Util.ToString<AntennaConfiguration>(this.AntennaConfigurations, strBuilder);
            Util.ToString(this.ROReportSpec, strBuilder);
            Util.ToString(this.AccessReportSpec, strBuilder);
            Util.ToString(this.KeepAlive, strBuilder);
            Util.ToString<GpoWriteData>(this.GpoData, strBuilder);
            Util.ToString<GpiPortCurrentState>(this.GpiCurrentStates, strBuilder);
            Util.ToString(this.EventAndReport, strBuilder);
            Util.ToString<CustomParameterBase>(this.CustomParameters, strBuilder);
            strBuilder.Append("</Set Reader Configuration Message>");
            return strBuilder.ToString();
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.AccessReportSpec AccessReportSpec
        {
            get
            {
                return this.m_accessReportSpec;
            }
        }

        public Collection<AntennaConfiguration> AntennaConfigurations
        {
            get
            {
                return this.m_antennaConfigurations;
            }
        }

        public Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties> AntennaProperties
        {
            get
            {
                return this.m_antennaProps;
            }
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customs;
            }
        }

        public EventsAndReport EventAndReport
        {
            get
            {
                return this.m_eventAndReport;
            }
        }

        public Collection<GpiPortCurrentState> GpiCurrentStates
        {
            get
            {
                return this.m_gpiCurrentStates;
            }
        }

        public Collection<GpoWriteData> GpoData
        {
            get
            {
                return this.m_gpoDatas;
            }
        }

        public KeepAliveSpec KeepAlive
        {
            get
            {
                return this.m_keepAlive;
            }
        }

        public ReaderEventNotificationSpec ReaderEventNotification
        {
            get
            {
                return this.m_readerEventNotification;
            }
        }

        public bool ResetToFactoryDefault
        {
            get
            {
                return this.m_resetToFactoryDefault;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.ROReportSpec ROReportSpec
        {
            get
            {
                return this.m_roReportSpec;
            }
        }
    }
}
