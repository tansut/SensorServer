namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Rfid.Core;
    using Kalitte.Sensors.UI;

    [Serializable]
    [SensorCommandEditor("Rfid/QueryTagsCommandEditor.ascx")]
    public sealed class QueryTagsCommand : TagCommand
    {
        private readonly TagDataSelector dataSelector;
        private QueryTagsResponse response;

        public QueryTagsCommand(byte[] passCode) : base(passCode)
        {
            this.dataSelector = TagDataSelector.All;
            this.dataSelector.IsData = false;
        }

        public QueryTagsCommand(byte[] passCode, TagDataSelector dataSelector) : base(passCode)
        {
            this.dataSelector = TagDataSelector.All;
            this.dataSelector = dataSelector;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<queryTags>");
            builder.Append(base.ToString());
            builder.Append("<dataSelector>");
            builder.Append(this.dataSelector);
            builder.Append("</dataSelector>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</queryTags>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((null == this.dataSelector) || !this.dataSelector.IsInitialized)
            {
                throw new ArgumentException("InvalidSelectorValue");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public TagDataSelector DataSelector
        {
            get
            {
                return this.dataSelector;
            }
        }

        public QueryTagsResponse Response
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
