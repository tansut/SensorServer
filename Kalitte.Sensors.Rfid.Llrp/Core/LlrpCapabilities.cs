namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class LlrpCapabilities : LlrpTlvParameterBase
    {
        private bool m_canDoRFSurvey;
        private bool m_canDoTagInventoryStateAwareSingulation;
        private bool m_canReportBufferFillWarning;
        private ushort m_clientRequestOpSpecTimeout;
        private uint m_maxNumAccessSpecs;
        private uint m_maxNumInventoryParmeterSpecPerAISpecs;
        private uint m_maxNumOfSpecPerROSpec;
        private uint m_maxNumOpSpecsPerAccessSpec;
        private byte m_maxPriorityLevelSupported;
        private uint m_maxROSpec;
        private bool m_supportsClientRequestOpSpec;
        private bool m_supportsEventAndReportHandling;

        internal LlrpCapabilities(BitArray bitArray, ref int index) : base(LlrpParameterType.LlrpCapabilities, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            bool canDoRFSurvey = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool canReportBufferFillWarning = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool supportsClientRequestOpSpec = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool canDoTagInventoryStateAwareSingulation = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool supportsEventAndReportHandling = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 3;
            byte maxPriorityLevelSupported = (byte) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8);
            ushort clientRequestOpSpecTimeout = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            uint maxROSpec = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint maxNumOfSpecPerROSpec = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint maxNumInventoryParmeterSpecPerAISpecs = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint maxNumAccessSpecs = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint maxNumOpSpecsPerAccessSpec = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(canDoRFSurvey, canReportBufferFillWarning, supportsClientRequestOpSpec, canDoTagInventoryStateAwareSingulation, supportsEventAndReportHandling, maxPriorityLevelSupported, clientRequestOpSpecTimeout, maxROSpec, maxNumOfSpecPerROSpec, maxNumInventoryParmeterSpecPerAISpecs, maxNumAccessSpecs, maxNumOpSpecsPerAccessSpec);
        }

        public LlrpCapabilities(bool canDoRFSurvey, bool canReportBufferFillWarning, bool supportsClientRequestOpSpec, bool canDoTagInventoryStateAwareSingulation, bool supportsEventAndReportHandling, byte maxPriorityLevelSupported, ushort clientRequestOpSpecTimeout, uint maxROSpec, uint maxNumberOfSpecPerROSpec, uint maxNumberInventoryParameterSpecPerAISpecs, uint maxNumberAccessSpecs, uint maxNumberOpSpecsPerAccessSpec) : base(LlrpParameterType.LlrpCapabilities)
        {
            this.Init(canDoRFSurvey, canReportBufferFillWarning, supportsClientRequestOpSpec, canDoTagInventoryStateAwareSingulation, supportsEventAndReportHandling, maxPriorityLevelSupported, clientRequestOpSpecTimeout, maxROSpec, maxNumberOfSpecPerROSpec, maxNumberInventoryParameterSpecPerAISpecs, maxNumberAccessSpecs, maxNumberOpSpecsPerAccessSpec);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append(this.CanDoRFSurvey, 1, true);
            stream.Append(this.CanReportBufferFillWarning, 1, true);
            stream.Append(this.SupportsClientRequestOPSpec, 1, true);
            stream.Append(this.CanDoTagInventoryStateAwareSingulation, 1, true);
            stream.Append(this.SupportsEventAndReportHandling, 1, true);
            stream.Append((ulong) 0L, 3, true);
            stream.Append((long) this.MaximumPriorityLevelSupported, 8, true);
            stream.Append((long) this.ClientRequestsOPSpecTimeout, 0x10, true);
            stream.Append((long) this.MaximumROSpec, 0x20, true);
            stream.Append((long) this.MaximumNumberOfSpecPerROSpec, 0x20, true);
            stream.Append((long) this.MaximumNumberInventoryParameterSpecPerAISpecs, 0x20, true);
            stream.Append((long) this.MaximumNumberOfAccessSpecs, 0x20, true);
            stream.Append((long) this.MaximumNumberOfOPSpecsPerAccessSpecs, 0x20, true);
        }

        private void Init(bool canDoRFSurvey, bool canReportBufferFillWarning, bool supportsClientRequestOpSpec, bool canDoTagInventoryStateAwareSingulation, bool supportsEventAndReportHandling, byte maxPriorityLevelSupported, ushort clientRequestOpSpecTimeout, uint maxROSpec, uint maxNumOfSpecPerROSpec, uint maxNumInventoryParmeterSpecPerAISpecs, uint maxNumAccessSpecs, uint maxNumOpSpecsPerAccessSpec)
        {
            this.m_canDoRFSurvey = canDoRFSurvey;
            this.m_canReportBufferFillWarning = canReportBufferFillWarning;
            this.m_supportsClientRequestOpSpec = supportsClientRequestOpSpec;
            this.m_canDoTagInventoryStateAwareSingulation = canDoTagInventoryStateAwareSingulation;
            this.m_supportsEventAndReportHandling = supportsEventAndReportHandling;
            this.m_maxPriorityLevelSupported = maxPriorityLevelSupported;
            this.m_clientRequestOpSpecTimeout = clientRequestOpSpecTimeout;
            this.m_maxROSpec = maxROSpec;
            this.m_maxNumOfSpecPerROSpec = maxNumOfSpecPerROSpec;
            this.m_maxNumInventoryParmeterSpecPerAISpecs = maxNumInventoryParmeterSpecPerAISpecs;
            this.m_maxNumAccessSpecs = maxNumAccessSpecs;
            this.m_maxNumOpSpecsPerAccessSpec = maxNumOpSpecsPerAccessSpec;
            this.ParameterLength = 0xc0;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<LLRP Capabilities>");
            builder.Append(base.ToString());
            builder.Append("<Can RF Survey>");
            builder.Append(this.CanDoRFSurvey);
            builder.Append("</Can RF Survey>");
            builder.Append("<Can Do Tag Inventory State Aware Singulation>");
            builder.Append(this.CanDoTagInventoryStateAwareSingulation);
            builder.Append("</Can Do Tag Inventory State Aware Singulation>");
            builder.Append("<Can Report Buffer Fill Warning>");
            builder.Append(this.CanReportBufferFillWarning);
            builder.Append("</Can Report Buffer Fill Warning>");
            builder.Append("<Maximum Number of RO Spec>");
            builder.Append(this.MaximumROSpec);
            builder.Append("</Maximum Number of RO Spec>");
            builder.Append("<Maximum Number of specs per RO Spec>");
            builder.Append(this.MaximumNumberOfSpecPerROSpec);
            builder.Append("</Maximum Number of specs per RO Spec>");
            builder.Append("<Maximum Number of Inventory specs per AI Spec>");
            builder.Append(this.MaximumNumberInventoryParameterSpecPerAISpecs);
            builder.Append("</Maximum Number of Inventory specs per AI Spec>");
            builder.Append("<Maximum Priority Level supported>");
            builder.Append(this.MaximumPriorityLevelSupported);
            builder.Append("</Maximum Priority Level supported>");
            builder.Append("<Maximum Number of Access Spec>");
            builder.Append(this.MaximumNumberOfAccessSpecs);
            builder.Append("</Maximum Number of Access Spec>");
            builder.Append("<Maximum Number of OP Spec per Access Spec>");
            builder.Append(this.MaximumNumberOfOPSpecsPerAccessSpecs);
            builder.Append("</Maximum Number of OP Spec per Access Spec>");
            builder.Append("<Support client Request OP Spec>");
            builder.Append(this.SupportsClientRequestOPSpec);
            builder.Append("</Support client Request OP Spec>");
            builder.Append("<Client Request Op Spec Timeout>");
            builder.Append(this.ClientRequestsOPSpecTimeout);
            builder.Append("</Client Request Op Spec Timeout>");
            builder.Append("<Supports Events and Report Holding>");
            builder.Append(this.SupportsEventAndReportHandling);
            builder.Append("</Supports Events and Report Holding>");
            builder.Append("</LLRP Capabilities>");
            return builder.ToString();
        }

        public bool CanDoRFSurvey
        {
            get
            {
                return this.m_canDoRFSurvey;
            }
        }

        public bool CanDoTagInventoryStateAwareSingulation
        {
            get
            {
                return this.m_canDoTagInventoryStateAwareSingulation;
            }
        }

        public bool CanReportBufferFillWarning
        {
            get
            {
                return this.m_canReportBufferFillWarning;
            }
        }

        public ushort ClientRequestsOPSpecTimeout
        {
            get
            {
                return this.m_clientRequestOpSpecTimeout;
            }
        }

        public uint MaximumNumberInventoryParameterSpecPerAISpecs
        {
            get
            {
                return this.m_maxNumInventoryParmeterSpecPerAISpecs;
            }
        }

        public uint MaximumNumberOfAccessSpecs
        {
            get
            {
                return this.m_maxNumAccessSpecs;
            }
        }

        public uint MaximumNumberOfOPSpecsPerAccessSpecs
        {
            get
            {
                return this.m_maxNumOpSpecsPerAccessSpec;
            }
        }

        public uint MaximumNumberOfSpecPerROSpec
        {
            get
            {
                return this.m_maxNumOfSpecPerROSpec;
            }
        }

        public byte MaximumPriorityLevelSupported
        {
            get
            {
                return this.m_maxPriorityLevelSupported;
            }
        }

        public uint MaximumROSpec
        {
            get
            {
                return this.m_maxROSpec;
            }
        }

        public bool SupportsClientRequestOPSpec
        {
            get
            {
                return this.m_supportsClientRequestOpSpec;
            }
        }

        public bool SupportsEventAndReportHandling
        {
            get
            {
                return this.m_supportsEventAndReportHandling;
            }
        }
    }
}
