namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Text;
    using Kalitte.Sensors.Rfid.Core;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class GetReadFilterResponse : Response
    {
        private readonly FilterExpressionTree filterExpressionTree;

        public GetReadFilterResponse(FilterExpressionTree filterExpressionTree)
        {
            this.filterExpressionTree = filterExpressionTree;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getReadFilterResponse>");
            builder.Append("<filterExpressionTree>");
            builder.Append(this.filterExpressionTree);
            builder.Append("</filterExpressionTree>");
            builder.Append("</getReadFilterResponse>");
            return builder.ToString();
        }

        public FilterExpressionTree FilterExpressionTree
        {
            get
            {
                return this.filterExpressionTree;
            }
        }
    }
}
