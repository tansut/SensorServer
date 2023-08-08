namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Rfid.Configuration;

    [Serializable]
    public sealed class PrintTagCommand : TagCommand
    {
        private readonly int count;
        private Dictionary<string, FieldIncrementInfo> fieldIncrements;
        private readonly PrintLabel printLabel;
        private PrintTagResponse response;

        public PrintTagCommand(byte[] passCode, PrintLabel printLabel) : base(passCode)
        {
            this.count = 1;
            this.printLabel = printLabel;
            this.ValidateParameters();
        }

        public PrintTagCommand(byte[] passCode, PrintLabel printLabel, int count, Dictionary<string, FieldIncrementInfo> fieldIncrements) : base(passCode)
        {
            this.count = 1;
            this.printLabel = printLabel;
            this.count = count;
            this.fieldIncrements = fieldIncrements;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<printTagCommand>");
            builder.Append(base.ToString());
            builder.Append("<printLabel>");
            builder.Append(this.printLabel);
            builder.Append("</printLabel>");
            builder.Append("<count>");
            builder.Append(this.count);
            builder.Append("</count>");
            builder.Append("<fieldIncrements>");
            if (this.fieldIncrements != null)
            {
                foreach (KeyValuePair<string, FieldIncrementInfo> pair in this.fieldIncrements)
                {
                    builder.Append("<fieldIncrement>");
                    builder.Append("<name>");
                    builder.Append(pair.Key);
                    builder.Append("</name>");
                    builder.Append("<increment>");
                    builder.Append(pair.Value);
                    builder.Append("</increment>");
                    builder.Append("</fieldIncrement>");
                }
            }
            builder.Append("</fieldIncrements>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</printTagCommand>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.printLabel == null)
            {
                throw new ArgumentNullException("printLabel");
            }
            if (0 >= this.count)
            {
                throw new ArgumentException("InvalidCount");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public int Count
        {
            get
            {
                return this.count;
            }
        }

        public Dictionary<string, FieldIncrementInfo> FieldIncrements
        {
            get
            {
                return this.fieldIncrements;
            }
        }

        public PrintLabel PrintLabel
        {
            get
            {
                return this.printLabel;
            }
        }

        public PrintTagResponse Response
        {
            get
            {
                return this.response;
            }
            set
            {
                this.response = value;
            }
        }
    }
}
