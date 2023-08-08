namespace Kalitte.Sensors.Rfid.Core
{
    using System;
    using System.Text;
    using Kalitte.Sensors.Core;

    [Serializable]
    public sealed class TagMetadata
    {
        private readonly int blockSize;
        private readonly bool dataAvailable;
        private readonly bool dataWritable;
        private readonly bool idWritable;
        private readonly string manufacturer;
        private readonly TagType tagType;
        private readonly int totalBlocks;
        private VendorData vendorSpecificData;

        public TagMetadata(TagType tagType, string manufacturer, int blockSize, int totalBlocks, bool idWritable, bool dataAvailable, bool dataWritable)
        {
            if (TagType.Uninitialized.Value >= tagType.Value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            this.tagType = tagType;
            if ((manufacturer == null) || (manufacturer.Length == 0))
            {
                throw new ArgumentNullException("manufacturer");
            }
            this.manufacturer = manufacturer;
            if (0 > blockSize)
            {
                throw new ArgumentException("NoNegative");
            }
            this.blockSize = blockSize;
            if (0 > totalBlocks)
            {
                throw new ArgumentException("NoNegative");
            }
            this.totalBlocks = totalBlocks;
            this.idWritable = idWritable;
            this.dataAvailable = dataAvailable;
            this.dataWritable = dataWritable;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<tagMetaData>");
            builder.Append("<tagType>");
            builder.Append(this.tagType);
            builder.Append("</tagType>");
            builder.Append("<manufacturer>");
            builder.Append(this.manufacturer);
            builder.Append("</manufacturer>");
            builder.Append("<blockSize>");
            builder.Append(this.blockSize);
            builder.Append("</blockSize>");
            builder.Append("<totalBlocks>");
            builder.Append(this.totalBlocks);
            builder.Append("</totalBlocks>");
            builder.Append("<idWritable>");
            builder.Append(this.idWritable);
            builder.Append("</idWritable>");
            builder.Append("<dataAvailable>");
            builder.Append(this.dataAvailable);
            builder.Append("</dataAvailable>");
            builder.Append("<dataWritable>");
            builder.Append(this.dataWritable);
            builder.Append("</dataWritable>");
            builder.Append("</tagMetaData>");
            return builder.ToString();
        }

        public int BlockSize
        {
            get
            {
                return this.blockSize;
            }
        }

        public bool DataAvailable
        {
            get
            {
                return this.dataAvailable;
            }
        }

        public bool DataWritable
        {
            get
            {
                return this.dataWritable;
            }
        }

        public bool IdWritable
        {
            get
            {
                return this.idWritable;
            }
        }

        public string Manufacturer
        {
            get
            {
                return this.manufacturer;
            }
        }

        public TagType TagType
        {
            get
            {
                return this.tagType;
            }
        }

        public int TotalBlocks
        {
            get
            {
                return this.totalBlocks;
            }
        }

        public VendorData VendorSpecificData
        {
            get
            {
                return this.vendorSpecificData;
            }
            set
            {
                this.vendorSpecificData = value;
            }
        }
    }
}
