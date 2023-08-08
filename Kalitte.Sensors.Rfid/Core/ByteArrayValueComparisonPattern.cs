namespace Kalitte.Sensors.Rfid.Core
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Rfid.Utilities;
    using Kalitte.Sensors.Utilities;

    [Serializable]
    public sealed class ComparePatternByteArray
    {
        private readonly byte[] maskPart;
        private readonly int positionInTargetFieldToCompare;
        private readonly byte[] valuePart;

        public ComparePatternByteArray(byte[] valuePart, byte[] maskPart, int positionInTargetFieldToCompare)
        {
            this.valuePart = valuePart;
            this.maskPart = maskPart;
            this.positionInTargetFieldToCompare = positionInTargetFieldToCompare;
            this.ValidateParameters();
        }

        public byte[] GetMaskPart()
        {
            if (this.maskPart == null)
            {
                return null;
            }
            return CollectionsHelper.CloneByte(this.maskPart);
        }

        public byte[] GetValuePart()
        {
            if (this.valuePart == null)
            {
                return null;
            }
            return CollectionsHelper.CloneByte(this.valuePart);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<comparePatternByteArray>");
            builder.Append("<valuePart>");
            builder.Append(Convert.ToBase64String(this.valuePart));
            builder.Append("</valuePart>");
            builder.Append("<maskPart>");
            builder.Append(Convert.ToBase64String(this.maskPart));
            builder.Append("</maskPart>");
            builder.Append("<positionInTargetFieldToCompare>");
            builder.Append(this.positionInTargetFieldToCompare);
            builder.Append("</positionInTargetFieldToCompare>");
            builder.Append("</comparePatternByteArray>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.valuePart == null)
            {
                throw new ArgumentNullException("valuePart");
            }
            if ((this.maskPart != null) && (this.maskPart.Length != this.valuePart.Length))
            {
                throw new ArgumentException("ValueMaskLengthMismatch");
            }
            if (this.maskPart == null)
            {
                throw new ArgumentNullException("maskPart");
            }
            if (0 > this.positionInTargetFieldToCompare)
            {
                throw new ArgumentException("NoNegative", "positionInTargetFieldToCompare");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public int PositionInTargetFieldToCompare
        {
            get
            {
                return this.positionInTargetFieldToCompare;
            }
        }
    }
}
