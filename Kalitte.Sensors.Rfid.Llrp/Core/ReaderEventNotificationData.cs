namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    public sealed class ReaderEventNotificationData : LlrpTlvParameterBase
    {
        private Collection<CustomParameterBase> m_customParameters;
        private Collection<LlrpEvent> m_llrpEvents;
        private Kalitte.Sensors.Rfid.Llrp.Core.Uptime m_uptime;
        private Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp m_utcTimestamp;

        internal ReaderEventNotificationData(BitArray bitArray, ref int index) : base(LlrpParameterType.ReaderEventNotificationData, bitArray, index)
        {
            LlrpParameterType type;
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Collection<LlrpParameterType> expectedTypes = new Collection<LlrpParameterType>();
            expectedTypes.Add(LlrpParameterType.UtcTimestamp);
            expectedTypes.Add(LlrpParameterType.Uptime);
            Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp utcTimestamp = null;
            Kalitte.Sensors.Rfid.Llrp.Core.Uptime upTime = null;
            if (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, out type))
            {
                switch (type)
                {
                    case LlrpParameterType.UtcTimestamp:
                        utcTimestamp = new Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp(bitArray, ref index);
                        break;

                    case LlrpParameterType.Uptime:
                        upTime = new Kalitte.Sensors.Rfid.Llrp.Core.Uptime(bitArray, ref index);
                        break;
                }
            }
            Collection<LlrpEvent> llrpEvents = new Collection<LlrpEvent>();
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.HoppingEvent, bitArray, index, parameterEndLimit))
            {
                llrpEvents.Add(new HoppingEvent(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.GpiEvent, bitArray, index, parameterEndLimit))
            {
                llrpEvents.Add(new GpiEvent(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ROSpecEvent, bitArray, index, parameterEndLimit))
            {
                llrpEvents.Add(new ROSpecEvent(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ReportBufferLevelWarningEvent, bitArray, index, parameterEndLimit))
            {
                llrpEvents.Add(new ReportBufferLevelWarningEvent(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ReportBufferOverflowErrorEvent, bitArray, index, parameterEndLimit))
            {
                llrpEvents.Add(new ReportBufferOverflowErrorEvent(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ReaderExceptionEvent, bitArray, index, parameterEndLimit))
            {
                llrpEvents.Add(new ReaderExceptionEvent(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.RFSurveyEvent, bitArray, index, parameterEndLimit))
            {
                llrpEvents.Add(new RFSurveyEvent(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AISpecEvent, bitArray, index, parameterEndLimit))
            {
                llrpEvents.Add(new AISpecEvent(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AntennaEvent, bitArray, index, parameterEndLimit))
            {
                llrpEvents.Add(new AntennaEvent(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ConnectionAttemptEvent, bitArray, index, parameterEndLimit))
            {
                llrpEvents.Add(new ConnectionAttemptEvent(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ConnectionCloseEvent, bitArray, index, parameterEndLimit))
            {
                llrpEvents.Add(new ConnectionCloseEvent(bitArray, ref index));
            }
            Collection<CustomParameterBase> customParameters = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index, parameterEndLimit))
            {
                customParameters.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(utcTimestamp, upTime, llrpEvents, customParameters);
        }

        public ReaderEventNotificationData(Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp utcTimestamp, Kalitte.Sensors.Rfid.Llrp.Core.Uptime uptime, Collection<LlrpEvent> llrpEvents, Collection<CustomParameterBase> customParameters) : base(LlrpParameterType.ReaderEventNotificationData)
        {
            this.Init(utcTimestamp, uptime, llrpEvents, customParameters);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            if (this.m_utcTimestamp != null)
            {
                Util.Encode(this.m_utcTimestamp, stream);
            }
            else
            {
                Util.Encode(this.m_uptime, stream);
            }
            foreach (LlrpEvent event2 in this.m_llrpEvents)
            {
                Util.Encode(event2, stream);
            }
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
        }

        private void Init(Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp utcTimestamp, Kalitte.Sensors.Rfid.Llrp.Core.Uptime upTime, Collection<LlrpEvent> llrpEvents, Collection<CustomParameterBase> customParameters)
        {
            if ((utcTimestamp == null) && (upTime == null))
            {
                throw new ArgumentException(LlrpResources.NoneTimestampPresent);
            }
            if ((upTime != null) && (utcTimestamp != null))
            {
                throw new ArgumentException(LlrpResources.BothTimestampPresent);
            }
            Util.CheckCollectionForNonNullElement<LlrpEvent>(llrpEvents);
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customParameters);
            this.m_utcTimestamp = utcTimestamp;
            this.m_uptime = upTime;
            this.m_llrpEvents = llrpEvents;
            this.m_customParameters = customParameters;
            this.ParameterLength = ((Util.GetBitLengthOfParam(this.UtcTimestamp) + Util.GetBitLengthOfParam(this.Uptime)) + Util.GetTotalBitLengthOfParam<LlrpEvent>(llrpEvents)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.CustomParameters);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Reader Event Notification Data>");
            strBuilder.Append(base.ToString());
            Util.ToString(this.UtcTimestamp, strBuilder);
            Util.ToString(this.Uptime, strBuilder);
            Util.ToString<LlrpEvent>(this.LlrpEvents, strBuilder);
            Util.ToString<CustomParameterBase>(this.CustomParameters, strBuilder);
            strBuilder.Append("</Reader Event Notification Data>");
            return strBuilder.ToString();
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customParameters;
            }
        }

        public Collection<LlrpEvent> LlrpEvents
        {
            get
            {
                return this.m_llrpEvents;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.Uptime Uptime
        {
            get
            {
                return this.m_uptime;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.UtcTimestamp UtcTimestamp
        {
            get
            {
                return this.m_utcTimestamp;
            }
        }
    }
}
