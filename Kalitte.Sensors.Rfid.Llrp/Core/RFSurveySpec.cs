namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class RFSurveySpec : LlrpTlvParameterBase
    {
        private ushort m_antennaId;
        private Collection<CustomParameterBase> m_customParameter;
        private uint m_endFrequency;
        private uint m_startFrequency;
        private RFSurveySpecStopTrigger m_stopTrigger;

        internal RFSurveySpec(BitArray bitArray, ref int index) : base(LlrpParameterType.RFSurveySpec, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort antennaId = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            uint startFrequency = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint endFrequency = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            RFSurveySpecStopTrigger stopTrigger = null;
            Collection<CustomParameterBase> customParameter = new Collection<CustomParameterBase>();
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.RFSurveySpecStopTrigger, bitArray, index, parameterEndLimit))
            {
                stopTrigger = new RFSurveySpecStopTrigger(bitArray, ref index);
            }
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index, parameterEndLimit))
            {
                customParameter.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(antennaId, startFrequency, endFrequency, stopTrigger, customParameter);
        }

        public RFSurveySpec(ushort antennaId, uint startFrequency, uint endFrequency, RFSurveySpecStopTrigger stopTrigger, Collection<CustomParameterBase> customParameter) : base(LlrpParameterType.RFSurveySpec)
        {
            this.Init(antennaId, startFrequency, endFrequency, stopTrigger, customParameter);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.AntennaId, 0x10, true);
            stream.Append((long) this.StartFrequency, 0x20, true);
            stream.Append((long) this.EndFrequency, 0x20, true);
            Util.Encode(this.StopTrigger, stream);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
        }

        private void Init(ushort antennaId, uint startFrequency, uint endFrequency, RFSurveySpecStopTrigger stopTrigger, Collection<CustomParameterBase> customParameter)
        {
            if (stopTrigger == null)
            {
                throw new ArgumentNullException("stopTrigger");
            }
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customParameter);
            this.m_antennaId = antennaId;
            this.m_startFrequency = startFrequency;
            this.m_endFrequency = endFrequency;
            this.m_stopTrigger = stopTrigger;
            this.m_customParameter = customParameter;
            this.ParameterLength = (80 + Util.GetBitLengthOfParam(this.StopTrigger)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.CustomParameters);
        }

        public ushort AntennaId
        {
            get
            {
                return this.m_antennaId;
            }
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customParameter;
            }
        }

        public uint EndFrequency
        {
            get
            {
                return this.m_endFrequency;
            }
        }

        public uint StartFrequency
        {
            get
            {
                return this.m_startFrequency;
            }
        }

        public RFSurveySpecStopTrigger StopTrigger
        {
            get
            {
                return this.m_stopTrigger;
            }
        }
    }
}
