namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    [Serializable]
    public sealed class ROSpec : LlrpTlvParameterBase
    {
        private Collection<AISpec> m_aiSpec;
        private ROBoundarySpec m_boundaryConditions;
        private ROSpecState m_currentState;
        private Collection<CustomParameterBase> m_customParameters;
        private uint m_Id;
        private byte m_priority;
        private ROReportSpec m_reportSpec;
        private Collection<Kalitte.Sensors.Rfid.Llrp.Core.RFSurveySpec> m_rfSurveySpec;

        internal ROSpec(BitArray bitArray, ref int index) : base(LlrpParameterType.ROSpec, bitArray, index)
        {
            LlrpParameterType type;
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            uint id = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            byte priority = (byte) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8);
            ROSpecState enumInstance = BitHelper.GetEnumInstance<ROSpecState>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            ROBoundarySpec boundarySpec = null;
            Collection<AISpec> aiSpec = new Collection<AISpec>();
            Collection<Kalitte.Sensors.Rfid.Llrp.Core.RFSurveySpec> rfSurvey = new Collection<Kalitte.Sensors.Rfid.Llrp.Core.RFSurveySpec>();
            Collection<CustomParameterBase> customParams = new Collection<CustomParameterBase>();
            ROReportSpec reportSpec = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ROBoundarySpec, bitArray, index, parameterEndLimit))
            {
                boundarySpec = new ROBoundarySpec(bitArray, ref index);
            }
            Collection<LlrpParameterType> expectedTypes = new Collection<LlrpParameterType>();
            expectedTypes.Add(LlrpParameterType.AISpec);
            expectedTypes.Add(LlrpParameterType.RFSurveySpec);
            expectedTypes.Add(LlrpParameterType.Custom);
            while (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, parameterEndLimit, out type))
            {
                LlrpParameterType type2 = type;
                if (type2 != LlrpParameterType.AISpec)
                {
                    if (type2 == LlrpParameterType.RFSurveySpec)
                    {
                        goto Label_00D0;
                    }
                    if (type2 == LlrpParameterType.Custom)
                    {
                        goto Label_00E0;
                    }
                }
                else
                {
                    aiSpec.Add(new AISpec(bitArray, ref index));
                }
                continue;
            Label_00D0:
                rfSurvey.Add(new Kalitte.Sensors.Rfid.Llrp.Core.RFSurveySpec(bitArray, ref index));
                continue;
            Label_00E0:
                customParams.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ROReportSpec, bitArray, index, parameterEndLimit))
            {
                reportSpec = new ROReportSpec(bitArray, ref index);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(id, priority, enumInstance, boundarySpec, aiSpec, rfSurvey, customParams, reportSpec);
        }

        public ROSpec(byte priority, ROBoundarySpec boundarySpec, Collection<AISpec> aiSpec, Collection<Kalitte.Sensors.Rfid.Llrp.Core.RFSurveySpec> rfSurvey, Collection<CustomParameterBase> customParameters, ROReportSpec reportSpec) : base(LlrpParameterType.ROSpec)
        {
            this.Init(IdGenerator.GenerateROSpecId(), priority, ROSpecState.Disabled, boundarySpec, aiSpec, rfSurvey, customParameters, reportSpec);
        }

        public ROSpec(uint id, byte priority, ROBoundarySpec boundarySpec, Collection<AISpec> aiSpec, Collection<Kalitte.Sensors.Rfid.Llrp.Core.RFSurveySpec> rfSurvey, Collection<CustomParameterBase> customParameters, ROReportSpec reportSpec) : base(LlrpParameterType.ROSpec)
        {
            this.Init(id, priority, ROSpecState.Disabled, boundarySpec, aiSpec, rfSurvey, customParameters, reportSpec);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.Id, 0x20, true);
            stream.Append((long) this.Priority, 8, true);
            stream.Append((long) ((byte) this.CurrentState), 8, true);
            Util.Encode(this.BoundarySpec, stream);
            Util.Encode<AISpec>(this.AISpecs, stream);
            Util.Encode<RFSurveySpec>(this.RFSurveySpec, stream);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
            Util.Encode(this.ReportSpec, stream);
        }

        private void Init(uint id, byte priority, ROSpecState currentState, ROBoundarySpec boundarySpec, Collection<AISpec> aiSpec, Collection<Kalitte.Sensors.Rfid.Llrp.Core.RFSurveySpec> rfSurvey, Collection<CustomParameterBase> customParams, ROReportSpec reportSpec)
        {
            if (id == 0)
            {
                throw new ArgumentOutOfRangeException("id");
            }
            if ((priority < 0) || (priority > 7))
            {
                throw new ArgumentOutOfRangeException("priority");
            }
            if (boundarySpec == null)
            {
                throw new ArgumentNullException("boundarySpec");
            }
            if (((aiSpec == null) && (rfSurvey == null)) && (customParams == null))
            {
                throw new ArgumentException(LlrpResources.ROSpecNoSpec);
            }
            Util.CheckCollectionForNonNullElement<AISpec>(aiSpec);
            Util.CheckCollectionForNonNullElement<Kalitte.Sensors.Rfid.Llrp.Core.RFSurveySpec>(rfSurvey);
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customParams);
            this.m_Id = id;
            this.m_priority = priority;
            this.m_boundaryConditions = boundarySpec;
            this.m_aiSpec = aiSpec;
            this.m_rfSurveySpec = rfSurvey;
            this.m_customParameters = customParams;
            this.m_reportSpec = reportSpec;
            this.m_currentState = currentState;
            this.ParameterLength = ((((0x30 + Util.GetBitLengthOfParam(this.BoundarySpec)) + Util.GetTotalBitLengthOfParam<AISpec>(this.AISpecs)) + Util.GetTotalBitLengthOfParam<Kalitte.Sensors.Rfid.Llrp.Core.RFSurveySpec>(this.RFSurveySpec)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.CustomParameters)) + Util.GetBitLengthOfParam(this.ReportSpec);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            Util.ToStringSerialize(this, strBuilder);
            return strBuilder.ToString();
        }

        public Collection<AISpec> AISpecs
        {
            get
            {
                return this.m_aiSpec;
            }
        }

        public ROBoundarySpec BoundarySpec
        {
            get
            {
                return this.m_boundaryConditions;
            }
        }

        public ROSpecState CurrentState
        {
            get
            {
                return this.m_currentState;
            }
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customParameters;
            }
        }

        public uint Id
        {
            get
            {
                return this.m_Id;
            }
        }

        public byte Priority
        {
            get
            {
                return this.m_priority;
            }
        }

        public ROReportSpec ReportSpec
        {
            get
            {
                return this.m_reportSpec;
            }
        }

        public Collection<Kalitte.Sensors.Rfid.Llrp.Core.RFSurveySpec> RFSurveySpec
        {
            get
            {
                return this.m_rfSurveySpec;
            }
        }
    }
}
