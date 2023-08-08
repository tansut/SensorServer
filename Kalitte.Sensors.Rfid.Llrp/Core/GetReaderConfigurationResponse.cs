namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class GetReaderConfigurationResponse : LlrpMessageResponseBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.AccessReportSpec m_accessReportSpec;
        private Collection<AntennaConfiguration> m_antennaConfigurations;
        private Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties> m_antennaProps;
        private Collection<CustomParameterBase> m_customs;
        private EventsAndReport m_eventAndReport;
        private Collection<GpiPortCurrentState> m_gpiCurrentStates;
        private Collection<GpoWriteData> m_gpoDatas;
        private IdentificationParameter m_identification;
        private KeepAliveSpec m_keepAlive;
        private LlrpConfigurationStateValue m_llrpConfigState;
        private ReaderEventNotificationSpec m_readerEventNotification;
        private Kalitte.Sensors.Rfid.Llrp.Core.ROReportSpec m_roReportSpec;

        internal GetReaderConfigurationResponse(BitArray bitArray) : base(LlrpMessageType.GetReaderConfigResponse, bitArray)
        {
            int baseLength = base.BaseLength;
            IdentificationParameter id = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Identification, bitArray, baseLength))
            {
                id = new IdentificationParameter(bitArray, ref baseLength);
            }
            Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties> antennaProp = new Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AntennaProperties, bitArray, baseLength))
            {
                antennaProp.Add(new Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties(bitArray, ref baseLength));
            }
            Collection<AntennaConfiguration> antennaConfig = new Collection<AntennaConfiguration>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AntennaConfiguration, bitArray, baseLength))
            {
                antennaConfig.Add(new AntennaConfiguration(bitArray, ref baseLength));
            }
            ReaderEventNotificationSpec readerEvent = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ReaderEventNotificationSpec, bitArray, baseLength))
            {
                readerEvent = new ReaderEventNotificationSpec(bitArray, ref baseLength);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.ROReportSpec roReport = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ROReportSpec, bitArray, baseLength))
            {
                roReport = new Kalitte.Sensors.Rfid.Llrp.Core.ROReportSpec(bitArray, ref baseLength);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.AccessReportSpec accessReport = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AccessReportSpec, bitArray, baseLength))
            {
                accessReport = new Kalitte.Sensors.Rfid.Llrp.Core.AccessReportSpec(bitArray, ref baseLength);
            }
            LlrpConfigurationStateValue configState = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.LlrpConfigurationStateValue, bitArray, baseLength))
            {
                configState = new LlrpConfigurationStateValue(bitArray, ref baseLength);
            }
            KeepAliveSpec keepAlive = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.KeepAliveSpec, bitArray, baseLength))
            {
                keepAlive = new KeepAliveSpec(bitArray, ref baseLength);
            }
            Collection<GpiPortCurrentState> gpiCurrentStates = new Collection<GpiPortCurrentState>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.GpiPortCurrentState, bitArray, baseLength))
            {
                gpiCurrentStates.Add(new GpiPortCurrentState(bitArray, ref baseLength));
            }
            Collection<GpoWriteData> gpoWriteDatas = new Collection<GpoWriteData>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.GpoWriteData, bitArray, baseLength))
            {
                gpoWriteDatas.Add(new GpoWriteData(bitArray, ref baseLength));
            }
            EventsAndReport eventsAndReport = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.EventsAndReports, bitArray, baseLength))
            {
                eventsAndReport = new EventsAndReport(bitArray, ref baseLength);
            }
            Collection<CustomParameterBase> customs = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, baseLength))
            {
                customs.Add(CustomParameterBase.GetInstance(bitArray, ref baseLength));
            }
            BitHelper.ValidateEndOfParameterOrMessage(baseLength, (uint) bitArray.Count, base.GetType().FullName);
            this.Init(id, antennaProp, antennaConfig, readerEvent, roReport, accessReport, configState, keepAlive, gpiCurrentStates, gpoWriteDatas, eventsAndReport, customs);
        }

        public GetReaderConfigurationResponse(uint messageId, LlrpStatus status, IdentificationParameter id, Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties> antennaProp, Collection<AntennaConfiguration> antennaConfig, ReaderEventNotificationSpec readerEvent, Kalitte.Sensors.Rfid.Llrp.Core.ROReportSpec roReport, Kalitte.Sensors.Rfid.Llrp.Core.AccessReportSpec accessReport, LlrpConfigurationStateValue configState, KeepAliveSpec keepAlive, Collection<GpiPortCurrentState> gpiCurrentStates, Collection<GpoWriteData> gpoWriteData, EventsAndReport eventsAndReport, Collection<CustomParameterBase> customs) : base(LlrpMessageType.GetReaderConfigResponse, messageId, status)
        {
            this.Init(id, antennaProp, antennaConfig, readerEvent, roReport, accessReport, configState, keepAlive, gpiCurrentStates, gpoWriteData, eventsAndReport, customs);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = base.CreateResponseHeaderStream();
            Util.Encode(this.m_identification, stream);
            Util.Encode<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties>(this.m_antennaProps, stream);
            Util.Encode<AntennaConfiguration>(this.m_antennaConfigurations, stream);
            Util.Encode(this.m_readerEventNotification, stream);
            Util.Encode(this.m_roReportSpec, stream);
            Util.Encode(this.m_accessReportSpec, stream);
            Util.Encode(this.m_llrpConfigState, stream);
            Util.Encode(this.m_keepAlive, stream);
            Util.Encode<GpiPortCurrentState>(this.m_gpiCurrentStates, stream);
            Util.Encode<GpoWriteData>(this.m_gpoDatas, stream);
            Util.Encode(this.m_eventAndReport, stream);
            Util.Encode<CustomParameterBase>(this.m_customs, stream);
            return stream.Merge();
        }

        private void Init(IdentificationParameter id, Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties> antennaProp, Collection<AntennaConfiguration> antennaConfig, ReaderEventNotificationSpec readerEvent, Kalitte.Sensors.Rfid.Llrp.Core.ROReportSpec roReport, Kalitte.Sensors.Rfid.Llrp.Core.AccessReportSpec accessReport, LlrpConfigurationStateValue configState, KeepAliveSpec keepAlive, Collection<GpiPortCurrentState> gpiCurrentStates, Collection<GpoWriteData> gpoWriteDatas, EventsAndReport eventsAndReport, Collection<CustomParameterBase> customs)
        {
            Util.CheckCollectionForNonNullElement<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties>(antennaProp);
            Util.CheckCollectionForNonNullElement<AntennaConfiguration>(antennaConfig);
            Util.CheckCollectionForNonNullElement<GpiPortCurrentState>(gpiCurrentStates);
            Util.CheckCollectionForNonNullElement<GpoWriteData>(gpoWriteDatas);
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customs);
            this.m_identification = id;
            this.m_antennaProps = antennaProp;
            this.m_antennaConfigurations = antennaConfig;
            this.m_readerEventNotification = readerEvent;
            this.m_roReportSpec = roReport;
            this.m_accessReportSpec = accessReport;
            this.m_llrpConfigState = configState;
            this.m_keepAlive = keepAlive;
            this.m_gpiCurrentStates = gpiCurrentStates;
            this.m_gpoDatas = gpoWriteDatas;
            this.m_eventAndReport = eventsAndReport;
            this.m_customs = customs;
            this.MessageLength = ((((((((((Util.GetBitLengthOfParam(id) + Util.GetTotalBitLengthOfParam<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties>(antennaProp)) + Util.GetTotalBitLengthOfParam<AntennaConfiguration>(antennaConfig)) + Util.GetBitLengthOfParam(readerEvent)) + Util.GetBitLengthOfParam(roReport)) + Util.GetBitLengthOfParam(accessReport)) + Util.GetBitLengthOfParam(configState)) + Util.GetBitLengthOfParam(keepAlive)) + Util.GetTotalBitLengthOfParam<GpiPortCurrentState>(gpiCurrentStates)) + Util.GetTotalBitLengthOfParam<GpoWriteData>(gpoWriteDatas)) + Util.GetBitLengthOfParam(eventsAndReport)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(customs);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Get Reader Configuration Response>");
            strBuilder.Append(base.ToString());
            Util.ToString(this.LlrpConfigurationState, strBuilder);
            Util.ToString(this.ReaderEventNotification, strBuilder);
            Util.ToString<Kalitte.Sensors.Rfid.Llrp.Core.AntennaProperties>(this.AntennaProperties, strBuilder);
            Util.ToString<AntennaConfiguration>(this.AntennaConfigurations, strBuilder);
            Util.ToString(this.ROReportSpec, strBuilder);
            Util.ToString(this.AccessReportSpec, strBuilder);
            Util.ToString(this.Identification, strBuilder);
            Util.ToString(this.KeepAlive, strBuilder);
            Util.ToString<GpiPortCurrentState>(this.GpiCurrentStates, strBuilder);
            Util.ToString<GpoWriteData>(this.GpoData, strBuilder);
            Util.ToString(this.EventAndReport, strBuilder);
            Util.ToString<CustomParameterBase>(this.CustomParameters, strBuilder);
            strBuilder.Append("</Get Reader Configuration Response>");
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

        public IdentificationParameter Identification
        {
            get
            {
                return this.m_identification;
            }
        }

        public KeepAliveSpec KeepAlive
        {
            get
            {
                return this.m_keepAlive;
            }
        }

        public LlrpConfigurationStateValue LlrpConfigurationState
        {
            get
            {
                return this.m_llrpConfigState;
            }
        }

        public ReaderEventNotificationSpec ReaderEventNotification
        {
            get
            {
                return this.m_readerEventNotification;
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
