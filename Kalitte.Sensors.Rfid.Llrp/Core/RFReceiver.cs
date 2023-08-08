namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class RFReceiver : LlrpTlvParameterBase
    {
        private ushort m_indexIntoReceiverSensitivityTable;

        public RFReceiver(ushort indexIntoReceiverSensitivityTable) : base(LlrpParameterType.RFReceiver)
        {
            this.Init(indexIntoReceiverSensitivityTable);
        }

        internal RFReceiver(BitArray bitArray, ref int index) : base(LlrpParameterType.RFReceiver, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort indexIntoReceiverSensitivityTable = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(indexIntoReceiverSensitivityTable);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.IndexIntoReceiverSensitivityTable, 0x10, true);
        }

        private void Init(ushort indexIntoReceiverSensitivityTable)
        {
            this.m_indexIntoReceiverSensitivityTable = indexIntoReceiverSensitivityTable;
            this.ParameterLength = 0x10;
        }

        public ushort IndexIntoReceiverSensitivityTable
        {
            get
            {
                return this.m_indexIntoReceiverSensitivityTable;
            }
        }
    }
}
