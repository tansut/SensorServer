namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class GeneralDeviceCapabilities : LlrpTlvParameterBase
    {
        private Collection<PerAntennaReceiveSensitivityRange> m_antennaReceiveSensitivityRange;
        private Collection<PerAntennaAirProtocol> m_antennasAirProtocol;
        private bool m_canSetAntennaProperties;
        private uint m_deviceManufactureName;
        private Kalitte.Sensors.Rfid.Llrp.Core.GpioCapabilities m_gpioCapabilities;
        private bool m_hasUTCClockCapability;
        private ushort m_maxNumberOfAntennaSupported;
        private uint m_modelName;
        private string m_readerFirmwareVersion;
        private Collection<ReceiveSensitivityTableEntry> m_receiveTableEntries;

        internal GeneralDeviceCapabilities(BitArray bitArray, ref int index) : base(LlrpParameterType.GeneralDeviceCapabilities, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            string readerFirmwareVersion = null;
            Collection<ReceiveSensitivityTableEntry> receiveTableEntries = new Collection<ReceiveSensitivityTableEntry>();
            Collection<PerAntennaReceiveSensitivityRange> antennaReceiveSensitivityRange = new Collection<PerAntennaReceiveSensitivityRange>();
            Kalitte.Sensors.Rfid.Llrp.Core.GpioCapabilities gpioCapabilities = null;
            Collection<PerAntennaAirProtocol> antennasAirProtocol = new Collection<PerAntennaAirProtocol>();
            ushort maxAntennaSupported = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            bool canSetAntennaProperties = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            bool hasUTCClock = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 14;
            uint deviceManufactureName = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint modelName = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            ushort byteCount = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            readerFirmwareVersion = BitHelper.GetString(bitArray, ref index, byteCount);
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.ReceiveSensitivityTableEntry, bitArray, index, parameterEndLimit))
            {
                receiveTableEntries.Add(new ReceiveSensitivityTableEntry(bitArray, ref index));
            }
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.PerAntennaReceiveSensitivityRange, bitArray, index, parameterEndLimit))
            {
                antennaReceiveSensitivityRange.Add(new PerAntennaReceiveSensitivityRange(bitArray, ref index));
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.GpioCapabilities, bitArray, index, parameterEndLimit))
            {
                gpioCapabilities = new Kalitte.Sensors.Rfid.Llrp.Core.GpioCapabilities(bitArray, ref index);
            }
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.PerAntennaAirProtocol, bitArray, index, parameterEndLimit))
            {
                antennasAirProtocol.Add(new PerAntennaAirProtocol(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(maxAntennaSupported, canSetAntennaProperties, hasUTCClock, deviceManufactureName, modelName, readerFirmwareVersion, receiveTableEntries, antennaReceiveSensitivityRange, gpioCapabilities, antennasAirProtocol);
        }

        public GeneralDeviceCapabilities(ushort maxAntennaSupported, bool canSetAntennaProperties, bool hasUTCClock, uint deviceManufactureName, uint modelName, string readerFirmwareVersion, Collection<ReceiveSensitivityTableEntry> receiveTableEntries, Collection<PerAntennaReceiveSensitivityRange> antennaReceiveSensitivityRange, Kalitte.Sensors.Rfid.Llrp.Core.GpioCapabilities gpioCapabilities, Collection<PerAntennaAirProtocol> antennasAirProtocol) : base(LlrpParameterType.GeneralDeviceCapabilities)
        {
            this.Init(maxAntennaSupported, canSetAntennaProperties, hasUTCClock, deviceManufactureName, modelName, readerFirmwareVersion, receiveTableEntries, antennaReceiveSensitivityRange, gpioCapabilities, antennasAirProtocol);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.MaxNumberOfAntennaSupported, 0x10, true);
            stream.Append(this.CanSetAntennaProperties, 1, true);
            stream.Append(this.HasUtcClockCapability, 1, true);
            stream.Append((ulong) 0L, 14, true);
            stream.Append((long) this.DeviceManufacturerName, 0x20, true);
            stream.Append((long) this.ModelName, 0x20, true);
            if (this.ReaderFirmwareVersion != null)
            {
                byte[] element = Util.ConvertUnicodeToUTF8(this.ReaderFirmwareVersion);
                stream.Append((long) ((ushort) element.Length), 0x10, true);
                stream.Append(element, (uint) (element.Length * 8), false);
            }
            else
            {
                stream.Append((long) 0L, 0x10, true);
            }
            Util.Encode<ReceiveSensitivityTableEntry>(this.ReceiveTableEntries, stream);
            Util.Encode<PerAntennaReceiveSensitivityRange>(this.AntennaReceiveSensitivityRange, stream);
            Util.Encode(this.GpioCapabilities, stream);
            Util.Encode<PerAntennaAirProtocol>(this.AntennasAirProtocol, stream);
        }

        private void Init(ushort maxAntennaSupported, bool canSetAntennaProperties, bool hasUTCClock, uint deviceManufactureName, uint modelName, string readerFirmwareVersion, Collection<ReceiveSensitivityTableEntry> receiveTableEntries, Collection<PerAntennaReceiveSensitivityRange> antennaReceiveSensitivityRange, Kalitte.Sensors.Rfid.Llrp.Core.GpioCapabilities gpioCapabilities, Collection<PerAntennaAirProtocol> antennasAirProtocol)
        {
            if ((receiveTableEntries == null) || (receiveTableEntries.Count <= 0))
            {
                throw new ArgumentException("receiveTableEntries");
            }
            if (gpioCapabilities == null)
            {
                throw new ArgumentNullException("gpioCapabilities");
            }
            if ((antennasAirProtocol == null) || (antennasAirProtocol.Count <= 0))
            {
                throw new ArgumentException("antennasAirProtocol");
            }
            Util.CheckCollectionForNonNullElement<ReceiveSensitivityTableEntry>(receiveTableEntries);
            Util.CheckCollectionForNonNullElement<PerAntennaReceiveSensitivityRange>(antennaReceiveSensitivityRange);
            Util.CheckCollectionForNonNullElement<PerAntennaAirProtocol>(antennasAirProtocol);
            this.m_maxNumberOfAntennaSupported = maxAntennaSupported;
            this.m_canSetAntennaProperties = canSetAntennaProperties;
            this.m_hasUTCClockCapability = hasUTCClock;
            this.m_deviceManufactureName = deviceManufactureName;
            this.m_modelName = modelName;
            this.m_readerFirmwareVersion = readerFirmwareVersion;
            this.m_receiveTableEntries = receiveTableEntries;
            this.m_antennaReceiveSensitivityRange = antennaReceiveSensitivityRange;
            this.m_gpioCapabilities = gpioCapabilities;
            this.m_antennasAirProtocol = antennasAirProtocol;
            ushort num = 0;
            if (this.ReaderFirmwareVersion != null)
            {
                num = (ushort) (Util.ConvertUnicodeToUTF8(this.ReaderFirmwareVersion).Length * 8);
            }
            this.ParameterLength = (((((uint) (0x70 + num)) + Util.GetTotalBitLengthOfParam<ReceiveSensitivityTableEntry>(this.ReceiveTableEntries)) + Util.GetTotalBitLengthOfParam<PerAntennaReceiveSensitivityRange>(this.AntennaReceiveSensitivityRange)) + Util.GetBitLengthOfParam(this.GpioCapabilities)) + Util.GetTotalBitLengthOfParam<PerAntennaAirProtocol>(this.AntennasAirProtocol);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<General Device Capabilities>");
            strBuilder.Append(base.ToString());
            strBuilder.Append("<Device Manufacture Name>");
            strBuilder.Append(this.DeviceManufacturerName);
            strBuilder.Append("</Device Manufacture Name>");
            strBuilder.Append("<Model Name>");
            strBuilder.Append(this.ModelName);
            strBuilder.Append("</Model Name>");
            strBuilder.Append("<Firmware Version>");
            strBuilder.Append(this.ReaderFirmwareVersion);
            strBuilder.Append("</Firmware Version>");
            strBuilder.Append("<Maximum Antenna Supported>");
            strBuilder.Append(this.MaxNumberOfAntennaSupported);
            strBuilder.Append("</Maximum Antenna Supported>");
            strBuilder.Append("<Can Set Antenna Properties>");
            strBuilder.Append(this.CanSetAntennaProperties);
            strBuilder.Append("</Can Set Antenna Properties>");
            Util.ToString<ReceiveSensitivityTableEntry>(this.ReceiveTableEntries, strBuilder);
            Util.ToString<PerAntennaReceiveSensitivityRange>(this.AntennaReceiveSensitivityRange, strBuilder);
            Util.ToString<PerAntennaAirProtocol>(this.AntennasAirProtocol, strBuilder);
            Util.ToString(this.GpioCapabilities, strBuilder);
            strBuilder.Append("<Has UTC Capability>");
            strBuilder.Append(this.HasUtcClockCapability);
            strBuilder.Append("</Has UTC Capability>");
            strBuilder.Append("</General Device Capabilities>");
            return strBuilder.ToString();
        }

        public Collection<PerAntennaReceiveSensitivityRange> AntennaReceiveSensitivityRange
        {
            get
            {
                return this.m_antennaReceiveSensitivityRange;
            }
        }

        public Collection<PerAntennaAirProtocol> AntennasAirProtocol
        {
            get
            {
                return this.m_antennasAirProtocol;
            }
        }

        public bool CanSetAntennaProperties
        {
            get
            {
                return this.m_canSetAntennaProperties;
            }
        }

        public uint DeviceManufacturerName
        {
            get
            {
                return this.m_deviceManufactureName;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.GpioCapabilities GpioCapabilities
        {
            get
            {
                return this.m_gpioCapabilities;
            }
        }

        public bool HasUtcClockCapability
        {
            get
            {
                return this.m_hasUTCClockCapability;
            }
        }

        public ushort MaxNumberOfAntennaSupported
        {
            get
            {
                return this.m_maxNumberOfAntennaSupported;
            }
        }

        public uint ModelName
        {
            get
            {
                return this.m_modelName;
            }
        }

        public string ReaderFirmwareVersion
        {
            get
            {
                return this.m_readerFirmwareVersion;
            }
        }

        public Collection<ReceiveSensitivityTableEntry> ReceiveTableEntries
        {
            get
            {
                return this.m_receiveTableEntries;
            }
        }
    }
}
