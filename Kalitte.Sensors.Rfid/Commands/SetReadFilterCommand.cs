namespace Kalitte.Sensors.Rfid.Commands
{
   using Kalitte.Sensors.Rfid;
    using System;
    using System.Text;
    using Kalitte.Sensors.Rfid.Core;
    using Kalitte.Sensors.Commands;

    [Serializable, MayChangeState]
    public sealed class SetReadFilterCommand : SensorCommand
    {
        private readonly FilterExpressionTree filterExpressionTree;
        private SetReadFilterResponse response;

        public SetReadFilterCommand(FilterExpressionTree filterExpressionTree)
        {
            this.filterExpressionTree = filterExpressionTree;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<setReadFilter>");
            builder.Append(base.ToString());
            builder.Append("<filterExpressionTree>");
            builder.Append(this.filterExpressionTree);
            builder.Append("</filterExpressionTree>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</setReadFilter>");
            return builder.ToString();
        }

        public FilterExpressionTree FilterExpressionTree
        {
            get
            {
                return this.filterExpressionTree;
            }
        }

        public SetReadFilterResponse Response
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
