namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class C1G2LockPayload : LlrpTlvParameterBase
    {
        private C1G2LockDataField m_dataField;
        private C1G2LockPrivilege m_privilege;

        public C1G2LockPayload(C1G2LockPrivilege privilege, C1G2LockDataField dataField) : base(LlrpParameterType.C1G2LockPayload)
        {
            this.Init(privilege, dataField);
        }

        internal C1G2LockPayload(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2LockPayload, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            C1G2LockPrivilege enumInstance = BitHelper.GetEnumInstance<C1G2LockPrivilege>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            C1G2LockDataField dataField = BitHelper.GetEnumInstance<C1G2LockDataField>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, dataField);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.Privilege), 8, true);
            stream.Append((long) ((byte) this.DataField), 8, true);
        }

        private void Init(C1G2LockPrivilege privilege, C1G2LockDataField dataField)
        {
            this.m_privilege = privilege;
            this.m_dataField = dataField;
            this.ParameterLength = 0x10;
        }

        public C1G2LockDataField DataField
        {
            get
            {
                return this.m_dataField;
            }
        }

        public C1G2LockPrivilege Privilege
        {
            get
            {
                return this.m_privilege;
            }
        }
    }
}
