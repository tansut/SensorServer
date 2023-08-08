namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    
    
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Events.Management;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Llrp.Events;
    using Kalitte.Sensors.Events;

    public sealed class ReaderExceptionEvent : LlrpEvent
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.AccessSpecId m_accessSpecId;
        private Kalitte.Sensors.Rfid.Llrp.Core.AntennaId m_antennaId;
        private Collection<CustomParameterBase> m_customParamters;
        private InventoryParameterSpecId m_inventorySpecId;
        private string m_message;
        private Kalitte.Sensors.Rfid.Llrp.Core.OPSpecId m_opSpecId;
        private Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId m_roSpecId;
        private Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex m_specIndex;

        internal override Notification ConvertToRfidNotification()
        {
            VendorData vendorData = new VendorData();
            if (this.ROSpecId != null)
            {
                vendorData.Add("RO Spec Id", this.ROSpecId.Id);
            }
            if (this.SpecIndex != null)
            {
                vendorData.Add("Spec Index", this.SpecIndex.Index);
            }
            if (this.InventorySpecId != null)
            {
                vendorData.Add("Inventory Parameter Spec Id", this.InventorySpecId.Id);
            }
            if (this.AntennaId != null)
            {
                vendorData.Add("Antenna Name", Util.GetAntennaName(this.AntennaId.Id));
            }
            if (this.AccessSpecId != null)
            {
                vendorData.Add("Access spec Id", this.AccessSpecId.Id);
            }
            if (this.OPSpecId != null)
            {
                vendorData.Add("Operation Spec Id", this.OPSpecId.Id);
            }
            vendorData.Add("Message", this.Message);
            return new Notification(new VendorDefinedManagementEvent(EventLevel.Error, LlrpEventTypes.ReaderExceptionEvent, LlrpEventTypes.ReaderExceptionEvent.Description, typeof(ReaderExceptionEvent).Name, vendorData));
        }

 

 


        internal ReaderExceptionEvent(BitArray bitArray, ref int index) : base(bitArray, index, LlrpParameterType.ReaderExceptionEvent)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort byteCount = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            string message = BitHelper.GetString(bitArray, ref index, byteCount);
            Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId roSpecId = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ROSpecId, bitArray, index, parameterEndLimit))
            {
                roSpecId = new Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex specIndex = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.SpecIndex, bitArray, index, parameterEndLimit))
            {
                specIndex = new Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex(bitArray, ref index);
            }
            InventoryParameterSpecId inventoryParameterSpecId = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.InventoryParameterSpecId, bitArray, index, parameterEndLimit))
            {
                inventoryParameterSpecId = new InventoryParameterSpecId(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.AntennaId antennaId = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AntennaId, bitArray, index, parameterEndLimit))
            {
                antennaId = new Kalitte.Sensors.Rfid.Llrp.Core.AntennaId(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.AccessSpecId accessSpecId = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AccessSpecId, bitArray, index, parameterEndLimit))
            {
                accessSpecId = new Kalitte.Sensors.Rfid.Llrp.Core.AccessSpecId(bitArray, ref index);
            }
            Kalitte.Sensors.Rfid.Llrp.Core.OPSpecId opSpecId = null;
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.OPSpecId, bitArray, index, parameterEndLimit))
            {
                opSpecId = new Kalitte.Sensors.Rfid.Llrp.Core.OPSpecId(bitArray, ref index);
            }
            Collection<CustomParameterBase> customParameters = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index, parameterEndLimit))
            {
                customParameters.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(message, roSpecId, specIndex, inventoryParameterSpecId, antennaId, accessSpecId, opSpecId, customParameters);
        }

        public ReaderExceptionEvent(string message, Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId roSpecId, Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex specIndex, InventoryParameterSpecId inventoryParameterSpecId, Kalitte.Sensors.Rfid.Llrp.Core.AntennaId antennaId, Kalitte.Sensors.Rfid.Llrp.Core.AccessSpecId accessSpecId, Kalitte.Sensors.Rfid.Llrp.Core.OPSpecId opSpecId, Collection<CustomParameterBase> customParameters) : base(LlrpParameterType.ReaderExceptionEvent)
        {
            this.Init(message, roSpecId, specIndex, inventoryParameterSpecId, antennaId, accessSpecId, opSpecId, customParameters);
        }


        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            if (this.Message != null)
            {
                byte[] element = Util.ConvertUnicodeToUTF8(this.Message);
                stream.Append((long) ((ushort) element.Length), 0x10, true);
                stream.Append(element, (uint) (element.Length * 8), false);
            }
            else
            {
                stream.Append((long) 0L, 0x10, true);
            }
            Util.Encode(this.ROSpecId, stream);
            Util.Encode(this.SpecIndex, stream);
            Util.Encode(this.InventorySpecId, stream);
            Util.Encode(this.AntennaId, stream);
            Util.Encode(this.AccessSpecId, stream);
            Util.Encode(this.OPSpecId, stream);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
        }

        private void Init(string message, Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId roSpecId, Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex specIndex, InventoryParameterSpecId inventoryParameterSpecId, Kalitte.Sensors.Rfid.Llrp.Core.AntennaId antennaId, Kalitte.Sensors.Rfid.Llrp.Core.AccessSpecId accessSpecId, Kalitte.Sensors.Rfid.Llrp.Core.OPSpecId opSpecId, Collection<CustomParameterBase> customParameters)
        {
            this.m_message = message;
            this.m_roSpecId = roSpecId;
            this.m_specIndex = specIndex;
            this.m_inventorySpecId = inventoryParameterSpecId;
            this.m_antennaId = antennaId;
            this.m_accessSpecId = accessSpecId;
            this.m_opSpecId = opSpecId;
            this.m_customParamters = customParameters;
            ushort num = 0;
            if (this.Message != null)
            {
                byte[] buffer = Util.ConvertUnicodeToUTF8(this.Message);
                num = (buffer != null) ? ((ushort) (buffer.Length * 8)) : ((ushort) 0);
            }
            this.ParameterLength = ((((((((uint) (0x10 + num)) + Util.GetBitLengthOfParam(this.ROSpecId)) + Util.GetBitLengthOfParam(this.SpecIndex)) + Util.GetBitLengthOfParam(this.InventorySpecId)) + Util.GetBitLengthOfParam(this.AntennaId)) + Util.GetBitLengthOfParam(this.AccessSpecId)) + Util.GetBitLengthOfParam(this.OPSpecId)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.CustomParameters);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Reader Exception Event>");
            strBuilder.Append(base.ToString());
            Util.ToString(this.ROSpecId, strBuilder);
            Util.ToString(this.SpecIndex, strBuilder);
            Util.ToString(this.InventorySpecId, strBuilder);
            Util.ToString(this.AntennaId, strBuilder);
            Util.ToString(this.AccessSpecId, strBuilder);
            Util.ToString(this.OPSpecId, strBuilder);
            strBuilder.Append("<Message>");
            strBuilder.Append(this.Message);
            strBuilder.Append("</Message>");
            Util.ToString<CustomParameterBase>(this.CustomParameters, strBuilder);
            strBuilder.Append("</Reader Exception Event>");
            return strBuilder.ToString();
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.AccessSpecId AccessSpecId
        {
            get
            {
                return this.m_accessSpecId;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.AntennaId AntennaId
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
                return this.m_customParamters;
            }
        }

        public InventoryParameterSpecId InventorySpecId
        {
            get
            {
                return this.m_inventorySpecId;
            }
        }

        public string Message
        {
            get
            {
                return this.m_message;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.OPSpecId OPSpecId
        {
            get
            {
                return this.m_opSpecId;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.ROSpecId ROSpecId
        {
            get
            {
                return this.m_roSpecId;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.SpecIndex SpecIndex
        {
            get
            {
                return this.m_specIndex;
            }
        }
    }
}
