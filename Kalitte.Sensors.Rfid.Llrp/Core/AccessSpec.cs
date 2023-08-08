namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class AccessSpec : LlrpTlvParameterBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.AccessCommand m_accessCommand;
        private ushort m_antennaId;
        private Collection<CustomParameterBase> m_customs;
        private bool m_disabled;
        private Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolId m_protocolId;
        private AccessReportSpec m_reportSpec;
        private uint m_roSpecId;
        private uint m_specId;
        private AccessSpecStopTrigger m_stopTrigger;

        internal AccessSpec(BitArray bitArray, ref int index) : base(LlrpParameterType.AccessSpec, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            uint specId = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            ushort antennaId = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolId enumInstance = BitHelper.GetEnumInstance<Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolId>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            bool currentState = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 7;
            uint roSpecId = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            AccessSpecStopTrigger trigger = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AccessSpecStopTrigger, bitArray, index, parameterEndLimit))
            {
                trigger = new AccessSpecStopTrigger(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.AccessCommand cmd = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AccessSpecCommand, bitArray, index, parameterEndLimit))
            {
                cmd = new Kalitte.Sensors.Rfid.Llrp.Core.AccessCommand(bitArray, ref index);
            }
            AccessReportSpec report = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AccessReportSpec, bitArray, index, parameterEndLimit))
            {
                report = new AccessReportSpec(bitArray, ref index);
            }
            Collection<CustomParameterBase> customs = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index, parameterEndLimit))
            {
                customs.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(specId, antennaId, enumInstance, currentState, roSpecId, trigger, cmd, report, customs);
        }

        public AccessSpec(ushort antennaId, Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolId protocolId, uint roSpecId, AccessSpecStopTrigger trigger, Kalitte.Sensors.Rfid.Llrp.Core.AccessCommand cmd, AccessReportSpec report, Collection<CustomParameterBase> customs) : base(LlrpParameterType.AccessSpec)
        {
            this.Init(IdGenerator.GenerateAccessSpecId(), antennaId, protocolId, false, roSpecId, trigger, cmd, report, customs);
        }

        public AccessSpec(uint id, ushort antennaId, Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolId protocolId, uint roSpecId, AccessSpecStopTrigger trigger, Kalitte.Sensors.Rfid.Llrp.Core.AccessCommand cmd, AccessReportSpec report, Collection<CustomParameterBase> customs) : base(LlrpParameterType.AccessSpec)
        {
            this.Init(id, antennaId, protocolId, false, roSpecId, trigger, cmd, report, customs);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.Id, 0x20, true);
            stream.Append((long) this.AntennaId, 0x10, true);
            stream.Append((long) this.AirProtocolId, 8, true);
            stream.Append(this.CurrentState, 1, true);
            stream.Append((ulong) 0L, 7, true);
            stream.Append((long) this.ROSpecId, 0x20, true);
            Util.Encode(this.StopTrigger, stream);
            Util.Encode(this.AccessCommand, stream);
            Util.Encode(this.ReportSpec, stream);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
        }

        private void Init(uint specId, ushort antennaId, Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolId protocolId, bool currentState, uint roSpecId, AccessSpecStopTrigger trigger, Kalitte.Sensors.Rfid.Llrp.Core.AccessCommand cmd, AccessReportSpec report, Collection<CustomParameterBase> customs)
        {
            if (specId == 0)
            {
                throw new ArgumentOutOfRangeException("specId");
            }
            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customs);
            this.m_specId = specId;
            this.m_antennaId = antennaId;
            this.m_protocolId = protocolId;
            this.m_disabled = currentState;
            this.m_roSpecId = roSpecId;
            this.m_stopTrigger = trigger;
            this.m_accessCommand = cmd;
            this.m_reportSpec = report;
            this.m_customs = customs;
            this.ParameterLength = (((0x60 + Util.GetBitLengthOfParam(this.StopTrigger)) + Util.GetBitLengthOfParam(this.AccessCommand)) + Util.GetBitLengthOfParam(this.ReportSpec)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.CustomParameters);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            Util.ToStringSerialize(this, strBuilder);
            return strBuilder.ToString();
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.AccessCommand AccessCommand
        {
            get
            {
                return this.m_accessCommand;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolId AirProtocolId
        {
            get
            {
                return this.m_protocolId;
            }
        }

        public ushort AntennaId
        {
            get
            {
                return this.m_antennaId;
            }
        }

        public bool CurrentState
        {
            get
            {
                return this.m_disabled;
            }
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customs;
            }
        }

        public uint Id
        {
            get
            {
                return this.m_specId;
            }
        }

        public AccessReportSpec ReportSpec
        {
            get
            {
                return this.m_reportSpec;
            }
        }

        public uint ROSpecId
        {
            get
            {
                return this.m_roSpecId;
            }
        }

        public AccessSpecStopTrigger StopTrigger
        {
            get
            {
                return this.m_stopTrigger;
            }
        }
    }
}
