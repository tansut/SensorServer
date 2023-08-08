namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class UhfC1G2RFModeTableEntry : LlrpTlvParameterBase
    {
        private uint m_bdrValue;
        private Kalitte.Sensors.Rfid.Llrp.Core.DRValue m_drValue;
        private bool m_epcGlobalTestingAndConformance;
        private Kalitte.Sensors.Rfid.Llrp.Core.ForwardLinkModulation m_flm;
        private uint m_maxTariValue;
        private uint m_minTariValue;
        private uint m_modeIdentifier;
        private Kalitte.Sensors.Rfid.Llrp.Core.MValue m_mValue;
        private uint m_pieValue;
        private Kalitte.Sensors.Rfid.Llrp.Core.SpecialMaskIndicator m_specialMaskIndicator;
        private uint m_stepTariValue;

        internal UhfC1G2RFModeTableEntry(BitArray bitArray, ref int index) : base(LlrpParameterType.UhfC1G2RFModeTableEntry, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            uint modeIdentifier = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            Kalitte.Sensors.Rfid.Llrp.Core.DRValue enumInstance = BitHelper.GetEnumInstance<Kalitte.Sensors.Rfid.Llrp.Core.DRValue>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1));
            bool epcGlobalTestingAndConformance = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 6;
            Kalitte.Sensors.Rfid.Llrp.Core.MValue mValue = BitHelper.GetEnumInstance<Kalitte.Sensors.Rfid.Llrp.Core.MValue>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            Kalitte.Sensors.Rfid.Llrp.Core.ForwardLinkModulation flm = BitHelper.GetEnumInstance<Kalitte.Sensors.Rfid.Llrp.Core.ForwardLinkModulation>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            Kalitte.Sensors.Rfid.Llrp.Core.SpecialMaskIndicator specialMaskIndicator = BitHelper.GetEnumInstance<Kalitte.Sensors.Rfid.Llrp.Core.SpecialMaskIndicator>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            uint bdrValue = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint pieValue = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint minTariValue = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint maxTariValue = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint stepTariValue = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(modeIdentifier, enumInstance, bdrValue, mValue, flm, pieValue, minTariValue, maxTariValue, stepTariValue, specialMaskIndicator, epcGlobalTestingAndConformance);
        }

        public UhfC1G2RFModeTableEntry(uint modeIdentifier, Kalitte.Sensors.Rfid.Llrp.Core.DRValue drValue, uint backscatterDataRateValue, Kalitte.Sensors.Rfid.Llrp.Core.MValue modulationValue, Kalitte.Sensors.Rfid.Llrp.Core.ForwardLinkModulation forwardLinkModulation, uint pieValue, uint minTariValue, uint maxTariValue, uint stepTariValue, Kalitte.Sensors.Rfid.Llrp.Core.SpecialMaskIndicator specialMaskIndicator, bool epcGlobalTestingAndConformance) : base(LlrpParameterType.UhfC1G2RFModeTableEntry)
        {
            this.Init(modeIdentifier, drValue, backscatterDataRateValue, modulationValue, forwardLinkModulation, pieValue, minTariValue, maxTariValue, stepTariValue, specialMaskIndicator, epcGlobalTestingAndConformance);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.ModeIdentifier, 0x20, true);
            stream.Append((long) ((byte) this.DRValue), 1, true);
            stream.Append(this.EpcGlobalTestingAndConformance, 1, true);
            stream.Append((ulong) 0L, 6, true);
            stream.Append((long) ((byte) this.MValue), 8, true);
            stream.Append((long) ((byte) this.ForwardLinkModulation), 8, true);
            stream.Append((long) ((byte) this.SpecialMaskIndicator), 8, true);
            stream.Append((long) this.BackscatterDataRateValue, 0x20, true);
            stream.Append((long) this.PieValue, 0x20, true);
            stream.Append((long) this.MinimumTariValue, 0x20, true);
            stream.Append((long) this.MaximumTariValue, 0x20, true);
            stream.Append((long) this.StepTariValue, 0x20, true);
        }

        private void Init(uint modeIdentifier, Kalitte.Sensors.Rfid.Llrp.Core.DRValue drValue, uint bdrValue, Kalitte.Sensors.Rfid.Llrp.Core.MValue mValue, Kalitte.Sensors.Rfid.Llrp.Core.ForwardLinkModulation flm, uint pieValue, uint minTariValue, uint maxTariValue, uint stepTariValue, Kalitte.Sensors.Rfid.Llrp.Core.SpecialMaskIndicator specialMaskIndicator, bool epcGlobalTestingAndConformance)
        {
            this.m_modeIdentifier = modeIdentifier;
            this.m_drValue = drValue;
            this.m_bdrValue = bdrValue;
            this.m_mValue = mValue;
            this.m_flm = flm;
            this.m_pieValue = pieValue;
            this.m_minTariValue = minTariValue;
            this.m_maxTariValue = maxTariValue;
            this.m_stepTariValue = stepTariValue;
            this.m_specialMaskIndicator = specialMaskIndicator;
            this.m_epcGlobalTestingAndConformance = epcGlobalTestingAndConformance;
            this.ParameterLength = 0xe0;
        }

        public uint BackscatterDataRateValue
        {
            get
            {
                return this.m_bdrValue;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.DRValue DRValue
        {
            get
            {
                return this.m_drValue;
            }
        }

        public bool EpcGlobalTestingAndConformance
        {
            get
            {
                return this.m_epcGlobalTestingAndConformance;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.ForwardLinkModulation ForwardLinkModulation
        {
            get
            {
                return this.m_flm;
            }
        }

        public uint MaximumTariValue
        {
            get
            {
                return this.m_maxTariValue;
            }
        }

        public uint MinimumTariValue
        {
            get
            {
                return this.m_minTariValue;
            }
        }

        public uint ModeIdentifier
        {
            get
            {
                return this.m_modeIdentifier;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.MValue MValue
        {
            get
            {
                return this.m_mValue;
            }
        }

        public uint PieValue
        {
            get
            {
                return this.m_pieValue;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.SpecialMaskIndicator SpecialMaskIndicator
        {
            get
            {
                return this.m_specialMaskIndicator;
            }
        }

        public uint StepTariValue
        {
            get
            {
                return this.m_stepTariValue;
            }
        }
    }
}
