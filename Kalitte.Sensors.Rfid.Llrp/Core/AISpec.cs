namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class AISpec : LlrpTlvParameterBase
    {
        private Collection<ushort> m_antennaIds;
        private Collection<CustomParameterBase> m_customs;
        private Collection<Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpec> m_inventoryParamSpecs;
        private AISpecStopTrigger m_stopTrigger;

        internal AISpec(BitArray bitArray, ref int index) : base(LlrpParameterType.AISpec, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort num2 = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            Collection<ushort> antennaIds = new Collection<ushort>();
            for (int i = 0; i < num2; i++)
            {
                antennaIds.Add((ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10));
            }
            AISpecStopTrigger stopTrigger = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AISpecStopTrigger, bitArray, index, parameterEndLimit))
            {
                stopTrigger = new AISpecStopTrigger(bitArray, ref index);
            }
            Collection<Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpec> inventoryParamSpecs = new Collection<Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpec>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.InventoryParameterSpec, bitArray, index, parameterEndLimit))
            {
                inventoryParamSpecs.Add(new Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpec(bitArray, ref index));
            }
            Collection<CustomParameterBase> customParams = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index, parameterEndLimit))
            {
                customParams.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(antennaIds, stopTrigger, inventoryParamSpecs, customParams);
        }

        public AISpec(Collection<ushort> antennaIds, AISpecStopTrigger stopTrigger, Collection<Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpec> inventoryParameterSpecs, Collection<CustomParameterBase> customParameters) : base(LlrpParameterType.AISpec)
        {
            this.Init(antennaIds, stopTrigger, inventoryParameterSpecs, customParameters);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            ushort num = (this.AntennaIds != null) ? ((ushort) this.AntennaIds.Count) : ((ushort) 0);
            stream.Append((long) num, 0x10, true);
            if (num > 0)
            {
                foreach (ushort num2 in this.AntennaIds)
                {
                    stream.Append((long) num2, 0x10, true);
                }
            }
            Util.Encode(this.StopTrigger, stream);
            Util.Encode<Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpec>(this.InventoryParameterSpec, stream);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
        }

        private void Init(Collection<ushort> antennaIds, AISpecStopTrigger stopTrigger, Collection<Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpec> inventoryParamSpecs, Collection<CustomParameterBase> customParams)
        {
            if (antennaIds == null)
            {
                throw new ArgumentNullException("antennaIds");
            }
            if (antennaIds.Count == 0)
            {
                throw new ArgumentOutOfRangeException("antennaIds");
            }
            if (stopTrigger == null)
            {
                throw new ArgumentNullException("stopTrigger");
            }
            if ((inventoryParamSpecs == null) || (inventoryParamSpecs.Count == 0))
            {
                throw new ArgumentException("inventoryParamSpecs");
            }
            Util.CheckCollectionForNonNullElement<Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpec>(inventoryParamSpecs);
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(customParams);
            this.m_antennaIds = antennaIds;
            this.m_stopTrigger = stopTrigger;
            this.m_inventoryParamSpecs = inventoryParamSpecs;
            this.m_customs = customParams;
            ushort num = (this.AntennaIds != null) ? ((ushort) (this.AntennaIds.Count * 0x10)) : ((ushort) 0);
            this.ParameterLength = ((((uint) (0x10 + num)) + this.StopTrigger.ParameterLength) + Util.GetTotalBitLengthOfParam<Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpec>(this.InventoryParameterSpec)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.CustomParameters);
        }

        public Collection<ushort> AntennaIds
        {
            get
            {
                return this.m_antennaIds;
            }
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_customs;
            }
        }

        public Collection<Kalitte.Sensors.Rfid.Llrp.Core.InventoryParameterSpec> InventoryParameterSpec
        {
            get
            {
                return this.m_inventoryParamSpecs;
            }
        }

        public AISpecStopTrigger StopTrigger
        {
            get
            {
                return this.m_stopTrigger;
            }
        }
    }
}
